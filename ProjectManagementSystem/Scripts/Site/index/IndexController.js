angapp.controller('IndexController',['$scope','$rootScope', '$location', '$state', function ($scope, $rootScope, $location, $state) {
    $scope.title = 'Привет!';
    $rootScope.$on('$stateChangeStart', function (evt, toState, toParams, fromState, fromParams) {
        console.log("$stateChangeStart " + fromState.name + JSON.stringify(fromParams) + " -> " + toState.name + JSON.stringify(toParams));
    });
    $rootScope.$on('$stateChangeSuccess', function () {
        console.log("$stateChangeSuccess " + fromState.name + JSON.stringify(fromParams) + " -> " + toState.name + JSON.stringify(toParams));
    });
    $rootScope.$on('$stateChangeError', function () {
        console.log("$stateChangeError " + fromState.name + JSON.stringify(fromParams) + " -> " + toState.name + JSON.stringify(toParams));
    });
    //$rootScope.$on('$stateChangeStart', function (e, toState, toParams, fromState, fromParams) {
    //    if (toState.module === 'private') {
    //        // If logged out and transitioning to a logged in page:
    //        e.preventDefault();
    //        //$state.go('public.login');
    //    } else if (toState.module === 'public') {
    //        // If logged in and transitioning to a logged out page:
    //        e.preventDefault();
    //        //$state.go('tool.suggestions');
    //    };
    //});
    if($location.path().length < 1)
        $state.go('base.main');
}]);