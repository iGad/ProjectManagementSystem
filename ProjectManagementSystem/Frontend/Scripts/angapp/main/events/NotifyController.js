angapp.controller('NotifyController', [
    '$scope', '$mdDialog', "uiGridConstants", 'UsersService', function ($scope, $mdDialog, uiGridConstants, UsersService) {
        var onError = function (err) {
            console.error(err);
        };
        $scope.users = [];
        

        function getUsers() {
            UsersService.getAllUsers().then(function (content) {
                $scope.user = content.data[0];
                $scope.users = content.data;
                
            }, onError);
        }

        $scope.send = function() {
            UsersService.sendNotification($scope.user.Id);
        };

        getUsers();
    }]);