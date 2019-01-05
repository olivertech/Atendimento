app.controller('errorController', function($scope, $location) {
    
    $scope.voltar = function() {
        $location.path("/");
    }

    $scope.sair = function() {
        $location.path("/");
    }
});