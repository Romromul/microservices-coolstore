var url = 'http://localhost:5000/'
var urlSpa = 'http://localhost:8080/'
var urlIdp = 'http://localhost:5001/'
var urlCat = 'http://localhost:5002'
var urlCart = 'http://localhost:5002'
var urlInv = 'http://localhost:5004'
var urlRat = 'http://localhost:5007'

const env = process.env.NODE_ENV
const config = {
    mode: env || 'development'
}
if (config.mode == 'production') {
    urlSpa = "http://coolstore.local/"
    urlIdp = "http://id.coolstore.local/"
    url = "http://api.coolstore.local/"

    var urlCat = 'http://api.coolstore.local/catalog/'
    var urlCart = 'http://api.coolstore.local/cart/'
    var urlInv = 'http://api.coolstore.local/inventory/'
    var urlRat = 'http://api.coolstore.local/rating/'
}


const PROXY_CONFIG = {
    baseUrl: url,
    idpUrl: urlIdp,
    spaUrl: urlSpa,
    '/api/*': {
        target: url,
        secure: false,
        logLevel: 'debug'
    },
    '/catalog/api/*': {
        target: urlCat,
        secure: false,
        logLevel: 'debug',
        changeOrigin: true,
        pathRewrite: { '^/catalog': '' }
    },
    '/rating/api/*': {
        target: urlRat,
        secure: false,
        logLevel: 'debug',
        changeOrigin: true,
        pathRewrite: { '^/rating': '' }
    },
    '/cart/api/*': {
        target: urlCart,
        secure: false,
        logLevel: 'debug',
        changeOrigin: true,
        pathRewrite: { '^/cart': '' }
    },
    '/inventory/api/*': {
        target: urlInv,
        secure: false,
        logLevel: 'debug',
        changeOrigin: true,
        pathRewrite: { '^/inventory': '' }
    },
    '/config': {
        target: `${urlIdp}.well-known/openid-configuration`,
        secure: false,
        logLevel: 'debug',
        ignorePath: true,
        changeOrigin: true,
        pathRewrite: { '^/config': '' }
    },
    '/.well-known/openid-configuration/jwks': {
        target: `${urlIdp}.well-known/openid-configuration/jwks`,
        secure: false,
        logLevel: 'debug',
        ignorePath: true
    },
    '/host/*': {
        target: urlIdp,
        secure: false,
        logLevel: 'debug',
        changeOrigin: true,
        pathRewrite: { '^/host': '' }
    },
    '/connect/*': {
        target: urlIdp,
        secure: true,
        logLevel: 'debug',
        changeOrigin: true,
        router: function (req) {
            return url
        }
    }
}

module.exports = PROXY_CONFIG
