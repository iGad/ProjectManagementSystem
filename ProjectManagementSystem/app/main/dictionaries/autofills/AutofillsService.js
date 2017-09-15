angapp.service("AutofillsService", ["$http", "$q", function ($http, $q) {
    var baseUrl = '/AutofillApi/';
    var workItemTypes;

    function getWorkItemTypesHttp() {
        return $http({
            url: baseUrl + 'GetWorkItemTypes',
            method: "GET",
            async: true
        });
    };


    this.getWorkItemTypes = function() {
        var defer = $q.defer();
        if (!workItemTypes) {
            getWorkItemTypesHttp().then(function(content) {
                    workItemTypes = content;
                    defer.resolve(content);
                },
                function(error) { defer.reject(error); });
        } else {
            defer.resolve(workItemTypes);
        }
        return defer.promise;
    };


    this.getAutofillList = function (filterOptions) {
        return $http({
            url: baseUrl + 'GetAutofillList',
            method: "POST",
            data: { filterModel: filterOptions }
        });
    };
    this.add = function (autofill) {
        return $http({
            url: baseUrl + 'Add',
            method: "POST",
            async: true,
            data: { autofill: autofill }
        });
    };
    this.getWorkItemTypes = function () {
        return $http({
            url: baseUrl + 'GetWorkItemTypes',
            method: "GET",
            async: true
        });
    };
    this.update = function (autofill) {
        return $http({
            url: baseUrl + 'Update',
            method: "POST",
            async: true,
            data: {autofill: autofill}
        });
    };
    this.delete = function (autofillId) {
        return $http({
            url: baseUrl + 'Delete',
            method: 'POST',
            async: true,
            data: { autofillId: autofillId }
        });
    };
}]);
