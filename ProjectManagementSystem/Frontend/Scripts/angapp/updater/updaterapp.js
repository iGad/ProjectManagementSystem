var angapp = angular.module("updaterapp", ['ui.router', 'ngMaterial', 'ngAnimate', 'ngAria', 'ngMessages'])
//.config(['$locationProvider', function ($locationProvider) { $locationProvider.html5Mode({ enabled: true, requireBase: false }); }])
.service("UpdaterService", ["$http", function ($http) {
    var baseUrl = '/DataUpdater/';
    
    this.update = function (setting) {
        return $http({
            url: baseUrl + 'Update',
            method: 'POST'
        });
    };
}])
.controller("UpdaterController", ['$scope', 'UpdaterService', 'Utils',
    function ($scope, updaterService, utils) {
        
        $scope.update = function () {

            updaterService.update().then(function (content) {
                utils.onErrorWithMessageBox({
                    data: {
                        Message: "Успешно, поздравляю!"
                    }
                });
            }, function(error) {
                console.error(error);
                utils.onErrorWithMessageBox(error);
            });
        }
    }]);
