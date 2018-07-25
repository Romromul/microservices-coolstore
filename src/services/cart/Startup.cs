using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.Infrastructure.Service;
using VND.CoolStore.Services.Cart.Infrastructure.Service.Impl;
using VND.FW.Infrastructure.AspNetCore;
using VND.FW.Infrastructure.AspNetCore.Extensions;
using VND.FW.Infrastructure.EfCore.SqlServer;

namespace VND.CoolStore.Services.Cart
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddEfCoreSqlServer();
      services.AddMiniService(typeof(Startup).GetTypeInfo().Assembly);
      services.AddHttpClient<RestClient>();
      services.AddScoped<ICatalogService, CatalogService>();
      services.AddScoped<IPromoService, PromoService>();
      services.AddScoped<IShippingService, ShippingService>();
      services.AddScoped<ICartService, CartService>();
      services.AddScoped<NoTaxCaculator, NoTaxCaculator>();
      services.AddScoped<TenPercentTaxCalculator, TenPercentTaxCalculator>();
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseMiniService();
    }
  }
}
