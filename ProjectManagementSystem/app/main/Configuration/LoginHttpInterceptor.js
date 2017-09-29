angapp.factory('LoginInterceptor', ['$q', '$state', '$location', function($q, $state, $location) {
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
            return $q.reject(reject);
            
        },
        response: function (response) {
            if (response.config.url.indexOf('Api') > 0)
                self.handled = false;
            return response;
        }
    };
    return myInterceptor;
}]);

angapp.config(['$httpProvider', function($httpProvider) {
    $httpProvider.interceptors.push('LoginInterceptor');
}]);
