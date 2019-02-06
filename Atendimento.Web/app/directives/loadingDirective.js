app.directive("ifLoading", ['$http',
    function ($http) {
    return {
        restrict: "A",
        link: function(scope, elem) {
            scope.isLoading = isLoading;
            scope.$watch(scope.isLoading, function(loading) {
                //console.log(loading);
                if (loading) {
                    elem[0].style.display = "block";
                } else {
                    elem[0].style.display = "none";
                }
            });

            function isLoading() {
                //console.log($http.pendingRequests.length);
                return $http.pendingRequests.length > 0;
            }
        }
    };
}]);