angapp.directive('antiForgeryToken', ['$http', function ($http) {
        return {
            restrict: 'E',
            //compile: function compile(templateElement, templateAttrs) {
            //    templateElement.html('<input type="hidden" name="__RequestVerificationToken" value={{antiForgery}}/>');
            //},
            controller: function ($scope) {
                $http({ method: 'GET', url: '/Account/GetToken' }).then(function (content) {
                    $http.defaults.headers.common['RequestVerificationToken'] = content.data || "no request verification token";
                    //$scope.antiForgery = content.data;
                }, function (result) {
                    //alert("Error: No data returned");
                });
            }
           
        };
    }]
);