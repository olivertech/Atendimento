var utility = angular.module('utilityModule', []);

utility.factory('generalUtility', ['paginationService', function(paginationService) {

    /** Function que retorna string de 5 caracteres aleatorios para formar senha temporaria */
    var _randomPwd = function() {

        var cookieKey = [];
        var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
      
        for (var i = 0; i < 5; i++)
          cookieKey[i] = possible.charAt(Math.floor(Math.random() * possible.length));
      
        return cookieKey.join('');
    };

    /** Função que alterna o type do campo de senha */
    var _showHidePwd = function(field) {
        var type = $(":input[id='" + field + "']").attr('type');
        if (type === "password") { $(":input[id='" + field + "']").attr('type', 'text'); }
        else { $(":input[id='" + field + "']").attr('type', 'password'); }
    };

    /** Função que mostra alerta de sucesso */
    var _showSuccessAlert = function() {
        $("#success-alert1").fadeTo(5000, 500).slideUp(500, function(){
            $("#success-alert1").slideUp(500);
        });
        $("#success-alert2").fadeTo(5000, 500).slideUp(500, function(){
            $("#success-alert2").slideUp(500);
        });
    };

    /** Função que mostra alerta de erro */
    var _showErrorAlert = function() {
        $("#error-alert1").fadeTo(5000, 500).slideUp(1500, function(){
            $("#error-alert1").slideUp(2500);
        });
        $("#error-alert2").fadeTo(5000, 500).slideUp(1500, function(){
            $("#error-alert2").slideUp(1500);
        });        
    };
    
    /** Função interna que prepara a paginação da grid de tickets */
    // var _getPagination = function(page, pageSize) {
    //     if ((page < 1 || page > $scope.pagination.totalPages) && $scope.pagination.totalPages > 0) {
    //         return;
    //     }

    //     // get pager object from service
    //     $scope.pagination = paginationService.getPagination($scope.totalRecords, page, pageSize);
    // };
    
    return {
        randomPwd: _randomPwd,
        showHidePwd: _showHidePwd,
        showSuccessAlert: _showSuccessAlert,
        showErrorAlert: _showErrorAlert
        //getPagination: _getPagination
    }; 
}]);