logToConsole = function(message) {
  console.log(message);
  return true;
};

getUser = function() {
  return mgr.getUser().then(function(user) {
    console.log(user);
    if (user == null) return {};
    return {
      accessToken: user.access_token,
      tokenType: user.token_type,
      Scope: user.scope,
      Profile: {
        UserId: user.profile.sub,
        Name: user.profile.name,
        Email: user.profile.email,
        Website: user.profile.website
      }
    };
  }).catch(function(err) {
    log(err);
  });
};

startSignin = function() {
  mgr.signinRedirect({ redirect_uri: "http://localhost:5003/callback" }).then(function() {
    log("signinRedirect done");
  }).catch(function(err) {
    log(err);
  });
};

callback = function() {
  new Oidc.UserManager().signinRedirectCallback().then(function(user) {
    // log("signin response success", user);
  }).catch(function(err) {
    log(err);
  });
};

silent = function() {
  new Oidc.UserManager().signinSilentCallback();
};

///////////////////////////////
// config
///////////////////////////////
Oidc.Log.logger = console;
Oidc.Log.level = Oidc.Log.DEBUG;

var settings = {
  authority: "https://demo.identityserver.io/",
  client_id: "implicit",
  redirect_uri: "http://localhost:5003/callback",
  post_logout_redirect_uri: "http://localhost:5003",
  response_type: "id_token token",
  scope: "openid profile email api",

  silent_redirect_uri: "http://localhost:5003/silent",
  automaticSilentRenew: true,
  //silentRequestTimeout:10000,

  filterProtocolClaims: true,
  loadUserInfo: true
};
var mgr = new Oidc.UserManager(settings);

///////////////////////////////
// events
///////////////////////////////
mgr.events.addAccessTokenExpiring(function() {
  console.log("token expiring");
  log("token expiring");
});

mgr.events.addAccessTokenExpired(function() {
  console.log("token expired");
  log("token expired");
});

mgr.events.addSilentRenewError(function(e) {
  console.log("silent renew error", e.message);
  log("silent renew error", e.message);
});

mgr.events.addUserLoaded(function(user) {
  console.log("user loaded", user);
  mgr.getUser().then(function() {
    console.log("getUser loaded user after userLoaded event fired");
  });
});

mgr.events.addUserUnloaded(function(e) {
  console.log("user unloaded");
});
