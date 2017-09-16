angapp.controller('ProfileIndexController',
    ['$scope', '$state', '$stateParams', '$location',
    function ($scope, $state, $stateParams, $location) {

        $scope.goTo = function (stateName) {
            $state.go(stateName);
        };

        if ($location.path().indexOf('settings') > 0)
            $scope.selectedTab = 'settings';
        else
            $scope.selectedTab = 'info';

    }]);