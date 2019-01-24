app.controller('usuarioController', function($scope, $sessionStorage, usuarioService, paginationService, generalUtility) {

    $scope.isAdmin = $sessionStorage.isAdmin;

    $scope.offSet = 0;
    $scope.numRows = 20;
    $scope.totalRecords = 0;
    $scope.tickets = 0;
    $scope.clientes = [];
    $scope.usuarios = [];
    $scope.usuario = {};
    $scope.pagination = {};
    $scope.direction = 'ASC';

    /** Função que recupera os usuarios */
    $scope.getPage = function(page, orderBy, direction) {

        let offset = (page - 1) * $scope.numRows;

        usuarioService.getUsuarios(offset, $scope.numRows, orderBy, direction)
            .then(function(response) {
                $scope.usuarios = response.data.content;
                $scope.totalRecords = response.data.totalRecords;

                if (page == 1)
                    getPagination(1, $scope.numRows);
                else 
                    getPagination(page, $scope.numRows);
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao recuperar os usuários.";
                generalUtility.showErrorAlert();
            });
    }

    /** Inicializa os campos da modal de cadastro de usuario */
    $scope.newUsuario = function() {
        $scope.usuario.id = 0;
        $scope.usuario.nome = "";
        $scope.usuario.username = "";
        $scope.usuario.password = "";
        $scope.usuario.email = "";
        $scope.usuario.telefoneFixo = "";
        $scope.usuario.telefoneCelular = "";
        $scope.usuario.copia = false;
        $scope.usuario.provisorio = false;
        $scope.usuario.ativo = false;
        $scope.getClientes();
        $scope.usuario.idCliente = undefined;
    }

    /** Função que salva usuario  */
    $scope.saveUsuario = function() {
        let id = $scope.usuario.id;
        let idCliente = $scope.usuario.idCliente;
        let nome = $scope.usuario.nome;
        let username = $scope.usuario.username;
        let password = $scope.usuario.password;
        let email = $scope.usuario.email;
        let telefoneFixo = $scope.usuario.telefoneFixo;
        let telefoneCelular = $scope.usuario.telefoneCelular;
        let copia = $scope.usuario.copia;
        let provisorio = $scope.usuario.provisorio;
        let ativo = $scope.usuario.ativo;

        usuarioService.saveUsuario(id, idCliente, nome, username, password, email, telefoneFixo, telefoneCelular, copia, provisorio, ativo)
            .then(function(response) {
                $scope.mensagem = response.data.message;
                generalUtility.showSuccessAlert();
                $scope.getPage(1, "id", $scope.direction);
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao salvar o usuário.";
                generalUtility.showErrorAlert();
            });        
    }
    
    /** Função que carrega a modal com os dados do usuario */
    $scope.showUsuario = function(idUsuario) {
        usuarioService.showUsuario(idUsuario)
            .then(function(response) {
                $scope.usuario.id = response.data.content.id;
                $scope.usuario.idCliente = response.data.content.idCliente;
                $scope.usuario.nome = response.data.content.nome;
                $scope.usuario.username = response.data.content.username;
                $scope.usuario.password = response.data.content.password;
                $scope.usuario.email = response.data.content.email;
                $scope.usuario.telefoneFixo = response.data.content.telefoneFixo;
                $scope.usuario.telefoneCelular = response.data.content.telefoneCelular;
                $scope.usuario.copia = response.data.content.copia;
                $scope.usuario.provisorio = response.data.content.provisorio;
                $scope.usuario.ativo = response.data.content.ativo;
                $scope.getClientes();
                $scope.getTickets(response.data.content.id);
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao recuperar o usuário.";
                generalUtility.showErrorAlert();
            });   
    }
    
    /** Função que remove usuario */
    $scope.deleteUsuario = function(idUsuario) {
        if (confirm("Confirma remoção do usuário ?") == true) {
            
            usuarioService.deleteUsuario(idUsuario)
            .then(function(response) {
                $scope.mensagem = response.data.message;
                generalUtility.showSuccessAlert();
                $scope.getPage(1, "id", $scope.direction);
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao remover o usuário.";
                generalUtility.showErrorAlert();
            }); 
        }  
    }

    /** Função que recupera todos os clientes */
    $scope.getClientes = function() {
        usuarioService.getClientes()
            .then(function(response) {
                $scope.clientes = response.data.content;
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao recuperar a lista de clientes.";
                generalUtility.showErrorAlert(); 
            });
    }
    
    /** Função que retorna o total de tickets associados ao usuario */
    $scope.getTickets = function(idUsuario) {
        usuarioService.getTickets(idUsuario)
            .then(function(response) {
                $scope.tickets = response.data.content;
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao recuperar a lista de tickets.";
                generalUtility.showErrorAlert(); 
            });
    }

    /** Função que recupera a lista de clientes com determinada ordenação */
    $scope.ordenarPor = function(campo) {
        $scope.orderBy = campo;
        $scope.direction = $scope.direction === "ASC" ? "DESC" : "ASC";
        $scope.getPage(1, campo, $scope.direction);
    }
    
    /** Função que gera senha randomicamente */
    $scope.gerarPwd = function()
    {
        let pwd = generalUtility.randomPwd();
        $scope.usuario.password = pwd;
    }

    /** Função que alterna o type do campo de senha */
    $scope.showHidePwd = function(field) {
        generalUtility.showHidePwd(field);
    }
    
    /**
     *  INTERNAL FUNCTIONS
     */

    /** Função interna que prepara a paginação da grid de clientes */
    var getPagination = function(page, pageSize) {
        if ((page < 1 || page > $scope.pagination.totalPages) && $scope.pagination.totalPages > 0) {
            return;
        }

        // get pager object from service
        $scope.pagination = paginationService.getPagination($scope.totalRecords, page, pageSize);
    }
    
    /** Carrega os clientes */
    $scope.getPage(1, "id", $scope.direction);
});