app.factory("ticketService", function($http, config) {
    
    /** Recupera os detalhes do ticket */
    var _getTicket = function(idTicket) {
        return $http.get(config.baseUrl + "/Ticket/GetById?id=" + idTicket);
    }

    var _getMessages = function(idTicket) {
        return $http.get(config.baseUrl + "/TicketMensagem/GetAllByTicketId?idTicket=" + idTicket);
    }

    var _getTemplates = function() {
        return $http.get(config.baseUrl + "/TemplateResposta/GetAll");
    }

    var _updateStatusTicket = function(idTicket, idStatusTicket) {
        
        var request = 
        {
            "id": idTicket,
            "idStatusTicket": idStatusTicket            
        }

        return $http.put(config.baseUrl + "/Ticket/UpdateStatusTicket", request);
    }

    var _saveNewMessage = function(idTicket, idUsuario, tipoUsuario, descricao, interno, pathAnexos) {

        var newMessage = 
        {
            "idTicket": idTicket,
            "idAutor": idUsuario,
            "tipoUsuario": tipoUsuario,
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
        getTemplates: _getTemplates,
        saveNewMessage: _saveNewMessage
    };    
});