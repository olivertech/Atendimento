app.controller('dashboardController', function($scope, $sessionStorage, $location, $q, $interval, dashboardService, paginationService, generalUtility) {

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
    $scope.qtdeanexos = 0;

    $scope.orderBy = "";
    $scope.direction = "";
    
    /** Função que controla a paginação da grid de tickets */
    $scope.getPage = function(page, orderBy, direction) {

        let offset = (page - 1) * $scope.numRows;
        let idTicketFiltro = "";
        let tituloFiltro = "";
        let descricaoFiltro = "";
        let dataInicialFiltro = "";
        let dataFinalFiltro = "";
        let idClienteFiltro = 0;
        let idCategoriaFiltro = 0;

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
    }

    $scope.filterTickets = function() {
        $scope.orderBy = "";
        $scope.direction = "";
        $scope.getPage(1, $scope.orderBy, $scope.direction);
    }

    /** Função associada ao botão de atualizar lista */
    $scope.updateTicketList = function() {
        $scope.idsStatusTicket = "1,4,5";
        $scope.orderBy = "";
        $scope.direction = "";
        $scope.getPage(1, $scope.orderBy, $scope.direction);
    }
    
    /** Função que atualização a lista de tickets */
    $scope.updateListByStatus = function(idsStatusTicket) {
        $scope.idsStatusTicket = idsStatusTicket;
        $scope.orderBy = "";
        $scope.direction = "";
        $scope.getPage(1, $scope.orderBy, $scope.direction);
    }

    /** Função que alimenta a dropdown de clientes da modal de abertura de novo atendimento */
    $scope.initModalNewSupport = function() {
        $scope.clientes = [];
        $scope.usuarios = [];
        $scope.qtdes = [];
        $scope.categorias = [];
        $scope.classificacoes = [];
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
    }

    $scope.initModalFilter = function() {
        if ($scope.clientes == undefined || $scope.clientes.length == 0) { $scope.getClientes(); }
        if ($scope.categorias == undefined || $scope.categorias.length == 0) { $scope.getCategorias(); }
    }

    $scope.clearModalFilter = function() {
        $scope.filtro.idTicket = "";
        $scope.filtro.titulo = "";
        $scope.filtro.descricao = "";
        $scope.filtro.dataInicial = "";
        $scope.filtro.dataFinal = "";
        $scope.clientes = undefined;
        $scope.categorias = undefined;
        $scope.getClientes();
        $scope.getCategorias();
    }

    /** Função que alimenta a dropdown de clientes, da modal de abertura de novo atendimento */
    $scope.getClientes = function() {
        dashboardService.getClientes()
            .then(function(response) {
                $scope.clientes = response.data.content;
            })
            .catch(function(error) {
                generalUtility.showErrorAlert(); 
            });
    }

    /** Função que alimenta a dropdown de usuários associados a um cliente, da modal de abertura de novo atendimento */
    $scope.getUsuarios = function(idCliente) {
        if (idCliente != undefined) {
            //Carregar dropdown de usuarios associados a um cliente
            dashboardService.getUsuarios(idCliente)
                .then(function(response) {
                    $scope.usuarios = response.data.content;
                })
                .catch(function(error) {
                    $scope.mensagem = "Ocorreu um erro ao recuperar usuários.";
                    generalUtility.showErrorAlert(); 
                });
        }
        else {
            $scope.usuarios = [];
        }
    }

    /** Função que alimenta a dropdown de categorias, da modal de abertura de novo atendimento */
    $scope.getCategorias = function() {
        dashboardService.getCategorias()
            .then(function(response) {
                $scope.categorias = response.data.content;
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao recuperar categorias.";
                generalUtility.showErrorAlert(); 
            });
    }

    /** Função que alimenta a dropdown de classificações, da modal de abertura de novo atendimento */
    $scope.getClassificacoes = function() {
        dashboardService.getClassificacoes()
            .then(function(response) {
                $scope.classificacoes = response.data.content;
            })
            .catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao recuperar classificações.";
                generalUtility.showErrorAlert(); 
            });
    }
    
    /** Função que monta a dropdown de quantidade de anexos */
    $scope.getQtdes = function() {
        let qtdes = [
            {id: 0, nome: 'Nenhum anexo'},
            {id: 1, nome: 'Anexar 1 arquivo'},
            {id: 2, nome: 'Anexar 2 arquivos'},
            {id: 3, nome: 'Anexar 3 arquivos'},
            {id: 4, nome: 'Anexar 4 arquivos'},
            {id: 5, nome: 'Anexar 5 arquivos'}
        ];

        $scope.qtdes = qtdes;
    }

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
    }
    
    /** Função que inicializa os nomes dos arquivos, para o caso do usuário selecionar novamente a opção "nenhum anexo" */
    $scope.validateFiles = function(qtdeanexos) {
        $scope.atendimento.file1 = "";
        $scope.atendimento.file2 = "";
        $scope.atendimento.file3 = "";
        $scope.atendimento.file4 = "";
        $scope.atendimento.file5 = "";
    }

    /** Função que salva o novo atendimento (ticket), criado pelo suporte */
    $scope.saveNewTicket = function() {
        
        $scope.idsAnexos = [];
        $scope.nomesAnexos = [];
        var promiseList = [];

        //Sobe primeiro os arquivos anexos, e se for com sucesso, grava o ticket
        if ($scope.qtdeAnexos == 0) { 
            $scope.uploadSuccess = true; }
        else {
            for (let index = 0; index <= $scope.qtdeAnexos - 1; index++) {
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
                    let idUsuarioFiltro = $scope.idCliente == 0 || $scope.idCliente == undefined ? $scope.atendimento.usuario : $scope.idUsuario;
                    let idCategoriaFiltro = $scope.atendimento.categoria;
                    let idClassificacaoFiltro = $scope.atendimento.classificacao;
                    let assuntoFiltro = $scope.atendimento.assunto;
                    let descricaoFiltro = $scope.atendimento.descricao;
                    let pathAnexos = $scope.pathAnexos;
                    let tipoUsuario = $sessionStorage.tipoUsuario;
                    let idAtendente = $sessionStorage.tipoUsuario == "Atendimento" ? $sessionStorage.idUsuario : 0;

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
            })
            .catch(function (error) {
                $scope.mensagem = "Erro no envio de um ou mais arquivos. Observar tamanho máximo de 5MB por arquivo.";
                $scope.uploadSuccess = false;
                $scope.uploadError = true;
                generalUtility.showErrorAlert();
        });
    }
    
    /** Função que faz o upload dos arquivos de anexo, um a um */
    $scope.uploadTicketFile = function(file, index) {
        let idUsuario = $scope.idCliente == undefined ? $scope.atendimento.usuario : $scope.idUsuario;
        return $q.when(dashboardService.uploadFile(file, idUsuario)
            .then(function(response) {
                $scope.pathAnexos = response.data;
                $scope.uploadSuccess = true;
                $scope.uploadError = false;
            })
            .catch(function(error) {
                $scope.mensagem = "Erro no envio de anexo. Observar tamanho máximo de 5MB por arquivo.";
                $scope.uploadSuccess = false;
                $scope.uploadError = true;
            }));
    }

    /** Função que chama a página com o detalhamento do atendimento (ticket) */
    $scope.showTicket = function(idTicket)
    {
        $location.hash("inicio");
        $location.path("/tickets/" + idTicket);
    }

    /** Função que recupera a lista de clientes com determinada ordenação */
    $scope.ordenarPor = function(campo) {
        $scope.orderBy = campo;
        $scope.direction = $scope.direction === "ASC" ? "DESC" : "ASC";
        $scope.getPage(1, $scope.orderBy, $scope.direction);
    }
    
    /**
     *  INTERNAL FUNCTIONS
     */

    /** Função que recarrega o dashboard a cada 2 minutos */
    var auto = $interval(function() {
        $scope.offSet = 0;
        $scope.numRows = 20;
        $scope.orderBy = "";
        $scope.direction = "";
        $scope.idsStatusTicket = "1,4,5";
        $scope.getPage(1, $scope.orderBy, $scope.direction);
      }, 120000);

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
            })  
    }

    /** Função interna que recupera os totais de atendimento por status */
    var loadTotalStatusTicket = function() {
        
        //Será undefined se o acesso se der pelo atendimento, e com isso atribuo 0
        if ($scope.idCliente == undefined) { $scope.idCliente = 0; }

        dashboardService.getTotals($scope.idCliente)
            .then(function(response) {
    
                //Percorre todos os totais retornados
                for (let index = 0; index < response.data.content.totais.length; index++) {
                    const element = response.data.content.totais[index];
                    
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
    }

    /** Função interna que prepara a paginação da grid de tickets */
    var getPagination = function(page, pageSize) {
        if ((page < 1 || page > $scope.pagination.totalPages) && $scope.pagination.totalPages > 0) {
            return;
        }

        // get pager object from service
        $scope.pagination = paginationService.getPagination($scope.totalRecords, page, pageSize);
    }

    /** Carrega todos os totalizadores (cards) de atendimento */
    loadTotalStatusTicket();

    /** Carrega a primeira página de tickets */
    $scope.getPage(1,"","");
});