app.factory("ticketService", function($http, config) {
    
    /** Recupera os detalhes do ticket */
    var _getTicket = function(idTicket) {
        //return $http.get(config.baseUrl + "/Ticket/GetById?id=" + idTicket);
        var ticket = 
        {
            "id": idTicket,
            "withAnexos": true
        }
        
        return $http({
            method: "POST",
            url: config.baseUrl + "/Ticket/GetById?id=" + idTicket,
            data: ticket,
            async: true,
            cache: false
        });
    }

    var _getMessages = function(idTicket, idCliente) {
        //return $http.get(config.baseUrl + "/TicketMensagem/GetAllByTicketId?idTicket=" + idTicket);
        var filter = 
        {
            "idTicket": idTicket,
            "idCliente": idCliente
        }

        return $http({
            method: "POST",
            url: config.baseUrl + "/TicketMensagem/GetAllByTicketId",
            data: filter,
            async: true,
            cache: false
        });
    }

    var _getTemplates = function() {
        //return $http.get(config.baseUrl + "/TemplateResposta/GetAll");
        return $http({
            method: "GET",
            url: config.baseUrl + "/TemplateResposta/GetAll",
            async: true,
            cache: false
        });        
    }

    var _updateStatusTicket = function(idTicket, idStatusTicket, idUsuarioCliente, tipoUsuarioAgente, idAtendente) {
        
        var request = 
        {
            "id": idTicket,
            "idStatusTicket": idStatusTicket,
            "idUsuarioCliente": idUsuarioCliente,
            "userTypeAgent": (tipoUsuarioAgente == "Atendimento" ? "S" : "C"),
            "idAtendente": (tipoUsuarioAgente == "Atendimento" ? idAtendente : 0)
        }

        return $http.put(config.baseUrl + "/Ticket/UpdateStatusTicket", request);
    }

    var _updateClassificacao = function(idTicket, idClassificacao, idUsuarioCliente, tipoUsuarioAgente, idAtendente) {
        
        var request = 
        {
            "id": idTicket,
            "idClassificacao": idClassificacao,
            "idUsuarioCliente": idUsuarioCliente,
            "userTypeAgent": (tipoUsuarioAgente == "Atendimento" ? "S" : "C"),
            "idAtendente": (tipoUsuarioAgente == "Atendimento" ? idAtendente : 0)
        }

        return $http.put(config.baseUrl + "/Ticket/UpdateClassificacao", request);
    }

    var _saveNewMessage = function(idTicket, idUsuario, tipoUsuarioAgente, descricao, interno, pathAnexos) {

        var newMessage = 
        {
            "idTicket": idTicket,
            "idAutor": idUsuario,
            "userType": (tipoUsuarioAgente == "Atendimento" ? "S" : "C"),
            "descricao": descricao,
            "interno": interno,
            "descricao": descricao,
            "pathAnexos": pathAnexos
        }

        return $http.post(config.baseUrl + "/TicketMensagem/Insert", newMessage);

    }

    return {
        getTicket: _getTicket,
        getMessages: _getMessages,
        updateStatusTicket: _updateStatusTicket,
        updateClassificacao: _updateClassificacao,
        getTemplates: _getTemplates,
        saveNewMessage: _saveNewMessage
    };    
});