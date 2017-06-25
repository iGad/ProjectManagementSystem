angapp.provider("Utils", function () {
  

    return {
        $get: ['$state', '$stateParams', '$mdDateLocale', '$mdDialog',
            function ($state, $stateParams, $mdDateLocaleProvider, $mdDialog) {
            var service = {
                onError: function(error) {
                    console.error(error);
                },
                onErrorWithMessageBox: function(error) {
                     $mdDialog.show(
                      $mdDialog.alert()
                        .clickOutsideToClose(true)
                        .title('Ошибка')
                        .textContent(error.data.Message)
                        .ariaLabel('Alert Dialog Demo')
                        .ok('ОК')
                    );
                },
                showMessageBox: function (text) {
                    $mdDialog.show(
                      $mdDialog.alert()
                        .clickOutsideToClose(true)
                        .title('Ошибка')
                        .textContent(text)
                        .ariaLabel('Alert Dialog Demo')
                        .ok('ОК')
                    );
                },
                parseStateParams: function (stateUrl, stateParams, arrayPropertyNames) {
                    var propertyNames = stateUrl.split('?')[1].split('&');//получение массива имен возможных параметров 
                    var filterOptions = {};
                    for (var i = 0; i < propertyNames.length; i++) {
                        filterOptions[propertyNames[i]] = stateParams[propertyNames[i]];
                        if (arrayPropertyNames && arrayPropertyNames.filter(x => x === propertyNames[i]).length && !Array.isArray(filterOptions[propertyNames[i]])) {
                            filterOptions[propertyNames[i]] = [filterOptions[propertyNames[i]]];
                        }
                    }
                    filterOptions.PageNumber = filterOptions.PageNumber || 1;
                    //filterOptions.PageSize = filterOptions.PageSize || 30;
                    return filterOptions;
                },
                goToReturnState: function(stateParams) {
                    if (!stateParams || !stateParams.returnStates || !stateParams.returnStates.length) {
                        $state.go('base.main');
                        return;
                    }
                    var returnState = stateParams.returnStates.splice(0, 1)[0];
                    var params = returnState.params ? returnState.params : {returnStates : stateParams.returnStates};
                    $state.go(returnState.name, params);
                },
                goToState: function(stateName, stateParams, currentParams) {
                    stateParams = stateParams ? stateParams : {};
                    currentParams = currentParams ? currentParams : {};
                    var currentStateInfo = { name: $state.$current.name, params: angular.copy(currentParams) };
                    var returnStates = currentParams.returnStates ? currentParams.returnStates : [];
                    returnStates.splice(0, 0, currentStateInfo);
                    stateParams.returnStates = returnStates;
                    $state.go(stateName, stateParams);
                },
                convertDateToJsString: function (sharpDateTime) {
                    return moment(parseInt(sharpDateTime.substr(6, sharpDateTime.length - 8))).format('DD.MM.YYYY HH:mm');
                },
                convertDateToJsDate: function (sharpDateTime) {
                    return $mdDateLocaleProvider.parseDate(sharpDateTime);
                },
                getPagination: function() {
                    return {
                        CurrentPageLabel: 'Текущая страница',
                        OfLabelString: 'из',
                        Records: 'Записей',
                        BackString: 'Назад',
                        ForwardString: 'Вперед',
                        ToLastPage: 'К последней странице',
                        ToFirstPage: 'К первой странице'
                    };
                },
                getUserInfo: function (user) {
                    if (!user)
                        return 'Отсутствует';
                    var info = '';
                    if (user.Surname)
                        info = user.Surname + ' ';
                    info += user.Name;
                    if (info.length === user.Name.length)
                        info += ' (' + user.Email + ')';
                    return info;
                }
        };
            return service;
        }]
    };
});