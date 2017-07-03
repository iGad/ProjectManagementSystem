angapp.controller('UsersMainController', [
    '$scope', '$mdDialog', "uiGridConstants", 'UsersService', 'Utils',
    function ($scope, $mdDialog, uiGridConstants, UsersService, utils) {
    $scope.users = [];
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
        paginationPageSize: 30,
    };  //UsersService.getDefaultGridOptions();

    $scope.gridOptions.onRegisterApi = function (gridApi) {
        $scope.gridApi = gridApi;
        $scope.gridApi.grid.getRolesString = function (entity) {
            var resultStr = "";
            for (var i = 0; i < entity.Roles.length; i++) {
                resultStr += entity.Roles[i].Name;
                if (i < entity.Roles.length - 1)
                    resultStr += ", ";
            }
            return resultStr;
        };
    };
    //$scope.gridOptions.rowTemplate = '<div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ui-grid-cell></div>';

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
            name: 'surname',
            displayName: 'Фамилия',
            field: 'Surname',
            enableHiding: false,
            enableSorting: true,
            enableFiltering: true,
            enableCellEdit: false,
            headerTooltip: true,
            cellTooltip: true
        },
        {
            name: 'email',
            displayName: 'E-mail',
            field: 'Email',
            enableHiding: false,
            enableSorting: true,
            enableFiltering: true,
            enableCellEdit: false,
            headerTooltip: true,
            cellTooltip: true
        },
        {
            name: 'roles',
            displayName: 'Роли',
            enableHiding: false,
            enableSorting: false,
            enableFiltering: false,
            enableCellEdit: false,
            headerTooltip: true,
            cellTooltip: true,
            cellTemplate: ' <div class="ui-grid-cell-contents" style="word-break: break-word;">{{grid.getRolesString(row.entity)}}</div>',
        }
    ];

    function dialog(ev, user,  callback) {
        $mdDialog.show({
            locals: {user: user},
            controller: 'AddUpdateUserController',
            templateUrl: 'Frontend/Views/main/users/AddUpdateUser.html',
            //parent: angular.element(document.body),
           // targetEvent: ev,
            clickOutsideToClose: true,
            fullscreen: false
        }).then(callback, function() {
            var i = 0;
        });
    }


    function getPagination() {
        var pagination = utils.getPagination();
        pagination.getRecordCount = function () { return $scope.users.length; };
        pagination.getTotalItems = function () { return $scope.users.length; };
        pagination.getPage = function () { return 1 };
        pagination.getTotalPages = function () { return 1 };
        pagination.seek = function (page) {  };
        pagination.previousPage = function () {  };
        pagination.nextPage = function () { };
        return pagination;
    };

    $scope.add = function (ev) {
        var user = {Roles:[]};
       
        dialog(ev, user, function(edittedUser) {
            UsersService.addUser(edittedUser, edittedUser.Password).then(function (result) {
                var newUser = angular.fromJson(result.data);
                $scope.gridOptions.data.push(newUser);
            }, utils.onError);
        });
    }


    $scope.edit = function (ev) {
        var selectedRows = $scope.gridApi.selection.getSelectedRows();
        if (selectedRows.length === 1) {
            var user = selectedRows[0];
            var index = $scope.gridOptions.data.indexOf(user);
            if (index !== -1) {
                dialog(ev, user, function (edittedUser) {
                    UsersService.updateUser(edittedUser).then(function () {
                        $scope.gridOptions.data[index] = edittedUser;
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
                .title('Вы действительно хотите удалить пользователя ' + (row.Surname?row.Surname + ' ':'') + row.Name + '?')
                .ariaLabel('delete user')
                .targetEvent(ev)
                .ok('Удалить')
                .cancel('Отмена');
                $mdDialog.show(confirm).then(function () {
                    UsersService.deleteUser(row.Id)
                     .then(function () {
                         $scope.gridOptions.data.splice(index, 1);
                     });
                }, utils.onError);
            }
        }
    };


    function reload() {
        UsersService.getAllUsers().then(function(content) {
            $scope.users = angular.fromJson(content.data);
            $scope.gridOptions.data = $scope.users;
            $scope.pagination = getPagination();
        }, utils.onError);
    }

    reload();
}]);