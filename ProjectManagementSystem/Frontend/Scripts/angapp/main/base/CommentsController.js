angapp.controller('CommentsController', [
    '$scope', '$stateParams', '$mdDialog', 'CommentsService', 'UsersService', 'Utils',
    function ($scope, $stateParams, $mdDialog, commentsService, usersService, utils) {
        $scope.newComment = { ObjectId: $scope.ObjectId };

        function prepareComment(comment) {
            comment.UserInfo = usersService.getUserDisplayText(comment.User);
            comment.Date = utils.convertDateToJsString(comment.CreatedDate);
        };

        function getComments() {
            commentsService.getComments($scope.ObjectId).then(function (content) {
                $scope.comments = content.data;
                for (var i = 0; i < $scope.comments.length; i++) {
                    prepareComment($scope.comments[i]);
                }
            }, utils.onError);
        };
        $scope.comments = [];

        $scope.deleteComment = function (ev, comment) {
            var confirm = $mdDialog.confirm()
             .title('Подтверждение')
             .textContent('Вы уверены, что хотите удалить комментарий')
             .ariaLabel('are you sure')
             .targetEvent(ev)
             .ok('Да')
             .cancel('Нет');
            $mdDialog.show(confirm).then(function () {
                commentsService.deleteComment(comment.Id).then(function() {
                    $scope.comments.splice($scope.comments.indexOf(comment), 1);
                }, utils.onError);
            }, function() {});
        };

        $scope.addComment = function(comment) {
            comment.ObjectId = $scope.ObjectId;
            commentsService.addComment(comment).then(function (content) {
                prepareComment(content.data);
                $scope.comments.splice(0, 0, content.data);
                $scope.newComment = { ObjectId: $scope.ObjectId };
            }, utils.onError);
        }

        getComments();
    }]);