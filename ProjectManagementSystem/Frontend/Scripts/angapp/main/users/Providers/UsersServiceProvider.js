angapp.provider("UsersService", ["uiGridConstants", function (uiGridConstants) {
    var usersUrl = '/UsersApi/';
    function createFilterFor(query) {
        var lowercaseQuery = angular.lowercase(query);

        return function filterFn(dictionary) {
            var name = dictionary.Name.trim();
            var words = name.split(' ');
            var i = 0;
            while (i != words.length && words[i].toLowerCase().indexOf(lowercaseQuery) !== 0) {
                i += 1;
            }
            return (i !== words.length);
        }
    };

    function createFilterForElements(query) {
        var lowercaseQuery = angular.lowercase(query);

        return function filterFn(element) {
            var name = element.Name.toLowerCase().trim();
            return name.indexOf(lowercaseQuery) >= 0;
        }
    };

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
                getDefaultGridOptions: function () {
                    return getDefaultGridOptions();
                },
                getAllUsers: function () {
                    return getAllUsers($http);
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
                }
                
                //----------------------------Атрибуты элементов------------------------------
                //getElementAttributesByElementId: function (id) {
                //    return utils.get({
                //        url: "/" + this.systemNameUrl + "/DictionaryApi/GetElementAttributesByElementId",
                //        data: { elementId: id }
                //    });
                //},
                //saveNewElementAttribute: function (elementAttribute, async) {
                //    return utils.post({
                //        url: "/" + this.systemNameUrl + "/DictionaryApi/AddNewElementAttribute",
                //        data: elementAttribute,
                //        async: async || false
                //    }, function (err) {
                //        console.log(err);
                //    });
                //},
                //updateElementAttribute: function (elementAttribute, async) {
                //    return utils.post({
                //        url: "/" + this.systemNameUrl + "/DictionaryApi/UpdateElementAttribute",
                //        data: elementAttribute,
                //        async: async || false
                //    });
                //},
                //deleteElementAttribute: function (id, async) {
                //    return utils.post({
                //        url: "/" + this.systemNameUrl + "/DictionaryApi/DeleteDictionaryElementAttribute",
                //        data: { id: id },
                //        async: async || false
                //    });
                //},
                //changeElementAttributeOrder: function (id, isIncrement) {
                //    return utils.post({
                //        url: "/" + this.systemNameUrl + "/DictionaryApi/ChangeElementAttributeOrder",
                //        data: { id: id, isIncrement },
                //                async: false
                //    });
                //}

            };
            return service;
        }]
    };
}]);