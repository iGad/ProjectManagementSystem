angapp.controller('LinkedWorkItemsController', [
    '$scope', '$state', '$stateParams', '$mdDialog', 'WorkItemService',
    function($scope, $state, $stateParams, $mdDialog, WorkItemService) {
        function onError(err) {
            console.error(err);
        };

        function goToWorkItemState(params) {
            var returnStates = $stateParams.returnStates;
            returnStates.splice(0, 0, { name: 'base.edit', params: angular.copy($stateParams) });
            params.returnStates = returnStates;
            $state.go('base.edit', params);
        }

        WorkItemService.getLinkedItemsForItem($stateParams.workItemId).then(function(content) {
            $scope.linkedItemsCollection = content.data;
        }, onError);

        $scope.edit = function(itemId) {
            goToWorkItemState({ workItemId: itemId });
        }

        $scope.addChildren = function() {
            goToWorkItemState({
                projectId: $scope.WorkItem.ProjectId,
                stageId: $scope.WorkItem.StageId,
                partitionId: $scope.WorkItem.PartitionId
            });
        }

        $scope.getWorkItemClass = function (item) {
           return WorkItemService.getWorkItemTypeName(item.Type) + '-color';
        };
    }
]);