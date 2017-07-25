angapp.controller('TestController', ['$scope', '$sce', '$state', '$stateParams', '$location', 'WorkItemService',
    function ($scope, $sce, $state, $stateParams, $location, WorkItemService) {
        $scope.messages = [];
        $scope.message = '';
       var chat = $.connection.notificationHub;
        // Create a function that the hub can call to broadcast messages.
        chat.client.broadcastMessage = function (name, message) {
            $scope.messages.push({name:name, message: message});
        };
        
        $.connection.hub.start().done(function () {
            $scope.sendMessage = function() {
                chat.server.newMessage('', $scope.message);
                $scope.message = '';
            };

        });
    }]);