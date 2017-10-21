angapp.controller('EditSettingController', [
    '$scope', '$mdDialog', 'SettingsService', 'Utils', 'setting',
    function ($scope, $mdDialog, settingsService, utils, setting) {
        var buffer = angular.copy(setting);

        $scope.$watch(function () {
            return !$scope.editSettingForm.$invalid && !angular.equals($scope.setting, buffer);
        }, function (newVal, oldVal) {
            if (newVal != oldVal) {
                $scope.isValueChanged = newVal;
            }
        });

        $scope.setting = angular.copy(setting);
        if ($scope.setting.Type === 1 && $scope.setting.Value !== undefined) {
            $scope.setting.Value = parseInt($scope.setting.Value);
        }

        $scope.save = function () {
            $mdDialog.hide($scope.setting);
        };
        

        $scope.cancel = function () {
            $mdDialog.cancel();
        };
    }]);