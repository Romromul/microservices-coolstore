using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VND.Fw.Infrastructure.AspNetCore.Miniservice;

namespace VND.FW.Infrastructure.AspNetCore.Miniservice
{
  public class AuthNConfigureApplication : IConfigureApplication
  {
    public int Priority { get; } = 700;
    public void Configure(IApplicationBuilder app)
    {
      var config = app.ApplicationServices.GetRequiredService<IConfiguration>();

      if (config.GetValue("EnableAuthN", false))
      {
        app.UseAuthentication();
      }
    }
  }
}