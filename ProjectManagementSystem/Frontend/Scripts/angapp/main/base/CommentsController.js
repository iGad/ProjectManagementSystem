angapp.controller('CommentsController', [
    '$scope', '$stateParams', '$mdDialog', 'CommentsService', 'Utils',
    function ($scope, $stateParams, $mdDialog, commentsService, utils) {

        function getComments() {
            commentsService.getComments($scope.ObjectId).then(function (content) {
                $scope.comments = content.data;
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
            commentsService.addComment(comment).then(function(content) {
                $scope.comments.splice(0, 0, content.data);
            }, utils.onError);
        }

        getComments();
    }]);