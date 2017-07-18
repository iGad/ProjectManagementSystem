angapp.factory('LoginInterceptor', ['$state', function($state) {
    var myInterceptor = {
        responseError: function (reject) {
            if (reject.statusCode == 403){
                $state.go('login', {returnUrl:reject.config.url});
            }
        }
    };
    return myInterceptor;
}]);

angapp.config(['$httpProvider', function($httpProvider) {
    $httpProvider.interceptors.push('LoginInterceptor');
}]);
