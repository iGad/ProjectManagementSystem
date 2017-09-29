angapp.controller('AddUpdateAutofillController',['$scope', '$state', '$stateParams', '$mdDialog', 'AutofillsService', 'Utils', 'autofill',
    function($scope, $state, $stateParams, $mdDialog, service, utils, autofill) {
        $scope.autofill = autofill;
        $scope.isNew = autofill.Id == undefined;

        if ($scope.isNew) {
            $scope.title = 'Добавление нового элемента';
        } else {
            $scope.title = 'Редактирование элемента ' + $scope.autofill.Name;
        }
        service.getWorkItemTypes().then(function(content) {
                $scope.workItemTypes = content.data;
                if ($scope.isNew && $scope.workItemTypes.length) {
                    $scope.autofill.WorkItemType = $scope.workItemTypes[0].Value;
                }
            },
            utils.onError);

        $scope.ok = function () {
            $scope.autofill.TypeViewModel =
                $scope.workItemTypes.filter(x => x.Value === $scope.autofill.WorkItemType)[0];
            $mdDialog.hide($scope.autofill);
        };

        $scope.cancel = function() {
            $mdDialog.cancel();
        };
    }
]);