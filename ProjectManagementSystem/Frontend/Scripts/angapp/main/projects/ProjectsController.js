angapp.controller('ProjectsController', ['$scope', '$state', '$stateParams', 'WorkItemService', "UsersService",
    function ($scope, $state, $stateParams, WorkItemService, UsersService) {
        var onError = function(err) {
            console.error(err);
        };
        var filterProjects = function(projects, options) {
            var result = [];
            for (var i = 0; i < projects.length; i++) {
                if ((options.showArchive || projects[i].State.Value !== 4) && (options.showDone || projects[i].State.Value !== 2)) {
                    result.push(projects[i]);
                }
            }
            return result;
        };
        $scope.filterOptions = {
            showArchive: true,
            showDone: true
        };
        $scope.getWorkItemTypeName = function (workItemType) {
            return WorkItemService.getWorkItemTypeName(workItemType).toLowerCase();
        };

        $scope.getExecutorText = function(workItemType) {
            return WorkItemService.getExecutorText(workItemType);
        };

        $scope.getUserDisplayText = function(workItem) {
            return WorkItemService.getUserDisplayText(workItem.Executor);
        };

        $scope.getDeadlineText = function(workItem) {
            return WorkItemService.getDeadlineText(workItem);
        };

        function getProjects(options) {
            if (!$scope.allProjects) {
                WorkItemService.getProjectsTree().then(function(content) {
                    $scope.allProjects = content.data;
                    $scope.projects = filterProjects($scope.allProjects, options);
                }, onError);
            } else {
                $scope.projects = filterProjects($scope.allProjects, options);
            }
        };

        getProjects($scope.filterOptions);

        $scope.$on("WorkItemChanged", function (event, workItem) {
            getProjects($scope.filterOptions);
            
        });


        $scope.addProject = function () {
            var params = { type: 1 };
            params.returnStates = [{ name: 'base.projects', params: angular.copy($stateParams) }];
            $state.go('base.add', params);
        };
        $scope.addChild = function(parentItem) {
            var params = { type: parentItem.Type + 1 };
            switch(parentItem.Type) {
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
            params.returnStates = [{ name: 'base.projects', params: angular.copy($stateParams) }];
            $state.go('base.add', params);
        };
        $scope.edit = function(workItem) {
            $state.go('base.edit', { workItemId: workItem.Id, returnStates: [{ name: 'base.projects', params: angular.copy($stateParams) }] });
        };
        $scope.showArchiveChanged = function() {
            //$scope.filterOptions.showArchive = !$scope.filterOptions.showArchive;
            getProjects($scope.filterOptions);
        };
        $scope.showDoneChanged = function() {
            //$scope.filterOptions.showDone = !$scope.filterOptions.showDone;
            getProjects($scope.filterOptions);
        };

        $scope.expand = function(workItem) {
            workItem.expanded = !workItem.expanded;
        };
        $scope.getWorkItemChildTypeDescription = function(workItemType) {
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