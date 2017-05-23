var angapp = angular.module("authapp", ['ngMaterial', 'ngAnimate', 'ngAria', 'ngMessages'])
//.config(['$locationProvider', function ($locationProvider) { $locationProvider.html5Mode({ enabled: true, requireBase: false }); }])
.controller("authController", ['$scope', '$http', 'authProvider', '$window', '$location',
    function ($scope, $http, authProvider, $window, $location) {
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
        var returnUrl;
        var returnUrlUgly = $location.absUrl().split('ReturnUrl=')[1];
        if (returnUrlUgly)
            returnUrl = "/" + returnUrlUgly.split('&')[0];
       
        authProvider.logOn(model, returnUrl).then(function (content) {
            var path = $location.path();
                $window.location.href = content.data;
        }, function(data) {
            $scope.errorResult = data.errorResult;
        });
    }
}]);