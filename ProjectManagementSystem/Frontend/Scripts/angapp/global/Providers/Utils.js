angapp.provider("Utils", function () {
  

    return {
        $get: [function () {
            var service = {
                transformSharpDateToString: function (date) {
                    return date.replaceAll('-', '').replaceAll(':', '');
                }


            };
            return service;
        }]
    };
});