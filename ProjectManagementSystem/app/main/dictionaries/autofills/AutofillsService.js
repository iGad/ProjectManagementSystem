angapp.service("AutofillsService", ["$http", function ($http) {
    var baseUrl = '/AutofillApi/';



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
