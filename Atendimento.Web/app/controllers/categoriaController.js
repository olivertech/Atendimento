app.controller('categoriaController', ['$scope', '$sessionStorage', 'categoriaService', 'paginationService', 'generalUtility',
    function($scope, $sessionStorage, categoriaService, paginationService, generalUtility) {

    $scope.isAdmin = $sessionStorage.isAdmin;

    $scope.offSet = 0;
    $scope.numRows = 5;
    $scope.totalRecords = 0;
    $scope.categorias = [];
    $scope.categoria = {};
    $scope.pagination = {};
    $scope.direction = 'ASC';
    $scope.tickets = 1;

    /** Função que controla a paginação da grid */
    $scope.getPage = function(page, orderBy, direction) {

        var offset = (page - 1) * $scope.numRows;

        categoriaService.getPagedCategorias(offset, $scope.numRows, orderBy, direction)
            .then(function(response) {
                $scope.categorias = response.data.content;
                $scope.totalRecords = response.data.totalRecords;

                if (page == 1)
                    getPagination(1, $scope.numRows);
                else 
                    getPagination(page, $scope.numRows);
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao recuperar as categorias.";
                generalUtility.showErrorAlert();
            });
    };

    /** Função que inicializa os campos da modal de cadastro de nova categoria */
    $scope.newCategoria = function() {
        $scope.categoria.id = 0;
        $scope.categoria.nome = '';
    };

    /** Função que salva nova categoria */
    $scope.saveCategoria = function() {
        var id = $scope.categoria.id;
        var nome = $scope.categoria.nome;

        categoriaService.saveCategoria(id, nome)
            .then(function(response) {
                $scope.mensagem = response.data.message;
                $scope.categorias = response.data.content;
                $scope.totalRecords = response.data.totalRecords;
                generalUtility.showSuccessAlert();
                $scope.getPage(1, "id", $scope.direction);
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao salvar a categoria.";
                generalUtility.showErrorAlert();
            });        
    };

    /** Função que remove uma categoria */
    $scope.deleteCategoria = function(idCategoria) {
        if (confirm("Confirma remoção da categoria ?") == true) {
         
            categoriaService.deleteCategoria(idCategoria)
            .then(function(response) {
                $scope.mensagem = response.data.message;
                generalUtility.showSuccessAlert();
                $scope.getPage(1, "id", $scope.direction);
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao remover a categoria.";
                generalUtility.showErrorAlert();
            }); 
        }  
    };

    /** Função que carrega a modal com os dados da categoria selecionada */
    $scope.showCategoria = function(idCategoria) {
        $scope.getTickets(idCategoria);
        categoriaService.showCategoria(idCategoria)
        .then(function(response) {
            $scope.categoria.id = response.data.content.id;
            $scope.categoria.nome = response.data.content.nome;
        })
        .catch(function(error) {
            $scope.mensagem = "Ocorreu um erro ao recuperar a categoria.";
            generalUtility.showErrorAlert();
        });   
    };

    /** Função que recupera a lista de categorias com determinada ordenação */
    $scope.ordenarPor = function(campo) {
        $scope.orderBy = campo;
        $scope.direction = $scope.direction === "ASC" ? "DESC" : "ASC";
        $scope.getPage(1, campo, $scope.direction);
    };

    /** Função que retorna o total de tickets associados a categoria */
    $scope.getTickets = function(idCategoria) {
        categoriaService.getTickets(idCategoria)
            .then(function(response) {
                $scope.tickets = response.data.content;
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao recuperar a lista de tickets.";
                generalUtility.showErrorAlert(); 
            });
    };
    
    /**
     *  INTERNAL FUNCTIONS
     */

    /** Função interna que prepara a paginação da grid de categorias */
    var getPagination = function(page, pageSize) {
        if ((page < 1 || page > $scope.pagination.totalPages) && $scope.pagination.totalPages > 0) {
            return;
        }

        // get pager object from service
        $scope.pagination = paginationService.getPagination($scope.totalRecords, page, pageSize);
    };
    
    /** Carrega a primeira página de categorias */
    $scope.getPage(1, "id", $scope.direction);
}]);