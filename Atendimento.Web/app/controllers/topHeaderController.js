app.controller('topHeaderController', ['$scope', '$location', '$sessionStorage',
    function($scope, $location, $sessionStorage) {

    /** Função de saída do app */
    $scope.logout = function() {
        $sessionStorage.tokenAuthentication = '';
        $sessionStorage.isOnDashboard = false;
        $sessionStorage.usuario = '';
        $sessionStorage.idUsuario = '';
        $sessionStorage.idCliente = '';
        $sessionStorage.nomeCliente = '';
        $sessionStorage.tipoUsuario = '';
        $sessionStorage.logicalPathAnexos = '';
        $location.hash("");
        $location.path("/");
    };
}]);