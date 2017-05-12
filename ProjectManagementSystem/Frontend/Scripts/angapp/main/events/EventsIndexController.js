angapp.controller('EventsIndexController',
    ['$scope', '$state', '$stateParams', '$location',
    function ($scope, $state, $stateParams, $location) {

        $scope.goTo = function (stateName) {
            $state.go('base.events.' + stateName);
        };

        if ($location.path().indexOf('seen') > 0)
            $scope.selectedTab = 'seen';
        else
            $scope.selectedTab = 'new';

    }]);