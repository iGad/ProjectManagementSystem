angapp.controller('SearchMiniController', [
    '$scope', '$stateParams', '$mdDialog', 'Utils',
    function ($scope, $stateParams, $mdDialog, utils) {

        $scope.searchText = '';

        $scope.find = function () {
            var text = $scope.searchText;
            $scope.searchText = null;
            $('#searchMiniInput').blur();
            utils.goToState('base.search', { SearchText: text }, $stateParams);
            
        };
        
    }]);