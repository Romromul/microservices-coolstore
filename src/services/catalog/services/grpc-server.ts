import * as protoLoader from '@grpc/proto-loader'
import * as grpc from 'grpc'
import { default as Logger } from './logger'
import { ProductProtoServices } from './product'

const getProto = (protofile: any) => {
  const packageDefinition = protoLoader.loadSync(protofile, {
    keepCase: true,
    longs: String,
    enums: String,
    defaults: true,
    oneofs: true,
    includeDirs: ['node_modules/google-proto-files', 'proto']
  })
  return grpc.loadPackageDefinition(packageDefinition)
}

export default async () => {
  const server = new grpc.Server()
  const proto = getProto('./proto/catalog.proto')

  Logger.info(ProductProtoServices)

  await server.addService(proto.coolstore.CatalogService.service, {
    ...ProductProtoServices
  })

  server.bind(`0.0.0.0:${process.env.PORT || 5002}`, grpc.ServerCredentials.createInsecure())
  server.start()
  Logger.info(`gRPC server running on port ${process.env.PORT}.\n Press CTRL-C to stop.\n`)
}
