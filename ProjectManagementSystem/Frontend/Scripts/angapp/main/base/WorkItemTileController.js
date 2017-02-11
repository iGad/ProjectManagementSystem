angapp.controller('WorkItemTileController', ['$scope', '$state', '$stateParams', '$location', 'WorkItemService',
    function ($scope, $state, $stateParams, $location, WorkItemService) {
        var dateFormat = 'DD.MM.YYYY HH:mm';
       

        $scope.getUserString = function (user) {
            return WorkItemService.getUserDisplayText(user);
        };

        $scope.edit = function (itemId) {
            var returnStates = [{ name: $state.$current.name }];
            $state.go('base.edit', { workItemId: itemId, returnStates : returnStates });
        };

        $scope.getWorkItemClass = function (item) {
            return WorkItemService.getWorkItemTypeName(item.Type) + '-color';
        };
     
    }]);