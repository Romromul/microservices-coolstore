using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using VND.CoolStore.Services.Inventory.UseCases.Service;
using VND.CoolStore.Services.Inventory.UseCases.Service.Impl;
using VND.FW.Infrastructure.AspNetCore.Extensions;
using VND.FW.Infrastructure.EfCore.SqlServer;

namespace VND.CoolStore.Services.Inventory
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddEfCoreSqlServer();
      services.AddScoped<IInventoryService, InventoryService>();
      services.AddMiniService(typeof(Startup).GetTypeInfo().Assembly);
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseMiniService();
    }
  }
}
