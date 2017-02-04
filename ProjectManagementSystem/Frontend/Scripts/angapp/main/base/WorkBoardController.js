angapp.controller('WorkBoardController', ['$scope','$sce', '$state', '$stateParams', '$location', 'WorkItemService',
    function ($scope, $sce, $state, $stateParams, $location, WorkItemService) {
        
        WorkItemService.getActualWorkItems().then(function(content) {
            $scope.itemsPerType = content.data;
        });

        //$scope.getUserString = function(user) {
        //    var str = user.Name;
        //    if (user.Surname) {
        //        str += ' ' + user.Surname;
        //    }
        //    return str;
        //};

        //$scope.edit = function(itemId) {
        //    $state.go('base.edit', { workItemId: itemId, returnStateName: $state.$current.name });
        //};

        //$scope.getWorkItemClass = function (item) {
        //    var today = moment();
        //    var deadline = moment(item.DeadLine, 'DD.MM.YYYY HH:mm');
        //    if (deadline.isBefore(today)) {
        //        return 'warn-color';
        //    }
        //    return WorkItemService.getWorkItemTypeName(item.Type)+'-color';
        //};
    }]);