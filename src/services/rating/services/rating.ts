import * as uuid from 'uuid'
import { default as Logger } from './logger'
import { default as ratingModel, RatingModel } from '../models/rating'

export const RatingProtoServices = {
  getRatings: async (call: any, callback: any) => {
    Logger.info('request', call.request)
    const ratings = await ratingModel.find({}).exec()
    const results = ratings.map((rating: any) => {
      return {
        id: rating._id,
        product_id: rating.productId,
        user_id: rating.userId,
        cost: rating.cost
      }
    })
    Logger.info(results)
    callback(null, { ratings: results })
  },
  getRatingByProductId: async (call: any, callback: any) => {
    Logger.info('request', call.request)
    const rating: any = await RatingService.findRatingByProductId(call.request.product_id)

    if (rating == null) {
      callback(null, [])
    }

    callback(null, {
      rating: {
        id: rating._id,
        product_id: rating.productId,
        user_id: rating.userId,
        cost: rating.cost
      }
    })
  },
  createRating: async (call: any, callback: any) => {
    Logger.info('request', call.request)
    const model: RatingModel = {
      id: uuid.v1(),
      productId: call.request.product_id,
      userId: call.request.user_id,
      cost: call.request.cost
    }
    const rating: any = await RatingService.createRating(model)
    callback(null, {
      rating: {
        id: rating._id,
        product_id: rating.productId,
        user_id: rating.userId,
        cost: rating.cost
      }
    })
  },
  updateRating: async (call: any, callback: any) => {
    Logger.info('request', call.request)
    const model: RatingModel = {
      id: call.request.id,
      productId: call.request.product_id,
      userId: call.request.user_id,
      cost: call.request.cost
    }
    const rating: any = await RatingService.updateRating(model)
    callback(null, {
      rating: {
        id: rating._id,
        product_id: rating.productId,
        user_id: rating.userId,
        cost: rating.cost
      }
    })
  }
}

export class RatingService {
  static async findRatingByProductId(productId: string) {
    return await ratingModel.findOne({ productId: productId }).exec()
  }
  static async findRatings() {
    return await ratingModel.find({}).exec()
  }
  static async createRating(model: RatingModel) {
    const id = uuid.v1()
    Logger.info({ ...model, _id: id })
    return await new ratingModel({ ...model, _id: id }).save()
  }
  static async updateRating(model: RatingModel) {
    return await new ratingModel().update({ id: model.id }, { ...model })
  }
}
