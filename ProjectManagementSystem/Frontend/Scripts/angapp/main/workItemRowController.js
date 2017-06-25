angapp.controller('WorkItemRowController', ['$scope', '$state', '$stateParams', 'WorkItemService', "UsersService", 'Utils',
    function ($scope, $state, $stateParams, WorkItemService, UsersService, Utils) {
       
        $scope.getWorkItemTypeName = function (workItemType) {
            return WorkItemService.getWorkItemTypeName(workItemType).toLowerCase();
        };

        $scope.getExecutorText = function (workItemType) {
            return WorkItemService.getExecutorText(workItemType);
        };

        $scope.getUserInfo = function (workItem) {
            return Utils.getUserInfo(workItem.Executor);
        };

        $scope.getDeadlineText = function (workItem) {
            return WorkItemService.getDeadlineText(workItem);
        };
        
        
        $scope.addChild = function (parentItem) {
            var params = { type: parentItem.Type + 1 };
            switch (parentItem.Type) {
            case 1:
                params.ProjectId = parentItem.Id;
                break;
            case 2:
                params.StageId = parentItem.Id;
                params.ProjectId = parentItem.Parent.Id;
                break;
            case 3:
                params.PartitionId = parentItem.Id;
                params.StageId = parentItem.Parent.Id;
                params.ProjectId = parentItem.Parent.Parent.Id;
                break;
            }
            Utils.goToState('base.add', params, $stateParams);
        };
        $scope.edit = function (workItem) {
            Utils.goToState('base.edit', { workItemId: workItem.Id }, $stateParams);
            
        };
       

        $scope.expand = function (workItem) {
            workItem.expanded = !workItem.expanded;
        };
        $scope.getWorkItemChildTypeDescription = function (workItemType) {
            switch (workItemType) {
            case 1:
                return 'стадию';
            case 2:
                return 'раздел';
            case 3:
                return 'задачу';
            }
            return 'задачу';
        };
    }]);