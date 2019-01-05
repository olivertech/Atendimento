app.controller('topHeaderController', function($scope, $location, $sessionStorage) {
    /** Função de saída do app */
    $scope.logout = function() {
        $sessionStorage.tokenAuthentication = '';
        $location.path("/");
    }
});