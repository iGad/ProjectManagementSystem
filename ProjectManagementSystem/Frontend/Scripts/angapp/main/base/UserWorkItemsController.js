var WorkItemStatesEnum = {
    New: 0,
    Planned: 1,
    Done: 2,
    Deleted: 3,
    Archive: 4,
    Reviewing: 5,
    AtWork: 6
}
angapp.controller('UserWorkItemsController', ['$scope', '$state', '$stateParams', 'WorkItemService', 'Utils',
    function ($scope, $state, $stateParams, workItemService, utils) {

        $scope.tileTemplate = '/Frontend/Views/main/base/workItemTileTemplate.html';

        function canNotUserDrop(sortable) {
            return false;
        }

        function getNewState(sortable) {
            
            if (sortable.droptargetModel === $scope.AtWork)
                return WorkItemStatesEnum.AtWork;
            if (sortable.droptargetModel === $scope.Reviewing)
                return WorkItemStatesEnum.Reviewing;
            if (sortable.droptargetModel === $scope.Done)
                return WorkItemStatesEnum.Done;
            if (sortable.model.Executor)
                return WorkItemStatesEnum.Planned;
            return WorkItemStatesEnum.New;
            
        }

        $scope.sortableOptions = {
            placeholder: "workitem-tile",
            connectWith: "#uid" + ($scope.userInfo ? $scope.userInfo.UserId : '') + " .workitem-container",
            update: function (e, ui) {
                if (canNotUserDrop(ui.item.sortable)) {
                    ui.item.sortable.cancel();
                }
                var newState = getNewState(ui.item.sortable);
                if (ui.item.sortable.model.State.Value !== newState) {
                    workItemService.updateWorkItemState(ui.item.sortable.model.Id, newState).then(function () { }, utils.onError);
                    ui.item.sortable.model.State.Value = newState;
                }
            }
        };

        function getData() {
            $scope.getData().then(function (content) {
                var itemsPerType = content.data;
                $scope.New = itemsPerType.New;
                $scope.Planned = itemsPerType.Planned;
                for (var i = 0; i < $scope.New.length; i++)
                    $scope.Planned.push($scope.New[i]);
                //$scope.AllPlanned = itemsPerType.New.concat(itemsPerType.Planned);
                $scope.AtWork = itemsPerType.AtWork;
                $scope.Reviewing = itemsPerType.Reviewing;
                $scope.Done = itemsPerType.Done;
            });
        };

        getData();

        $scope.$on("WorkItemChanged", function (event, workItem) {
            getData();
            
        });

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