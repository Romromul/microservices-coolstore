using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VND.FW.Infrastructure.AspNetCore;
using VND.FW.Infrastructure.AspNetCore.CleanArch;

namespace VND.CoolStore.Services.Cart.v1.UseCases.DeleteItemInCart
{
  [ApiVersion("1.0")]
  [Route("api/carts")]
  public class CartController : EvtControllerBase
  {
    public CartController(IMediator mediator) : base(mediator) { }

    [HttpDelete]
    [Route("{cartId:guid}/items/{productId:guid}")]
    [Auth(Policy = "access_cart_api")]
    public async Task<IActionResult> RemoveItemInCart(Guid cartId, Guid productId)
    {
      return await Eventor.SendStream<DeleteItemRequest, DeleteItemResponse>(
        new DeleteItemRequest { CartId = cartId, ProductId = productId },
        x => x.ProductId);
    }
  }
}
