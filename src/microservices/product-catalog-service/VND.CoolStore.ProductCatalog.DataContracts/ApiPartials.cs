using MediatR;

namespace VND.CoolStore.ProductCatalog.DataContracts.V1
{
    public partial class GetProductsRequest : IRequest<GetProductsResponse>
    {
    }

    public partial class GetProductByIdRequest : IRequest<GetProductByIdResponse>
    {
    }

    public partial class CreateProductRequest : IRequest<CreateProductResponse>
    {
    }
}
