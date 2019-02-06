app.controller('templateController', ['$scope', '$sessionStorage', 'templateService', 'paginationService', 'generalUtility',
    function($scope, $sessionStorage, templateService, paginationService, generalUtility) {

    $scope.isAdmin = $sessionStorage.isAdmin;

    $scope.offSet = 0;
    $scope.numRows = 20;
    $scope.totalRecords = 0;
    $scope.templates = [];
    $scope.template = {};
    $scope.pagination = {};
    $scope.direction = 'ASC';

    /** Função que controla a paginação da grid de tickets */
    $scope.getPage = function(page, orderBy, direction) {

        var offset = (page - 1) * $scope.numRows;

        templateService.getPagedTemplates(offset, $scope.numRows, orderBy, direction)
            .then(function(response) {
                $scope.templates = response.data.content;
                $scope.totalRecords = response.data.totalRecords;

                if (page == 1)
                    getPagination(1, $scope.numRows);
                else 
                    getPagination(page, $scope.numRows);
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao recuperar os templates.";
                generalUtility.showErrorAlert();
            });
    };

    /** Função que inicializa os campos da modal de cadastro de novo template */
    $scope.newTemplate = function() {
        $scope.template.id = 0;
        $scope.template.titulo = '';
        $scope.template.descricao = '';
        $scope.template.conteudo = '';
    };

    /** Função que salva novo template */
    $scope.saveTemplate = function() {
        var id = $scope.template.id;
        var titulo = $scope.template.titulo;
        var descricao = $scope.template.descricao;
        var conteudo = $scope.template.conteudo;

        templateService.saveTemplate(id, titulo, descricao, conteudo)
            .then(function(response) {
                $scope.mensagem = response.data.message;
                $scope.templates = response.data.content;
                $scope.totalRecords = response.data.totalRecords;
                generalUtility.showSuccessAlert();
                $scope.getPage(1, "id", $scope.direction);
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao salvar o template.";
                generalUtility.showErrorAlert();
            });        
    };

    /** Função que remove um template */
    $scope.deleteTemplate = function(idTemplate) {
        if (confirm("Confirma remoção do template ?") == true) {
         
            templateService.deleteTemplate(idTemplate)
            .then(function(response) {
                $scope.mensagem = response.data.message;
                generalUtility.showSuccessAlert();
                $scope.getPage(1, "id", $scope.direction);
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao remover o template.";
                generalUtility.showErrorAlert();
            }); 
        }  
    };

    /** Função que carrega a modal com os dados do template selecionado */
    $scope.showTemplate = function(idTemplate) {
        templateService.showTemplate(idTemplate)
        .then(function(response) {
            $scope.template.id = response.data.content.id;
            $scope.template.titulo = response.data.content.titulo;
            $scope.template.descricao = response.data.content.descricao;
            $scope.template.conteudo = response.data.content.conteudo;
        })
        .catch(function(error) {
            $scope.mensagem = "Ocorreu um erro ao recuperar o template.";
            generalUtility.showErrorAlert();
        });   
    };

    /** Função que recupera a lista de templates com determinada ordenação */
    $scope.ordenarPor = function(campo) {
        $scope.orderBy = campo;
        $scope.direction = $scope.direction === "ASC" ? "DESC" : "ASC";
        $scope.getPage(1, campo, $scope.direction);
    };

    /**
     *  INTERNAL FUNCTIONS
     */

    /** Função interna que prepara a paginação da grid de tickets */
    var getPagination = function(page, pageSize) {
        if ((page < 1 || page > $scope.pagination.totalPages) && $scope.pagination.totalPages > 0) {
            return;
        }

        // get pager object from service
        $scope.pagination = paginationService.getPagination($scope.totalRecords, page, pageSize);
    };
    
    /** Carrega a primeira página de tickets */
    $scope.getPage(1, "id", $scope.direction);
}]);