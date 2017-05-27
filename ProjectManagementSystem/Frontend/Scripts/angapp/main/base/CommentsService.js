angapp.service("CommentsService", ["$http", function ($http) {
    var baseUrl = '/CommentsApi/';



    this.getComments = function (objectId) {
        return $http({
            url: baseUrl + 'GetCommentsForObject',
            method: "GET",
            params: {objectId: objectId}
        });
    };

    this.addComment = function (comment) {
        return $http({
            url: baseUrl + 'AddComment',
            method: 'POST',
            data: { comment: comment }
        });
    };

    this.deleteComment = function (id) {
        return $http({
            url: baseUrl + 'DeleteComment',
            method: 'POST',
            data: { id: id }
        });
    };
}]);