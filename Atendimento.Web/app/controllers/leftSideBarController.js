app.controller('leftSideBarController', function($scope, $sessionStorage) {
    
    $scope.usuario = $sessionStorage.usuario;

    /** Função que prepara o cumprimento ao usuário */
    $scope.greeting = function() {
        $scope.hi = '';
        var d = new Date();
        var horas = d.getHours();

        if (horas >= 0 && horas < 12) {
            $scope.hi = "Bom Dia";
        }
        else {
            if (horas >= 12 && horas < 18) {
                $scope.hi = "Boa Tarde";
            }
            else {
                $scope.hi = "Boa Noite";
            }
        }
    }

    //Prepara o cumprimento ao usuário logado
    $scope.greeting();    
});