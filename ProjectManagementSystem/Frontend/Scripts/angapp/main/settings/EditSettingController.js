angapp.controller('SettingsController', [
    '$scope', '$mdDialog', 'SettingsService', 'Utils', 'setting',
    function ($scope, $mdDialog, settingsService, utils, setting) {

        
        $scope.setting = angular.copy(setting);

        $scope.save = function () {
            $mdDialog.hide(setting);
        };

        $scope.cancel = function() {
            $mdDialog.cancal();
        };
    }]);