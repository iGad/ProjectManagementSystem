var angapp = angular.module("updaterapp", ['ui.router', 'ngMaterial', 'ngAnimate', 'ngAria', 'ngMessages', 'toastr'])
//.config(['$locationProvider', function ($locationProvider) { $locationProvider.html5Mode({ enabled: true, requireBase: false }); }])
.service("UpdaterService", ["$http", function ($http) {
    var baseUrl = '/DataUpdater/';
    
    this.update = function (setting) {
        return $http({
            url: baseUrl + 'Update',
            method: 'POST'
        });
    };
        this.generateUsers = function() {
            return $http({
                url: baseUrl + 'GenerateUsers',
                method: 'POST'
            });
    };
        this.generateItems = function (parameters) {
            return $http({
                url: baseUrl + 'GenerateItems',
                method: 'POST',
                data: { parameters: parameters }
            });
        };
    }])
.controller("UpdaterController", ['$scope', 'UpdaterService', 'Utils',
    function ($scope, updaterService, utils) {

        $scope.parameters = {
            ProjectCount: 1,
            StagesPerProjectCount: 1,
            PartitionsPerStageFrom: 1,
            PartitionsPerStageTo: 1,
            TasksPerPartitionFrom: 1,
            TasksPerPartitionTo: 1
        };

        $scope.update = function() {

            updaterService.update().then(function(content) {
                    utils.onErrorWithMessageBox({
                        data: {
                            Message: "Успешно, поздравляю!"
                        }
                    });
                },
                function(error) {
                    console.error(error);
                    utils.onErrorWithMessageBox(error);
                });
        };

        $scope.generateUsers = function () {
            updaterService.generateUsers().then(function (content) {
                    utils.onErrorWithMessageBox({
                        data: {
                            Message: "Успешно, поздравляю!"
                        }
                    });
                },
                function (error) {
                    console.error(error);
                    utils.onErrorWithMessageBox(error);
                });
        };

        $scope.generateItems = function () {
            updaterService.generateItems($scope.parameters).then(function (content) {
                    utils.onErrorWithMessageBox({
                        data: {
                            Message: "Успешно, поздравляю!"
                        }
                    });
                },
                function (error) {
                    console.error(error);
                    utils.onErrorWithMessageBox(error);
                });
        };
    }]);
