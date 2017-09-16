angapp.controller('WorkBoardController',
    ['$scope', '$state', '$stateParams', '$timeout', '$location', 'WorkItemService', 'Utils',
    function ($scope, $state, $stateParams, $timeout, $location, workItemService, utils) {

        $scope.goTo = function(stateName) {
            utils.goToState(stateName, {}, $stateParams);
        };

        if ($location.path().indexOf('users') > 0)
            $scope.selectedTab = 'users';
        else
            $scope.selectedTab = 'all';

    }]);