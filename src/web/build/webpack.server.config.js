const webpack = require('webpack')
const merge = require('webpack-merge')
const base = require('./webpack.base.config')
const nodeExternals = require('webpack-node-externals')
const VueSSRServerPlugin = require('vue-server-renderer/server-plugin')

console.log("this is test server")
console.log(JSON.stringify(process.env.WEB_HOST_ALIAS || 'development'))

module.exports = merge(base, {
  target: 'node',
  devtool: '#source-map',
  entry: './src/entry-server.js',
  output: {
    filename: 'server-bundle.js',
    libraryTarget: 'commonjs2'
  },
  resolve: {
    alias: {
      'create-api': './create-api-server.js'
    }
  },
  // https://webpack.js.org/configuration/externals/#externals
  // https://github.com/liady/webpack-node-externals
  externals: nodeExternals({
    // do not externalize CSS files in case we need to import it from a dep
    whitelist: ['buefy', /\.css$/]
  }),
  plugins: [
    new webpack.DefinePlugin({
      'process.env.NODE_ENV': JSON.stringify(process.env.NODE_ENV || 'development'),
      'process.env.WEB_HOST_ALIAS': JSON.stringify(process.env.WEB_HOST_ALIAS || 'localhost:8080'),
      'process.env.ID_HOST_ALIAS': JSON.stringify(process.env.ID_HOST_ALIAS || 'localhost:5001'),
      'process.env.API_HOST_ALIAS': JSON.stringify(process.env.API_HOST_ALIAS || 'localhost:5000'),
      'process.env.VUE_ENV': '"server"'
    }),
    new VueSSRServerPlugin()
  ]
})
