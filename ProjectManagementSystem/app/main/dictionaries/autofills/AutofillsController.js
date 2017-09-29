angapp.controller('AutofillsController', [
    '$scope', '$state', '$stateParams', '$mdDialog', "uiGridConstants", 'AutofillsService', 'Utils',
    function ($scope, $state, $stateParams, $mdDialog, uiGridConstants, service, utils) {
        $scope.selectedTab = 'autofills';
        $scope.filterOptions = {};
        $scope.autofills = [];
        $scope.moveToPage = 1;

        $scope.filterOptions = utils.parseStateParams($state.$current.url.pattern, $stateParams);

        $scope.gridOptions = {
            enableRowSelection: true,
            enableFullRowSelection: true,
            enableRowHeaderSelection: false,
            enableRowReordering: false,

            enableSorting: true,
            useExternalSorting: true,

            enableHorizontalScrollbar: uiGridConstants.scrollbars.NEVER,
            enableVerticalScrollbar: uiGridConstants.scrollbars.NEVER,

            enableFiltering: false,
            enablePaginationControls: false,
            multiSelect: false,
            modifierKeysToMultiSelect: false,
            noUnselect: true,
            paginationPageSize: 30,
        };  //UsersService.getDefaultGridOptions();

        $scope.gridOptions.onRegisterApi = function (gridApi) {
            $scope.gridApi = gridApi;
            reloadData();
            gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                if (sortColumns[0]) {
                    $scope.filterOptions.Sorting.Direction = sortColumns[0].sort.direction;
                    $scope.filterOptions.Sorting.FieldName = sortColumns[0].field;
                } else {
                    $scope.filterOptions.Sorting = {
                        FieldName: 'Article',
                        Direction: 'asc'
                    };
                }
                reloadData();
            });
        };
        //$scope.gridOptions.rowTemplate = '<div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ui-grid-cell></div>';

        

        

        function reloadData() {
            var params = angular.copy($scope.filterOptions);
            params.Sorting = {
                FieldName: $scope.filterOptions.FieldName,
                Direction: $scope.filterOptions.Direction
            };
            service.getAutofillList(params).then(function(content) {
                $scope.gridOptions.data = content.data.Collection;
                $scope.gridOptions.totalItems = content.data.TotalCount;
                var lastPage = $scope.gridApi.pagination.getTotalPages();
                var queryPage = parseInt($scope.filterOptions.PageNumber);
                $scope.filterOptions.PageNumber = lastPage < queryPage ? lastPage : queryPage;
                $scope.toPage = $scope.filterOptions.PageNumber;
                $scope.pagination = getPagination();
            });
        };

        function getPagination() {
            var pagination = utils.getPagination();
            pagination.getPage = function () { return $scope.filterOptions.PageNumber; };
            pagination.getRecordCount= function () { return $scope.gridOptions.data.length; };
            pagination.getTotalItems = function () { return $scope.gridOptions.totalItems; };
            pagination.seek= function (page) {
                if (page > 0) {
                    $scope.filterOptions.PageNumber = page;
                    reloadData();
                }
            };
            pagination.previousPage= function () {
                if ($scope.filterOptions.PageNumber > 1) {
                    $scope.filterOptions.PageNumber--;
                    reloadData();
                }
            };
            pagination.nextPage= function () {
                if ($scope.filterOptions.PageNumber < $scope.gridApi.pagination.getTotalPages()) {
                    $scope.filterOptions.PageNumber++;
                    reloadData();
                }
            }
            return pagination;
        };

        function dialog(ev, autofill, callback) {
            $mdDialog.show({
                locals: { autofill: autofill },
                controller: 'AddUpdateAutofillController',
                templateUrl: 'app/main/dictionaries/autofills/AddUpdateAutofill.html',
                //parent: angular.element(document.body),
                // targetEvent: ev,
                clickOutsideToClose: true,
                fullscreen: false
            }).then(callback, function () {
            });
        }

        $scope.add = function (ev) {
            var autofill = { };
            dialog(ev, autofill, function (newAutofill) {
                service.add(newAutofill).then(function () {
                    reloadData();
                }, utils.onError);
            });
        }


        $scope.edit = function (ev) {
            var selectedRows = $scope.gridApi.selection.getSelectedRows();
            if (selectedRows.length === 1) {
                var autofill = selectedRows[0];
                var index = $scope.gridOptions.data.indexOf(autofill);
                if (index !== -1) {
                    dialog(ev, autofill, function (edittedEntity) {
                        service.update(edittedEntity).then(function () {
                            $scope.gridOptions.data[index] = edittedEntity;
                        }, utils.onError);
                    });
                }
            }
        };

        $scope.delete = function (ev) {
            var row, index;
            if ($scope.gridApi.selection.getSelectedCount() === 1) {
                row = $scope.gridApi.selection.getSelectedRows()[0];
                index = $scope.gridOptions.data.indexOf(row);
                if (index !== -1) {
                    var confirm = $mdDialog.confirm()
                        .title('Вы действительно хотите удалить автозаполнение ' + row.Name + ' - '  + row.Description + '?')
                        .ariaLabel('delete autofill')
                        .targetEvent(ev)
                        .ok('Удалить')
                        .cancel('Отмена');
                    $mdDialog.show(confirm).then(function () {
                        service.delete(row.Id)
                            .then(function () {
                                reloadData();
                            });
                    }, utils.onError);
                }
            }
        };

        var onSearchHandler = $scope.$on('search',
            function(event, data) {
                $scope.filterOptions.SearchText = data;
                reloadData();
            });

        var onDestroyHandler = $scope.$on('destroy',
            function(event, data) {
                onSearchHandler();
                onDestroyHandler();
            });

        
        $scope.gridOptions.columnDefs = [
            {
                name: 'name',
                displayName: 'Имя',
                field: 'Name',
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
                name: 'type',
                displayName: 'Тип элементов',
                field: 'Type',
                enableHiding: false,
                enableSorting: true,
                enableFiltering: true,
                enableCellEdit: false,
                headerTooltip: true,
                cellTooltip: true,
                cellTemplate: '<div class="ui-grid-cell-contents">{{row.entity.TypeViewModel.Description}}</div>'
            }
        ];
    }]);