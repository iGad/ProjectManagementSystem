angapp.controller('EventsController', [
    '$scope', '$state', '$stateParams', "uiGridConstants", 'EventsService', 'Utils',
    function ($scope, $state, $stateParams, uiGridConstants, eventsService, utils) {
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

        $scope.events = [];
        $scope.moveToPage = 1;
        $scope.gridOptions = {
            enableRowSelection: true,
            enableFullRowSelection: true,
            enableRowHeaderSelection: false,
            enableRowReordering: false,

            enableSorting: true,

            enableHorizontalScrollbar: uiGridConstants.scrollbars.NEVER,
            enableVerticalScrollbar: uiGridConstants.scrollbars.NEVER,
            useExternalPagination: true,
            enableFiltering: true,
            enablePaginationControls: false,
            multiSelect: false,
            modifierKeysToMultiSelect: false,
            noUnselect: true,
            paginationPageSize: 5,
            appScopeProvider : {
                edit:function(workItemId) {
                    utils.goToState('base.edit', { workItemId: workItemId }, $stateParams);
                },
                applyFilter: function() {
                    //utils.applyFilters($scope.gridApi)  
                }
            }
        };  

        $scope.gridOptions.onRegisterApi = function (gridApi) {
            $scope.gridApi = gridApi;
            
        };
        //$scope.gridOptions.rowTemplate = '<div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ui-grid-cell></div>';

        $scope.gridOptions.columnDefs = [
            {
                name: 'date',
                displayName: 'Дата',
                field: 'Date',
                width: '15%',
                enableHiding: false,
                enableSorting: true,
                enableFiltering: true,
                enableCellEdit: false,
                headerTooltip: true,
                cellTooltip: true
            },
            {
                name: 'description',
                displayName: 'Описание',
                field: 'Description',
                enableHiding: false,
                enableSorting: true,
                enableFiltering: true,
                enableCellEdit: false,
                headerTooltip: true,
                cellTooltip: true
            },
            {
                name: 'details',
                displayName: '',
                field: 'Details',
                enableHiding: false,
                enableSorting: false,
                enableFiltering: false,
                enableCellEdit: false,
                headerTooltip: true,
                cellTooltip: true,
                width: '5%',
                cellTemplate: ' <div class="ui-grid-cell-contents"><md-button aria-label="Open edit window" class="md-icon-button" ng-click="edit(row.entity.ObjectId)"><md-icon md-svg-icon="navigation:more_horiz"></md-icon><md-tooltip md-direction="bottom">Подробнее</md-tooltip></md-button></div>'
            }
        ];

        function replaceDateTime(collection) {
            for (var i = 0; i < collection.length; i++) {
                collection[i].Date = utils.convertDateToJs(collection[i].Date);
            }
        }
        function getData() {
            var filterOptions = angular.copy($scope.filterOptions);
            filterOptions.Sorting = { FieldName: filterOptions.FieldName, Direction: filterOptions.Direction === 'asc' ? 0 : 1 };
            filterOptions.DateRange = { Start: filterOptions.DateStart, End: filterOptions.DateEnd };
            eventsService.getEventsForUser(filterOptions).then(function (content) {
                $scope.events = content.data.Collection;
                replaceDateTime($scope.events);
                $scope.gridOptions.data = $scope.events;
                $scope.gridOptions.totalItems = content.data.TotalItems;
            }, onError);
        };

        getData();
    }]);