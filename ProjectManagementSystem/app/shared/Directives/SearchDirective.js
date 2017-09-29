'use strict';

(function () {
    var DIRECTIVE_NAME = 'search';

    angapp
        .directive(DIRECTIVE_NAME, ['$timeout', '$rootScope',
            function ($timeout, $rootScope) {
                return {
                    restrict: 'A',
                    scope: {
                        params: '=' + DIRECTIVE_NAME
                    },
                    link: function (scope, element, attrs) {
                        var timeout = scope.params && scope.params.timeout || 2000,
                            event = scope.params && scope.params.event || 'search',
                            broadcast = scope.params && !!scope.params.broadcast || false,
                            timer;

                        function elementChangeHandler() {
                            var newValue = element.val();

                            if (element.data('old-value') != newValue) {
                                if (timer) {
                                    $timeout.cancel(timer);
                                }

                                timer = $timeout(function () {
                                    element.data('old-value', newValue);

                                    signal(newValue);
                                }, timeout);
                            }
                        };
                        function elementEnterHandler(evt) {
                            if (evt.which === 13) {
                                var newValue = element.val();

                                if (timer) {
                                    $timeout.cancel(timer);
                                }

                                scope.$apply(function () {
                                    element.data('old-value', newValue);

                                    signal(newValue);
                                });
                            }
                        };
                        function signal(value) {
                            if (angular.isString(value)) {
                                if (broadcast) {
                                    $rootScope.$broadcast(event, value);
                                } else {
                                    scope.$emit(event, value);
                                }
                            }
                        };

                        element.on('propertychange keyup paste', elementChangeHandler);
                        element.on('keydown keypress', elementEnterHandler);

                        scope.$on('$destroy', function () {
                            element.off('propertychange keyup paste', elementChangeHandler);
                            element.off('keydown keypress', elementEnterHandler);

                            if (timer) {
                                $timeout.cancel(timer);
                            }
                        });
                    }
                }
            }]);
})();