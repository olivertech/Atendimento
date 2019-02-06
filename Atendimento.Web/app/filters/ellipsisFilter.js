app.filter("ellipsis", function() {
    return function(input, size) {
        if (input.length <= size) return input;

        //Aqui no (size || 4) eu leio como se o size vier como
        //zero, undefined, null, seta o valor pra 4
        var ouput = input.substring(0, (size || 4)) + "...";
        return ouput;
    };
});