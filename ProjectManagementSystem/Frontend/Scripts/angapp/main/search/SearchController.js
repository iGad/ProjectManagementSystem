angapp.controller('SearchController', [
    '$scope', '$state', '$stateParams', '$location', 'SearchService', 'Utils',
    function ($scope, $state, $stateParams, $location, searchService, utils) {
        var propertyList = ['UserIds', 'Types', 'States', 'SearchText'];
        $scope.showDetails = false;

        $scope.filterOptions = utils.parseStateParams($state.$current.url.pattern,$stateParams,['UserIds', 'Types', 'States']);
        
        function loadAdditionalData() {
            searchService.getUsers().then(function(content) {
                    $scope.users = content.data;
                    if ($scope.filterOptions.UserIds) {
                        for (var i = 0; i < $scope.filterOptions.UserIds.length; i++) {
                            var user = $scope.users.filter(x => x.Id === $scope.filterOptions.UserIds[i])[0];
                            if (user)
                                user.isSelected = true;
                        }
                    }
                },
                utils.onError);
            searchService.getWorkItemStates().then(function(content) {
                    $scope.states = content.data;
                    if ($scope.filterOptions.States) {
                        for (var i = 0; i < $scope.filterOptions.States.length; i++) {
                            var state = $scope.states.filter(x => x.Id === $scope.filterOptions.States[i])[0];
                            if (state)
                                state.isSelected = true;
                        }
                    }
                },
                utils.onError);
            searchService.getWorkItemTypes().then(function(content) {
                    $scope.types = content.data;
                    if ($scope.filterOptions.Types) {
                        for (var i = 0; i < $scope.filterOptions.Types.length; i++) {
                            var type = $scope.types.filter(x => x.Id === $scope.filterOptions.Types[i])[0];
                            if (type)
                                type.isSelected = true;
                        }
                    }
                },
                utils.onError);
        };

        loadAdditionalData();

        $scope.getUserInfo = function (user) {
            return utils.getUserInfo(user);
        };

        $scope.find = function () {
            reload();
        };

        $scope.clear = function () {
            $scope.filterOptions = {};
            
            reload();
        };

        function replaceDateTime(collection) {
            for (var i = 0; i < collection.length; i++) {
                collection[i].Date = utils.convertDateToJsString(collection[i].Date);
            }
        }

        function reload() {
            var currentPath = $location.path().split('?')[0];
            $location.path(currentPath);
            var params = angular.copy($scope.filterOptions);
            $location.search(params); //После этого действия проризойдет переход в новое состояние и данный контроллер заново создастся.
        };

        function getPagination() {
            var pagination = utils.getPagination();
            pagination.getRecordCount = function () { return $scope.events.length; };
            pagination.getTotalItems = function () { return $scope.totalItems; };
            pagination.getPage = function () { return $scope.filterOptions.PageNumber };
            pagination.getTotalPages = function () { return $scope.totalPageCount };
            pagination.seek = function (page) { $scope.filterOptions.PageNumber = page; reload(); };
            pagination.previousPage = function () { if ($scope.filterOptions.PageNumber > 1) { $scope.filterOptions.PageNumber--; reload(); } };
            pagination.nextPage = function () { if ($scope.filterOptions.PageNumber < $scope.totalPageCount) { $scope.filterOptions.PageNumber++; reload(); } };
            return pagination;
        };


        $scope.events = [];
        
        $scope.toDetails = function (item) {
            utils.goToState('base.edit', { workItemId: item.Id }, $stateParams);
        };
        function getData() {
            var filterOptions = angular.copy($scope.filterOptions);
            filterOptions.Sorting = { FieldName: filterOptions.FieldName, Direction: filterOptions.Direction === 'asc' ? 0 : 1 };
            var isNeedResponse = false;
            for (var i = 0; i < propertyList.length; i++) {
                isNeedResponse = isNeedResponse || filterOptions[propertyList[i]];
            }
            if (isNeedResponse) {
                $scope.isWaitingResult = true;
                searchService.find(filterOptions).then(function (content) {
                    if (content.data.TotalCount === 1) {
                        utils.goToState('base.edit', { workItemId: content.data.Collection[0].Id }, $stateParams);
                    } else {
                        $scope.result = content.data.Collection;
                        $scope.totalItems = content.data.TotalCount;
                        $scope.isWaitingResult = false;
                    }
                        
                    },
                    utils.onError);
            }
        };

        getData();
    }]);