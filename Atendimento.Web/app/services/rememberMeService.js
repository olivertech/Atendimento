app.factory("rememberMeService", ['$cookies',
    function($cookies) {

    /** Recupera cookies */
    var _fetchRememberMeCookie = function(name) {
        
        var cookieValues = document.cookie.split("; ");

        for (var i = 0; i < cookieValues.length; i++) {
            
            var cookiePair = cookieValues[i].split("=");
            
            //Se existir o cookie
            if (name === cookiePair[0]) {
                var value = '';
                
                try {
                    value = cookiePair[1];
                } catch (e) {
                    value = unescape(cookiePair[1]);
                }

                return value;
            }
        }

        //Se não existir o cookie
        return null;
    };

    /** Grava cookie de login */
    var _setRememberMeCookie = function(name, value) {
        if (arguments.length === 1) return fetchValue(name);

        var cookie = name + '=';
        var date = new Date();

        cookie += value + ';';
        date.setDate(date.getDate() + 365);
        
        cookie += 'expires=' + date.toString() + ';';
        document.cookie = cookie;
    };

    /** remove todos os cookies */
    var _resetAllCookies = function() {
        var cookies = $cookies.getAll();
        angular.forEach(cookies, function (v, k) {
            $cookies.remove(k);
        });
    };

    return {
        fetchRememberMeCookie: _fetchRememberMeCookie,
        setRememberMeCookie: _setRememberMeCookie,
        resetAllCookies: _resetAllCookies
    };
}]);