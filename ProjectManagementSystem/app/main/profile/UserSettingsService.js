angapp.service("UserSettingsService", ["$http", function ($http) {
    var baseUrl = '/SettingsApi/';



    this.getSettings = function () {
        return $http({
            url: baseUrl + 'GetUserSettings',
            method: "GET"
        });
    };
   
    this.updateSetting = function (setting) {
        return $http({
            url: baseUrl + 'UpdateUserSetting',
            method: 'POST',
            data: {setting:setting}
        });
    };
}]);