angapp.controller('WorkItemController', ['$scope', '$state', '$stateParams', '$mdDialog', 'UsersService', 'WorkItemService', 
     function ($scope, $state, $stateParams, $mdDialog, UsersService, WorkItemService) {
         function onError(err) {
             console.error(err);
         };
         function goToReturnState() {
             $state.go($stateParams.returnStateName);
         }
         function getStates() {
             WorkItemService.getStates().then(function(content) {
                 $scope.states = content.data;
             });
         };

         getStates();

         function getUsers(typeId) {
             UsersService.getAllowedUsersForWorkItemType(typeId).then(function (content) {
                 $scope.users = angular.fromJson(content.data);
                 //if (!$scope.workItem.ExecutorId)
                 //    $scope.workItem.ExecutorId = $scope.users[0].Id;
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

         if ($stateParams.workItemId) {
             WorkItemService.getWorkItem($stateParams.workItemId).then(function(content) {
                 $scope.workItem = angular.fromJson(content.data);
                 $scope.workItem.DeadLine = moment($scope.workItem.DeadLine).toDate();
                 $scope.workItem.DeadLineHours = $scope.workItem.DeadLine.getHours();
                 $scope.workItem.DeadLineMinutes = $scope.workItem.DeadLine.getMinutes();
                 getTypes();
             }, onError);
             $scope.isNew = false;
         } else {
             $scope.workItem = {
                 DeadLineHours: 17,
                 DeadLineMinutes: 0,
                 DeadLine: moment().add(1, 'days').toDate()
             };
             $scope.isNew = $scope.workItem.Id == undefined;
             getTypes();
         }

         

         
         
         
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

         $scope.getUserDisplayText = function (user) {
             var text = '';
             if (user.Surname)
                 text = user.Surname + ' ';
             if (user.Name)
                 text += user.Name;
             if (user.Email)
                 text += ' (' + user.Email + ')';
             return text;
         };

         function getPartitions(stageId) {
             WorkItemService.getChildItems(stageId).then(function (content) {
                 $scope.partitions = angular.fromJson(content.data);
                 if ($scope.partitions.length) {
                     if ($scope.isTask && $scope.workItem.ParentId) {
                         $scope.parentPartitionId = $scope.workItem.ParentId;
                     } else {
                         $scope.parentPartitionId = $scope.partitions[0].Id;
                     }
                 }
             });
         }

         function getStages(projectId) {
             WorkItemService.getChildItems(projectId).then(function (content) {
                 $scope.stages = angular.fromJson(content.data);
                 if ($scope.stages.length) {
                     if ($scope.isPartition && $scope.workItem.ParentId) {
                         $scope.parentStageId = $scope.workItem.ParentId;
                     } else {
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
                     if ($scope.isStage && $scope.workItem.ParentId) {
                         $scope.parentProjectId = $scope.workItem.ParentId;
                     } else {
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
             switch($scope.workItem.Type) {
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
                 WorkItemService.addWorkItem($scope.workItem).then(function() {
                     goToReturnState();
                 }, onError);
             }
         };

         $scope.cancel = function(ev) {
             var confirm = $mdDialog.confirm()
                 .title('Вы уверены, что хотите выйти?')
                 .textContent('Все изменения будут потеряны')
                 .ariaLabel('Lucky day')
                 .targetEvent(ev)
                 .ok('Да')
                 .cancel('Нет');

             $mdDialog.show(confirm).then(function() {
                 goToReturnState();
             }, function() {});
         };

         
     }]);