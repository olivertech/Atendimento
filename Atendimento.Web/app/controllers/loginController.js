app.controller('loginController', function($scope, $location, $sessionStorage, loginService, rememberMeService, generalUtility) {

    $scope.login = '';
    $scope.login.isPasswordRecovered = false;
    $scope.login.isRecoverFormSelected = false;
    $scope.login.changePassword = false;
    $scope.isAuthenticationError = false;
    $scope.login.email = '';
    $scope.tipos = [];
    $scope.tipo = {};

    //Verifica se foi passado algum status code 401 - erro de autenticação 
    if($scope.statusCode == '401' || $scope.statusCode == '405') {
        $scope.isAuthenticationError = true;
        $scope.statusCode = ''
    }

    /** Executa o login */
    $scope.login = function() {

        if (typeof(Storage) !== "undefined") {

            if($scope.login.username != '' && $scope.login.password != '') {
                
                loginService.login($scope.login.username, $scope.login.password, $scope.login.typeloginform)
                    .then(function(response) {
                        if (!response.data.content.usuario.provisorio)
                        {
                            //Armazeno o token de autenticação na session
                            setLocalStorage(response);
                            $location.path("/dashboard");
                        }
                        else
                            $scope.login.changePassword = true;
                    })
                    .catch(function(error) {
                        resetLocalStorage();
                        $location.path("/error");
                    });
            }

        } else {
            //Criar uma página de erro especifica para esse problema do navegador ser antigo e não trabalhar com recursos mais atuais
            $location.path("/error");
        }
    }

    $scope.changePassword = function() {
        let username = $scope.login.username;
        let oldPassword = $scope.login.senhaProvisoria;
        let newPassword = $scope.login.novaSenha;
        let tipo = $scope.login.typechangepwdform;

        loginService.changePassword(username, oldPassword, newPassword, tipo)
            .then(function(response) {

                $scope.login.username = username;
                $scope.login.password = newPassword;
                $scope.login.typeloginform = tipo;
                
                $scope.login();
            })
            .catch(function(error) {
                resetLocalStorage();
                $location.path("/error");
            });
    }

    /** Verifica se o usuário tem cookie */
    var checkRememberMe = function() {

        //================================================================
        // '1ipBBiRn = equivale a chave de cookie associada ao username' 
        // 'fMFKHVLj = equivale a chave de cookie associada a password'
        // 'GeTfLi22 = equivale a chave de cookie associada a type'
        //================================================================

        var usernameCookie = rememberMeService.fetchRememberMeCookie('1ipBBiRn');
        var passwordCookie = rememberMeService.fetchRememberMeCookie('fMFKHVLj');
        var typeCookie = rememberMeService.fetchRememberMeCookie('GeTfLi22');

        if (usernameCookie && passwordCookie && typeCookie) {
            $scope.login.username = usernameCookie;
            $scope.login.password = window.atob(passwordCookie);
            $scope.login.typeloginform = typeCookie;
            $scope.login.rememberMe = true;
        } else {
            $scope.login.username = '';
            $scope.login.password = '';
            $scope.login.typeloginform = '';
            $scope.login.rememberMe = false;
        }
    }

    /** Reset de todos os cookies gravados, caso o usuário digite no campo de username e abandone (evento on blur) */
    $scope.resetCookies = function() {
        rememberMeService.setRememberMeCookie('1ipBBiRn', '');
        rememberMeService.setRememberMeCookie('fMFKHVLj', '');
        rememberMeService.setRememberMeCookie('GeTfLi22', '');
        $scope.login.typeloginform = '';
        $scope.login.rememberMe = false;
    }

    /** Grava os dados do usuário no cookie */
    $scope.setRememberMe = function() {

        $scope.login.rememberMe = !$scope.login.rememberMe;

        if ($scope.login.username && $scope.login.password && $scope.login.rememberMe) {

            var encondedPassword = window.btoa($scope.login.password);

            rememberMeService.setRememberMeCookie('fMFKHVLj', encondedPassword);
            rememberMeService.setRememberMeCookie('1ipBBiRn', $scope.login.username);
            rememberMeService.setRememberMeCookie('GeTfLi22', $scope.login.typeloginform);
        } else {
            rememberMeService.setRememberMeCookie('1ipBBiRn', '');
            rememberMeService.setRememberMeCookie('fMFKHVLj', '');
            rememberMeService.setRememberMeCookie('GeTfLi22', '');
        }
    }

    $scope.recoverPassword = function() {

        if($scope.login.email) {
                
            loginService.recoverPassword($scope.login.email, $scope.login.typerecoverform)
                .then(function(response) {
                    $scope.login.isPasswordRecovered = true;
                })
                .catch(function(error) {
                    resetLocalStorage();
                    $location.path("/error");
                });
        }
    }

    $scope.showHideRecoverForm = function() {
        $scope.login.isRecoverFormSelected = !$scope.login.isRecoverFormSelected;
        $scope.login.isPasswordRecovered = false;
    }
    
    $scope.showHideChangePasswordForm = function() {
        $scope.login.changePassword = false;
        $scope.login.isPasswordRecovered = false;
    }

    /** Função que alterna o type do campo de senha */
    $scope.showHidePwd = function(field) {
        generalUtility.showHidePwd(field);
    }    

    /**
     *  INTERNAL FUNCTIONS
     */

    /** Guarda todas as variáveis de sessão na sessionStorage */
    var setLocalStorage = function(response) {
        $sessionStorage.tokenAuthentication = response.data.content.token;
        $sessionStorage.isOnDashboard = true;
        $sessionStorage.usuario = response.data.content.usuario.nome;
        $sessionStorage.idUsuario = response.data.content.usuario.id;
        $sessionStorage.isAdmin = response.data.content.usuario.isAdmin;
        $sessionStorage.idCliente = response.data.content.usuario.cliente != undefined ? response.data.content.usuario.cliente.id : 0;
        $sessionStorage.nomeCliente = response.data.content.usuario.cliente != undefined ? response.data.content.usuario.cliente.nome : '';
        $sessionStorage.tipoUsuario = $scope.login.typeloginform;
        $sessionStorage.logicalPathAnexos = "http://localhost:51765/Anexos/";
    }

    /** Apago todas as variáveis de sessão */
    var resetLocalStorage = function() {
        $sessionStorage.tokenAuthentication = '';
        $sessionStorage.isOnDashboard = false;
        $sessionStorage.usuario = '';
        $sessionStorage.idUsuario = '';
        $sessionStorage.isAdmin = false;
        $sessionStorage.idCliente = '';
        $sessionStorage.nomeCliente = '';
        $sessionStorage.tipoUsuario = '';
        $sessionStorage.logicalPathAnexos = '';
    }

    checkRememberMe();
});