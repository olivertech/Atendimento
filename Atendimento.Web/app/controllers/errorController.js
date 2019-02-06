app.controller('errorController', ['$scope', '$location',
    function($scope, $location) {
    
    $scope.voltar = function() {
        $location.path("/");
    };

    $scope.sair = function() {
        $location.path("/");
    };
}]);