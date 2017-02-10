angapp.controller('WorkItemTileController', ['$scope', '$state', '$stateParams', '$location', 'WorkItemService',
    function ($scope, $state, $stateParams, $location, WorkItemService) {
        var dateFormat = 'DD.MM.YYYY HH:mm';
       

        $scope.getUserString = function (user) {
            var str = user.Name;
            if (user.Surname) {
                str += ' ' + user.Surname;
            }
            return str;
        };

        $scope.edit = function (itemId) {
            var returnStates = [{ name: $state.$current.name }];
            $state.go('base.edit', { workItemId: itemId, returnStates : returnStates });
        };

        $scope.getWorkItemClass = function (item) {
            return WorkItemService.getWorkItemTypeName(item.Type) + '-color';
        };
        $scope.isDeadlineSoon = function(workItem) {
            var today = moment();
            var deadline = moment(workItem.DeadLine, dateFormat);
            var duration = moment.duration(today.diff(deadline));
            return WorkItemService.isItemInWork(workItem) && !deadline.isBefore(today) && duration.asHours() < 48;
        };
        $scope.isDeadlineHappend = function(workItem) {
            var today = moment();
            var deadline = moment(workItem.DeadLine, dateFormat);
            return WorkItemService.isItemInWork(workItem) && deadline.isBefore(today);
        };
    }]);