app.factory("loginService", ['$http', 'config',
    function($http, config) {

    var _login = function(username, password, type) {
        
        if (!username && !password) return;

        var user = { "username": username, "password": password, "userType": type };
        
        return $http({
            method: "POST",
            url: config.baseUrl + "/Login/Authenticate",
            data: user,
            async: true,
            cache: false
        }); 
    };

    var _recoverPassword = function(email, userType) {
        if (!email) return;

        var user = { "email": email, "userType": userType };

        return $http({
            method: "POST",
            url: config.baseUrl + "/Login/Authenticate",
            data: user,
            async: true,
            cache: false
        }); 
    };

    /** Função que troca a senha provisória do atendente ou do usuário */
    var _changePassword = function(username, oldPassword, newPassword, tipo) {
        var  request = {
            "username": username,
            "oldPassword": oldPassword,
            "newPassword": newPassword,
            "userType": tipo
        };

        return $http({
            method: "POST",
            url: config.baseUrl + "/Login/ChangePassword",
            data: request,
            async: true,
            cache: false
        }); 
    };

    return {
        login: _login,
        recoverPassword: _recoverPassword,
        changePassword: _changePassword
    };
}]);