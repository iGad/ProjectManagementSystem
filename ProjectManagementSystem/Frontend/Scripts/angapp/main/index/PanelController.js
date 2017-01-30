angapp.controller('PanelController', [
    '$scope', '$state', 'WorkItemService', '$timeout', "uiGridConstants", function ($scope, $state, WorkItemService, $timeout, uiGridConstants) {
        $scope.add = function() {
            $state.go('base.main');
        };

        WorkItemService.getProjects().then(function(content) {
            $scope.projects = angular.fromJson(content.data);
        });
    }]);