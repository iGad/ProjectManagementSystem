angapp.controller('WorkItemController', ['$scope', 'UsersService', 'WorkItemService',
     function ($scope, UsersService, WorkItemService) {
         $scope.workItem = {};
         function getTypes() {
             WorkItemService.getTypes().then(function(content) {
                 $scope.types = angular.fromJson(content.data);
             });
         };
         getTypes();
         function getUsers(typeId) {
             UsersService.getAllowedUsersForWorkItemType(typeId).then(function (content) {
                 $scope.users = angular.fromJson(content.data);
             });
         };

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

         function getProjects() {
             WorkItemService.getProjects().then(function (content) {
                 $scope.projects = angular.fromJson(content.data);
             });
         }

         function getStages(projectId) {
             WorkItemService.getChildItems(projectId).then(function (content) {
                 $scope.stages = angular.fromJson(content.data);
             });
         }

         function getPartitions(stageId) {
             WorkItemService.getChildItems(stageId).then(function (content) {
                 $scope.partitions = angular.fromJson(content.data);
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
             console.log(projectId);
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
             return  $scope.workItem.TypeId == $scope.types[0].Id;
         };

         $scope.isStage = function () {
             return $scope.workItem.TypeId == $scope.types[1].Id;
         };
         $scope.isPartition = function () {
             return $scope.workItem.TypeId == $scope.types[2].Id;
         };
         $scope.save = function () {
             //var deadline = Date
             if ($scope.workItem.Id) {

             } else {
                 
             }
         };
     }]);