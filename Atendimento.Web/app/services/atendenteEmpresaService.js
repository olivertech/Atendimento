app.factory("atendenteService", function($http, config) {
    
    /** Retorna todos os atendente */
    var _getAtendentes = function(offSet, numRows) {
        var filter = {
            "offset": offSet,
            "numrows": numRows
        };

        return $http({
            method: "POST",
            url: config.baseUrl + "/AtendenteEmpresa/GetAllPaged",
            data: filter,
            async: true,
            cache: false
        });  
    }

    /** Salva o Atendente */
    var _saveAtendente = function(id, idEmpresa, nome, username, password, email, telefoneFixo, telefoneCelular, copia, provisorio, ativo, isAdmin, isDefault) {
        let request = {
            "idEmpresa": idEmpresa,
            "nome": nome,
            "username": username,
            "password": password,
            "email": email,
            "telefoneFixo": telefoneFixo,
            "telefoneCelular": telefoneCelular,
            "ativo": ativo,
            "provisorio": provisorio,
            "copia": copia,
            "isAdmin": isAdmin,
            "isDefault": isDefault
        };

        let method = '';
        let url = config.baseUrl + "/AtendenteEmpresa/";
        
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


    /** Recupera o atendente */
    var _showAtendente = function(idAtendente) {
        return $http({
            method: "GET",
            url: config.baseUrl + "/AtendenteEmpresa/GetById?id=" + idAtendente,
            async: true,
            cache: false
        });
    }

    /** Remove atendente */
    var _deleteAtendente = function(idAtendente) {
        return $http({
            method: "DELETE",
            url: config.baseUrl + "/AtendenteEmpresa/Delete?id=" + idAtendente,
            async: true,
            cache: false
        });
    }

    /** Retorna todas as empresas */
    var _getEmpresas = function() {
        return $http({
            method: "GET",
            url: config.baseUrl + "/Empresa/GetAll",
            async: true,
            cache: false
        });
    }
    
    return {
        getAtendentes: _getAtendentes,
        saveAtendente: _saveAtendente,
        showAtendente: _showAtendente,
        deleteAtendente: _deleteAtendente,
        getEmpresas: _getEmpresas
    };
});