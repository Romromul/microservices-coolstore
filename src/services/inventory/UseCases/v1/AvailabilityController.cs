using Microsoft.AspNetCore.Mvc;
using System;
using VND.CoolStore.Services.Inventory.UseCases.Service;

namespace VND.CoolStore.Services.Inventory.UseCases.v1
{
  [ApiVersion("1.0")]
  [Route("api/v{api-version:apiVersion}/[controller]")]
  public class AvailabilityController : Controller
  {
    private readonly IInventoryService _inventoryService = null;

    public AvailabilityController(IInventoryService inventoryService)
    {
      _inventoryService = inventoryService;
    }

    [HttpGet("{id}")]
    public ActionResult<Domain.Inventory> Get(Guid id)
    {
      return _inventoryService.GetInventory(id);
    }
  }
}
