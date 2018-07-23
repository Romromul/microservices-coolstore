using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using VND.FW.Infrastructure.AspNetCore;

namespace VND.CoolStore.Services.ApiGateway.UseCases.v1
{
  [ApiVersion("1.0")]
  [Route("api/v{api-version:apiVersion}/reviews")]
  public class ReviewController : FW.Infrastructure.AspNetCore.ControllerBase
  {
    [HttpGet]
    [Auth(Policy = "access_review_api")]
    [SwaggerOperation(Tags = new[] { "review-service" })]
    [Route("{itemId:guid}")]
    public IActionResult Index(Guid itemId)
    {
      return Ok(itemId);
    }
  }
}
