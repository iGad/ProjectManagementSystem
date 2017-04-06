angapp.provider("EventsService", ["uiGridConstants", function (uiGridConstants) {
    var baseUrl = '/EventsApi/';


    function getDefaultGridOptions() {
        return {
            enableRowSelection: true,
            fullRowSelection: true,
            //enableRowHeaderSelection: false,
            //enableRowReordering: false,

            //enableSorting: false,

            //enableHorizontalScrollbar: uiGridConstants.scrollbars.NEVER,
            //enableVerticalScrollbar: uiGridConstants.scrollbars.NEVER,

            //enableFiltering: true,
            //enablePaginationControls: false,
            //multiSelect: false,
            //modifierKeysToMultiSelect: false,
            //noUnselect: true,
            paginationPageSize: 20,
        };
    };

    var getEventsForUser = function ($http) {
        return $http({
            url: baseUrl + 'GetEventsForCurrentUser',
            method: "GET"
        });
    };

    var makeEventSeen = function ($http) {
        return $http({
            url: baseUrl + 'ChangeEventState',
            method: "POST",
            data: 1
        });
    };

    return {
        $get: ["$q", "$http", function ($q, $http) {
            var service = {
                getDefaultGridOptions: function () {
                    return getDefaultGridOptions();
                },
                getEventsForUser: function () {
                    return getEventsForUser($http);
                },
                makeEventSeen: function () {
                    return makeEventSeen($http);
                },

            };
            return service;
        }]
    };
}]);