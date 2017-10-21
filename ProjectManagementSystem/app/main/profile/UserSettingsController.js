angapp.controller('UserSettingsController', [
    '$scope', '$mdDialog', 'UserSettingsService', 'Utils',
    function ($scope, $mdDialog, settingsService, utils) {

        function getSettings() {
            settingsService.getSettings().then(function (content) {
                $scope.settings = content.data;
            }, utils.onError);
        };
        $scope.settings = [];
       
        $scope.edit = function (ev, setting) {
            $mdDialog.show({
                locals: { setting: setting },
                controller: 'EditSettingController',
                templateUrl: 'app/main/profile/editUserSetting.html',
                parent: angular.element(document.body),
                targetEvent: ev,
                clickOutsideToClose: true,
                fullscreen: false
            }).then(function (resultSetting) {
                var index = $scope.settings.indexOf(setting);
                $scope.settings[index] = resultSetting;
                settingsService.updateSetting(resultSetting).then(function() {}, utils.onError);
            }, function () {});
        };

        getSettings();
    }]);