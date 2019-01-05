app.factory("loginService", function($http, config) {

    var _login = function(username, password, type) {
        
        if (!username && !password) return;

        var user = { "username": username, "password": password, "userType": type };
        return $http.post(config.baseUrl + "/Login/Authenticate", user);
    }

    var _recoverPassword = function(email, userType) {
        if (!email) return;

        var user = { "email": email, "userType": userType };
        return $http.post(config.baseUrl + "/Login/Recover", user);
    }

    return {
        login: _login,
        recoverPassword: _recoverPassword
    };
});