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

        function canNotUserDrop(sortable, newState) {
            if (!$scope.Permissions.CanChangeForeignWorkItem && $scope.User.Id !== sortable.model.Executor.Id)
                return true;
            
            if (!$scope.Permissions.CanMoveToDone && newState === WorkItemStatesEnum.Done)
                return true;
            if (!$scope.Permissions.CanMoveToReviewing && newState === WorkItemStatesEnum.Reviewing)
                return true;
            if (!$scope.Permissions.CanMoveToAtWork && newState === WorkItemStatesEnum.AtWork)
                return true;
            if (!$scope.Permissions.CanMoveToPlanned && newState === WorkItemStatesEnum.Planned)
                return true;
            return false;
        }

        

        $scope.sortableOptions = {
            placeholder: "workitem-tile",
            connectWith: "#uid" + ($scope.userInfo && ($scope.Permissions.CanChangeForeignWorkItem || $scope.User.Id === $scope.userInfo.UserId) ? $scope.userInfo.UserId : '') + " .workitem-container",
            update: function (e, ui) {
                if (!ui.item.sortable.isCanceled() && !ui.item.sortable.received) {
                    var newState = getNewState(ui.item.sortable);
                    if (canNotUserDrop(ui.item.sortable, newState)) {
                        utils.showMessageBox('У вас нет разрешения для выполнения данного действия');
                        ui.item.sortable.cancel();
                    } else {
                        workItemService.updateWorkItemState(ui.item.sortable.model.Id, newState).then(function () { }, utils.onError);
                        ui.item.sortable.model.State.Value = newState;
                    }
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
            
        });

    }]);