angapp.directive('gridPagination', [function () {
        return {
            restrict: 'E',
            scope: {
                gridApi: '='
            },
            templateUrl: 'app/shared/Pagination.html',
            link: function (scope) {
                ///scope.moveToPage = scope.gridApi.pagination.getTotalPages();
            }
        };
    }]
);