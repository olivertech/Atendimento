app.factory("dashboardService", function($http, config) {
    
    /** Retorna os totais de todos os status */
    var _getTotals = function() {
        return $http.get(config.baseUrl + "/Ticket/GetCounts");
    }

    /** Retorna todos os tickets paginando */
    var _getPagedTicket = function(offSet, numRows, idsStatusTicket, idTicket, titulo, descricao, dataInicial, dataFinal, idCliente, idCategoria) {
        var filter = {
            "offset": offSet,
            "numrows": numRows,
            "idTicket": idTicket,
            "titulo": titulo,
            "descricao": descricao,
            "dataInicial": dataInicial,
            "dataFinal": dataFinal,
            "idCliente": idCliente,
            "idCategoria": idCategoria,
            "idsStatus": idsStatusTicket
          };

        return $http.post(config.baseUrl + "/Ticket/GetAllPaged", filter);
    }

    /** Retorna todos os clientes */
    var _getClientes = function() {
        return $http.get(config.baseUrl + "/Cliente/GetAll");
    }

    /** Retorna os usuarios associados a um cliente */
    var _getUsuarios = function(idCliente) {
        return $http.get(config.baseUrl + "/UsuarioCliente/GetAllById?idCliente=" + idCliente);
    }

    /** Retorna todas as categorias */
    var _getCategorias = function() {
        return $http.get(config.baseUrl + "/Categoria/GetAll");
    }

    /** Retorna todas as classificacoes */
    var _getClassificacoes = function() {
        return $http.get(config.baseUrl + "/Classificacao/GetAll");
    }

    /** Salva um novo ticket */
    var _saveNewSupport = function(idUsuario, idCategoria, idClassificacao, assunto, descricao, pathAnexos) {

        var newSupport = 
        {
            "idStatusTicket": 1,
            "idUsuarioCliente": idUsuario,
            "idCategoria": idCategoria,
            "idClassificacao": idClassificacao,
            "titulo": assunto,
            "descricao": descricao,
            "pathAnexos": pathAnexos
        }

        return $http.post(config.baseUrl + "/Ticket/Insert", newSupport);
    }

    /** Faz o upload de um arquivo */
    var _uploadFile = function(file, idUsuario) {
        var fd = new FormData();
        fd.append('file', file);

        return $http.post(config.baseUrl + "/FileUpload/Upload?idUsuario=" + idUsuario, fd, { transformRequest: angular.identity, headers: {'Content-Type': undefined}});
    }
    
    /** Faz a deleção da base, de todos os anexos que estiverem dentro da lista recebida */
    var _removeAttachments = function(ids) {
        var list = {ids};

        var req = {
            method: 'DELETE',
            url: config.baseUrl + "/Anexo/DeleteList",
            headers: {
              'Content-Type':'application/json'
            },
            data: ids
           }
           
           return $http(req);
    }

    return {
        getTotals: _getTotals,
        getPagedTicket: _getPagedTicket,
        getClientes: _getClientes,
        getUsuarios: _getUsuarios,
        getCategorias: _getCategorias,
        getClassificacoes: _getClassificacoes,
        uploadFile: _uploadFile,
        saveNewSupport: _saveNewSupport,
        removeAttachments: _removeAttachments
    };
});