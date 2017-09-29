angapp.controller('DictionariesController', [
    '$scope', '$state', '$location',
    function ($scope, $state, $location) {
        var tab;
        Object.defineProperty($scope,
            'selectedTab',
            {
                get: function() { return tab; },
                set: function(val) { tab = val; }
            });

    }]);