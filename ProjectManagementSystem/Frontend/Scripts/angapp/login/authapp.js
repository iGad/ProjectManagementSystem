var angapp = angular.module("authapp", ['ngMaterial', 'ngAnimate', 'ngAria', 'ngMessages'])
.controller("authController", function ($scope, $http, authProvider, $window, $location) {
    $scope.email = "";
    $scope.password = "";
    $scope.rememberMe = false;

    $scope.logOn = function () {

        $scope.errorResult = "";

        var model = {
            Email: $scope.email,
            Password: $scope.password,
            RememberMe: $scope.rememberMe
        }
        var promiseObj = authProvider.logOn(model);
        promiseObj.then(function (data) {
            if (data.errorResult)
                $scope.errorResult = data.errorResult;
            else
                $window.location.href = data.url;
        });
    }
});