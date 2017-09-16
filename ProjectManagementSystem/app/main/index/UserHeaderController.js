angapp.controller('UserHeaderController',
    ['$scope', '$location', '$window', '$mdMenu', '$state', '$stateParams', 'UsersService', 'Utils',
    function ($scope, $location, $window, $mdMenu, $state, $stateParams, usersService, utils) {

        //usersService.getCurrentUser().then(function(content) {
        //    $scope.user = content.data;
        //}, utils.onError);

        $scope.openMenu = function(mdMenu, $event) {
            mdMenu.open($event);
        };

        $scope.goToState = function(stateName) {
            $state.go(stateName, {}, {reload:true});
        };

        $scope.logoff = function() {
            if ($scope.user) {
                usersService.logoff($scope.user.Id).then(function() {
                    $state.go('login');
                   // $window.location.href ='/Account/Login';
                }, utils.onError);
            }
        };
    }]);