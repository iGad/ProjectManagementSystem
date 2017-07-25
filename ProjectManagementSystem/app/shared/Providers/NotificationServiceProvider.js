angapp.provider("NotificationService", function () {
    var getActionInfo = function (eventName, notificationModel, Utils, $stateParams) {
        var result = {};
        switch (eventName.toLowerCase()) {
            case 'workitemappointed':
            case 'workitemdisappoint':
            case 'workitemstatechanged':
            case 'workitemchanged':
            case 'workitemadded':
                result.actionName = 'Посмотреть';
                result.action =  function () { Utils.goToState('base.edit', { workItemId: notificationModel.WorkItemId }, $stateParams); };
                break;
        }
        return result;
    };

    function isProject(type) {
        return type === 0;
    };
    function isStage(type) {
        return type === 1;
    };
    function isPartition(type) {
        return type === 2;
    };
    function isTask(type) {
        return type === 3;
    };

    function getWorkItemTypeLocale(type, form) {
        switch(form) {
            case 'a':
                if (isProject(type.Value) || isPartition(type.Value))
                    return type.Description;
                if (isStage(type.Value))
                    return 'Стадию';
                if (isTask(type.Value))
                    return 'Задачу';
            case 'g':
                if (isProject(type.Value))
                    return 'Проекта';
                if (isStage(type.Value))
                    return 'Стадии';
                if (isPartition(type.Value))
                    return 'Раздела';
                if (isTask(type.Value))
                    return 'Задачи';
            default:
                return type.Description;
        }
    };

    function getFormattedWorkItemInfo(item) {
        return item.Id + ' (' + item.Name + ')';
    }

    function getTextForStateChangedEvent(model) {
        var text = model.User;
        var needAddition = true;
        switch(model.Data.New.Value) {
            case 1:
                text += ' переместил в запланированные ' + getWorkItemTypeLocale(model.Type, 'g').toLowerCase();
                break;
            case 2:
                if (model.Data.Old.Value === 5) {
                    text += ' подтвердил завершение ';
                    needAddition = false;
                } else text += 'завершил ';
                break;
            case 3:
                text += 'удалил ';
                break;
            case 4:
                text += ' переместил в архив ';
                break;
            case 5:
                text += ' переместил на проверку ';
                break;
            case 6:
                if (model.Data.Old.Value === 5)
                    text += ' переместил на доработку';
                else
                    text += ' взял в работу ';
                break;
        }
        if (needAddition)
            text += getWorkItemTypeLocale(model.Type.Value, 'a');
        return text + ' ' + getFormattedWorkItemInfo(model);
    }

    function getTextAndAction(Utils, $stateParams, notificationName, model) {
        var result = {};
        switch(notificationName.toLowerCase()) {
            case 'workitemappoint':
                result.text = model.User + 'назначил(-а) вам ' + getWorkItemTypeLocale(model.Type, 'a').toLowerCase() + ' ' + getFormattedWorkItemInfo(model);
                result.actionName = 'просмотреть';
                result.action = function () { Utils.goToState('base.edit', { workItemId: model.Id }, $stateParams); };
                break;
            case 'workitemdisappoint':
                result.text = model.User + 'снял с вас ответственность за ' + getWorkItemTypeLocale(model.Type, 'a').toLowerCase() + ' ' + getFormattedWorkItemInfo(model);
                result.actionName = 'просмотреть';
                result.action = function () { Utils.goToState('base.edit', { workItemId: model.Id }, $stateParams); };
                break;
            case 'workitemstatechanged':
                result.text = getTextForStateChangedEvent(model);
                result.actionName = 'подробнее';
                result.action = function () { Utils.goToState('base.edit', { workItemId: model.Id }, $stateParams); };
                break;
            case 'workitemchanged':
                result.text = model.User + ' изменил ' + getFormattedWorkItemInfo(model);
                result.actionName = 'подробнее';
                result.action = function () { Utils.goToState('base.edit', { workItemId: model.Id }, $stateParams); };
                break;//workitemadded
            case 'workitemadded':
                result.text = model.User + ' добавил ' + getWorkItemTypeLocale(model.Type, 'a').toLowerCase() + ' ' + getFormattedWorkItemInfo(model);
                result.actionName = 'подробнее';
                result.action = function () { Utils.goToState('base.edit', { workItemId: model.Id }, $stateParams); };
                break;//workitemadded
        }
        return result;
    };

    return {
        $get: ['$stateParams', '$mdToast', 'Utils', function ($stateParams, $mdToast, Utils) {
            var service = {
                notify: function (eventName, args) {
                    var toastData = getTextAndAction(Utils, $stateParams, eventName, args);
                    var toast = $mdToast.simple()
                          .textContent(toastData.text)
                          .action(toastData.actionName.toUpperCase())
                          .highlightAction(true)
                          .position('bottom right');
                    $mdToast.show(toast).then(function (response) {
                        if (response === 'ok') {
                            toastData.action();
                        }
                    });
               
                },
                showToast: function (eventName, notificationModel) {
                    var actionInfo = getActionInfo(eventName, notificationModel, Utils, $stateParams);
                    var toast = $mdToast.simple()
                         .textContent(notificationModel.Text)
                         .action(actionInfo.actionName.toUpperCase())
                         .highlightAction(true)
                         .position('bottom right');
                    $mdToast.show(toast).then(function (response) {
                        if (response === 'ok') {
                            actionInfo.action();
                        }
                    });
                }
            };
            return service;
        }]
    };
});