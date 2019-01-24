app.controller('clienteController', function($scope, $sessionStorage, clienteService, paginationService, generalUtility) {

    $scope.isAdmin = $sessionStorage.isAdmin;

    $scope.offSet = 0;
    $scope.numRows = 20;
    $scope.totalRecords = 0;
    $scope.clientes = [];
    $scope.usuarios = 0;
    $scope.cliente = {};
    $scope.pagination = {};
    $scope.direction = 'ASC';

    /** Função que recupera os clientes */
    $scope.getPage = function(page, orderBy, direction) {

        let offset = (page - 1) * $scope.numRows;

        clienteService.getClientes(offset, $scope.numRows, orderBy, direction)
            .then(function(response) {
                $scope.clientes = response.data.content;
                $scope.totalRecords = response.data.totalRecords;

                if (page == 1)
                    getPagination(1, $scope.numRows);
                else 
                    getPagination(page, $scope.numRows);
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao recuperar os clientes.";
                generalUtility.showErrorAlert();
            });
    }

    /** Inicializa os campos da modal de cadastro de cliente */
    $scope.newCliente = function() {
        $scope.cliente.id = 0;
        $scope.cliente.nome = "";
        $scope.cliente.cnpj = "";
        $scope.cliente.email = "";
        $scope.cliente.telefoneFixo = "";
        $scope.cliente.telefoneCelular = "";
        $scope.cliente.logradouro = "";
        $scope.cliente.numeroLogradouro = "";
        $scope.cliente.complementoLogradouro = "";
        $scope.cliente.estado = "";
        $scope.cliente.cidade = "";
        $scope.cliente.bairro = "";
        $scope.cliente.cep = "";
        $scope.cliente.descricao = "";
        $scope.cliente.ativo = false;
        $scope.getEmpresas();
        $scope.cliente.idEmpresa = undefined;
    }

    /** Função que salva cliente */
    $scope.saveCliente = function() {
        let id = $scope.cliente.id;
        let idEmpresa = $scope.cliente.idEmpresa;
        let nome = $scope.cliente.nome;
        let cnpj = $scope.cliente.cnpj;
        let email = $scope.cliente.email;
        let telefoneFixo = $scope.cliente.telefoneFixo;
        let telefoneCelular = $scope.cliente.telefoneCelular;
        let logradouro = $scope.cliente.logradouro;
        let numeroLogradouro = $scope.cliente.numeroLogradouro;
        let complementoLogradouro = $scope.cliente.complementoLogradouro;
        let estado = $scope.cliente.estado;
        let cidade = $scope.cliente.cidade;
        let bairro = $scope.cliente.bairro;
        let cep = $scope.cliente.cep;
        let descricao = $scope.cliente.descricao;
        let ativo = $scope.cliente.ativo;

        clienteService.saveCliente(id, idEmpresa, nome, cnpj, email, telefoneFixo, telefoneCelular, logradouro, numeroLogradouro, complementoLogradouro, estado, cidade, bairro, cep, descricao, ativo)
            .then(function(response) {
                $scope.mensagem = response.data.message;
                generalUtility.showSuccessAlert();
                $scope.getPage(1, "id", $scope.direction);
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao salvar o cliente.";
                generalUtility.showErrorAlert();
            });        
    }
    
    /** Função que carrega a modal com os dados do cliente */
    $scope.showCliente = function(idCliente) {
        clienteService.showCliente(idCliente)
            .then(function(response) {
                $scope.cliente.id = response.data.content.id;
                $scope.cliente.idEmpresa = response.data.content.idEmpresa;
                $scope.cliente.nome = response.data.content.nome;
                $scope.cliente.cnpj = response.data.content.cnpj;
                $scope.cliente.email = response.data.content.email;
                $scope.cliente.telefoneFixo = response.data.content.telefoneFixo;
                $scope.cliente.telefoneCelular = response.data.content.telefoneCelular;
                $scope.cliente.logradouro = response.data.content.endereco != null ? response.data.content.endereco.logradouro : "";
                $scope.cliente.numeroLogradouro = response.data.content.endereco != null ? response.data.content.endereco.numeroLogradouro : "";
                $scope.cliente.complementoLogradouro = response.data.content.endereco != null ? response.data.content.endereco.complementoLogradouro : "";
                $scope.cliente.estado = response.data.content.endereco != null ? response.data.content.endereco.estado : "";
                $scope.cliente.cidade = response.data.content.endereco != null ? response.data.content.endereco.cidade : "";
                $scope.cliente.bairro = response.data.content.endereco != null ? response.data.content.endereco.bairro : "";
                $scope.cliente.cep = response.data.content.endereco != null ? response.data.content.endereco.cep : "";
                $scope.cliente.descricao = response.data.content.descricao;
                $scope.cliente.ativo = response.data.content.ativo;
                $scope.getEmpresas();
                $scope.getUsuariosCliente(response.data.content.id);
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao recuperar o cliente.";
                generalUtility.showErrorAlert();
            });   
    }
    
    /** Função que remove cliente */
    $scope.deleteCliente = function(idCliente) {
        if (confirm("Confirma remoção do cliente ?") == true) {
            
            clienteService.deleteCliente(idCliente)
            .then(function(response) {
                $scope.mensagem = response.data.message;
                generalUtility.showSuccessAlert();
                $scope.getPage(1, "id", $scope.direction);
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao remover o cliente.";
                generalUtility.showErrorAlert();
            }); 
        }  
    }

    /** Função que recupera todas as empresas */
    $scope.getEmpresas = function() {
        clienteService.getEmpresas()
            .then(function(response) {
                $scope.empresas = response.data.content;
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao recuperar a lista de empresas.";
                generalUtility.showErrorAlert(); 
            });
    }
    
    /** Função que retorna todos os usuários associados ao cliente */
    $scope.getUsuariosCliente = function(idCliente) {
        clienteService.getUsuariosCliente(idCliente)
            .then(function(response) {
                $scope.usuarios = response.data.content;
            })
            .catch(function(error) {
                generalUtility.showErrorAlert(); 
            });
    }

    /** Função que recupera a lista de clientes com determinada ordenação */
    $scope.ordenarPor = function(campo) {
        $scope.orderBy = campo;
        $scope.direction = $scope.direction === "ASC" ? "DESC" : "ASC";
        $scope.getPage(1, campo, $scope.direction);
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