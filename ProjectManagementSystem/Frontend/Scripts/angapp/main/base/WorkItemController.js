angapp.controller('WorkItemController', ['$scope', '$state', '$stateParams', '$mdDialog', 'UsersService', 'WorkItemService', 
     function ($scope, $state, $stateParams, $mdDialog, UsersService, WorkItemService) {
         function onError(err) {
             console.error(err);
         };
         $scope.workItem = {
             DeadLineHours: 17,
             DeadLineMinutes: 0,
             DeadLine: moment().add(1, 'days').toDate()
         };

         function getUsers(typeId) {
             UsersService.getAllowedUsersForWorkItemType(typeId).then(function (content) {
                 $scope.users = angular.fromJson(content.data);
                 if ($scope.users.length)
                     $scope.workItem.ExecutorId = $scope.users[0].Id;
             });
         };

         function getTypes() {
             WorkItemService.getTypes().then(function(content) {
                 $scope.types = angular.fromJson(content.data);
                 if ($scope.types.length && !$scope.workItem.Type) {
                     $scope.workItem.Type = $scope.types[1].Id;
                     $scope.typeChanged($scope.types[1].Id);
                 }
             });
         };
         getTypes();
         

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
                 if ($scope.partitions.length)
                     $scope.parentPartitionId = $scope.partitions[0].Id;
             });
         }

         function getStages(projectId) {
             WorkItemService.getChildItems(projectId).then(function (content) {
                 $scope.stages = angular.fromJson(content.data);
                 if ($scope.stages.length) {
                     $scope.parentStageId = $scope.types[1].Id;
                     $scope.stageChanged($scope.stages[0].Id);
                 }
             });
         }

         function getProjects() {
             WorkItemService.getProjects().then(function (content) {
                 $scope.projects = angular.fromJson(content.data);
                 if ($scope.projects.length) {
                     $scope.parentProjectId = $scope.types[1].Id;
                     $scope.projectChanged($scope.projects[0].Id);
                 }
             });
         }

         

        

         $scope.typeChanged = function (typeId) {
             if (typeId) {
                 getUsers(typeId);
             }
             if (typeId !== $scope.types[0].Id)
                 getProjects();
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

         $scope.isProject = function () {
             return  $scope.workItem.Type === $scope.types[0].Id;
         };

         $scope.isStage = function () {
             return $scope.workItem.Type === $scope.types[1].Id;
         };
         $scope.isPartition = function () {
             return $scope.workItem.Type === $scope.types[2].Id;
         };
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
             //var deadline = $scope.workItem.DeadLine;
             //deadline = deadline.setHours($scope.workItem.DeadLineHours).setMinutes($scope.workItem.DeadLineMinutes);
             $scope.workItem.DeadLine.setHours($scope.workItem.DeadLineHours);
             $scope.workItem.DeadLine.setMinutes($scope.workItem.DeadLineMinutes);
             $scope.workItem.DeadLine.setSeconds(0);
            // $scope.workItem.DeadLine = deadline;
             if ($scope.workItem.Id) {

             } else {
                 WorkItemService.addWorkItem($scope.workItem).then(function() {
                     $state.go('base.projects');
                 }, onError);
             }
         };

         $scope.cancel = function (ev) {
             var confirm = $mdDialog.confirm()
                  .title('Вы уверены, что хотите выйти?')
                  .textContent('Все изменения будут потеряны')
                  .ariaLabel('Lucky day')
                  .targetEvent(ev)
                  .ok('Да')
                  .cancel('Нет');

             $mdDialog.show(confirm).then(function () {
                 $state.go('base.projects');
             }, function () {});
         }
     }]);