app.factory("empresaService", function($http, config) {
    
    /** Retorna todas as empresas */
    var _getEmpresas = function(offSet, numRows) {
        var filter = {
            "offset": offSet,
            "numrows": numRows
        };

        return $http({
            method: "POST",
            url: config.baseUrl + "/Empresa/GetAllPaged",
            data: filter,
            async: true,
            cache: false
        });  
    }

    /** Salva a empresa */
    var _saveEmpresa = function(id, nome, email, telefoneFixo, telefoneCelular, descricao, isDefault) {
        let request = {
            "nome": nome,
            "email": email,
            "telefoneFixo": telefoneFixo,
            "telefoneCelular": telefoneCelular,
            "descricao": descricao,
            "isDefault": isDefault
        };

        let method = '';
        let url = config.baseUrl + "/Empresa/";
        
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
    }

    /** Recupera a empresa */
    var _showEmpresa = function(idEmpresa) {
        return $http({
            method: "GET",
            url: config.baseUrl + "/Empresa/GetById?id=" + idEmpresa,
            async: true,
            cache: false
        });
    }

    /** Remove empresa */
    var _deleteEmpresa = function(idEmpresa) {
        return $http({
            method: "DELETE",
            url: config.baseUrl + "/Empresa/Delete?id=" + idEmpresa,
            async: true,
            cache: false
        });
    }

    return {
        getEmpresas: _getEmpresas,
        saveEmpresa: _saveEmpresa,
        showEmpresa: _showEmpresa,
        deleteEmpresa: _deleteEmpresa
    };
});