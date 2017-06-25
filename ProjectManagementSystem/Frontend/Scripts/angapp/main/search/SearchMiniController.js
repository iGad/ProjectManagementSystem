angapp.controller('SearchMiniController', [
    '$scope', '$stateParams', '$mdDialog', 'Utils',
    function ($scope, $stateParams, $mdDialog, utils) {

        $scope.searchText = '';

        $scope.find = function() {
            utils.goToState('base.search', { SearchText: $scope.searchText }, $stateParams);
        };
        
    }]);