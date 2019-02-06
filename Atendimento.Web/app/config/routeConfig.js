app.config(['$stateProvider', '$urlRouterProvider', '$locationProvider',
    function($stateProvider, $urlRouterProvider, $locationProvider) {

    //var baseUrl = $("base").first().attr("href");

    $urlRouterProvider.otherwise("/");

    $stateProvider

        .state('login', {
            url: '/:statusCode',
            //templateUrl: baseUrl + 'app/views/login.html',
            templateUrl: 'app/views/login.html',
            cache: true,
            controller: ['$scope', '$stateParams', function($scope, $stateParams) {
                // Recupero o status code caso tenha sido passado
                $scope.statusCode = $stateParams.statusCode;
              }]
        })
        
        .state('dashboard', {
            url: '/dashboard',
            //templateUrl: baseUrl + 'app/views/dashboard.html',
            templateUrl: 'app/views/dashboard.html',
            cache: false,
            controller: 'dashboardController'
        })

        .state('tickets', {
            url: '/tickets/:idTicket',
            //templateUrl: baseUrl + 'app/views/ticket.html',
            templateUrl: 'app/views/ticket.html',
            cache: false,
            controller: ['$scope', '$stateParams', function($scope, $stateParams) {
                // Recupero o status code caso tenha sido passado
                $scope.idTicket = $stateParams.idTicket;
              }]
        })

        .state('templates', {
            url: '/templates',
            //templateUrl: baseUrl + 'app/views/templates.html',
            templateUrl: 'app/views/templates.html',
            cache: false,
            controller: 'templateController'
        })

        .state('empresas', {
            url: '/empresas',
            //templateUrl: baseUrl + 'app/views/empresa.html',
            templateUrl: 'app/views/empresa.html',
            cache: false,
            controller: 'empresaController'
        })

        .state('atendentes', {
            url: '/atendentes',
            //templateUrl: baseUrl + 'app/views/atendenteEmpresa.html',
            templateUrl: 'app/views/atendenteEmpresa.html',
            cache: false,
            controller: 'atendenteEmpresaController'
        })

        .state('clientes', {
            url: '/clientes',
            //templateUrl: baseUrl + 'app/views/cliente.html',
            templateUrl: 'app/views/cliente.html',
            cache: false,
            controller: 'clienteController'
        })

        .state('usuarios', {
            url: '/usuarios',
            //templateUrl: baseUrl + 'app/views/usuarioCliente.html',
            templateUrl: 'app/views/usuarioCliente.html',
            cache: false,
            controller: 'usuarioController'
        })

        .state('categorias', {
            url: '/categorias',
            //templateUrl: baseUrl + 'app/views/categoria.html',
            templateUrl: 'app/views/categoria.html',
            cache: false,
            controller: 'categoriaController'
        })

        .state('error', {
            url: '/error',
            cache: false,
            //templateUrl: baseUrl + 'app/views/error.html',
            templateUrl: 'app/views/error.html',
            controller: 'errorController'
        })

        .state('authorizederror', {
            url: '/authorizederror',
            cache: false,
            //templateUrl: baseUrl + 'app/views/authorizederror.html',
            templateUrl: 'app/views/authorizederror.html',
            controller: 'errorController'
        });

    //use the HTML5 History API - Esse recurso não funciona com o site publicado no IIS
    //$locationProvider.html5Mode(true);
}]);