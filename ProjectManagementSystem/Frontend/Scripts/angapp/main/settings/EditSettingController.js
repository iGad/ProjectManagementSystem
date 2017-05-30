angapp.controller('EditSettingController', [
    '$scope', '$mdDialog', 'SettingsService', 'Utils', 'setting',
    function ($scope, $mdDialog, settingsService, utils, setting) {

        
        $scope.setting = angular.copy(setting);

        $scope.save = function () {
            $mdDialog.hide($scope.setting);
        };

        $scope.valueChanged = function() {
            $scope.isValueChanged = true;
        }

        $scope.cancel = function () {
            $scope.A = 0;
            $mdDialog.cancel();
        };
    }]);