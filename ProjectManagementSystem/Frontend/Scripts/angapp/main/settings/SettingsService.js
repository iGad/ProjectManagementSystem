angapp.service("SettingsService", ["$http", function ($http) {
    var baseUrl = '/SettingsApi/';



    this.getSettings = function () {
        return $http({
            url: baseUrl + 'GetSettings',
            method: "GET"
        });
    };
   
    this.updateSetting = function (setting) {
        return $http({
            url: baseUrl + 'UpdateSetting',
            method: 'POST',
            data: {setting:setting}
        });
    };
}]);