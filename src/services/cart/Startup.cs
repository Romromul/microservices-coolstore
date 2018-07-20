using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using VND.CoolStore.Services.Cart.UseCases.Services;
using VND.CoolStore.Services.Cart.UseCases.Services.Impl;
using VND.FW.Infrastructure.AspNetCore.Extensions;
using VND.FW.Infrastructure.EfCore.SqlServer;

namespace VND.CoolStore.Services.Cart
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddEfCoreSqlServer();
      services.AddScoped<ICartService, CartService>();
      services.AddMiniService(typeof(Startup).GetTypeInfo().Assembly);
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseMiniService();
    }
  }
}
