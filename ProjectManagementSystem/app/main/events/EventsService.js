angapp.service("EventsService", ["$http", function ($http) {
    var baseUrl = '/EventsApi/';



    this.getSeenEventsForUser = function (filterOptions) {
        return $http({
            url: baseUrl + 'GetSeenEventsForCurrentUser',
            method: "POST",
            data: {filterModel : filterOptions}
        });
    };
    this.getNewEventsForUser = function () {
        return $http({
            url: baseUrl + 'GetNewEventsForCurrentUser',
            method: "GET"
        });
    };
    this.makeSeen = function (eventIds) {
        return $http({
            url: baseUrl + 'ChangeEventsState',
            method: "POST",
            data: { eventIds: eventIds, state: 1 }
        });
    };
    this.getUnseenEventCount = function () {
        return $http({
            url: baseUrl + 'GetUnseenEventCountForCurrentUser',
            method: "GET"
        });
    };
    this.changeIsFavorite = function (event) {
        return $http({
            url: baseUrl + 'ChangeIsFavorite',
            method: 'POST',
            data: { eventId: event.Id, isFavorite: event.IsFavorite }
        });
    };
}]);
