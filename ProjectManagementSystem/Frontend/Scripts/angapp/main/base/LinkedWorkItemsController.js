angapp.controller('LinkedWorkItemsController', [
    '$scope', '$state', '$stateParams', '$mdDialog', 'WorkItemService', 'Utils',
    function($scope, $state, $stateParams, $mdDialog, WorkItemService, Utils) {
        function onError(err) {
            console.error(err);
        };

        if (!$scope.IsNew) {
            WorkItemService.getLinkedItemsForItem($stateParams.workItemId).then(function(content) {
                $scope.linkedItemsCollection = content.data;
            }, onError);
        }

        $scope.edit = function (itemId) {
            Utils.goToState('base.edit', { workItemId: itemId }, $stateParams);
        }

        $scope.addChildren = function () {
            var params = { type: $scope.WorkItem.Type + 1 };
            switch($scope.WorkItem.Type) {
                case 1:
                    params.projectId = $scope.WorkItem.Id;
                    break;
                case 2:
                    params.projectId = $scope.parentProjectId;
                    params.stageId = $scope.WorkItem.Id;
                    break;
                case 3:
                    params.projectId = $scope.parentProjectId;
                    params.stageId = $scope.parentStageId;
                    params.partitionId = $scope.WorkItem.Id;
                    break;
            }
            Utils.goToState('base.add', params, $stateParams);
        }

        $scope.getWorkItemClass = function (item) {
           return WorkItemService.getWorkItemTypeName(item.Type) + '-color';
        };
    }
]);