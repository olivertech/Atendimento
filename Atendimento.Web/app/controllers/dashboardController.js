app.controller('dashboardController', function($scope, $sessionStorage, $location, $q, dashboardService, paginationService) {

    var file1Upload, file2Upload, file3Upload, file4Upload, file5Upload;

    $scope.idUsuario = $sessionStorage.idUsuario;
    $scope.usuario = $sessionStorage.usuario;
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

    $scope.uploadSuccess = false;
    $scope.uploadError = false;
    $scope.newTicketSuccess = false;
    $scope.newTicketError = false;
    $scope.pagination = {};
    $scope.qtdeanexos = 0;

    /** Função que controla a paginação da grid de tickets */
    $scope.setPage = function(page) {

        let offset = (page - 1) * $scope.numRows;
        let idTicket = "";
        let titulo = "";
        let descricao = "";
        let dataInicial = "";
        let dataFinal = "";
        let idCliente = 0;
        let idCategoria = 0;

        if ($scope.filtro != undefined) {
            idTicket = $scope.filtro.idTicket;
            titulo = $scope.filtro.titulo;
            descricao = $scope.filtro.descricao;
            dataInicial = $scope.filtro.dataInicial;
            dataFinal = $scope.filtro.dataFinal;
            idCliente = $scope.filtro.idCliente;
            idCategoria = $scope.filtro.idCategoria;
        }

        dashboardService.getPagedTicket(offset, $scope.numRows, $scope.idsStatusTicket, idTicket, titulo, descricao, dataInicial, dataFinal, idCliente, idCategoria)
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
        $scope.setPage(1);
    }

    /** Função associada ao botão de atualizar lista */
    $scope.updateTicketList = function() {
        $scope.idsStatusTicket = "1,4,5";
        $scope.setPage(1);
    }
    
    /** Função que atualização a lista de tickets */
    $scope.updateListByStatus = function(idsStatusTicket) {
        $scope.idsStatusTicket = idsStatusTicket;
        $scope.setPage(1);
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
                showErrorAlert(); 
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
                    showErrorAlert(); 
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
                showErrorAlert(); 
            });
    }

    /** Função que alimenta a dropdown de classificações, da modal de abertura de novo atendimento */
    $scope.getClassificacoes = function() {
        dashboardService.getClassificacoes()
            .then(function(response) {
                $scope.classificacoes = response.data.content;
            })
            .catch(function(error) {
                showErrorAlert(); 
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

    /** Função que salva o novo atendimento, criado pelo suporte */
    $scope.saveNewTicket = function() {
        
        $scope.idsAnexos = [];
        $scope.nomesAnexos = [];
        var promiseList = [];

        //Sobe primeiro os arquivos anexos, e se for com sucesso, grava o ticket
        if ($scope.qtdeAnexos == 0) { $scope.uploadSuccess == true; }
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
                    let usuario = $scope.atendimento.usuario;
                    let categoria = $scope.atendimento.categoria;
                    let classificacao = $scope.atendimento.classificacao;
                    let assunto = $scope.atendimento.assunto;
                    let descricao = $scope.atendimento.descricao;
                    let pathAnexos = $scope.pathAnexos;

                    dashboardService.saveNewSupport(usuario, categoria, classificacao, assunto, descricao, pathAnexos)
                        .then(function(response) {
                            $scope.idTicket = response.data.content.id;
                            $scope.mensagem = response.data.message;
                            $scope.newTicketSuccess = true;
                            showSuccessAlert();
                        })
                        .catch(function(error) {
                            $scope.mensagem = error.data.message;
                            $scope.newTicketError = true;
                            showErrorAlert();
                        });
                }
                else {
                    $scope.mensagem = "Erro no envio de um ou mais arquivos.";
                    $scope.uploadSuccess = false;
                    $scope.uploadError = true;    
                    showErrorAlert();
                }
            })
            .catch(function (error) {
                $scope.mensagem = "Erro no envio de um ou mais arquivos.";
                $scope.uploadSuccess = false;
                $scope.uploadError = true; 
                showErrorAlert(); 
        });
    }
    
    /** Função que faz o upload dos arquivos de anexo, um a um */
    $scope.uploadTicketFile = function(file, index) {
        return $q.when(dashboardService.uploadFile(file, $scope.atendimento.usuario)
            .then(function(response) {
                //Guardo os ids dos anexos gravados na tabela Anexo, pra poder relaciona-los com o novo ticket
                //$scope.idsAnexos[index] = response.data.anexoResponse.id;
                $scope.pathAnexos = response.data;
                $scope.uploadSuccess = true;
                $scope.uploadError = false;
            })
            .catch(function(error) {
                $scope.mensagem = "Erro no envio de anexo.";
                $scope.uploadSuccess = false;
                $scope.uploadError = true;
            }));
    }

    /** Função que chama a página com o detalhamento do atendimento (ticket) */
    $scope.showTicket = function(idTicket)
    {
        $location.hash("inicio");
        $location.path("/ticket/" + idTicket);
    }

    /**
     *  INTERNAL FUNCTIONS
     */

     /** Função que mostra alerta de sucesso */
    var showSuccessAlert = function() {
        $("#success-alert").fadeTo(5000, 500).slideUp(500, function(){
            $("#success-alert").slideUp(500);
        });
    }

    /** Função que mostra alerta de erro */
    var showErrorAlert = function() {
        $("#error-alert").fadeTo(5000, 500).slideUp(500, function(){
            $("#error-alert").slideUp(500);
        });
    }

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
        dashboardService.getTotals()
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
                showErrorAlert();
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
    $scope.setPage(1);
});