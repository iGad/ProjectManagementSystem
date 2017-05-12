angapp.controller('SeenEventsController', [
    '$scope', '$state', '$stateParams', '$location', "uiGridConstants", 'EventsService', 'Utils',
    function ($scope, $state, $stateParams, $location, uiGridConstants, eventsService, utils) {
        var onError = function (err) {
            console.error(err);
        };
        function getFilterOptions(stateParams, arrayPropertyNames) {
            var result = {};
            var propertyNames = $state.$current.url.pattern.split('?')[1].split('&');
            for (var i = 0; i < propertyNames.length; i++) {
                result[propertyNames[i]] = stateParams[propertyNames[i]];
                if (arrayPropertyNames.indexOf(x => x === propertyNames[i]) >= 0 && result[propertyNames[i]] && ! (result[propertyNames[i]] instanceof Array)) {
                    result[propertyNames[i]] = [result[propertyNames[i]]];
                }
            }
            if (!result.PageSize)
                result.PageSize = 30;
            if (!result.PageNumber)
                result.PageNumber = 1;
            return result;
        };

        $scope.filterOptions = getFilterOptions($stateParams, 'ItemsIds');


        function replaceDateTime(collection) {
            for (var i = 0; i < collection.length; i++) {
                collection[i].Date = utils.convertDateToJs(collection[i].Date);
            }
        }

        function reload() {
            var currentPath = $location.path().split('?')[0];
            $location.path(currentPath);
            var params = angular.copy($scope.filterOptions);
            $location.search(params); //После этого действия проризойдет переход в новое состояние и данный контроллер заново создастся.
        };

        function getPagination() {
            return {
                CurrentPageLabel: 'Текущая страница',
                OfLabelString: 'из',
                Records: 'Записей',
                BackString: 'Назад',
                ForwardString: 'Вперед',
                ToLastPage: 'К последней странице',
                ToFirstPage: 'К первой странице',
                getRecordCount: function () { return $scope.events.length; },
                getTotalItems: function () { return $scope.totalItems; },
                getPage: function () { return $scope.filterOptions.PageNumber },
                getTotalPages: function () { return $scope.totalPageCount },
                seek: function (page) { $scope.filterOptions.PageNumber = page; reload(); },
                previousPage: function () { if ($scope.filterOptions.PageNumber > 1) { $scope.filterOptions.PageNumber--; reload(); } },
                nextPage: function () { if ($scope.filterOptions.PageNumber < $scope.totalPageCount) { $scope.filterOptions.PageNumber++; reload(); } }
            };
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
                $scope.totalPageCount = $scope.totalItems === 0 ? 1 : parseInt($scope.totalItems / $scope.filterOptions.PageSize) + ($scope.totalItems % $scope.filterOptions.PageSize !== 0 ? 1 : 0);
                $scope.pagination = getPagination();
            }, onError);
        };

        getData();
    }]);