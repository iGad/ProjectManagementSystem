angapp.controller('SeenEventsController', [
    '$scope', '$state', '$stateParams', '$location', "uiGridConstants", 'EventsService', 'UsersService', 'Utils',
    function ($scope, $state, $stateParams, $location, uiGridConstants, eventsService, usersService, utils) {
        
        function getFilterOptions(stateParams, arrayPropertyNames) {
            var result = {};
            var propertyNames = $state.$current.url.pattern.split('?')[1].split('&');
            for (var i = 0; i < propertyNames.length; i++) {
                result[propertyNames[i]] = stateParams[propertyNames[i]];
                if (arrayPropertyNames.indexOf(x => x === propertyNames[i]) >= 0 && result[propertyNames[i]] && ! (result[propertyNames[i]] instanceof Array)) {
                    result[propertyNames[i]] = [result[propertyNames[i]]];
                }
                if (propertyNames[i] !== 'PageNumber' && result[propertyNames[i]])
                    $scope.isFilterApplyed = true;
            }

            if (!result.PageNumber)
                result.PageNumber = 1;
            return result;
        };

        function getUsers() {
            usersService.getAllUsers().then(function (content) {
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
        };

        function assignDate() {
            if ($scope.filterOptions.DateStart)
                $scope.DateStartDate = moment($scope.filterOptions.DateStart, "DD.MM.YYYY").toDate();
            if ($scope.filterOptions.DateEnd)
                $scope.DateEndDate = moment($scope.filterOptions.DateEnd, "DD.MM.YYYY").toDate();
        };

        $scope.filterOptions = utils.parseStateParams($state.$current.url.pattern, $stateParams, ['UserIds']);
        $scope.isFilterApplyed = $scope.filterOptions.UserIds && $scope.filterOptions.UserIds.length || $scope.filterOptions.DateStart || $scope.filterOptions.DateEnd || $scope.filterOptions.ItemsIds;
        assignDate();
        $scope.filter = $scope.filterOptions;

        getUsers();

        $scope.getUserInfo = function(user) {
            return utils.getUserInfo(user);
        };

        $scope.applyFilter = function() {
            $scope.filterOptions = $scope.filter;
            reload();
        };

        $scope.clearFilter = function(propertyName) {
            if (propertyName) {
                $scope.filter[propertyName] = null;
                $scope.filterOptions[propertyName] = null;
            } else {
                $scope.filter = { IsFavorite: $scope.filter.IsFavorite };
                $scope.filterOptions = { IsFavorite: $scope.filter.IsFavorite };
            }
            reload();
        };

        $scope.dateStartChange = function (date) {
            $scope.filter.DateStart = moment(date).format('DD.MM.YYYY');
        };

        $scope.dateEndChange = function (date) {
            $scope.filter.DateEnd = moment(date).format('DD.MM.YYYY');
        };

        function replaceDateTime(collection) {
            for (var i = 0; i < collection.length; i++) {
                collection[i].Date = utils.convertDateToJsString(collection[i].Date);
            }
        }

        function reload() {
            var currentPath = $location.path().split('?')[0];
            $location.path(currentPath);
            var params = angular.copy($scope.filter);
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
        
        $scope.changeIsFavorite = function (item) {
            item.IsFavorite = !item.IsFavorite;
            eventsService.changeIsFavorite(item).then(function () { }, utils.onError);
        };
        $scope.toDetails = function(item) {
            utils.goToState('base.edit', { workItemId: item.ObjectId }, $stateParams);
        };
        function getData() {
            var filterOptions = angular.copy($scope.filterOptions);
            filterOptions.Sorting = { FieldName: filterOptions.FieldName, Direction: filterOptions.Direction === 'asc' ? 0 : 1 };
            filterOptions.DateRange = { Start: filterOptions.DateStart, End: filterOptions.DateEnd };
            eventsService.getSeenEventsForUser(filterOptions).then(function (content) {
                $scope.events = content.data.Collection;
                replaceDateTime($scope.events);
                $scope.totalItems = content.data.TotalCount;
                var pageSize = $scope.filterOptions.PageSize && $scope.filterOptions.PageSize > 0 ? $scope.filterOptions.PageSize : 30;
                $scope.totalPageCount = $scope.totalItems === 0 ? 1 : parseInt($scope.totalItems / pageSize) + ($scope.totalItems % pageSize !== 0 ? 1 : 0);
                $scope.pagination = getPagination();
                
            }, utils.onError);
        };

        utils.makeDatePickerPreventMenuClose("dateFrom");
        utils.makeDatePickerPreventMenuClose("dateTo");
        getData();
    }]);