angapp.service("WorkItemService", ["$http", function ($http) {
    var usersUrl = '/UsersApi/';
    var workItemUrl = '/WorkItemsApi/';

    var getWorkItemTypeName = function (type) {
        switch (type) {
            case 1:
                return 'project';
            case 2:
                return 'stage';
            case 3:
                return 'partition';
            case 4:
                return 'task';
        }
        return 'task';
    };

    var getExecutorText = function (workItemType) {
        switch (workItemType) {
            case 1:
                return 'ГИП';
            case 2:
                return 'Менеджер';
            case 3:
                return 'Руководитель направления';
            case 4:
                return 'Исполнитель';
        }
        return 'Исполнитель';
    };

    var getUserDisplayText = function (user) {
        if (!user)
            return 'Отсутствует';
        var text = user.Name + (user.Surname ? (' ' + user.Surname) : (' (' + user.Email + ')'));
        return text;
    };

    var getDeadlineText = function (workItem) {
        return workItem.FinishDate ? workItem.FinishDate : workItem.DeadLine;
    };

    this.getUserWorkItemsAggregateInfo = function() {
        return $http({
            url: workItemUrl + 'GetUserWorkItemsAggregateInfo',
            method: 'GET'
        });
    };

    this.getData = function(url, params) {
        return $http({
            url: url,//workItemUrl + 'GetActualWorkItems',
            method: 'GET',
            params: params
        });
    };

    this.isItemInWork = function (workItem) {
        return workItem.State !== 2 && workItem.State !== 4;
    };
    this.getWorkItemTypeName = function (type) {
        return getWorkItemTypeName(type);
    };
    this.getExecutorText = function (workItemType) {
        return getExecutorText(workItemType);
    };
    this.getUserDisplayText = function (user) {
        return getUserDisplayText(user);
    };
    this.getDeadlineText = function (workItem) {
        return getDeadlineText(workItem);
    };
    this.getTypes = function () {
        return $http({
            url: workItemUrl + 'GetWorkItemTypes',
            method: "GET"
        });
    };
    this.getStates = function () {
        return $http({
            url: workItemUrl + 'GetStates',
            method: 'GET'
        });
    };
    this.getWorkItem = function (id) {
        return $http({
            url: workItemUrl + 'GetWorkItem',
            method: 'GET',
            params: { id: id }
        });
    };
    this.getProjects = function () {
        return $http({
            url: workItemUrl + 'GetProjects',
            method: "GET"
        });
    };
    this.getChildItems = function (parentId) {
        return $http({
            url: workItemUrl + 'GetChildWorkItems',
            method: "GET",
            params: { parentId: parentId }
        });
    };
    this.getProjectsTree = function () {
        return $http({
            url: workItemUrl + 'GetProjectsTree',
            method: 'GET'
        });
    };
    this.addWorkItem = function (workItem) {
        return $http({
            url: workItemUrl + 'AddWorkItem',
            method: 'POST',
            data: { workItem: workItem }
        });
    };
    this.updateWorkItem = function (workItem) {
        return $http({
            url: workItemUrl + 'UpdateWorkItem',
            method: 'POST',
            data: { workItem: workItem }
        });
    };
    this.updateWorkItemState = function(workItemId, newState) {
        return $http({
            url: workItemUrl + 'UpdateWorkItemState',
            method: 'POST',
            data: { workItemId: workItemId, newState: newState }
        });
    };

    this.deleteWorkItem = function (id) {
        return $http({
            url: workItemUrl + 'DeleteWorkItem',
            method: 'POST',
            data: { id: id }
        });
    };
    this.getActualWorkItems = function () {
        return $http({
            url: workItemUrl + 'GetActualWorkItems',
            method: 'GET'
        });
    };
    this.getLinkedItemsForItem = function (workItemId) {
        return $http({
            url: workItemUrl + 'GetLinkedItemsForItem',
            method: 'GET',
            params: { workItemId: workItemId }
        });
    }

}]);