angapp.service("SearchService",
    [
        "$http", "Utils", "UsersService", "WorkItemsService",
        function($http, utils, usersService, workItemsService) {
            var baseUrl = '/SearchApi/';


            this.find = function(searchModel) {
                return $http({
                    url: baseUrl + 'Find',
                    method: "POST",
                    data: { searchModel }
                });
            };
            this.getUsers = function() {
                return usersService.getUsers();
            };
            this.getWorkItemStates = function() {
                return workItemsService.getStates();
            };
            this.getWorkItemTypes = function () {
                return workItemsService.getTypes();
            };
        }
    ]);