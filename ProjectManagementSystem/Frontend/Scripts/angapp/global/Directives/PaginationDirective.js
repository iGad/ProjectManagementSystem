angapp.directive('gridPagination', [function () {
        return {
            restrict: 'E',
            scope: {
                gridApi: '='
            },
            templateUrl: 'Frontend/Views/shared/Pagination.html',
            link: function (scope) {
                ///scope.moveToPage = scope.gridApi.pagination.getTotalPages();
            }
        };
    }]
);