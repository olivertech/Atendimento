app.controller('atendenteEmpresaController', function($scope, $sessionStorage, atendenteService, paginationService, generalUtility) {

    $scope.isAdmin = $sessionStorage.isAdmin;

    $scope.offSet = 0;
    $scope.numRows = 20;
    $scope.totalRecords = 0;
    $scope.pagination = {};
    $scope.atendente = {};
    $scope.atendentes = [];
    $scope.empresas = [];
    $scope.mensagem = "";

    /** Função que recupera os atendentes */
    $scope.getPage = function(page) {

        let offset = (page - 1) * $scope.numRows;

        atendenteService.getAtendentes(offset, $scope.numRows)
            .then(function(response) {
                $scope.atendentes = response.data.content;
                $scope.totalRecords = response.data.totalRecords;

                if (page == 1)
                    getPagination(1, $scope.numRows);
                else 
                    getPagination(page, $scope.numRows);
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao recuperar os atendentes.";
                generalUtility.showErrorAlert();
            });
    }

    /** Inicializa os campos da modal de cadastro de atendente */
    $scope.newAtendente = function() {
        $scope.atendente.id = 0;
        $scope.atendente.nome = "";
        $scope.atendente.username = "";
        $scope.atendente.password = "";
        $scope.atendente.email = "";
        $scope.atendente.telefoneFixo = "";
        $scope.atendente.telefoneCelular = "";
        $scope.atendente.ativo = 0;
        $scope.atendente.isAdmin = 0;
        $scope.atendente.copia = 0;
        $scope.atendente.provisorio = 0;
        $scope.getEmpresas();
        $scope.atendente.idEmpresa = undefined;
    }

    /** Função que salva atendente */
    $scope.saveAtendente = function() {
        let id = $scope.atendente.id;
        let idEmpresa = $scope.atendente.idEmpresa;
        let nome = $scope.atendente.nome;
        let username = $scope.atendente.username;
        let password = $scope.atendente.password;
        let email = $scope.atendente.email;
        let telefoneFixo = $scope.atendente.telefoneFixo;
        let telefoneCelular = $scope.atendente.telefoneCelular;
        let copia = $scope.atendente.copia;
        let provisorio = $scope.atendente.provisorio;
        let ativo = $scope.atendente.ativo;
        let isAdmin = $scope.atendente.isAdmin;
        let isDefault = $scope.atendente.id == 1 ? 1 : 0;

        atendenteService.saveAtendente(id, idEmpresa, nome, username, password, email, telefoneFixo, telefoneCelular, copia, provisorio, ativo, isAdmin, isDefault)
            .then(function(response) {
                $scope.mensagem = response.data.message;
                generalUtility.showSuccessAlert();
                $scope.getPage(1);
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao salvar o atendente.";
                generalUtility.showErrorAlert();
            });        
    }
    
    /** Função que carrega a modal com os dados do atendente */
    $scope.showAtendente = function(idAtendente) {
        atendenteService.showAtendente(idAtendente)
            .then(function(response) {
                $scope.atendente.id = response.data.content.id;
                $scope.atendente.idEmpresa = response.data.content.idEmpresa;
                $scope.atendente.nome = response.data.content.nome;
                $scope.atendente.username = response.data.content.username;
                $scope.atendente.password = response.data.content.password;
                $scope.atendente.email = response.data.content.email;
                $scope.atendente.telefoneFixo = response.data.content.telefoneFixo;
                $scope.atendente.telefoneCelular = response.data.content.telefoneCelular;
                $scope.atendente.copia = response.data.content.copia;
                $scope.atendente.provisorio = response.data.content.provisorio;
                $scope.atendente.ativo = response.data.content.ativo;
                $scope.atendente.isAdmin = response.data.content.isAdmin;
                
                $scope.getEmpresas();
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao recuperar a empresa.";
                generalUtility.showErrorAlert();
            });   
    }
    
    /** Função que remove atendente */
    $scope.deleteAtendente = function(idAtendente) {
        if (confirm("Confirma remoção do atendente ?") == true) {
            
            atendenteService.deleteAtendente(idAtendente)
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
    
    /** Função que recupera todas as empresas */
    $scope.getEmpresas = function() {
        atendenteService.getEmpresas()
            .then(function(response) {
                $scope.empresas = response.data.content;
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao lista as empresas.";
                generalUtility.showErrorAlert(); 
            });
    }
    
    /** Função que gera senha randomicamente */
    $scope.gerarPwd = function()
    {
        let pwd = generalUtility.randomPwd();
        $scope.atendente.password = pwd;
    }

    /** Função que alterna o type do campo de senha */
    $scope.showHidePwd = function(field) {
        generalUtility.showHidePwd(field);
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