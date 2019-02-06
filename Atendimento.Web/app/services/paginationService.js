app.factory("paginationService", function() {
    
    var _getPagination = function(totalItems, currentPage, pageSize) {
        //Define a página corrente
        currentPage = currentPage || 1;

        //O tamanho de página default é 10 se não for informado
        pageSize = pageSize || 10;
        
        // calcula o total de páginas
        var totalPages = Math.ceil(totalItems / pageSize);

        var startPage, endPage;

        if (totalPages <= 10) {
            // Se o total de páginas for menor que 10, mostra tudo
            startPage = 1;
            endPage = totalPages;
        } else {
            // Se o total de páginas for maior que 10, então calcular a página inicial e final do intervalo
            if (currentPage <= 6) {
                startPage = 1;
                endPage = 10;
            } else if (currentPage + 4 >= totalPages) {
                startPage = totalPages - 9;
                endPage = totalPages;
            } else {
                startPage = currentPage - 5;
                endPage = currentPage + 4;
            }
        }

        // calculate o index dos itens inicial e final
        var startIndex = (currentPage - 1) * pageSize;
        var endIndex = Math.min(startIndex + pageSize - 1, totalItems - 1);

        // monto array de páginas para percorrer no ng-repeat
        var pages = range(startPage, endPage);

        // retorna objeto com todas as propriedades usadas na view
        return {
            totalItems: totalItems,
            currentPage: currentPage,
            pageSize: pageSize,
            totalPages: totalPages,
            startPage: startPage,
            endPage: endPage,
            startIndex: startIndex,
            endIndex: endIndex,
            pages: pages
        };
    };

    // var range = function(start, end) {
    //     return (new Array(end - start + 1)).fill(undefined).map((_, i) => i + start);
    // };

    var range = function range(start, end) {
        return new Array(end - start + 1).fill(undefined).map(function (_, i) {
            return i + start;
        });
    };

    return {
        getPagination: _getPagination
    };
});