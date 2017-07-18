//var angapp = angular.module("authapp", ['ngMaterial', 'ngAnimate', 'ngAria', 'ngMessages'])
//.config(['$locationProvider', function ($locationProvider) { $locationProvider.html5Mode({ enabled: true, requireBase: false }); }])
angapp.controller("LoginController", ['$scope', '$state', '$stateParams', '$http', 'authProvider', '$window', '$location',
    function ($scope, $state, $stateParams, $http, authProvider, $window, $location) {
    $scope.email = "";
    $scope.password = "";
    $scope.rememberMe = false;

    $scope.logOn = function () {

        $scope.errorResult = "";

        var model = {
            Email: $scope.email,
            Password: $scope.password,
            RememberMe: $scope.rememberMe
        };
        var returnUrl = $stateParams.returnUrl;
        var returnUrlUgly = $location.absUrl().split('ReturnUrl=')[1];
        if (returnUrlUgly)
            returnUrl = "/" + returnUrlUgly.split('&')[0];
       
        authProvider.logOn(model, returnUrl).then(function (content) {
            
                $window.location.href = content.data;
        }, function(data) {
            $scope.errorResult = data.data.Message;
        });
    }
}]);