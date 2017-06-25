angapp.service("SearchService",
    [
        "$http", "Utils", "UsersService", "WorkItemService",
        function($http, utils, usersService, workItemService) {
            var baseUrl = '/SearchApi/';


            this.find = function(searchModel) {
                return $http({
                    url: baseUrl + 'Find',
                    method: "POST",
                    data: { searchModel }
                });
            };
            this.getUsers = function() {
                return usersService.getAllUsers();
            };
            this.getWorkItemStates = function() {
                return workItemService.getAllStates();
            };
            this.getWorkItemTypes = function () {
                return workItemService.getTypes();
            };
        }
    ]);