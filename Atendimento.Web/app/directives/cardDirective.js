app.directive("card", function () {
    return {
        templateUrl: "app/views/card.html",
        replace: true,
        restrict: "E",
        scope: {
            css: "@",
            total: "@",
            text: "@",
            id: "@",
            idstatus: "@",
            numberscss: "@",
            textcss: "@"
        }
    };
});

/**
 *  bg-primary:   $orange,
 *  bg-secondary: $gray-100,
 *  bg-success:   $green,
 *  bg-info:      $blue,
 *  bg-warning:   $yellow,
 *  bg-danger:    $red,
 *  bg-light:     $gray-100,
 *  bg-cyan:      $cyan,
 *  bg-dark:      $gray-800,
 *  bg-purple:    $purple
 **/