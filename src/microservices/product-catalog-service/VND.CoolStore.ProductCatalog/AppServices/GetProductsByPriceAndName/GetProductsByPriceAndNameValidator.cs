using FluentValidation;
using VND.CoolStore.ProductCatalog.DataContracts.V1;

namespace VND.CoolStore.ProductCatalog.AppServices.GetProductsByPriceAndName
{
    public class GetProductsByPriceAndNameValidator : AbstractValidator<GetProductsRequest>
    {
        public GetProductsByPriceAndNameValidator()
        {

        }
    }
}
