using System;
using System.Threading;
using System.Threading.Tasks;
using NetCoreKit.Domain;
using NetCoreKit.Infrastructure.AspNetCore.CleanArch;
using NetCoreKit.Infrastructure.EfCore.Extensions;
using VND.CoolStore.Services.Review.Extensions;

namespace VND.CoolStore.Services.Review.v1.UseCases.UpdateReview
{
  public class RequestHandler : TxRequestHandlerBase<UpdateReviewRequest, UpdateReviewResponse>
  {
    public RequestHandler(IUnitOfWorkAsync uow, IQueryRepositoryFactory qrf) : base(uow, qrf)
    {
    }

    public override async Task<UpdateReviewResponse> Handle(UpdateReviewRequest request,
      CancellationToken cancellationToken)
    {
      var reviewQueryRepo = QueryFactory.QueryEfRepository<Domain.Review>();
      var reviewRepo = CommandFactory.Repository<Domain.Review>();

      var review = await reviewQueryRepo.FindOneAsync(x => x.Id == request.ReviewId);
      if (review == null) throw new Exception($"Couldn't find a review #{request.ReviewId}.");

      review.Content = request.Content;
      var result = await reviewRepo.UpdateAsync(review);

      return new UpdateReviewResponse {Result = result.ToDto()};
    }
  }
}
