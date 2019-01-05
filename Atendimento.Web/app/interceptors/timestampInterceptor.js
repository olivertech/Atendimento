app.factory('timestampInterceptor', function() {
    return {
        request: function(config) {
            var url = config.url;
            //Esse if desconsidera as requisições de views
            if (url.indexOf("view") > -1) return config;

            //Peço o tempo em milisegundos
            var timestamp = new Date().getTime();

            if (url.indexOf("?") == -1)
                config.url = url + "?timestamp=" + timestamp;
            else
                config.url = url + "&timestamp=" + timestamp;

            //console.log(config.url);
            return config;
        }
    };
});