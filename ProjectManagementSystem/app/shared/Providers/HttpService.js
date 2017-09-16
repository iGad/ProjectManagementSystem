angapp.provider("HttpService", function () {


    return {
        $get: ['$http', function ($http) {
            var service = {
                get: function (httpParams) {
                    httpParams.method = 'GET';
                    return $http(httpParams);
                },
                goToState: function (stateName, stateParams, currentParams) {
                    stateParams = stateParams ? stateParams : {};
                    currentParams = currentParams ? currentParams : {};
                    var currentStateInfo = { name: $state.$current.name, params: angular.copy(currentParams) };
                    var returnStates = currentParams.returnStates ? currentParams.returnStates : [];
                    returnStates.splice(0, 0, currentStateInfo);
                    stateParams.returnStates = returnStates;
                    $state.go(stateName, stateParams);
                },
                convertDateToJs: function (sharpDateTime) {
                    return moment(parseInt(sharpDateTime.substr(6, sharpDateTime.length - 8))).format('DD.MM.YYYY HH:mm');
                }
            };
            return service;
        }]
    };
});