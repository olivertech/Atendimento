//usando ngRoute
// app.config(function($routeProvider, $locationProvider) {

//     $routeProvider
//         .when("/", {
//             templateUrl: "app/views/login.html",
//             controller: "loginController"
//         });

//     $routeProvider
//         .when("/dashboard", {
//             templateUrl: "app/views/dashboard.html",
//             controller: "dashboardController"
//         });

//     $routeProvider
//         .when("/error", {
//             templateUrl: "app/views/error.html"
//         });  

//     $routeProvider.otherwise({redirectTo: "/"});

//     // use the HTML5 History API
//     $locationProvider.html5Mode(true);
// });

//Usando ui-route
app.config(function($stateProvider, $urlRouterProvider, $locationProvider) {

    $urlRouterProvider.otherwise("/");

    $stateProvider

        .state('login', {
            url: '/:statusCode',
            templateUrl: 'app/views/login.html',
            cache: true,
            controller: function($scope, $stateParams) {
                // Recupero o status code caso tenha sido passado
                $scope.statusCode = $stateParams.statusCode;
              }
        })
        
        .state('dashboard', {
            url: '/dashboard',
            templateUrl: 'app/views/dashboard.html',
            cache: false,
            controller: 'dashboardController'
        })

        .state('tickets', {
            url: '/tickets/:idTicket',
            templateUrl: 'app/views/ticket.html',
            cache: false,
            controller: function($scope, $stateParams) {
                // Recupero o status code caso tenha sido passado
                $scope.idTicket = $stateParams.idTicket;
              }
        })

        .state('templates', {
            url: '/templates',
            templateUrl: 'app/views/templates.html',
            cache: false,
            controller: 'templateController'
        })

        .state('empresas', {
            url: '/empresas',
            templateUrl: 'app/views/empresa.html',
            cache: false,
            controller: 'empresaController'
        })

        .state('atendentes', {
            url: '/atendentes',
            templateUrl: 'app/views/atendenteEmpresa.html',
            cache: false,
            controller: 'atendenteEmpresaController'
        })

        .state('clientes', {
            url: '/clientes',
            templateUrl: 'app/views/cliente.html',
            cache: false,
            controller: 'clienteController'
        })

        .state('usuarios', {
            url: '/usuarios',
            templateUrl: 'app/views/usuarioCliente.html',
            cache: false,
            controller: 'usuarioController'
        })

        .state('error', {
            url: '/error',
            cache: false,
            templateUrl: 'app/views/error.html',
            controller: 'errorController'
        })

        .state('authorizederror', {
            url: '/authorizederror',
            cache: false,
            templateUrl: 'app/views/authorizederror.html',
            controller: 'errorController'
        })

    // use the HTML5 History API
    $locationProvider.html5Mode(true);
});