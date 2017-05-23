angapp.controller('EditProfileInfoController',
    ['$scope', '$state', '$stateParams', '$location', 'UsersService', 'Utils',
    function ($scope, $state, $stateParams, $location, usersService, utils) {

        usersService.getCurrentUser().then(function (content) {
            $scope.user = content.data;
            if($scope.user.Birthday)
                $scope.user.Birthday = utils.convertDateToJsDate($scope.user.Birthday);
            $scope.userCache = angular.copy($scope.user);
        }, utils.onError);

        $scope.modelChanged = function() {
            $scope.isFormChanged = true;
        };

        $scope.save = function() {
            usersService.updateUser($scope.user).then(function(content) {
                $state.reload();
            }, utils.onError);
        };

        $scope.cancel = function() {
            $scope.user = angular.copy($scope.userCache);
            $scope.isFormChanged = false;
        };

        $scope.changePassword = function(newPassword) {
            usersService.changePassword(user.Id, newPassword).then(function() {
                $scope.user.NewPassword = undefined;
            }, utils.onError);
        };

    }]);