angapp.factory('LoginInterceptor', ['$state', '$location', function($state, $location) {
    var myInterceptor = {
        handled: false,
        request: function(config) {
            //if (config.url[0] === '/')
            //    config.header = { X-Requested-With: "XMLHttpRequest"}
            //    config.url = '/api' + config.url;
            return config;
        },
        responseError: function (reject) {
            if (reject.status === 401) {
                if (!self.handled) {
                    $state.go('login', { returnUrl: $location.path() });
                    self.handled = true;
                }
            }
        },
        response: function (response) {
            self.handled = false;
            return response;
        }
    };
    return myInterceptor;
}]);

angapp.config(['$httpProvider', function($httpProvider) {
    $httpProvider.interceptors.push('LoginInterceptor');
}]);
