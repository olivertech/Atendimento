app.factory('errorInterceptor', ['$q', '$location',
    function($q, $location) {
    return {
        responseError: function(rejection) {
            
            if (rejection.status === 401) {
                $location.path('/{statusCode:401}');
            }
            
            if (rejection.status === 405) {
                $location.path('/{statusCode:405}');
            }

            // if (rejection.status === 403) {
            //     canceller.resolve('Forbidden');  
            //     $location.path('/');
            // }

            if(rejection.status !== 200) {
                //$location.path("/error");
            }

            return $q.reject(rejection);
        }
    };
}]);