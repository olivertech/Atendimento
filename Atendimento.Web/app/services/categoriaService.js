app.factory("categoriaService", ['$http', 'config',
    function($http, config) {
    
    /** Retorna todas as categorias paginando */
    var _getPagedCategorias = function(offSet, numRows, orderBy, direction) {
        var filter = {
            "offset": offSet,
            "numrows": numRows,
            "orderBy": orderBy,
            "direction": direction
        };

        return $http({
            method: "POST",
            url: config.baseUrl + "/Categoria/GetAllPaged",
            data: filter,
            async: true,
            cache: false
        });  
    };

    /** Salva uma categoria */
    var _saveCategoria = function(id, nome) {
        var  request = {
            "nome": nome
        };

        var  method = '';
        var  url = config.baseUrl + "/Categoria/";
        
        if (id > 0) {
            method = "PUT";
            url = url + "Update?id=" + id;
        }
        else {
            method = "POST";
            url = url + "Insert";
        }
        
        return  $http({
            method: method,
            url: url,
            data: request,
            async: true,
            cache: false
        });  
    };

    /** Remove uma categoria */
    var _deleteCategoria = function(idCategoria) {
        return $http({
            method: "DELETE",
            url: config.baseUrl + "/Categoria/Delete?id=" + idCategoria,
            async: true,
            cache: false
        });
    };

    /** Recupera os dados de uma categoria */
    var _showCategoria = function(idCategoria) {
        return $http({
            method: "GET",
            url: config.baseUrl + "/Categoria/GetById?id=" + idCategoria,
            async: true,
            cache: false
        });
    };

    /** Retorna total de tickets associado a categoria */
    var _getTickets = function(idCategoria) {
        return $http({
            method: "GET",
            url: config.baseUrl + "/Ticket/GetTotalTicketsCategoria?idCategoria=" + idCategoria,
            async: true,
            cache: false
        });
    };
    
    return {
        getPagedCategorias: _getPagedCategorias,
        saveCategoria: _saveCategoria,
        deleteCategoria: _deleteCategoria,
        showCategoria: _showCategoria,
        getTickets: _getTickets
    };
}]);