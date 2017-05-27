angapp.provider("UsersService", ["uiGridConstants", function (uiGridConstants) {
    var usersUrl = '/UsersApi/';
    var accountUrl = '/Account/';
    var permissionsUrl = '/PermissionsApi/';

    function getDefaultGridOptions() {
        return {
            enableRowSelection: true,
            fullRowSelection: true,
            //enableRowHeaderSelection: false,
            //enableRowReordering: false,

            //enableSorting: false,

            //enableHorizontalScrollbar: uiGridConstants.scrollbars.NEVER,
            //enableVerticalScrollbar: uiGridConstants.scrollbars.NEVER,

            //enableFiltering: true,
            //enablePaginationControls: false,
            //multiSelect: false,
            //modifierKeysToMultiSelect: false,
            //noUnselect: true,
            paginationPageSize: 20,
        };
    };

    var getAllUsers = function($http) {
        return $http({
            url: usersUrl + 'GetUsers',
            method: "GET"
        });
    };

    return {
        $get: ["$q", "$http", function ($q, $http) {
            var service = {
                getUserDisplayText : function (user) {
                    var text = '';
                    if (user.Surname)
                        text = user.Surname + ' ';
                    if (user.Name)
                        text += user.Name;
                    return text;
                },

                sendNotification: function(userId) {
                    return $http({
                        url: '/EventsApi/SendNotification',
                        method: 'GET',
                        params: {userId}
                    });
                },
                getCurrentUser: function() {
                    return $http({
                        url: usersUrl + 'GetCurrentUser',
                        method: 'GET'
                    });
                },
                changePassword: function(userId, password) {
                    return $http({
                        url: usersUrl + 'ChangePassword',
                        method: 'POST',
                        data: { userId, password }
                    });
                },
                logoff: function() {
                    return $http({
                        url: accountUrl + 'LogOff',
                        method: 'POST'
                    });
                },
                getDefaultGridOptions: function () {
                    return getDefaultGridOptions();
                },
                getAllUsers: function () {
                    return getAllUsers($http);
                },
                getAllowedUsersForWorkItemType: function(typeId) {
                    return $http({
                        url: usersUrl + 'GetAllowedUsersForWorkItemType',
                        method: 'GET',
                        params: { typeId: typeId }
                    });
                },
                getRoles: function() {
                    return $http({
                        url: usersUrl + 'GetRoles',
                        method: 'GET'
                    });
                },
                addUser: function(user, password) {
                    return $http({
                        url: usersUrl + 'AddUser',
                        method: 'POST',
                        data: { user: user, password: password }
                    });
                },
                updateUser: function (user) {
                    return $http({
                        url: usersUrl + 'UpdateUser',
                        method: 'POST',
                        data: { user: user }
                    });
                },
                deleteUser: function(userId) {
                    return $http({
                        url: usersUrl + 'DeleteUser',
                        method: 'POST',
                        data: { userId: userId }
                    });
                },
                isEmailUnique: function(email) {
                    return $http({
                        url: usersUrl + 'IsEmailUnique',
                        method: 'GET',
                        params: { email: email }
                    });
                },
                /******User permissions******/
                getUserPermissions : function() {
                    return $http({
                        url: permissionsUrl + 'GetUserPermissions',
                        method: 'GET'
                    });
                },
                hasPermissions: function(actions) {
                    return $http({
                        url: usersUrl + 'HasPermissions',
                        method: 'GET',
                        params: {actions: actions}
                    });
                },
                hasPermissionsForWorkItem: function (actions, workItemId) {
                    return $http({
                        url: usersUrl + 'HasPermissionsForWorkItem',
                        method: 'GET',
                        params: { actions: actions, workItemId: workItemId }
                    });
                }

            };
            return service;
        }]
    };
}]);