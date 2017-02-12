angapp.controller('WorkItemTileController', ['$scope', '$state', '$stateParams', '$location', 'WorkItemService', 'Utils',
    function ($scope, $state, $stateParams, $location, WorkItemService, Utils) {
        var dateFormat = 'DD.MM.YYYY HH:mm';
       

        $scope.getUserString = function (user) {
            return WorkItemService.getUserDisplayText(user);
        };

        $scope.edit = function (itemId) {
            Utils.goToState('base.edit', { workItemId: itemId }, $stateParams);
            //var returnStates = [{ name: $state.$current.name }];
            //$state.go('base.edit', { workItemId: itemId, returnStates : returnStates });
        };

        $scope.getWorkItemClass = function (item) {
            return WorkItemService.getWorkItemTypeName(item.Type) + '-color';
        };
     
    }]);