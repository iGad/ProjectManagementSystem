﻿angapp.controller('AddFilesController', [
    '$scope', '$stateParams', '$mdDialog', 'Utils',
    function ($scope, $stateParams, $mdDialog, utils) {
        $scope.add = function() {
            $mdDialog.hide($scope.files);
        };
    }]);