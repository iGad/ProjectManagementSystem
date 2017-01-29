﻿angapp.provider("WorkItemService", function () {
    var usersUrl = '/UsersApi/';
    var workItemUrl = '/WorkItemsApi/';

    var getAllUsers = function ($http) {
        return $http({
            url: usersUrl + 'GetUsers',
            method: "GET"
        });
    };

    return {
        $get: ["$q", "$http", function ($q, $http) {
            var service = {
                getTypes: function () {
                    return $http({
                        url: workItemUrl + 'GetWorkItemTypes',
                        method: "GET"
                    });
                },
                getProjects: function () {
                    return $http({
                        url: workItemUrl + 'GetProjects',
                        method: "GET"
                    });
                },
                getChildItems: function (parentId) {
                    return $http({
                        url: workItemUrl + 'GetChildWorkItems',
                        method: "GET",
                        params: {parentId: parentId}
                    });
                },
                getDefaultGridOptions: function () {
                    return getDefaultGridOptions();
                },
                getAllUsers: function () {
                    return getAllUsers($http);
                },
                getRoles: function () {
                    return $http({
                        url: usersUrl + 'GetRoles',
                        method: 'GET'
                    });
                },
                getMainInfo: function (id) {
                    return $http({
                        url: usersUrl + 'GetMainInfo',
                        method: 'GET',
                        params: { id: id }
                    });
                },
                addWorkItem: function (user, password) {
                    return $http({
                        url: usersUrl + 'AddUser',
                        method: 'POST',
                        data: { user: user, password: password }
                    });
                },
                updateWorkItem: function (user) {
                    return $http({
                        url: usersUrl + 'UpdateUser',
                        method: 'POST',
                        data: { user: user }
                    });
                },
                deleteUser: function (userId) {
                    return $http({
                        url: usersUrl + 'DeleteUser',
                        method: 'POST',
                        data: { userId: userId }
                    });
                },
                isEmailUnique: function (email) {
                    return $http({
                        url: usersUrl + 'IsEmailUnique',
                        method: 'GET',
                        params: { email: email }
                    });
                }


            };
            return service;
        }]
    };
});