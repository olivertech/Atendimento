app.controller('dashboardController', ['$scope', '$sessionStorage', '$location', '$q', 'dashboardService', 'paginationService', 'generalUtility', 'config', 
    function($scope, $sessionStorage, $location, $q, dashboardService, paginationService, generalUtility, config) {

    var file1Upload, file2Upload, file3Upload, file4Upload, file5Upload;

    $scope.idUsuario = $sessionStorage.idUsuario;
    $scope.usuario = $sessionStorage.usuario;
    $scope.isAdmin = $sessionStorage.isAdmin;
    $scope.idCliente = $sessionStorage.idCliente;
    $scope.nomeCliente = $sessionStorage.nomeCliente;
    
    $scope.token = $sessionStorage.tokenAuthentication;
    $scope.totalAtendimentos = 0;
    $scope.totalAguardando = 0;
    $scope.totalPendente = 0;
    $scope.totalEmAnalise = 0;
    $scope.totalConcluido = 0;
    $scope.totalCancelado = 0;

    $scope.offSet = 0;
    $scope.numRows = 20;
    $scope.idsStatusTicket = "1,4,5";
    $scope.totalRecords = 0;

    $scope.mensagem = "";
    $scope.uploadSuccess = false;
    $scope.uploadError = false;
    $scope.newTicketSuccess = false;
    $scope.newTicketError = false;
    $scope.pagination = {};
    $scope.filtro = {};
    $scope.atendimento = {};
    $scope.cliente = {};
    $scope.clientes = [];
    $scope.qtdeanexos = 0;

    $scope.orderBy = "";
    $scope.direction = "";

    /** Função que controla a paginação da grid de tickets */
    $scope.getPage = function(page, orderBy, direction) {

        var offset = (page - 1) * $scope.numRows;
        var idTicketFiltro = "";
        var tituloFiltro = "";
        var descricaoFiltro = "";
        var dataInicialFiltro = "";
        var dataFinalFiltro = "";
        var idClienteFiltro = 0;
        var idCategoriaFiltro = 0;

        if ($scope.filtro != undefined) {
            idTicketFiltro = $scope.filtro.idTicket;
            tituloFiltro = $scope.filtro.titulo;
            descricaoFiltro = $scope.filtro.descricao;
            dataInicialFiltro = $scope.filtro.dataInicial;
            dataFinalFiltro = $scope.filtro.dataFinal;
            idClienteFiltro = $scope.filtro.idCliente;
            idCategoriaFiltro = $scope.filtro.idCategoria;
        }

        dashboardService.getTickets(offset, $scope.numRows, orderBy, direction, $scope.idsStatusTicket, $scope.idCliente, idTicketFiltro, tituloFiltro, descricaoFiltro, dataInicialFiltro, dataFinalFiltro, idClienteFiltro, idCategoriaFiltro)
            .then(function(response) {
                $scope.tickets = response.data.content;
                $scope.totalRecords = response.data.totalRecordsFiltered;

                if (page == 1)
                    getPagination(1, $scope.numRows);
                else 
                    getPagination(page, $scope.numRows);
            })
            .catch(function(error) {
                $location.path("/error");
            });
    };

    /** Função que executa o filtro dos atendimentos/tickets */
    $scope.filterTickets = function() {
        $scope.orderBy = "";
        $scope.direction = "";
        $scope.getPage(1, $scope.orderBy, $scope.direction);
    };

    /** Função associada ao botão de atualizar lista */
    $scope.updateTicketList = function() {
        $scope.idsStatusTicket = "1,4,5";
        $scope.orderBy = "";
        $scope.direction = "";
        $scope.getPage(1, $scope.orderBy, $scope.direction);
    };
    
    /** Função que atualização a lista de tickets */
    $scope.updateListByStatus = function(idsStatusTicket) {
        $scope.idsStatusTicket = idsStatusTicket;
        $scope.orderBy = "";
        $scope.direction = "";
        $scope.getPage(1, $scope.orderBy, $scope.direction);
    };

    /** Função que alimenta a dropdown de clientes da modal de abertura de novo atendimento */
    $scope.initModalNewSupport = function() {
        $scope.clientes = [];
        $scope.usuarios = [{id: "", nome: "Selecione o usuário"}];
        $scope.qtdes = [];
        $scope.categorias = [];
        $scope.classificacoes = [];
        $scope.atendimento.idUsuario = "";
        $scope.atendimento.assunto = "";
        $scope.atendimento.descricao = "";
        $scope.atendimento.file1 = "";
        $scope.atendimento.file2 = "";
        $scope.atendimento.file3 = "";
        $scope.atendimento.file4 = "";
        $scope.atendimento.file5 = "";
        $scope.getClientes();
        $scope.getCategorias();
        $scope.getClassificacoes();
        $scope.getQtdes();
        $scope.qtdeAnexos = 0;
    };

    /** Função que inicializa e prepara a modal de filtro antes de ser carregada */
    $scope.initModalFilter = function() {
        if ($scope.clientes == undefined || $scope.clientes.length == 0) { $scope.getClientes(); }
        if ($scope.categorias == undefined || $scope.categorias.length == 0) { $scope.getCategorias(); }
    };

    /** Função que inicializa os controles da modal filter */
    $scope.clearModalFilter = function() {
        $scope.filtro = {};
        $scope.clientes = [];
        $scope.categorias = [];
        $scope.getClientes();
        $scope.getCategorias();
    };

    /** Função que alimenta a dropdown de clientes, da modal de abertura de novo atendimento */
    $scope.getClientes = function() {
        dashboardService.getClientes()
            .then(function(response) {
                var defaultOption = {id: "", nome: 'Selecione o cliente'}
                var lista = response.data.content;
                $scope.clientes = lista.sort();
                $scope.clientes.unshift(defaultOption);
                $scope.filtro.idCliente = "";
                $scope.atendimento.idCliente = "";
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao recuperar os clientes.";
                generalUtility.showErrorAlert(); 
            });
    };

    /** Função que alimenta a dropdown de usuários associados a um cliente, da modal de abertura de novo atendimento */
    $scope.getUsuarios = function(idCliente) {
        if (idCliente != undefined) {
            //Carregar dropdown de usuarios associados a um cliente
            dashboardService.getUsuarios(idCliente)
                .then(function(response) {
                    var defaultOption = {id: "", nome: 'Selecione o usuário'}
                    var lista = response.data.content;
                    $scope.usuarios = lista.sort();
                    $scope.usuarios.unshift(defaultOption);
                    $scope.atendimento.idUsuario = "";
                })
                .catch(function(error) {
                    $scope.mensagem = "Ocorreu um erro ao recuperar usuários.";
                    generalUtility.showErrorAlert(); 
                });
        }
        else {
            $scope.usuarios = [{id: "", nome: "Selecione o usuário"}];
            $scope.atendimento.idUsuario = "";
        }
    };

    /** Função que alimenta a dropdown de categorias, da modal de abertura de novo atendimento */
    $scope.getCategorias = function() {
        dashboardService.getCategorias()
            .then(function(response) {
                var defaultOption = {id: "", nome: 'Selecione a categoria'}
                var lista = response.data.content;
                $scope.categorias = lista.sort();
                $scope.categorias.unshift(defaultOption);
                $scope.filtro.idCategoria = "";
                $scope.atendimento.idCategoria = "";
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao recuperar categorias.";
                generalUtility.showErrorAlert(); 
            });
    };

    /** Função que alimenta a dropdown de classificações, da modal de abertura de novo atendimento */
    $scope.getClassificacoes = function() {
        dashboardService.getClassificacoes()
            .then(function(response) {
                var defaultOption = {id: "", nome: 'Selecione a Classificação'}
                var lista = response.data.content;
                $scope.classificacoes = lista.sort();
                $scope.classificacoes.unshift(defaultOption);
                $scope.filtro.idClassificacao = "";
                $scope.atendimento.idClassificacao = "";
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao recuperar classificações.";
                generalUtility.showErrorAlert(); 
            });
    };
    
    /** Função que monta a dropdown de quantidade de anexos */
    $scope.getQtdes = function() {
        var qtdes = [
            {id: 0, nome: 'Nenhum anexo'},
            {id: 1, nome: 'Anexar 1 arquivo'},
            {id: 2, nome: 'Anexar 2 arquivos'},
            {id: 3, nome: 'Anexar 3 arquivos'},
            {id: 4, nome: 'Anexar 4 arquivos'},
            {id: 5, nome: 'Anexar 5 arquivos'}
        ];

        $scope.qtdes = qtdes;
    };

    /** Função que recupera os arquivos selecionados nos campos input=file da modal de novo atendimento */
    $scope.fileSelected = function(files, event) {
        if (files.length) {
            switch (event.currentTarget.id) {
                case "file1":
                    $scope.atendimento.file1 = " - " + files[0].name;
                    file1Upload = files[0];    
                    break;
                case "file2":
                    $scope.atendimento.file2 = " - " + files[0].name;
                    file2Upload = files[0];
                    break;
                case "file3":
                    $scope.atendimento.file3 = " - " + files[0].name;        
                    file3Upload = files[0];
                    break;
                case "file4":
                    $scope.atendimento.file4 = " - " + files[0].name;
                    file4Upload = files[0];
                    break;
                case "file5":
                    $scope.atendimento.file5 = " - " + files[0].name;
                    file5Upload = files[0];
                    break;
            }
        }       
    };
    
    /** Função que inicializa os nomes dos arquivos, para o caso do usuário selecionar novamente a opção "nenhum anexo" */
    $scope.validateFiles = function(qtdeanexos) {
        $scope.atendimento.file1 = "";
        $scope.atendimento.file2 = "";
        $scope.atendimento.file3 = "";
        $scope.atendimento.file4 = "";
        $scope.atendimento.file5 = "";
    };

    /** Função que salva o novo atendimento (ticket), criado pelo suporte */
    $scope.saveNewTicket = function() {
        
        $scope.idsAnexos = [];
        $scope.nomesAnexos = [];
        var promiseList = [];

        //Sobe primeiro os arquivos anexos, e se for com sucesso, grava o ticket
        if ($scope.qtdeAnexos == 0) { 
            $scope.uploadSuccess = true; }
        else {
            for (var index = 0; index <= $scope.qtdeAnexos - 1; index++) {
                switch (index) {
                    case 0:
                        if(file1Upload != undefined && file1Upload != null) {
                            promiseList.push($scope.uploadTicketFile(file1Upload, index));
                        }
                        break;
                    case 1:
                        if(file2Upload != undefined && file2Upload != null) {
                            promiseList.push($scope.uploadTicketFile(file2Upload, index));
                        }
                        break;
                    case 2:
                        if(file3Upload != undefined && file3Upload != null) {
                            promiseList.push($scope.uploadTicketFile(file3Upload, index));
                        }
                        break;
                    case 3:
                        if(file4Upload != undefined && file4Upload != null) {
                            promiseList.push($scope.uploadTicketFile(file4Upload, index));
                        }
                        break;
                    case 4:
                        if(file5Upload != undefined && file5Upload != null) {
                            promiseList.push($scope.uploadTicketFile(file5Upload, index));
                        }
                        break;
                }
            }
        }

        $q.all(promiseList)
            .then(function (response) {
                if ($scope.uploadSuccess) {
                    //Se todos os anexos foram incluidos com sucesso, 
                    //então faz o insert dos dados do novo atendimento
                    //Se a session for de um cliente, o idCliente != 0    ===> Pega o idUsuario da session
                    //Se a session for de um atendente, o idCliente == 0  ===> Pega o idUsuario da dropdown de usuario
                    var idUsuarioFiltro = $scope.idCliente == 0 || $scope.idCliente == undefined ? $scope.atendimento.idUsuario : $scope.idUsuario;
                    var idCategoriaFiltro = $scope.atendimento.idCategoria;
                    var idClassificacaoFiltro = $scope.atendimento.idClassificacao;
                    var assuntoFiltro = $scope.atendimento.assunto;
                    var descricaoFiltro = $scope.atendimento.descricao;
                    var pathAnexos = $scope.pathAnexos;
                    var tipoUsuario = $sessionStorage.tipoUsuario;
                    var idAtendente = $sessionStorage.tipoUsuario == "Atendimento" ? $sessionStorage.idUsuario : 0;

                    dashboardService.saveNewSupport(idUsuarioFiltro, idCategoriaFiltro, idClassificacaoFiltro, assuntoFiltro, descricaoFiltro, pathAnexos, tipoUsuario, idAtendente)
                        .then(function(response) {
                            $scope.idTicket = response.data.content.id;
                            $scope.mensagem = response.data.message;
                            $scope.newTicketSuccess = true;
                            $scope.updateTicketList();
                            generalUtility.showSuccessAlert();
                        })
                        .catch(function(error) {
                            $scope.mensagem = "Ocorreu um erro ao salvar o atendimento."; //prever não vir o texto da mensagem aqui e colocar uma default...
                            $scope.newTicketError = true;
                            generalUtility.showErrorAlert();
                        });
                }
                else {
                    $scope.mensagem = "Erro no envio de um ou mais arquivos. Observar tamanho máximo de 5MB por arquivo.";
                    $scope.uploadSuccess = false;
                    $scope.uploadError = true;
                    generalUtility.showErrorAlert();
                }

                file1Upload = undefined;
                file2Upload = undefined;
                file3Upload = undefined;
                file4Upload = undefined;
                file5Upload = undefined;
            })
            .catch(function (error) {
                $scope.mensagem = "Erro no envio de um ou mais arquivos. Observar tamanho máximo de 5MB por arquivo.";
                $scope.uploadSuccess = false;
                $scope.uploadError = true;
                generalUtility.showErrorAlert();
                file1Upload = undefined;
                file2Upload = undefined;
                file3Upload = undefined;
                file4Upload = undefined;
                file5Upload = undefined;
        });
    };
    
    /** Função que faz o upload dos arquivos de anexo, um a um */
    $scope.uploadTicketFile = function(file, index) {
        var idUsuario = $scope.idCliente == undefined ? $scope.atendimento.idUsuario : $scope.idUsuario;
        return $q.when(dashboardService.uploadFile(file, idUsuario)
            .then(function(response) {
                $scope.pathAnexos = response.data;
                $scope.uploadSuccess = true;
                $scope.uploadError = false;
                file1Upload = undefined;
                file2Upload = undefined;
                file3Upload = undefined;
                file4Upload = undefined;
                file5Upload = undefined;
            })
            .catch(function(error) {
                $scope.mensagem = "Erro no envio de anexo. Observar tamanho máximo de 5MB por arquivo.";
                $scope.uploadSuccess = false;
                $scope.uploadError = true;
                file1Upload = undefined;
                file2Upload = undefined;
                file3Upload = undefined;
                file4Upload = undefined;
                file5Upload = undefined;
            }));
    };

    /** Função que chama a página com o detalhamento do atendimento (ticket) */
    $scope.showTicket = function(idTicket)
    {
        $location.hash("details");
        $location.path("/tickets/" + idTicket);
    };

    /** Função que recupera a lista de clientes com determinada ordenação */
    $scope.ordenarPor = function(campo) {
        $scope.orderBy = campo;
        $scope.direction = $scope.direction === "ASC" ? "DESC" : "ASC";
        $scope.getPage(1, $scope.orderBy, $scope.direction);
    };
    
    /** Função que habilita o botão de filtro da modal de filtro, apenas quando um campo pelo menos está preenchido */
    $scope.isFiltroInvalido = function() {
        if ($scope.filtro != undefined) {
            if (Boolean($scope.filtro.idTicket) ||
                Boolean($scope.filtro.titulo) || 
                Boolean($scope.filtro.descricao) ||
                Boolean($scope.filtro.dataInicial) ||
                Boolean($scope.filtro.dataFinal) ||
                Boolean($scope.filtro.idCliente) ||
                Boolean($scope.filtro.idCategoria))
                    return false;
            else
                    return true;
        }
    }

    /**
     *  INTERNAL FUNCTIONS
     */

    /** Função que realiza a remoção de imagens que foram enviadas para o serviço,
     * mas que por algum motivo, deu erro no processo de cadastro do novo 
     * atendimento, e as imagens ficaram sem ter uma associação com um ticket 
     * no banco de dados. Essa função remove do banco de dados essas ocorrências.
     */
    var removeAttachments = function() {
        dashboardService.removeAttachments($scope.idsAnexos)
            .then(function(response) {
                console.log("Anexos inválidos removidos com sucesso.");
            })
            .catch(function(error) {
                console.log("Não foi possível remover os anexos inválidos.");
            });
    };

    /** Função interna que recupera os totais de atendimento por status */
    var loadTotalStatusTicket = function() {
        
        //Será undefined se o acesso se der pelo atendimento, e com isso atribuo 0
        if ($scope.idCliente == undefined) { $scope.idCliente = 0; }

        dashboardService.getTotals($scope.idCliente)
            .then(function(response) {
    
                //Percorre todos os totais retornados
                for (var index = 0; index < response.data.content.totais.length; index++) {
                    var element = response.data.content.totais[index];
                    
                    switch (element.idStatusTicket) {
                        case 0:
                            $scope.totalAtendimentos = element.totalRegistros;
                            break;
                        case 1:
                            $scope.totalAguardando = element.totalRegistros;
                            break;
                        case 2:
                            $scope.totalConcluido = element.totalRegistros;
                            break;
                        case 3:
                            $scope.totalCancelado = element.totalRegistros;
                            break;
                        case 4:
                            $scope.totalPendente = element.totalRegistros;
                            break;
                        case 5:
                            $scope.totalEmAnalise = element.totalRegistros;
                            break;
                    }
                }
    
                $scope.totalEmAberto = $scope.totalAguardando + $scope.totalPendente + $scope.totalEmAnalise;
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao recuperar os totais de atendimentos por status.";
                generalUtility.showErrorAlert();
            });
    };

    /** Função interna que prepara a paginação da grid de tickets */
    var getPagination = function(page, pageSize) {
        if ((page < 1 || page > $scope.pagination.totalPages) && $scope.pagination.totalPages > 0) {
            return;
        }

        // get pager object from service
        $scope.pagination = paginationService.getPagination($scope.totalRecords, page, pageSize);
    };

    /** Função que é executada a cada 2 minutos para carregamento da tela de dashboard */
    var reloadDashboard = function() {
        if(this.location.href == config.baseWeb) {
            $scope.offSet = 0;
            $scope.numRows = 20;
            $scope.orderBy = "";
            $scope.direction = "";
            $scope.idsStatusTicket = "1,4,5";
            $scope.getPage(1, $scope.orderBy, $scope.direction);
        }
    };

    /** Carrega todos os totalizadores (cards) de atendimento */
    loadTotalStatusTicket();

    /** Carrega a primeira página de tickets */
    $scope.getPage(1,"","");

    $(document).ready(function() {
        setInterval(function() {
          reloadDashboard();
        }, 120000);
    });
}]);