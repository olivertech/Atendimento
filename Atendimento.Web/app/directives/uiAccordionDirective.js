app.directive("uiAccordions", function () {
    return {
        controller: function($scope, $element, $attrs) {

            var accordions = [];
            var current = 0;

            this.registerAccordions = function(accordion) {
                accordions.push(accordion);
            };

            this.closeAll = function() {
                accordions.forEach(function(accordion) {
                    accordion.isOpened = false;
                });
            };
        }
    };
});

app.directive("uiAccordion", function () {
    return {
        templateUrl: "app/views/accordion.html",
        transclude: true,
        scope: {
            title: "@",
            id: "@"
        },
        require: "^uiAccordions",
        link: function(scope, element, attrs, ctrl) {
            
            ctrl.registerAccordions(scope);

            scope.open = function() {
                ctrl.closeAll();

                if (ctrl.current != scope.id && !scope.isOpened) {
                    ctrl.current = scope.id;
                    scope.isOpened = true;
                }
                else {
                    ctrl.current = 0;
                    scope.isOpened = false;
                }
            }
        }
    };
});