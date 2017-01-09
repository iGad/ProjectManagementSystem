angapp.controller('IndexController',['$scope', '$state', function ($scope, $state) {
    $scope.title = 'Привет!';
    $state.go('base.main');
}]);