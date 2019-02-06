app.controller('clienteController', ['$scope', '$sessionStorage', 'clienteService', 'paginationService', 'generalUtility',
    function($scope, $sessionStorage, clienteService, paginationService, generalUtility) {

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

        var offset = (page - 1) * $scope.numRows;

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
    };

    /** Inicializa os campos da modal de cadastro de cliente */
    $scope.newCliente = function() {
        $scope.getEmpresas();
        $scope.getEstados();
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
    };

    /** Função que salva cliente */
    $scope.saveCliente = function() {
        var id = $scope.cliente.id;
        var idEmpresa = $scope.cliente.idEmpresa;
        var nome = $scope.cliente.nome;
        var cnpj = $scope.cliente.cnpj != "" ? $scope.cliente.cnpj : null;
        var email = $scope.cliente.email;
        var telefoneFixo = $scope.cliente.telefoneFixo != "" ? $scope.cliente.telefoneFixo : null;
        var telefoneCelular = $scope.cliente.telefoneCelular != "" ? $scope.cliente.telefoneCelular : null;
        var logradouro = $scope.cliente.logradouro != "" ? $scope.cliente.logradouro : null;
        var numeroLogradouro = $scope.cliente.numeroLogradouro != "" ? $scope.cliente.numeroLogradouro : null;
        var complementoLogradouro = $scope.cliente.complementoLogradouro != "" ? $scope.cliente.complementoLogradouro : null;
        var estado = $scope.cliente.estado != "" ? $scope.cliente.estado : null;
        var cidade = $scope.cliente.cidade != "" ? $scope.cliente.cidade : null;
        var bairro = $scope.cliente.bairro != "" ? $scope.cliente.bairro : null;
        var cep = $scope.cliente.cep != "" ? $scope.cliente.cep : null;
        var descricao = $scope.cliente.descricao;
        var ativo = $scope.cliente.ativo;

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
    };
    
    /** Função que carrega a modal com os dados do cliente */
    $scope.showCliente = function(idCliente) {
        clienteService.showCliente(idCliente)
            .then(function(response) {
                $scope.getEmpresas(response.data.content.idEmpresa);
                $scope.getEstados();
                $scope.getUsuariosCliente(response.data.content.id);
                $scope.cliente.id = response.data.content.id;
                $scope.cliente.idEmpresa = response.data.content.idEmpresa;
                $scope.cliente.nome = response.data.content.nome;
                $scope.cliente.cnpj = response.data.content.cnpj;
                $scope.cliente.email = response.data.content.email;
                $scope.cliente.telefoneFixo = response.data.content.telefoneFixo;
                $scope.cliente.telefoneCelular = response.data.content.telefoneCelular;
                $scope.cliente.logradouro = response.data.content.logradouro != null ? response.data.content.logradouro : "";
                $scope.cliente.numeroLogradouro = response.data.content.numeroLogradouro != null ? response.data.content.numeroLogradouro : "";
                $scope.cliente.complementoLogradouro = response.data.content.complementoLogradouro != null ? response.data.content.complementoLogradouro : "";
                $scope.cliente.estado = response.data.content.estado != null ? response.data.content.estado : "";
                $scope.cliente.cidade = response.data.content.cidade != null ? response.data.content.cidade : "";
                $scope.cliente.bairro = response.data.content.bairro != null ? response.data.content.bairro : "";
                $scope.cliente.cep = response.data.content.cep != null ? response.data.content.cep : "";
                $scope.cliente.descricao = response.data.content.descricao;
                $scope.cliente.ativo = response.data.content.ativo;
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao recuperar o cliente.";
                generalUtility.showErrorAlert();
            });   
    };
    
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
    };

    /** Função que recupera todas as empresas */
    $scope.getEmpresas = function(idEmpresa) {
        clienteService.getEmpresas()
            .then(function(response) {
                var defaultOption = {id: "", nome: 'Selecione a Empresa Responsável pelo Atendimento'}
                var lista = response.data.content;
                $scope.empresas = lista.sort();
                $scope.empresas.unshift(defaultOption);
                $scope.cliente.idEmpresa = idEmpresa != undefined ? idEmpresa : "";
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao recuperar a lista de empresas.";
                generalUtility.showErrorAlert(); 
            });
    };
    
    /** Função que alimenta a lista de estados */
    $scope.getEstados = function() {
        var estados = [
            {id:"",nome: "Selecione o estado"},
            {id:"AL",nome:"Alagoas"},
            {id:"AP",nome:"Amapá"},
            {id:"AM",nome:"Amazonas"},
            {id:"BA",nome:"Bahia"},
            {id:"CE",nome:"Ceará"},
            {id:"DF",nome:"Distrito Federal"},
            {id:"ES",nome:"Espírito Santo"},
            {id:"GO",nome:"Goiás"},
            {id:"MA",nome:"Maranhão"},
            {id:"MT",nome:"Mato Grosso"},
            {id:"MS",nome:"Mato Grosso do Sul"},
            {id:"MG",nome:"Minas Gerais"},
            {id:"PA",nome:"Pará"},
            {id:"PB",nome:"Paraíba"},
            {id:"PR",nome:"Paraná"},
            {id:"PE",nome:"Pernambuco"},
            {id:"PI",nome:"Piauí"},
            {id:"RJ",nome:"Rio de Janeiro"},
            {id:"RN",nome:"Rio Grande do Norte"},
            {id:"RS",nome:"Rio Grande do Sul"},
            {id:"RO",nome:"Rondônia"},
            {id:"RR",nome:"Roraima"},
            {id:"SC",nome:"Santa Catarina"},
            {id:"SP",nome:"São Paulo"},
            {id:"SE",nome:"Sergipe"},
            {id:"TO",nome:"Tocantins"},
            {id:"ES",nome:"Estrangeiro"}
        ];
        $scope.estados = estados;
        $scope.cliente.estado = "";
    };

    /** Função que retorna todos os usuários associados ao cliente */
    $scope.getUsuariosCliente = function(idCliente) {
        clienteService.getUsuariosCliente(idCliente)
            .then(function(response) {
                $scope.usuarios = response.data.content;
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao recuperar os usuários associados ao cliente.";
                generalUtility.showErrorAlert(); 
            });
    };

    /** Função que recupera a lista de clientes com determinada ordenação */
    $scope.ordenarPor = function(campo) {
        $scope.orderBy = campo;
        $scope.direction = $scope.direction === "ASC" ? "DESC" : "ASC";
        $scope.getPage(1, campo, $scope.direction);
    };
    
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
    };
    
    /** Carrega os clientes */
    $scope.getPage(1, "id", $scope.direction);
}]);