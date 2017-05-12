angapp.controller('NewEventsController', [
    '$scope', '$state', '$stateParams', '$location', "uiGridConstants", 'EventsService', 'Utils',
    function ($scope, $state, $stateParams, $location, uiGridConstants, eventsService, utils) {
        
        function replaceDateTime(collection) {
            for (var i = 0; i < collection.length; i++) {
                collection[i].Date = utils.convertDateToJs(collection[i].Date);
            }
        }

        $scope.selectAll = function (value) {
            if (value === undefined) {
                $scope.isAllSelected = !$scope.isAllSelected;
                $scope.isAnySelected = $scope.isAllSelected;
                $scope.selectAll($scope.isAllSelected);
            } else {
                for (var i = 0; i < $scope.events.length; i++) {
                    $scope.events[i].isSelected = value;
                }
            }
        };

        function getIsAllSelected() {
            var isAllSelected = true;
            $scope.isAllSelected = false;
            for (var i = 0; i < $scope.events.length && isAllSelected; i++) {
                isAllSelected = $scope.events[i].isSelected;
                $scope.isAnySelected = $scope.isAnySelected || isAllSelected;
            }
            return isAllSelected;
        };

        $scope.selectByRow = function (item) {
            item.isSelected = !item.isSelected;
            $scope.select(item);

        };

        $scope.select = function (item) {
            $scope.isCheckedAll = getIsAllSelected();
        };

        function getData(saveState) {
            eventsService.getNewEventsForUser().then(function (content) {
                $scope.isAnySelected = false;
                $scope.isAllSelected = false;
                if (saveState && $scope.events) {
                    var copy = angular.copy($scope.events).filter(x => x.isSelected);
                    for (var i = 0; i < content.data; i++) {
                        var event = copy.filter(x => x.Id === content.data[i].Id)[0];
                        if (event) {
                            content.data[i].isSelected = true;
                            $scope.isAnySelected = true;
                        }
                    }
                }
                $scope.events = content.data;
                replaceDateTime($scope.events);
            }, utils.onError);
        };
        $scope.events = [];
        $scope.$on("WorkItemChanged", function (event, workItem) {
            getData(true);
        });

        $scope.toDetails = function (item) {
            utils.goToState('base.edit', { workItemId: item.ObjectId }, $stateParams);
        };

        $scope.makeSeen = function() {
            var events = $scope.events.filter(x => x.isSelected);
            if (events.length) {
                var ids = [];
                for (var i = 0; i < events.length; i++) {
                    ids.push(events[i].Id);
                }
                eventsService.makeSeen(ids).then(function() {
                    getData();
                }, utils.onError);
            }
        };
        

        getData(false);
    }]);