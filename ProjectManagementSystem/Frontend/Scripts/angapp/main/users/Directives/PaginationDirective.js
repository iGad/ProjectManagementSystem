angapp.directive('gridPagination', [function () {
        return {
            restrict: 'E',
            scope: {
                gridApi: '='
            },
            templateUrl: 'Frontend/Views/main/users/Pagination.html',
            link: function (scope) {
                ///scope.moveToPage = scope.gridApi.pagination.getTotalPages();
            }
        };
    }]
);