angapp.controller('UserWorkItemsController', ['$scope', '$state', '$stateParams', 'WorkItemService',
    function ($scope, $state, $stateParams, workItemService) {

        $scope.tileTemplate = '/Frontend/Views/main/base/workItemTileTemplate.html';

        function getData() {
            $scope.getData().then(function (content) {
                var itemsPerType = content.data;
                $scope.New = itemsPerType.New;
                $scope.Planned = itemsPerType.Planned;
                $scope.AtWork = itemsPerType.AtWork;
                $scope.Reviewing = itemsPerType.Reviewing;
                $scope.Done = itemsPerType.Done;
            });
        };

        getData();

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

    }]);