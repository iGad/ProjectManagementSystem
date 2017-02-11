angapp.provider("WorkItemService", function () {
    var usersUrl = '/UsersApi/';
    var workItemUrl = '/WorkItemsApi/';

    var getWorkItemTypeName = function(type) {
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

    var getExecutorText = function(workItemType) {
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

    return {
        $get: ["$q", "$http", function ($q, $http) {
            var service = {
                isItemInWork: function(workItem) {
                    return workItem.State !== 2 && workItem.State !== 4;
                },
                getWorkItemTypeName: function(type) {
                    return getWorkItemTypeName(type);
                },
                getExecutorText: function(workItemType) {
                    return getExecutorText(workItemType);
                },
                getUserDisplayText: function(user) {
                    return getUserDisplayText(user);
                },
                getDeadlineText: function(workItem) {
                    return getDeadlineText(workItem);
                },
                getTypes: function() {
                    return $http({
                        url: workItemUrl + 'GetWorkItemTypes',
                        method: "GET"
                    });
                },
                getStates: function() {
                    return $http({
                        url: workItemUrl + 'GetStates',
                        method: 'GET'
                    });
                },
                getWorkItem: function(id) {
                    return $http({
                        url: workItemUrl + 'GetWorkItem',
                        method: 'GET',
                        params: { id: id }
                    });
                },
                getProjects: function() {
                    return $http({
                        url: workItemUrl + 'GetProjects',
                        method: "GET"
                    });
                },
                getChildItems: function(parentId) {
                    return $http({
                        url: workItemUrl + 'GetChildWorkItems',
                        method: "GET",
                        params: { parentId: parentId }
                    });
                },
                getProjectsTree: function() {
                    return $http({
                        url: workItemUrl + 'GetProjectsTree',
                        method: 'GET'
                    });
                },
                addWorkItem: function (workItem) {
                    return $http({
                        url: workItemUrl + 'AddWorkItem',
                        method: 'POST',
                        data: { workItem: workItem }
                    });
                },
                updateWorkItem: function (workItem) {
                    return $http({
                        url: workItemUrl + 'UpdateWorkItem',
                        method: 'POST',
                        data: { workItem: workItem }
                    });
                },
                deleteWorkItem: function (id) {
                    return $http({
                        url: workItemUrl + 'DeleteWorkItem',
                        method: 'POST',
                        data: { id: id }
                    });
                },
                getActualWorkItems: function() {
                    return $http({
                        url: workItemUrl + 'GetActualWorkItems',
                        method: 'GET'
                    });
                },
                getLinkedItemsForItem: function(workItemId) {
                    return $http({
                        url: workItemUrl + 'GetLinkedItemsForItem',
                        method: 'GET',
                        params: { workItemId: workItemId }
                    });
                }
                
            };
            return service;
        }]
    };
});