﻿angapp.controller('WorkItemController', ['$scope', '$state', '$stateParams', '$mdDialog', 'UsersService', 'WorkItemService', 'Utils',
     function ($scope, $state, $stateParams, $mdDialog, UsersService, WorkItemService, Utils) {
         function onError(err) {
             console.error(err);
         };
         function goToReturnState() {
             Utils.goToReturnState($stateParams);
             //var returnStates = $stateParams.returnStates;
             //if (returnStates && returnStates.length) {
             //    var returnState = returnStates.splice(0, 1)[0];
             //    var params = returnState.params ? returnState.params : {};
             //    params.returnStates = returnStates;
             //    $state.go(returnState.name, params);
             //} else {
             //    $state.go('base.main');
             //}
         }
         function getStates() {
             WorkItemService.getStates().then(function (content) {
                 $scope.states = content.data;
             });
         };

         function getUsers(typeId) {
             UsersService.getAllowedUsersForWorkItemType(typeId).then(function (content) {
                 $scope.users = angular.fromJson(content.data);
             });
         };

         function getTypes() {
             WorkItemService.getTypes().then(function (content) {
                 $scope.types = angular.fromJson(content.data);
                 if (!$scope.workItem.Type) {
                     $scope.workItem.Type = $scope.types[1].Id;
                 }
                 $scope.typeChanged($scope.workItem.Type);
             });
         };

         getStates();

         $scope.isFormChanged = false;
         $scope.canEdit = false;
         $scope.canAdd = false;

         if ($stateParams.workItemId) {
             $scope.isNew = false;
             WorkItemService.getWorkItem($stateParams.workItemId).then(function (content) {
                 $scope.workItem = angular.fromJson(content.data);
                 $scope.workItem.DeadLine = moment($scope.workItem.DeadLine).toDate();
                 $scope.workItem.DeadLineHours = $scope.workItem.DeadLine.getHours();
                 $scope.workItem.DeadLineMinutes = $scope.workItem.DeadLine.getMinutes();
                 getTypes();
             }, onError);
             UsersService.hasPermissionsForWorkItem([1, 2], $stateParams.workItemId).then(function (content) {
                 var permissions = content.data;
                 $scope.canAdd = permissions[0];
                 $scope.canEdit = permissions[0] && permissions[1];
             }, onError);
         } else {
             $scope.isNew = true;
             $scope.workItem = {
                 DeadLineHours: 17,
                 DeadLineMinutes: 0,
                 DeadLine: moment().add(1, 'days').toDate(),
                 Type: $stateParams.type
             };
             $scope.parentPartitionId = $stateParams.partitionId;
             $scope.parentStageId = $stateParams.stageId;
             $scope.parentProjectId = $stateParams.projectId;
             UsersService.hasPermissions([1, 2]).then(function (content) {
                 var permissions = content.data;
                 $scope.canAdd = permissions[0];
                 $scope.canEdit = permissions[0] && permissions[1];
             }, onError);
             getTypes();
         }


         Object.defineProperty($scope, 'IsNew', {
             get: function () {
                 return $scope.isNew;
             }
         });

         Object.defineProperty($scope, 'WorkItem', {
             get: function () {
                 return $scope.workItem;
             }
         });

         function setFlags(workItemType) {
             $scope.isProject = false;
             $scope.isStage = false;
             $scope.isPartition = false;
             $scope.isTask = false;
             $scope.workItemName = 'Название';
             switch (workItemType) {
                 case $scope.types[0].Id:
                     $scope.isProject = true;
                     $scope.workItemName = 'Номер';
                     break;
                 case $scope.types[1].Id:
                     $scope.isStage = true;
                     break;
                 case $scope.types[2].Id:
                     $scope.isPartition = true;
                     break;
                 case $scope.types[3].Id:
                     $scope.isTask = true;
                     break;
             }
         }

         $scope.formChanged = function () {
             $scope.isFormChanged = true;
         };

         $scope.getUserDisplayText = function (user) {
             var text = '';
             if (user.Surname)
                 text = user.Surname + ' ';
             if (user.Name)
                 text += user.Name;
             return text;
         };

         function getPartitions(stageId) {
             WorkItemService.getChildItems(stageId).then(function (content) {
                 $scope.partitions = angular.fromJson(content.data);
                 if ($scope.partitions.length) {
                     if ($scope.workItem.PartitionId && $scope.partitions.indexOf($scope.workItem.PartitionId) >= 0) {
                         $scope.parentPartitionId = $scope.workItem.PartitionId;
                     } else if (!$scope.parentPartitionId) {
                         $scope.parentPartitionId = $scope.partitions[0].Id;
                     }
                 }
             });
         }

         function getStages(projectId) {
             WorkItemService.getChildItems(projectId).then(function (content) {
                 $scope.stages = angular.fromJson(content.data);
                 if ($scope.stages.length) {
                     if ($scope.workItem.StageId && $scope.stages.indexOf($scope.workItem.StageId) >= 0) {
                         $scope.parentStageId = $scope.workItem.StageId;
                     } else if (!$scope.parentStageId) {
                         $scope.parentStageId = $scope.stages[0].Id;
                     }
                     $scope.stageChanged($scope.parentStageId);
                 }
             });
         }

         function getProjects() {
             WorkItemService.getProjects().then(function (content) {
                 $scope.projects = angular.fromJson(content.data);
                 if ($scope.projects.length) {
                     if ($scope.workItem.ProjectsId && $scope.projects.indexOf($scope.workItem.ProjectId) >= 0) {
                         $scope.parentProjectId = $scope.workItem.ProjectId;
                     } else if (!$scope.parentProjectId) {
                         $scope.parentProjectId = $scope.projects[0].Id;
                     }
                     $scope.projectChanged($scope.parentProjectId);
                 }
             });
         }





         $scope.typeChanged = function (typeId) {
             if (typeId) {
                 setFlags(typeId);
                 getUsers(typeId);
                 if (typeId !== $scope.types[0].Id && !$scope.projects)
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
                 case $scope.types[1].Id:
                     parentId = $scope.parentProjectId;
                     break;
                 case $scope.types[2].Id:
                     parentId = $scope.parentStageId;
                     break;
                 case $scope.types[3].Id:
                     parentId = $scope.parentPartitionId;
                     break;
             }
             $scope.workItem.ParentId = parentId;
             $scope.workItem.DeadLine.setHours($scope.workItem.DeadLineHours);
             $scope.workItem.DeadLine.setMinutes($scope.workItem.DeadLineMinutes);
             $scope.workItem.DeadLine.setSeconds(0);
             if (!$scope.isNew) {
                 WorkItemService.updateWorkItem($scope.workItem).then(function () {
                     goToReturnState();
                 }, onError);
             } else {
                 WorkItemService.addWorkItem($scope.workItem).then(function () {
                     goToReturnState();
                 }, onError);
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
         }

         $scope.cancel = function (ev) {
             if (!$scope.isFormChanged) {
                 goToReturnState();
                 return;
             }
             showDialog(ev, 'Вы уверены, что хотите выйти?', 'Все изменения будут потеряны', function () {
                 goToReturnState();
             }, function () { });
         };

         $scope.delete = function (ev) {
             showDialog(ev, 'Вы уверены, что хотите удалить элемент?', 'Все данные о работе будут удалены', function () {
                 WorkItemService.deleteWorkItem($scope.workItem.Id).then(goToReturnState, onError);
             }, function () { });
         };
     }]);