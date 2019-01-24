app.factory("templateService", function($http, config) {
    
    /** Retorna todos os templates paginando */
    var _getPagedTemplates = function(offSet, numRows, orderBy, direction) {
        var filter = {
            "offset": offSet,
            "numrows": numRows,
            "orderBy": orderBy,
            "direction": direction
        };

        return $http({
            method: "POST",
            url: config.baseUrl + "/TemplateResposta/GetAllPaged",
            data: filter,
            async: true,
            cache: false
        });  
    }

    var _saveTemplate = function(id, titulo, descricao, conteudo) {
        let request = {
            "titulo": titulo,
            "descricao": descricao,
            "conteudo": conteudo
        };

        let method = '';
        let url = config.baseUrl + "/TemplateResposta/";
        
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

    var _deleteTemplate = function(idTemplate) {
        return $http({
            method: "DELETE",
            url: config.baseUrl + "/TemplateResposta/Delete?id=" + idTemplate,
            async: true,
            cache: false
        });
    }

    var _showTemplate = function(idTemplate) {
        return $http({
            method: "GET",
            url: config.baseUrl + "/TemplateResposta/GetById?id=" + idTemplate,
            async: true,
            cache: false
        });
    }

    return {
        getPagedTemplates: _getPagedTemplates,
        saveTemplate: _saveTemplate,
        deleteTemplate: _deleteTemplate,
        showTemplate: _showTemplate
    };
});