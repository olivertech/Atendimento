app.controller('ticketController', ['$scope', '$location', '$q', '$anchorScroll', '$sessionStorage', 'dashboardService', 'ticketService', 'generalUtility', 'ngClipboard',
    function($scope, $location, $q, $anchorScroll, $sessionStorage, dashboardService, ticketService, generalUtility, ngClipboard) {
    
    var file1Upload, file2Upload, file3Upload, file4Upload, file5Upload;

    $anchorScroll();

    $scope.idUsuario = $sessionStorage.idUsuario;
    $scope.idCliente = $sessionStorage.idCliente;
    $scope.isAdmin = $sessionStorage.isAdmin;
    $scope.nomeCliente = $sessionStorage.nomeCliente;
    $scope.logicalPathAnexos = $sessionStorage.logicalPathAnexos;
    $scope.mensagensTicket = [];

    $scope.qtdeanexos = 0;

    $scope.mensagemTicket = {};
    $scope.status = {};
    $scope.autor = {};
    $scope.classificacao = {};
    $scope.template = {};

    $scope.uploadSuccess = false;
    $scope.uploadError = false;
    $scope.newMessageSuccess = false;
    $scope.newMessageError = false;

    $scope.back = function() {
        $location.hash("");
        $location.path("/dashboard");
    };

    $scope.splitAnexos = function(anexos) {
        if(anexos != null) {
            var array = anexos.split('|');
            return array;
        }
    };

    /** Função que inicializa a modal de nova mensagem */
    $scope.initModalNewMessage = function() {
        $scope.mensagemTicket.descricao = "";
        $scope.mensagemTicket.file1 = "";
        $scope.mensagemTicket.file2 = "";
        $scope.mensagemTicket.file3 = "";
        $scope.mensagemTicket.file4 = "";
        $scope.mensagemTicket.file5 = "";
        $scope.mensagemTicket.interno = false;
        $scope.mensagemTicket.idStatusTicket = 1;
        $scope.getQtdes();
        $scope.qtdeAnexos = 0;
    };

    /** Função que inicializa a modal de templates */
    $scope.initModalTemplates = function() {
        $scope.mensagemTicket.descricao = "";
        $scope.template.conteudo = "";
        $scope.getTemplates();
    };

    /** Função que altera o status do ticket */
    $scope.updateStatusTicket = function(idStatus) {
        ticketService.updateStatusTicket($scope.idTicket, idStatus, $scope.autor.id, $sessionStorage.tipoUsuario)
            .then(function(response) {
                $scope.mensagem = response.data.message;
                generalUtility.showSuccessAlert();
                getTicket();
            }).catch(function(error) {
                $scope.mensagem = "Ocorreu um erro ao atualizar o status do atendimento.";
                generalUtility.showErrorAlert();
            });
    };

    /** Função que atualiza a classificação do ticket */
    $scope.updateClassificacao = function(idClassificacao) {
        ticketService.updateClassificacao($scope.idTicket, idClassificacao, $scope.autor.id, $sessionStorage.tipoUsuario)
        .then(function(response) {
            $scope.mensagem = response.data.message;
            generalUtility.showSuccessAlert();
            getTicket();
        }).catch(function(error) {
            $scope.mensagem = "Ocorreu um erro ao atualizar a classificação do atendimento.";
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

    /** Função que recupera os arquivos selecionados nos campos input=file da modal de nova mensagem */
    $scope.fileSelected = function(files, event) {
        if (files.length) {
            switch (event.currentTarget.id) {
                case "file1":
                    $scope.mensagemTicket.file1 = " - " + files[0].name;
                    file1Upload = files[0];    
                    break;
                case "file2":
                    $scope.mensagemTicket.file2 = " - " + files[0].name;
                    file2Upload = files[0];
                    break;
                case "file3":
                    $scope.mensagemTicket.file3 = " - " + files[0].name;        
                    file3Upload = files[0];
                    break;
                case "file4":
                    $scope.mensagemTicket.file4 = " - " + files[0].name;
                    file4Upload = files[0];
                    break;
                case "file5":
                    $scope.mensagemTicket.file5 = " - " + files[0].name;
                    file5Upload = files[0];
                    break;
            }
        }       
    };

    /** Função que inicializa os nomes dos arquivos, para o caso do usuário selecionar novamente a opção "nenhum anexo" */
    $scope.validateFiles = function(qtdeanexos) {
        $scope.mensagemTicket.file1 = "";
        $scope.mensagemTicket.file2 = "";
        $scope.mensagemTicket.file3 = "";
        $scope.mensagemTicket.file4 = "";
        $scope.mensagemTicket.file5 = "";
    };

    /** Função que salva uma nova mensagem, associando a um ticket */
    $scope.saveNewMessage = function() {

        var promiseList = [];
        
        //Sobe primeiro os arquivos anexos, e se for com sucesso, grava a mensagem
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
                    //então faz o insert dos dados da nova mensagem
                    var idTicket = $scope.idTicket;
                    var idUsuario = $scope.idUsuario;
                    var tipoUsuarioAgente = $sessionStorage.tipoUsuario;
                    var descricao = $scope.mensagemTicket.descricao;
                    var interno = $scope.mensagemTicket.interno;
                    var idStatusTicket = $scope.mensagemTicket.idStatusTicket;
                    var pathAnexos = $scope.pathAnexos;

                    ticketService.saveNewMessage(idTicket, idUsuario, tipoUsuarioAgente, descricao, idStatusTicket, interno, pathAnexos)
                        .then(function(response) {
                            $scope.mensagem = response.data.message;
                            $scope.newMessageSuccess = true;
                            getTicket();
                            getMessages();
                            generalUtility.showSuccessAlert();
                        })
                        .catch(function(error) {
                            $scope.mensagem = "Ocorreu um erro ao salvar mensagem";
                            $scope.newMessageError = true;
                            generalUtility.showErrorAlert();
                            console.log(error);
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
        return $q.when(dashboardService.uploadFile(file, $scope.idUsuario)
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
                $scope.mensagem = "Erro no envio de um ou mais arquivos. Observar tamanho máximo de 5MB por arquivo.";
                $scope.uploadSuccess = false;
                $scope.uploadError = true;
                file1Upload = undefined;
                file2Upload = undefined;
                file3Upload = undefined;
                file4Upload = undefined;
                file5Upload = undefined;
            }));
    };

    /** Watch que observa a escolha do template para alimentar o campo de conteudo com a opção escolhida */
    $scope.$watch("template.idTemplateresposta", function() {
        if ($scope.template.idTemplateresposta) {
            var template = $scope.templates.filter(function(item) {
                return item.id === $scope.template.idTemplateresposta;
              })[0];
            $scope.template.conteudo = template.conteudo;
        }
    });

    /** Função que alimenta a dropdown de clientes, da modal de abertura de novo atendimento */
    $scope.getTemplates = function() {
        ticketService.getTemplates()
            .then(function(response) {
                var defaultOption = {id: "", titulo: 'Selecione o template'}
                var lista = response.data.content;
                $scope.templates = lista.sort();
                $scope.templates.unshift(defaultOption);
                $scope.template.idTemplateresposta = "";
            })
            .catch(function(error) {
                $scope.mensagem = "Erro foi possível recuperar os templates.";
                generalUtility.showErrorAlert(); 
            });
    };
    
    /**
     *  INTERNAL FUNCTIONS
     */

    /** Função busca os dados do atendimento original (ticket) */
    var getTicket = function() {

        ticketService.getTicket($scope.idTicket)
            .then(function(response) {
                $scope.categoria = response.data.content.categoria.nome;
                $scope.classificacao.id = response.data.content.classificacao.id;
                $scope.classificacao.nome = response.data.content.classificacao.nome;
                $scope.status.id = response.data.content.statusTicket.id;
                $scope.status.nome = response.data.content.statusTicket.nome;
                $scope.autor.id = response.data.content.usuarioCliente.id;
                $scope.autor.nome = response.data.content.usuarioCliente.nome + " (" + response.data.content.usuarioCliente.email + ")";
                $scope.autor.cliente = response.data.content.usuarioCliente.cliente.nome;
                $scope.data = response.data.content.dataHoraInicial;
                $scope.titulo = response.data.content.titulo;
                $scope.descricao = response.data.content.descricao;
                $scope.anexos = response.data.content.anexos;

                /** Chama a função que carrega todas as mensagens associadas ao ticket */
                getMessages();
            })
            .catch(function(error) {
                $scope.mensagem = "Erro foi possível recuperar os dados do ticket.";
                generalUtility.showErrorAlert(); 
            });
    };
    
    /** Função que recupera todas as mensagens associadas ao ticket */
    var getMessages = function() {

        ticketService.getMessages($scope.idTicket, $scope.idCliente)
            .then(function(response) {
                $scope.mensagensTicket = response.data.content;
            })
            .catch(function(error) {
                $scope.mensagem = "Erro foi possível recuperar as mensagens associadas ao ticket.";
                generalUtility.showErrorAlert(); 
            });
    };
    
    /** Chama a função que carrega os dados do atendimento original (ticket) */
    getTicket();
}]);