angapp.provider("authProvider", function () {
    var logIn = function($http, model, returnUrl) {
        return $http({
            url: "/Account/Login",
            method: "POST",
            data: { model:model, returnUrl:returnUrl }
        });
        };
    return {
        $get: [
            "$http", function ($http) {
                var service = {};
                service.logOn = function (model, returnUrl) {
                    return logIn($http, model, returnUrl);
                };
                return service;
            }
        ]
    };
});