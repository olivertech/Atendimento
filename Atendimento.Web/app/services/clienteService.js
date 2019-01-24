app.factory("clienteService", function($http, config) {
    
    /** Retorna todos os clientes */
    var _getClientes = function(offSet, numRows, orderBy, direction) {
        var filter = {
            "offset": offSet,
            "numrows": numRows,
            "orderBy": orderBy,
            "direction": direction
        };

        return $http({
            method: "POST",
            url: config.baseUrl + "/Cliente/GetAllPaged",
            data: filter,
            async: true,
            cache: false
        });  
    }

    /** Salva o clientes */
    var _saveCliente = function(id, idEmpresa, nome, cnpj, email, telefoneFixo, telefoneCelular, logradouro, numeroLogradouro, complementoLogradouro, estado, cidade, bairro, cep, descricao, ativo) {
        let request = {
            "idEmpresa": idEmpresa,
            "nome": nome,
            "cnpj": cnpj,
            "email": email,
            "telefoneFixo": telefoneFixo,
            "telefoneCelular": telefoneCelular,
            "endereco": {
              "logradouro": logradouro,
              "numeroLogradouro": numeroLogradouro,
              "complementoLogradouro": complementoLogradouro,
              "estado": estado,
              "cidade": cidade,
              "bairro": bairro,
              "cep": cep,
            },
            "descricao": descricao,
            "ativo": ativo
        };

        let method = '';
        let url = config.baseUrl + "/Cliente/";
        
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

    /** Recupera o cliente */
    var _showCliente = function(idCliente) {
        return $http({
            method: "GET",
            url: config.baseUrl + "/Cliente/GetById?id=" + idCliente,
            async: true,
            cache: false
        });
    }

    /** Remove cliente */
    var _deleteCliente = function(idCliente) {
        return $http({
            method: "DELETE",
            url: config.baseUrl + "/Cliente/Delete?id=" + idCliente,
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
    
    /** Retorna o total de usuarios associados ao cliente */
    var _getUsuariosCliente = function(idCliente) {
        return $http({
            method: "GET",
            url: config.baseUrl + "/UsuarioCliente/GetCount?idCliente=" + idCliente,
            async: true,
            cache: false
        });
    }
    
    return {
        getClientes: _getClientes,
        saveCliente: _saveCliente,
        showCliente: _showCliente,
        deleteCliente: _deleteCliente,
        getEmpresas: _getEmpresas,
        getUsuariosCliente: _getUsuariosCliente
    };
});