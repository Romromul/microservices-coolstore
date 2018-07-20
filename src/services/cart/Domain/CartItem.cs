using System;
using System.ComponentModel.DataAnnotations;
using VND.Fw.Domain;

namespace VND.CoolStore.Services.Cart.Domain
{
  public class CartItem : EntityBase
  {
    internal CartItem() : base(Guid.NewGuid())
    {
    }

    public CartItem(Guid id) : base(id)
    {
    }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public double PromoSavings { get; set; }

    [Required]
    public ProductId ProductId { get; set; }
  }
}
