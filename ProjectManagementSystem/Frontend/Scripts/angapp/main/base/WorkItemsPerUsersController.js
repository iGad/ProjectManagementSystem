angapp.controller('WorkItemsPerUsersController',
    ['$scope', 'WorkItemService', 'Utils',
    function ($scope, workItemService, utils) {

        

        workItemService.getUserWorkItemsAggregateInfo().then(function(content) {
            $scope.userInfos = content.data;
        }, utils.onError);

        //GetUsers with aggregate items info
    }]);