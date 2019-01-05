var app = angular.module('atendimento', ["ngMessages", "ngStorage", "ngCookies", "ngAnimate", "ui.router", "angular-loading-bar", "ngFileUpload", "ngClipboard"]);

/** 
 * Código que é executado assim que a aplicação angular levanta.
 * Colocar aqui todas as regras que forem necessárias para evitar
 * acesso indevido a aplicação, ou para evitar comportamentos
 * indesejados
 */
app.run(function($rootScope, $location, $sessionStorage){

    $rootScope.$on('$locationChangeStart', function(event, next, current) {
        
        /** 
         * Se o botão de back do navegador for clicado, não faz nada 
         */
        if ($location.$$urlUpdatedByLocation) {
            event.preventDefault();
        }

        /** 
         * Se a variável de sessão com o token de autenticação não estiver preenchiada
         * e o caminho informado for qualquer coisa maior que "/", redireciona para a 
         * a página de login
         */
        if ($sessionStorage.tokenAuthentication == "" && $location.$$url.length > 1) {
            $location.path('/401');
        }

        $rootScope.actualLocation = $location.path();
    }); 
});

/** Desliga o spinner de loading */
app.config(['cfpLoadingBarProvider', function(cfpLoadingBarProvider) {
    cfpLoadingBarProvider.includeSpinner = false;
}])