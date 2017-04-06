angapp.controller('EventsController', [
    '$scope', '$mdDialog', "uiGridConstants", 'EventsService', 'Utils',
    function ($scope, $mdDialog, uiGridConstants, eventsService, utils) {
        var onError = function (err) {
            console.error(err);
        };
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

            enableFiltering: true,
            enablePaginationControls: false,
            multiSelect: false,
            modifierKeysToMultiSelect: false,
            noUnselect: true,
            paginationPageSize: 20,
            appScopeProvider : {
                edit:function(workItemId) {
                    utils.goToState('base.edit', { workItemId: workItemId }, $stateParams);
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
                cellTemplate: ' <div class="ui-grid-cell-contents"><md-button aria-label="Open edit window" class="md-icon-button" ng-click="edit(row.entity.ObjectId)"><md-icon md-svg-icon="navigation:more_horiz"></md-icon><md-tooltip md-direction="bottom">Подробнее</md-tooltip></md-button></div>'
            }
        ];



        function getData() {
            eventsService.getEventsForUser().then(function (content) {
                $scope.events = content.data;
                $scope.gridOptions.data = $scope.events;
            }, onError);
        }

        getData();
    }]);