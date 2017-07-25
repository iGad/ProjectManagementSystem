angapp.controller('EditSettingController', [
    '$scope', '$mdDialog', 'SettingsService', 'Utils', 'setting',
    function ($scope, $mdDialog, settingsService, utils, setting) {

        
        $scope.setting = angular.copy(setting);
        if ($scope.setting.Type === 1 && $scope.setting.Value !== undefined) {
            $scope.setting.Value = parseInt($scope.setting.Value);
        }

        $scope.save = function () {
            $mdDialog.hide($scope.setting);
        };

        $scope.valueChanged = function() {
            $scope.isValueChanged = true;
        }

        $scope.cancel = function () {
            $mdDialog.cancel();
        };
    }]);