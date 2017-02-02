angapp.controller('PanelController', [
    '$scope', '$state', 'WorkItemService', '$timeout', "uiGridConstants", function ($scope, $state, WorkItemService, $timeout, uiGridConstants) {
        $scope.add = function() {
            $state.go('base.edit');
        };

        WorkItemService.getProjects().then(function(content) {
            $scope.projects = angular.fromJson(content.data);
        });

        $scope.edit = function(workItem) {
            $state.go('base.edit', { workItemId: workItem.Id });
        }
    }]);