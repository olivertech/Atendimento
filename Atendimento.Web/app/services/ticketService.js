app.factory("ticketService", ['$http', 'config',
    function($http, config) {
    
    /** Recupera os detalhes do ticket */
    var _getTicket = function(idTicket) {

        var ticket = 
        {
            "id": idTicket,
            "withAnexos": true
        };
        
        return $http({
            method: "POST",
            url: config.baseUrl + "/Ticket/GetById?id=" + idTicket,
            data: ticket,
            async: true,
            cache: false
        });
    };

    /** Recupera as mensagens associadas ao ticket */
    var _getMessages = function(idTicket, idCliente) {

        var filter = 
        {
            "idTicket": idTicket,
            "idCliente": idCliente
        };

        return $http({
            method: "POST",
            url: config.baseUrl + "/TicketMensagem/GetAllByTicketId",
            data: filter,
            async: true,
            cache: false
        });
    };

    /** Recuper a lista de templates de resposta */
    var _getTemplates = function() {

        return $http({
            method: "GET",
            url: config.baseUrl + "/TemplateResposta/GetAll",
            async: true,
            cache: false
        });        
    };

    /** Atualiza o status do ticket */
    var _updateStatusTicket = function(idTicket, idStatusTicket, idUsuarioCliente, tipoUsuarioAgente, idAtendente) {
        
        var request = 
        {
            "id": idTicket,
            "idStatusTicket": idStatusTicket,
            "idUsuarioCliente": idUsuarioCliente,
            "userTypeAgent": (tipoUsuarioAgente == "Atendimento" ? "S" : "C"),
            "idAtendente": (tipoUsuarioAgente == "Atendimento" ? idAtendente : 0)
        };

        return $http.put(config.baseUrl + "/Ticket/UpdateStatusTicket", request);
    };

    /** Atualiza a classificação do ticket */
    var _updateClassificacao = function(idTicket, idClassificacao, idUsuarioCliente, tipoUsuarioAgente, idAtendente) {
        
        var request = 
        {
            "id": idTicket,
            "idClassificacao": idClassificacao,
            "idUsuarioCliente": idUsuarioCliente,
            "userTypeAgent": (tipoUsuarioAgente == "Atendimento" ? "S" : "C"),
            "idAtendente": (tipoUsuarioAgente == "Atendimento" ? idAtendente : 0)
        };

        return $http.put(config.baseUrl + "/Ticket/UpdateClassificacao", request);
    };

    /** Salva nova mensagem */
    var _saveNewMessage = function(idTicket, idUsuario, tipoUsuarioAgente, descricao, idStatusTicket, interno, pathAnexos) {

        var newMessage = 
        {
            "idTicket": idTicket,
            "idAutor": idUsuario,
            "userType": (tipoUsuarioAgente == "Atendimento" ? "S" : "C"),
            "descricao": descricao,
            "interno": interno,
            "idStatusTicket": idStatusTicket,
            "pathAnexos": pathAnexos
        };

        return $http.post(config.baseUrl + "/TicketMensagem/Insert", newMessage);

    };

    return {
        getTicket: _getTicket,
        getMessages: _getMessages,
        updateStatusTicket: _updateStatusTicket,
        updateClassificacao: _updateClassificacao,
        getTemplates: _getTemplates,
        saveNewMessage: _saveNewMessage
    };    
}]);