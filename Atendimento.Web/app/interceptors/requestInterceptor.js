app.factory('requestInterceptor', ['$sessionStorage', '$location',
  function ($sessionStorage, $location) {
    return {
      request: function (config) {

        /** 
         * Envia o token de authentication apenas nas requisições pós login
         */
        if (config.url.indexOf('api/Login') === -1 && config.url.indexOf('views/') === -1) {

            //Recupera a token de autenticação da session storage e insere no header do request
            config.headers.Authorization = $sessionStorage.tokenAuthentication;
        }

        config.headers.Accept = 'application/json, text/plain, */*';
        config.headers.Pragma = 'no-cache';

        if(config.url.indexOf('api/FileUpload') === -1)
          config.headers['Content-Type'] = 'application/json';
        else
          config.headers['Content-Type'] = undefined;

        return config;
      }
    };
}]);  