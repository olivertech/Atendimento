app.factory("usuarioService", function($http, config) {
    
    /** Retorna todos os usuarios */
    var _getUsuarios = function(offSet, numRows, orderBy, direction) {
        var filter = {
            "offset": offSet,
            "numrows": numRows,
            "orderBy": orderBy,
            "direction": direction
        };

        return $http({
            method: "POST",
            url: config.baseUrl + "/UsuarioCliente/GetAllPaged",
            data: filter,
            async: true,
            cache: false
        });  
    }

    /** Salva o usuario */
    var _saveUsuario = function(id, idCliente, nome, username, password, email, telefoneFixo, telefoneCelular, copia, provisorio, ativo) {
        let request = {
            "idCliente": idCliente,
            "nome": nome,
            "username": username,
            "password": password,
            "email": email,
            "telefoneFixo": telefoneFixo,
            "telefoneCelular": telefoneCelular,
            "ativo": ativo,
            "copia": copia,
            "provisorio": provisorio
        };

        let method = '';
        let url = config.baseUrl + "/UsuarioCliente/";
        
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

    /** Recupera o usuario */
    var _showUsuario = function(idUsuario) {
        return $http({
            method: "GET",
            url: config.baseUrl + "/UsuarioCliente/GetById?id=" + idUsuario,
            async: true,
            cache: false
        });
    }

    /** Remove usuario */
    var _deleteUsuario = function(idUsuario) {
        return $http({
            method: "DELETE",
            url: config.baseUrl + "/UsuarioCliente/Delete?id=" + idUsuario,
            async: true,
            cache: false
        });
    }

    /** Retorna todos os clientes */
    var _getClientes = function() {
        return $http({
            method: "GET",
            url: config.baseUrl + "/Cliente/GetAll",
            async: true,
            cache: false
        });
    }
    
    /** Retorna total de tickets associado ao usuario */
    var _getTickets = function(idUsuario) {
        return $http({
            method: "GET",
            url: config.baseUrl + "/Ticket/GetTotalTicketsUsuario?idUsuario=" + idUsuario,
            async: true,
            cache: false
        });
    }
    
    return {
        getUsuarios: _getUsuarios,
        saveUsuario: _saveUsuario,
        showUsuario: _showUsuario,
        deleteUsuario: _deleteUsuario,
        getClientes: _getClientes,
        getTickets: _getTickets
    };
});