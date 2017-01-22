angapp.controller('AddUpdateUserController', ['$scope', '$mdDialog', '$timeout', 'UsersService', 'user',
    function ($scope, $mdDialog, $timeout, UsersService, user) {
        var onError = function (err) {
            console.error(err);
        };
        $scope.allRoles = [];
        $scope.roles = [];
        $scope.User = user;
        $scope.addForm = !user.Email;
        $scope.title = $scope.addForm ?  'Добавление пользователя' : 'Редактирование пользователя';
        $scope.okButton = $scope.addForm ? 'Добавить' : 'Сохранить';
        var timeoutId;
        function validateEmail(email) {
            var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            return re.test(email);
        }

        $scope.emailChanged = function (email) {
            $scope.addUpdateUserForm.emailInput.$error = { unique: undefined };
            $scope.addUpdateUserForm.emailInput.$invalid = false;
            if (timeoutId)
                $timeout.cancel(timeoutId);
            timeoutId = $timeout(function () {
                UsersService.isEmailUnique(email).then(function (content) {
                    var result = angular.fromJson(content.data);
                    if (!result) {
                        $scope.addUpdateUserForm.emailInput.$error = { unique: true };
                        $scope.addUpdateUserForm.emailInput.$invalid = true;
                    }

                }, onError);
                timeoutId = undefined;
            }, 2000);
        };

        $scope.ok = function () {
            $mdDialog.hide($scope.User);
            //    dialog(ev, function () {
            //        var copy = copyElementModel($scope.elementAttribute);
            //        dictionaryProvider.saveNewElementAttribute(copy, true)
            //            .then(function (result) {
            //                var fixed = utils.FixObjectFromJson(result.JsonData);
            //                fixed.TypeDescription = $scope.getDescriptionForType(fixed.TypeId);
            //                $scope.gridOptions.data.push(fixed);
            //            }, function (err) {
            //                console.error(err);
            //            });
            //    });
        };

        $scope.cancel = function() {
            $mdDialog.cancel();
        }

        function createFilterFor(query) {
            var lowercaseQuery = angular.lowercase(query);

            return function filterFn(role) {
                return (role.Name.toLowerCase().indexOf(lowercaseQuery) === 0);
            };

        }

        $scope.querySearch = function(query) {
            var results = query ? $scope.roles.filter(createFilterFor(query)) : $scope.roles;
            return results;
        };

        function getNotUsedRoles() {
            var roles = [];
            for (var i = 0; i < $scope.allRoles.length; i++) {
                var isUsed = false;
                for (var j = 0; j < $scope.User.allRoles.length; j++) {
                    if ($scope.User.Roles[j].Name === $scope.allRoles[i].Name)
                        isUsed = true;
                }
                if (!isUsed)
                    roles.push($scope.allRoles[i]);
            }
            return roles;
        };

        $scope.removedItem = function (role, $chip) {
            var index = $scope.User.Roles.indexOf(role);
            $scope.User.Roles.splice(index, 1);
            $scope.roles.push(role);
        };

        function hideAutocomplete() {
            var autoChild = document.getElementById('autocomplete').firstElementChild;
            var el = angular.element(autoChild);
            el.scope().$mdAutocompleteCtrl.blur();
        };

        $scope.selectedItemChange = function(role) {
            var index = $scope.roles.indexOf(role);
            $scope.roles.splice(index, 1);
            hideAutocomplete();
            //$scope.User.Roles.push(role);
        };

        

        function getRoles() {
            UsersService.getRoles().then(function(content) {
                $scope.allRoles = angular.fromJson(content.data);
                if ($scope.addForm)
                    $scope.roles = $scope.allRoles;
                else
                    $scope.roles = getNotUsedRoles();
            });
        };

        getRoles();
    }]);