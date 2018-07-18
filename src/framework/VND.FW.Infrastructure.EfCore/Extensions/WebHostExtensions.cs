﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using VND.Fw.Utils.Extensions;
using VND.FW.Infrastructure.EfCore.Db;
using VND.FW.Infrastructure.EfCore.Migration;

namespace VND.FW.Infrastructure.EfCore.Extensions
{
		public static class WebHostExtensions
		{
				public static IWebHost RegisterDbContext(this IWebHost webHost)
				{
						return webHost
								.MigrateDbContext<ApplicationDbContext>((context, services) =>
								{
										InstanceSeedData(services, context, typeof(ISeedData<>));
								});
				}

				internal static IWebHost MigrateDbContext<TContext>(this IWebHost webHost, Action<TContext, IServiceProvider> seeder)
						where TContext : DbContext
				{
						using (var scope = webHost.Services.CreateScope())
						{
								scope.ServiceProvider.MigrateDbContext(seeder);
						}

						return webHost;
				}

				public static void InstanceSeedData(this IServiceProvider resolver, DbContext context, Type seedData)
				{
						var configuration = resolver.GetService<IConfiguration>();
						var scanAssemblyPattern = configuration.GetSection("Persistence")["FullyQualifiedPrefix"];
						var seeders = scanAssemblyPattern.ResolveModularGenericTypes(seedData, context.GetType());

						// Console.WriteLine(scanAssemblyPattern);

						if (seeders == null) return;

						foreach (var seeder in seeders)
						{
								var seedInstance = Activator.CreateInstance(seeder, new[] { configuration });

								if (seedInstance != null)
								{
										var method = seeder.GetMethod("SeedAsync");
										((Task)method.Invoke(seedInstance, new[] { context })).Wait();
								}
						}
				}

				/// <summary>
				/// This function will open up the door to make the Setup page
				/// Because we can call to this function for provision new database
				/// </summary>
				/// <typeparam name="TContext"></typeparam>
				/// <param name="serviceProvider"></param>
				/// <param name="seeder"></param>
				/// <returns></returns>
				public static IServiceProvider MigrateDbContext<TContext>(
						this IServiceProvider serviceProvider,
						Action<TContext, IServiceProvider> seeder)
						where TContext : DbContext
				{
						var logger = serviceProvider.GetRequiredService<ILogger<TContext>>();
						var context = serviceProvider.GetService<TContext>();

						var policy = Policy
								.Handle<SqlException>()
								.WaitAndRetryForever(retryAttempt =>
										TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
										);

						policy.Execute(() =>
						{
								try
								{
										logger.LogInformation($"[VND] Migrating database associated with {typeof(TContext).FullName} context.");

										context.Database.OpenConnection();
										context.Database.EnsureCreated();

										logger.LogInformation($"[VND] Start to seed data for {typeof(TContext).FullName} context.");
										seeder(context, serviceProvider);

										logger.LogInformation($"[VND] Migrated database associated with {typeof(TContext).FullName} context.");
								}
								catch (Exception ex)
								{
										logger.LogError(ex,
												$"[VND] An error occurred while migrating the database used on {typeof(TContext).FullName} context.");
								}
						});

						return serviceProvider;
				}
		}
}
