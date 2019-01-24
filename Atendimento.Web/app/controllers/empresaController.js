app.controller('empresaController', function($scope, $location, $sessionStorage, empresaService, paginationService, generalUtility) {

    $scope.isAdmin = $sessionStorage.isAdmin;

    $scope.offSet = 0;
    $scope.numRows = 20;
    $scope.totalRecords = 0;
    $scope.pagination = {};
    $scope.empresa = {};
    $scope.empresas = 0;
    $scope.mensagem = "";

    /** Função que recupera as empresas */
    $scope.getPage = function(page) {

        let offset = (page - 1) * $scope.numRows;

        empresaService.getEmpresas(offset, $scope.numRows)
            .then(function(response) {
                $scope.empresas = response.data.content;
                $scope.totalRecords = response.data.totalRecords;

                if (page == 1)
                    getPagination(1, $scope.numRows);
                else 
                    getPagination(page, $scope.numRows);
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao recuperar as empresas.";
                generalUtility.showErrorAlert();
            });
    }

    /** Inicializa os campos da modal de cadastro de empresa */
    $scope.newEmpresa = function() {
        $scope.empresa.id = 0;
        $scope.empresa.nome = "";
        $scope.empresa.email = "";
        $scope.empresa.telefoneFixo = "";
        $scope.empresa.telefoneCelular = "";
        $scope.empresa.descricao = "";
    }

    /** Função que salva empresa */
    $scope.saveEmpresa = function() {
        let id = $scope.empresa.id;
        let nome = $scope.empresa.nome;
        let email = $scope.empresa.email;
        let telefoneFixo = $scope.empresa.telefoneFixo;
        let telefoneCelular = $scope.empresa.telefoneCelular;
        let descricao = $scope.empresa.descricao;
        let isDefault = $scope.empresa.id == 1 ? 1 : 0;

        empresaService.saveEmpresa(id, nome, email, telefoneFixo, telefoneCelular, descricao, isDefault)
            .then(function(response) {
                $scope.mensagem = response.data.message;
                generalUtility.showSuccessAlert();
                $scope.getPage(1);
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao salvar a empresa.";
                generalUtility.showErrorAlert();
            });        
    }
    
    /** Função que carrega a modal com os dados da empresa */
    $scope.showEmpresa = function(idEmpresa) {
        empresaService.showEmpresa(idEmpresa)
            .then(function(response) {
                $scope.empresa.id = response.data.content.id;
                $scope.empresa.nome = response.data.content.nome;
                $scope.empresa.email = response.data.content.email;
                $scope.empresa.telefoneFixo = response.data.content.telefoneFixo;
                $scope.empresa.telefoneCelular = response.data.content.telefoneCelular;
                $scope.empresa.descricao = response.data.content.descricao;            
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao recuperar a empresa.";
                generalUtility.showErrorAlert();
            });   
    }
    
    /** Função que remove empresa */
    $scope.deleteEmpresa = function(idEmpresa) {
        if (confirm("Confirma remoção da empresa ?") == true) {
            
            empresaService.deleteEmpresa(idEmpresa)
            .then(function(response) {
                $scope.mensagem = response.data.message;
                generalUtility.showSuccessAlert();
                $scope.getPage(1);
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao remover a empresa.";
                generalUtility.showErrorAlert();
            }); 
        }  
    }

    $scope.listarAtendentes = function() {
        $location.path("/atendentes");
    }
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
    }
    
    /** Carrega as empresas */
    $scope.getPage(1);
});