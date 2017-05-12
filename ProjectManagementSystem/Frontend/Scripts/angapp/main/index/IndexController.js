﻿angapp.controller('IndexController', ['$scope', '$rootScope', '$location', '$state', 'NotificationService', 'EventsService', 'Utils',
    function ($scope, $rootScope, $location, $state, notificationService, eventsService, utils) {
    $scope.title = 'Привет!';
    function getActiveStateIndex() {
        var path = $location.path().split('?')[0];
        var tabName = path.slice(path.lastIndexOf('/') + 1);
        switch (tabName.toLowerCase()) {
            case 'projects': return 0;
            case 'tasks': return 1;
            case 'about': return 2;
                //case 'tasks': return 3;
                //case 'map': return 4;
            case 'users': return 3;
                case 'events':return 4;
            default:
                return -1;
        }
    };

        function updateUnseenEventCount() {
            eventsService.getUnseenEventCount().then(function(content) {
                $scope.unseenEventCount = content.data;
            }, utils.onError);
        };

        updateUnseenEventCount();

    $scope.activeIndex = getActiveStateIndex();
    $rootScope.$on('$stateChangeStart', function (evt, toState, toParams, fromState, fromParams) {
        console.log("$stateChangeStart " + fromState.name + JSON.stringify(fromParams) + " -> " + toState.name + JSON.stringify(toParams));
    });
    $rootScope.$on('$stateChangeSuccess', function () {
        console.log("$stateChangeSuccess " + fromState.name + JSON.stringify(fromParams) + " -> " + toState.name + JSON.stringify(toParams));
    });
    $rootScope.$on('$stateChangeError', function () {
        console.log("$stateChangeError " + fromState.name + JSON.stringify(fromParams) + " -> " + toState.name + JSON.stringify(toParams));
    });
    var notificationHub = $.connection.notificationHub;
    // Create a function that the hub can call to broadcast messages.
    notificationHub.client.raiseEvent = function (eventName, params) {
        $scope.$broadcast(eventName, params);
        updateUnseenEventCount();
    };

    notificationHub.client.recieveNotification = function (eventName, args) {
        notificationService.showToast(eventName, args);
    };

    $.connection.hub.start().done(function () {
        $scope.isConnected = true;

    });
    //$rootScope.$on('$stateChangeStart', function (e, toState, toParams, fromState, fromParams) {
    //    if (toState.module === 'private') {
    //        // If logged out and transitioning to a logged in page:
    //        e.preventDefault();
    //        //$state.go('public.login');
    //    } else if (toState.module === 'public') {
    //        // If logged in and transitioning to a logged out page:
    //        e.preventDefault();
    //        //$state.go('tool.suggestions');
    //    };
    //});



    

    if($location.path().length < 1)
        $state.go('base.main');
}]);