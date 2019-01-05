app.controller('ticketController', function($scope, $location, $q, $anchorScroll, $sessionStorage, ticketService, ngClipboard) {
    
    var file1Upload, file2Upload, file3Upload, file4Upload, file5Upload;

    $anchorScroll();

    $scope.idUsuario = $sessionStorage.idUsuario;
    $scope.pathAnexos = $sessionStorage.pathAnexos;
    $scope.messages = [];

    $scope.qtdeanexos = 0;

    $scope.mensagem = {};
    $scope.status = {};
   
    $scope.uploadSuccess = false;
    $scope.uploadError = false;
    $scope.newMessageSuccess = false;
    $scope.newMessageError = false;

    $scope.back = function() {
        $location.hash("");
        $location.path("/dashboard");
    }

    $scope.splitAnexos = function(anexos) {
        if(anexos != null) {
            let array = anexos.split('|');
            return array;
        }
    }

    /** Função que inicializa a modal de nova mensagem */
    $scope.initModalNewMessage = function() {
        $scope.mensagem.descricao = "";
        $scope.mensagem.file1 = "";
        $scope.mensagem.file2 = "";
        $scope.mensagem.file3 = "";
        $scope.mensagem.file4 = "";
        $scope.mensagem.file5 = "";
        $scope.mensagem.interno = false;
        $scope.getQtdes();
        $scope.qtdeAnexos = 0;
    }

    /** Função que inicializa a modal de templates */
    $scope.initModalTemplates = function() {
        $scope.mensagem.descricao = "";
        $scope.template.conteudo = "";
        getTemplates();
    }

    /** Função que altera o status do ticket */
    $scope.updateStatusTicket = function(idStatus) {
        ticketService.updateStatusTicket($scope.idTicket, idStatus)
            .then(function(response) {
                $scope.mensagem = response.data.message;
                showSuccessAlert();
                getTicket();
            }).catch(function(error) {
                $scope.mensagem = error.data.message;
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

    /** Função que recupera os arquivos selecionados nos campos input=file da modal de nova mensagem */
    $scope.fileSelected = function(files, event) {
        if (files.length) {
            switch (event.currentTarget.id) {
                case "file1":
                    $scope.mensagem.file1 = " - " + files[0].name;
                    file1Upload = files[0];    
                    break;
                case "file2":
                    $scope.mensagem.file2 = " - " + files[0].name;
                    file2Upload = files[0];
                    break;
                case "file3":
                    $scope.mensagem.file3 = " - " + files[0].name;        
                    file3Upload = files[0];
                    break;
                case "file4":
                    $scope.mensagem.file4 = " - " + files[0].name;
                    file4Upload = files[0];
                    break;
                case "file5":
                    $scope.mensagem.file5 = " - " + files[0].name;
                    file5Upload = files[0];
                    break;
            }
        }       
    }

    /** Função que inicializa os nomes dos arquivos, para o caso do usuário selecionar novamente a opção "nenhum anexo" */
    $scope.validateFiles = function(qtdeanexos) {
        $scope.mensagem.file1 = "";
        $scope.mensagem.file2 = "";
        $scope.mensagem.file3 = "";
        $scope.mensagem.file4 = "";
        $scope.mensagem.file5 = "";
    }

    /** Função que salva uma nova mensagem, associando a um ticket */
    $scope.saveNewMessage = function() {
    
        $scope.idsAnexos = [];
        $scope.nomesAnexos = [];
        var promiseList = [];

        //Sobe primeiro os arquivos anexos, e se for com sucesso, grava a mensagem
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

        $q.all(promiseList)
            .then(function (response) {
                if ($scope.uploadSuccess) {
                    //Se todos os anexos foram incluidos com sucesso, 
                    //então faz o insert dos dados da nova mensagem
                    let idTicket = $scope.idTicket;
                    let idAtendente = $scope.idUsuario;
                    let descricao = $scope.mensagem.descricao;
                    let interno = $scope.mensagem.interno;
                    //let idStatusTicket = $scope.mensagem.status;
                    let pathAnexos = $scope.pathAnexos;

                    dashboardService.saveNewMessage(idTicket, idAtendente, descricao, interno, pathAnexos)
                        .then(function(response) {
                            $scope.mensagem = response.data.message;
                            $scope.newMessageSuccess = true;
                            showSuccessAlert();
                        })
                        .catch(function(error) {
                            $scope.mensagem = error.data.message;
                            $scope.newMessageError = true;
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
        return $q.when(dashboardService.uploadFile(file, $scope.idUsuario)
            .then(function(response) {
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

    /** Watch que observa a escolha do template para alimentar o campo de conteudo com a opção escolhida */
    $scope.$watch("template.templateresposta", function() {
        if ($scope.template.templateresposta != undefined) {
            //$scope.template = {};
            $scope.template.conteudo = $scope.template.templateresposta.conteudo;
        }
    });

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

    /** Função busca os dados do atendimento original (ticket) */
    var getTicket = function() {

        ticketService.getTicket($scope.idTicket)
            .then(function(response) {
                $scope.categoria = response.data.content.categoria.nome;
                $scope.classificacao = response.data.content.classificacao.nome;
                $scope.status.id = response.data.content.statusTicket.id;
                $scope.status.nome = response.data.content.statusTicket.nome;
                $scope.autor = response.data.content.usuarioCliente.nome + "( " + response.data.content.usuarioCliente.email + " )";
                $scope.data = response.data.content.dataHoraInicial;
                $scope.titulo = response.data.content.titulo;
                $scope.descricao = response.data.content.descricao;
                $scope.anexos = response.data.content.anexos;
            })
            .catch(function(error) {
                $location.path("/error");
            });
    }
    
    /** Função que recupera todas as mensagens associadas ao ticket */
    var getMessages = function() {

        ticketService.getMessages($scope.idTicket)
            .then(function(response) {
                $scope.messages = response.data.content;
            })
            .catch(function(error) {
                $location.path("/error");
            })
    }

    var getTemplates = function() {

        ticketService.getTemplates()
            .then(function(response) {
                $scope.templates = response.data.content;
            })
            .catch(function(error) {
                $location.path("/error");
            })
    }

    /** Chama a função que carrega os dados do atendimento original (ticket) */
    getTicket();

    /** Chama a função que carrega todas as mensagens associadas ao ticket */
    getMessages();
});