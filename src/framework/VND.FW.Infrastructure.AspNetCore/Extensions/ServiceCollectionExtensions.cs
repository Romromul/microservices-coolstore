﻿using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace VND.FW.Infrastructure.AspNetCore.Extensions
{
		public static class ServiceCollectionExtensions
		{
				public static IServiceCollection AddHttpPolly(this IServiceCollection services)
				{
						services.AddHttpClient<RestClient>()
								.SetHandlerLifetime(TimeSpan.FromMinutes(1))
								.AddPolicyHandler(GetRetryPolicy())
								.AddPolicyHandler(GetCircuitBreakerPolicy());

						return services;
				}

				private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
				{
						return HttpPolicyExtensions
								.HandleTransientHttpError()
								.OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
								.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
				}

				private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
				{
						return HttpPolicyExtensions
								.HandleTransientHttpError()
								.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
				}
		}
}
