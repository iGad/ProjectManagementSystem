angapp.controller('WorkItemController', [
    '$scope', '$q', '$state', '$stateParams', '$mdDialog', 'UsersService', 'WorkItemService', 'Utils',
    function ($scope, $q, $state, $stateParams, $mdDialog, UsersService, WorkItemService, Utils) {
        var autofills, allAutofills, buffer;

        function goToReturnState() {
            Utils.goToReturnState($stateParams);
        }

        function getStates() {
            WorkItemService.getStates().then(function (content) {
                $scope.states = content.data;
                if (!$scope.isNew && !$scope.states.filter(x => x.Value === $scope.workItem.State).length) {
                    $scope.states.splice(0, 0, $scope.workItem.StateViewModel);
                }
            }, Utils.onError);
        };

        function getUsers(typeId) {
            UsersService.getAllowedUsersForWorkItemType(typeId).then(function (content) {
                $scope.users = angular.fromJson(content.data);
                if (!$scope.isNew && $scope.workItem.ExecutorId && !$scope.users.filter(x => x.Id === $scope.workItem.ExecutorId).length) {
                    $scope.users.splice(0, 0, $scope.workItem.Executor);
                }
            }, Utils.onError);
        };

        function getAutofills() {
            WorkItemService.getAutofills().then(function(content) {
                allAutofills = content.data;
                autofills = allAutofills.filter(x => x.WorkItemType === $scope.workItem.Type);
            }, Utils.onError);
        };

        function getTypes() {
            WorkItemService.getTypes().then(function (content) {
                $scope.types = content.data;
                getAutofills();
                if (!$scope.workItem.Type) {
                    $scope.workItem.Type = $scope.types[1].Value; //TODO: сделать понятнее
                } else
                    if (!$scope.types.filter(x => x.Value === $scope.workItem.Type).length) {
                        $scope.types.splice(0, 0, $scope.workItem.TypeViewModel);
                    }

                $scope.typeChanged($scope.workItem.Type);
            }, Utils.onError);
        };


        var workItemChangedHandler = $scope.$on("WorkItemChanged", function (event, workItem) {
            if (workItem.Id === $scope.workItem.Id) {
                $mdDialog.show(
                    $mdDialog.alert()
                        .clickOutsideToClose(true)
                        .title('Внимание')
                        .textContent('Данный рабочий элемент был кем-то изменен.\nЕсли вы сохраните изменения, то изменения, внесенные другим пользователем могут быть отменены.\
                  \nРекомендуется сохранить текстовые изменения в текстовый редактор и обновить страницу.')
                        .ariaLabel('Alert Dialog')
                        .ok('ОК')
                );
            }
        });

        $scope.$on("$destroy", function () {
            workItemChangedHandler();
        });

        function startWatch() {
            $scope.$watch(function () {
                return !$scope.addUpdateWorkItemForm.$invalid && !angular.equals($scope.workItem, buffer);
            }, function (newVal, oldVal) {
                if (newVal != oldVal) {
                    $scope.canSave = newVal;
                }
            });
        }

        if ($stateParams.workItemId) {
            $scope.isNew = false;
            WorkItemService.getWorkItem($stateParams.workItemId).then(function (content) {
                $scope.workItem = angular.fromJson(content.data);
                $scope.workItem.DeadLine = Utils.convertDateToJsDate($scope.workItem.DeadLine);
                $scope.workItem.DeadLineHours = $scope.workItem.DeadLine.getHours();
                $scope.workItem.DeadLineMinutes = $scope.workItem.DeadLine.getMinutes();
                buffer = angular.copy($scope.workItem);
                startWatch();
                getStates();
                getTypes();
            }, Utils.onError);
            //getUserPermissions();
        } else {
            $scope.isNew = true;
            $scope.workItem = {
                DeadLineHours: 17,
                DeadLineMinutes: 0,
                DeadLine: moment().add(1, 'days').toDate(),
                Type: parseInt($stateParams.type)
            };
            $scope.parentPartitionId = $stateParams.partitionId ? parseInt($stateParams.partitionId) : null;
            $scope.parentStageId = $stateParams.stageId ? parseInt($stateParams.stageId) : null;
            $scope.parentProjectId = $stateParams.projectId ? parseInt($stateParams.projectId) : null;
            buffer = angular.copy($scope.workItem);
            startWatch();
            //getUserPermissions();
            getTypes();
            getStates();
        }



        $scope.isFormChanged = false;
        $scope.canEdit = false;
        $scope.canAddChildItem = false;
        $scope.canDelete = false;


        function updatePermissionFlags() {
            $scope.isCurrentUserExecutor = $scope.workItem.ExecutorId === $scope.User.Id;
            //$scope.canEdit = !$scope.isNew && ($scope.isCurrentUserExecutor || $scope.Permissions.CanChangeForeignWorkItem);
            //$scope.canChangeProject = $scope.isNew && $scope.isProject && $scope.Permissions.CanCreateProject || $scope.canEdit;
            //$scope.canChangeStage = $scope.isNew && $scope.isStage && $scope.Permissions.CanCreateStage || $scope.canEdit;
            //$scope.canChangePartition = $scope.isNew && $scope.isPartition && $scope.Permissions.CanCreatePartition || $scope.canEdit;
            //$scope.canChangeTask = $scope.isNew && $scope.isTask && $scope.Permissions.CanCreateTask || $scope.canEdit;
            $scope.canEdit = $scope.isCurrentUserExecutor || ($scope.isNew || $scope.Permissions.CanChangeForeignWorkItem) && ($scope.isProject && $scope.Permissions.CanCreateProject ||
                $scope.isStage && $scope.Permissions.CanCreateStage ||
                $scope.isPartition && $scope.Permissions.CanCreatePartition ||
                $scope.isTask && $scope.Permissions.CanCreateTask);
            $scope.canAddChildItem = !$scope.isNew && ($scope.isProject && $scope.Permissions.CanCreateStage ||
                $scope.isStage && $scope.Permissions.CanCreatePartition ||
                $scope.isPartition && $scope.Permissions.CanCreateTask);
            $scope.canEditExecutor = $scope.isTask || $scope.canEdit;
            //$scope.canDelete = !$scope.isNew && ($scope.isProject && $scope.Permissions.CanDeleteProject ||
            //    $scope.isStage && $scope.Permissions.CanDeleteStage ||
            //    $scope.isPartition && $scope.Permissions.CanDeletePartition ||
            //    $scope.isTask && $scope.Permissions.CanDeleteTask);
        };


        $scope.isDisabledEdit = function () {
            return $scope.IsNew && !$scope.CanAdd || !$scope.isNew && !$scope.canEdit;
        };
        

        Object.defineProperty($scope, 'IsNew', {
            get: function () {
                return $scope.isNew;
            }
        });

        Object.defineProperty($scope, 'CanEdit', {
            get: function () {
                return $scope.canEdit;
            }
        });

        Object.defineProperty($scope, 'WorkItem', {
            get: function () {
                return $scope.workItem;
            }
        });

        Object.defineProperty($scope, 'ObjectId', {
            get: function () {
                return $stateParams.workItemId;
            }
        });

        function setFlags(workItemType) {
            $scope.isProject = false;
            $scope.isStage = false;
            $scope.isPartition = false;
            $scope.isTask = false;
            $scope.workItemName = 'Название';
            $scope.workItemExecutorName = 'Менеджер';
            switch (workItemType) {
                case $scope.types[0].Value:
                    $scope.isProject = true;
                    $scope.workItemName = 'Номер';
                    $scope.workItemExecutorName = 'ГИП';
                    break;
                case $scope.types[1].Value:
                    $scope.isStage = true;
                    break;
                case $scope.types[2].Value:
                    $scope.isPartition = true;
                    break;
                case $scope.types[3].Value:
                    $scope.isTask = true;
                    $scope.workItemExecutorName = 'Исполнитель';
                    break;
            }
            updatePermissionFlags();
        }

        $scope.formChanged = function () {
            $scope.isFormChanged = true;
        };

        $scope.getUserDisplayText = function (user) {
            return Utils.getUserInfo(user);
        };

        function getPartitions(stageId) {
            WorkItemService.getChildItems(stageId).then(function (content) {
                $scope.partitions = angular.fromJson(content.data);
                if ($scope.partitions.length) {
                    if ($scope.workItem.PartitionId && $scope.partitions.filter(x => x.Id === $scope.workItem.PartitionId).length) {
                        $scope.parentPartitionId = $scope.workItem.PartitionId;
                    } else if (!$scope.parentPartitionId) {
                        $scope.parentPartitionId = $scope.partitions[0].Id;
                    }
                }
            }, Utils.onError);
        }

        function getStages(projectId) {
            WorkItemService.getChildItems(projectId).then(function (content) {
                $scope.stages = angular.fromJson(content.data);
                if ($scope.stages.length) {
                    if ($scope.workItem.StageId && $scope.stages.filter(x => x.Id === $scope.workItem.StageId).length) {
                        $scope.parentStageId = $scope.workItem.StageId;
                    } else if (!$scope.parentStageId) {
                        $scope.parentStageId = $scope.stages[0].Id;
                    }
                    $scope.stageChanged($scope.parentStageId);
                }
            }, Utils.onError);
        }

        function getProjects() {
            WorkItemService.getProjects().then(function (content) {
                $scope.projects = angular.fromJson(content.data);
                if ($scope.projects.length) {
                    if ($scope.workItem.ProjectId && $scope.projects.filter(x => x.Id === $scope.workItem.ProjectId).length) {
                        $scope.parentProjectId = $scope.workItem.ProjectId;
                    } else if (!$scope.parentProjectId) {
                        $scope.parentProjectId = $scope.projects[0].Id;
                    }
                    $scope.projectChanged($scope.parentProjectId);
                }
            }, Utils.onError);
        }




        $scope.typeChanged = function (typeId) {
            if (typeId) {
                setFlags(typeId);
                getUsers(typeId);
                if (allAutofills)
                    autofills = allAutofills.filter(x => x.WorkItemType === $scope.workItem.Type);
                if (typeId !== $scope.types[0].Value && !$scope.projects)
                    getProjects();
            }
        };

        $scope.projectChanged = function (projectId) {
            if (projectId) {
                getStages(projectId);
            }
            else
                $scope.stages = [];
        };

        $scope.stageChanged = function (stageId) {
            if (stageId) {
                getPartitions(stageId);
            }
            else
                $scope.partitions = [];
        }

        $scope.save = function () {
            var parentId;
            switch ($scope.workItem.Type) {
                case $scope.types[1].Value:
                    parentId = $scope.parentProjectId;
                    break;
                case $scope.types[2].Value:
                    parentId = $scope.parentStageId;
                    break;
                case $scope.types[3].Value:
                    parentId = $scope.parentPartitionId;
                    break;
            }
            $scope.workItem.ParentId = parentId;
            $scope.workItem.DeadLine.setHours($scope.workItem.DeadLineHours);
            $scope.workItem.DeadLine.setMinutes($scope.workItem.DeadLineMinutes);
            $scope.workItem.DeadLine.setSeconds(0);
            if (!$scope.isNew) {
                workItemChangedHandler();
                workItemChangedHandler = undefined;
                WorkItemService.updateWorkItem($scope.workItem).then(function () {
                    goToReturnState();
                }, Utils.onErrorWithMessageBox);
            } else {
                WorkItemService.addWorkItem($scope.workItem).then(function () {
                    goToReturnState();
                }, Utils.onErrorWithMessageBox);
            }
        };

        function showDialog(ev, title, text, callback, cancel) {
            var confirm = $mdDialog.confirm()
                .title(title)
                .textContent(text)
                .ariaLabel('are you sure')
                .targetEvent(ev)
                .ok('Да')
                .cancel('Нет');

            $mdDialog.show(confirm).then(function () {
                callback();
            }, cancel());
        };

        $scope.querySearch = function (searchText) {
            var defer = $q.defer();
            if (autofills && autofills.length) {
                defer.resolve(autofills.filter(x => !searchText ||
                    x.Name.indexOf(searchText) !== -1));
            } else {
                defer.resolve([]);
            }
            return defer.promise;
        };

        $scope.selectedAutofillChange = function (autofill) {
            if (autofill) {
                $scope.workItem.Name = autofill.Name;
                $scope.workItem.Description = autofill.Description;
            }
        };

        $scope.cancel = function (ev) {
            if (!$scope.canSave) {
                goToReturnState();
                return;
            }
            showDialog(ev, 'Вы уверены, что хотите выйти?', 'Все изменения будут потеряны', function () {
                goToReturnState();
            }, function () { });
        };

        $scope.delete = function (ev) {
            showDialog(ev, 'Вы уверены, что хотите удалить элемент?', 'Все данные об этом элементе будут удалены', function () {
                WorkItemService.deleteWorkItem($scope.workItem.Id).then(goToReturnState, Utils.onErrorWithMessageBox);
            }, function () { });
        };
    }]);