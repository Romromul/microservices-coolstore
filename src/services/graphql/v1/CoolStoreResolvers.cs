using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using tanka.graphql;
using tanka.graphql.resolvers;
using VND.CoolStore.Services.Cart.v1.Grpc;
using VND.CoolStore.Services.Catalog.v1.Grpc;
using VND.CoolStore.Services.Inventory.v1.Grpc;
using VND.CoolStore.Services.Rating.v1.Grpc;
using VND.CoolStore.Services.Review.v1.Grpc;
using static tanka.graphql.resolvers.Resolve;
using static VND.CoolStore.Services.Cart.v1.Grpc.CartService;
using static VND.CoolStore.Services.Catalog.v1.Grpc.CatalogService;
using static VND.CoolStore.Services.Inventory.v1.Grpc.InventoryService;
using static VND.CoolStore.Services.Rating.v1.Grpc.RatingService;
using static VND.CoolStore.Services.Review.v1.Grpc.ReviewService;

namespace VND.CoolStore.Services.GraphQL.v1
{
    public class CoolStoreResolvers : ResolverMap
    {
        public CoolStoreResolvers(ICoolStoreResolverService resolverService)
        {
            this["Product"] = new FieldResolverMap
            {
                {"id", PropertyOf<CatalogProductDto>(m => m.Id)},
                {"name", PropertyOf<CatalogProductDto>(m => m.Name)},
                {"price", PropertyOf<CatalogProductDto>(m => m.Price)},
                {"imageUrl", PropertyOf<CatalogProductDto>(m => m.ImageUrl)},
                {"desc", PropertyOf<CatalogProductDto>(m => m.Desc)}
            };

            this["Cart"] = new FieldResolverMap
            {
                {"id", PropertyOf<CartDto>(m => m.Id)},
                {"cartItemTotal", PropertyOf<CartDto>(m => m.CartItemTotal)},
                {"cartItemPromoSavings", PropertyOf<CartDto>(m => m.CartItemPromoSavings)},
                {"shippingTotal", PropertyOf<CartDto>(m => m.ShippingTotal)},
                {"shippingPromoSavings", PropertyOf<CartDto>(m => m.ShippingPromoSavings)},
                {"cartTotal", PropertyOf<CartDto>(m => m.CartTotal)},
                {"isCheckOut", PropertyOf<CartDto>(m => m.IsCheckOut)},
                {"items", PropertyOf<CartDto>(m => m.Items)}
            };

            this["CartItem"] = new FieldResolverMap
            {
                {"productId", PropertyOf<CartItemDto>(m => m.ProductId)},
                {"productName", PropertyOf<CartItemDto>(m => m.ProductName)},
                {"quantity", PropertyOf<CartItemDto>(m => m.Quantity)},
                {"price", PropertyOf<CartItemDto>(m => m.Price)},
                {"promoSavings", PropertyOf<CartItemDto>(m => m.PromoSavings)}
            };

            this["Inventory"] = new FieldResolverMap
            {
                {"id", PropertyOf<InventoryDto>(m => m.Id)},
                {"location", PropertyOf<InventoryDto>(m => m.Location)},
                {"quantity", PropertyOf<InventoryDto>(m => m.Quantity)},
                {"link", PropertyOf<InventoryDto>(m => m.Link)}
            };

            this["Rating"] = new FieldResolverMap
            {
                {"id", PropertyOf<RatingDto>(m => m.Id)},
                {"productId", PropertyOf<RatingDto>(m => m.ProductId)},
                {"userId", PropertyOf<RatingDto>(m => m.UserId)},
                {"cost", PropertyOf<RatingDto>(m => m.Cost)}
            };

            this["Review"] = new FieldResolverMap
            {
                {"id", PropertyOf<ReviewDto>(m => m.Id)},
                {"content", PropertyOf<ReviewDto>(m => m.Content)},
                {"authorId", PropertyOf<ReviewDto>(m => m.AuthorId)},
                {"authorName", PropertyOf<ReviewDto>(m => m.AuthorName)},
                {"productId", PropertyOf<ReviewDto>(m => m.ProductId)},
                {"productName", PropertyOf<ReviewDto>(m => m.ProductName)}
            };

            this["Query"] = new FieldResolverMap
            {
                {"products", resolverService.GetProductsAsync},
                {"product", resolverService.GetProductAsync},
                {"carts", resolverService.GetCartAsync},
                {"availabilities", resolverService.GetAvailabilitiesAsync},
                {"availability", resolverService.GetAvailabilityAsync},
                {"ratings", resolverService.GetRatingsAsync},
                {"rating", resolverService.GetRatingAsync},
                {"reviews", resolverService.GetReviewsAsync}
            };

            this["Mutation"] = new FieldResolverMap
            {
                {"createProduct", resolverService.CreateProductAsync},
                {"insertItemToNewCart", resolverService.InsertItemToNewCartAsync},
                {"updateItemInCart", resolverService.UpdateItemInCartAsync},
                {"deleteItem", resolverService.DeleteItemAsync},
                {"checkout", resolverService.CheckoutAsync},
                {"createRating", resolverService.CreateRatingAsync},
                {"updateRating", resolverService.UpdateRatingAsync},
                {"createReview", resolverService.CreateReviewAsync},
                {"editReview", resolverService.EditReviewAsync},
                {"deleteReview", resolverService.DeleteReviewAsync}
            };
        }
    }

    public interface ICoolStoreResolverService
    {
        ValueTask<IResolveResult> GetProductsAsync(ResolverContext context);
        ValueTask<IResolveResult> GetProductAsync(ResolverContext context);
        ValueTask<IResolveResult> CreateProductAsync(ResolverContext context);
        ValueTask<IResolveResult> InsertItemToNewCartAsync(ResolverContext context);
        ValueTask<IResolveResult> UpdateItemInCartAsync(ResolverContext context);
        ValueTask<IResolveResult> DeleteItemAsync(ResolverContext context);
        ValueTask<IResolveResult> CheckoutAsync(ResolverContext context);
        ValueTask<IResolveResult> GetCartAsync(ResolverContext context);
        ValueTask<IResolveResult> GetAvailabilitiesAsync(ResolverContext context);
        ValueTask<IResolveResult> GetAvailabilityAsync(ResolverContext context);
        ValueTask<IResolveResult> GetRatingsAsync(ResolverContext context);
        ValueTask<IResolveResult> GetRatingAsync(ResolverContext context);
        ValueTask<IResolveResult> CreateRatingAsync(ResolverContext context);
        ValueTask<IResolveResult> UpdateRatingAsync(ResolverContext context);
        ValueTask<IResolveResult> GetReviewsAsync(ResolverContext context);
        ValueTask<IResolveResult> CreateReviewAsync(ResolverContext context);
        ValueTask<IResolveResult> EditReviewAsync(ResolverContext context);
        ValueTask<IResolveResult> DeleteReviewAsync(ResolverContext context);
    }

    public class CoolStoreResolverService : ICoolStoreResolverService
    {
        private readonly CatalogServiceClient _catalogServiceClient;
        private readonly CartServiceClient _cartServiceClient;
        private readonly InventoryServiceClient _inventoryServiceClient;
        private readonly RatingServiceClient _ratingServiceClient;
        private readonly ReviewServiceClient _reviewServiceClient;

        public CoolStoreResolverService(
            CatalogServiceClient catalogServiceClient,
            CartServiceClient cartServiceClient,
            InventoryServiceClient inventoryServiceClient,
            RatingServiceClient ratingServiceClient,
            ReviewServiceClient reviewServiceClient)
        {
            _catalogServiceClient = catalogServiceClient;
            _cartServiceClient = cartServiceClient;
            _inventoryServiceClient = inventoryServiceClient;
            _ratingServiceClient = ratingServiceClient;
            _reviewServiceClient = reviewServiceClient;
        }

        public async ValueTask<IResolveResult> GetProductsAsync(ResolverContext context)
        {
            var input = context.GetArgument<GetProductsRequest>("input");
            var results = await _catalogServiceClient.GetProductsAsync(input);
            return As(results.Products);
        }

        public async ValueTask<IResolveResult> GetProductAsync(ResolverContext context)
        {
            var input = context.GetArgument<GetProductByIdRequest>("input");
            var result = await _catalogServiceClient.GetProductByIdAsync(input);
            return As(result.Product);
        }

        public async ValueTask<IResolveResult> CreateProductAsync(ResolverContext context)
        {
            var input = context.GetArgument<CreateProductRequest>("input");
            var result = await _catalogServiceClient.CreateProductAsync(input);
            return As(result.Product);
        }

        public async ValueTask<IResolveResult> InsertItemToNewCartAsync(ResolverContext context)
        {
            var input = context.GetArgument<InsertItemToNewCartRequest>("input");
            var result = await _cartServiceClient.InsertItemToNewCartAsync(input);
            return As(result.Result);
        }

        public async ValueTask<IResolveResult> UpdateItemInCartAsync(ResolverContext context)
        {
            var input = context.GetArgument<UpdateItemInCartRequest>("input");
            var result = await _cartServiceClient.UpdateItemInCartAsync(input);
            return As(result.Result);
        }

        public async ValueTask<IResolveResult> DeleteItemAsync(ResolverContext context)
        {
            var input = context.GetArgument<DeleteItemRequest>("input");
            var result = await _cartServiceClient.DeleteItemAsync(input);
            return As(result.ProductId);
        }

        public async ValueTask<IResolveResult> CheckoutAsync(ResolverContext context)
        {
            var input = context.GetArgument<CheckoutRequest>("input");
            var result = await _cartServiceClient.CheckoutAsync(input);
            return As(result.IsSucceed);
        }

        public async ValueTask<IResolveResult> GetCartAsync(ResolverContext context)
        {
            var input = context.GetArgument<GetCartRequest>("input");
            var result = await _cartServiceClient.GetCartAsync(input);
            return As(result.Result);
        }

        public async ValueTask<IResolveResult> GetAvailabilitiesAsync(ResolverContext context)
        {
            var result = await _inventoryServiceClient.GetInventoriesAsync(new Empty());
            return As(result.Inventories);
        }

        public async ValueTask<IResolveResult> GetAvailabilityAsync(ResolverContext context)
        {
            var input = context.GetArgument<GetInventoryRequest>("input");
            var result = await _inventoryServiceClient.GetInventoryAsync(input);
            return As(result.Result);
        }

        public async ValueTask<IResolveResult> GetRatingsAsync(ResolverContext context)
        {
            var result = await _ratingServiceClient.GetRatingsAsync(new Empty());
            return As(result.Ratings);
        }

        public async ValueTask<IResolveResult> GetRatingAsync(ResolverContext context)
        {
            var input = context.GetArgument<GetRatingByProductIdRequest>("input");
            var result = await _ratingServiceClient.GetRatingByProductIdAsync(input);
            return As(result.Rating);
        }

        public async ValueTask<IResolveResult> CreateRatingAsync(ResolverContext context)
        {
            var input = context.GetArgument<CreateRatingRequest>("input");
            var result = await _ratingServiceClient.CreateRatingAsync(input);
            return As(result.Rating);
        }

        public async ValueTask<IResolveResult> UpdateRatingAsync(ResolverContext context)
        {
            var input = context.GetArgument<UpdateRatingRequest>("input");
            var result = await _ratingServiceClient.UpdateRatingAsync(input);
            return As(result.Rating);
        }

        public async ValueTask<IResolveResult> GetReviewsAsync(ResolverContext context)
        {
            var input = context.GetArgument<GetReviewsRequest>("input");
            var result = await _reviewServiceClient.GetReviewsAsync(input);
            return As(result.Reviews);
        }

        public async ValueTask<IResolveResult> CreateReviewAsync(ResolverContext context)
        {
            var input = context.GetArgument<CreateReviewRequest>("input");
            var result = await _reviewServiceClient.CreateReviewAsync(input);
            return As(result.Result);
        }

        public async ValueTask<IResolveResult> EditReviewAsync(ResolverContext context)
        {
            var input = context.GetArgument<EditReviewRequest>("input");
            var result = await _reviewServiceClient.EditReviewAsync(input);
            return As(result.Result);
        }

        public async ValueTask<IResolveResult> DeleteReviewAsync(ResolverContext context)
        {
            var input = context.GetArgument<DeleteReviewRequest>("input");
            var result = await _reviewServiceClient.DeleteReviewAsync(input);
            return As(result.Id);
        }
    }
}
