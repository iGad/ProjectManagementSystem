angapp.controller('AllWorkItemsController', ['$scope', '$sce', '$state', '$stateParams', '$timeout', '$location', 'WorkItemService',
    function ($scope, $sce, $state, $stateParams, $timeout, $location, WorkItemService) {



        $scope.getData = function () {
            return WorkItemService.getData('/WorkItemsApi/GetActualWorkItems');
            //    .then(function (content) {
            //    var itemsPerType = content.data;
            //    $scope.New = itemsPerType.New;
            //    $scope.Planned = itemsPerType.Planned;
            //    $scope.AtWork = itemsPerType.AtWork;
            //    $scope.Reviewing = itemsPerType.Reviewing;
            //    $scope.Done = itemsPerType.Done;
            //    //$timeout(function () {
            //    //    $scope.New.splice(0, 1);
            //    //    $scope.New.push({ Type: 2, Name: 'Test' });
            //    //}, 5000);
            //});
        };

        //getData();

        function findElementInArray(array, element) {
            for (var i = 0; i < array.length; i++) {
                if (array[i].Id === element.Id)
                    return element;
            }
            return undefined;
        };

        function findElement(element) {
            var result = findElementInArray($scope.New, element);
            var array = $scope.New;
            if (!result) {
                result = findElementInArray($scope.Planned, element);
                array = $scope.Planned;
            }
            if (!result) {
                result = findElementInArray($scope.AtWork, element);
                array = $scope.AtWork;
            }
            if (!result) {
                result = findElementInArray($scope.Reviewing, element);
                array = $scope.Reviewing;
            }
            if (!result) {
                result = findElementInArray($scope.Done, element);
                array = $scope.Done;
            }
            return { element: result, array: array };
        };



        $scope.$on("WorkItemChanged", function (event, workItem) {
            getData();
            /*WorkItemService.getActualWorkItems().then(function (content) {
                $scope = content.data;
            });*/
            /* var data = findElement(workItem);
            //if (data.element.State !== workItem.State) {
            $scope.$apply(function() {
                data.array.splice(data.array.indexOf(function (x) { return x.Id === workItem.Id }), 1);
                switch (workItem.State) {
                    case 0:
                        $scope.New.push(workItem);
                        break;
                    case 1:
                        $scope.Planned.push(workItem);
                        break;
                    case 2:
                        $scope.Done.push(workItem);
                        break;
                    case 5:
                        $scope.Reviewing.push(workItem);
                        break;
                    case 6:
                        $scope.AtWork.push(workItem);
                        break;
                }
            });*/

            //}
        });

        //$scope.getUserString = function(user) {
        //    var str = user.Name;
        //    if (user.Surname) {
        //        str += ' ' + user.Surname;
        //    }
        //    return str;
        //};

        //$scope.edit = function(itemId) {
        //    $state.go('base.edit', { workItemId: itemId, returnStateName: $state.$current.name });
        //};

        //$scope.getWorkItemClass = function (item) {
        //    var today = moment();
        //    var deadline = moment(item.DeadLine, 'DD.MM.YYYY HH:mm');
        //    if (deadline.isBefore(today)) {
        //        return 'warn-color';
        //    }
        //    return WorkItemService.getWorkItemTypeName(item.Type)+'-color';
        //};
    }]);