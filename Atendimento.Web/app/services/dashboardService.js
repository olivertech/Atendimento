app.factory("dashboardService", function($http, config) {
    
    /** Retorna os totais de todos os status */
    var _getTotals = function(idCliente) {
        return $http({
            method: "GET",
            url: config.baseUrl + "/Ticket/GetCounts?idCliente=" + idCliente,
            async: true,
            cache: false
        }); 
    }

    /** Retorna todos os tickets paginando */
    var _getTickets = function(offSet, numRows, orderBy, direction, idsStatusTicket, idCliente, idTicketFiltro, tituloFiltro, descricaoFiltro, dataInicialFiltro, dataFinalFiltro, idClienteFiltro, idCategoriaFiltro) {
        var filter = {
            "offset": offSet,
            "numrows": numRows,
            "orderBy": orderBy,
            "direction": direction,
            "idTicket": idTicketFiltro,
            "titulo": tituloFiltro,
            "descricao": descricaoFiltro,
            "dataInicial": dataInicialFiltro,
            "dataFinal": dataFinalFiltro,
            "idCliente": idClienteFiltro,
            "idCategoria": idCategoriaFiltro,
            "idsStatus": idsStatusTicket,
            "idClienteSession": idCliente
          };

        return $http({
            method: "POST",
            url: config.baseUrl + "/Ticket/GetAllPaged",
            data: filter,
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

    /** Retorna os usuarios associados a um cliente */
    var _getUsuarios = function(idCliente) {
        return $http({
            method: "GET",
            url: config.baseUrl + "/UsuarioCliente/GetAllById?idCliente=" + idCliente,
            async: true,
            cache: false
        });
    }

    /** Retorna todas as categorias */
    var _getCategorias = function() {
        return $http({
            method: "GET",
            url: config.baseUrl + "/Categoria/GetAll",
            async: true,
            cache: false
        });
    }

    /** Retorna todas as classificacoes */
    var _getClassificacoes = function() {
        return $http({
            method: "GET",
            url: config.baseUrl + "/Classificacao/GetAll",
            async: true,
            cache: false
        });
    }

    /** Salva um novo ticket */
    var _saveNewSupport = function(idUsuarioFiltro, idCategoriaFiltro, idClassificacaoFiltro, assuntoFiltro, descricaoFiltro, pathAnexos, tipoUsuario, idAtendente) {

        var newSupport = 
        {
            "idStatusTicket": 1,
            "idUsuarioCliente": idUsuarioFiltro,
            "idCategoria": idCategoriaFiltro,
            "idClassificacao": idClassificacaoFiltro,
            "titulo": assuntoFiltro,
            "descricao": descricaoFiltro,
            "pathAnexos": pathAnexos,
            "userTypeAgent": (tipoUsuario == "Atendimento" ? "S" : "C"),
            "idAtendente": idAtendente
        }

        return $http({
            method: "POST",
            url: config.baseUrl + "/Ticket/Insert",
            data: newSupport,
            async: true,
            cache: false
        });         
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
        getTickets: _getTickets,
        getClientes: _getClientes,
        getUsuarios: _getUsuarios,
        getCategorias: _getCategorias,
        getClassificacoes: _getClassificacoes,
        uploadFile: _uploadFile,
        saveNewSupport: _saveNewSupport,
        removeAttachments: _removeAttachments
    };
});