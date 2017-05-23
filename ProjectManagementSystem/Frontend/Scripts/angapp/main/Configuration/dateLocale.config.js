angapp.config(function($mdDateLocaleProvider) {


    // Can change week display to start on Monday.
    $mdDateLocaleProvider.firstDayOfWeek = 1;


    // Example uses moment.js to parse and format dates.
    $mdDateLocaleProvider.parseDate = function(dateString) {
        var m = moment(dateString, 'L', true);
        var d = moment(parseInt(dateString.substr(6, dateString.length - 8)));
        return m.isValid() ? m.toDate() : ( d.isValid()?d.toDate(): new Date(NaN));
    };

    $mdDateLocaleProvider.formatDate = function(date) {
        var m = moment(date);
        return m.isValid() ? m.format('DD.MM.YYYY') : '';
    };

    //$mdDateLocaleProvider.monthHeaderFormatter = function(date) {
    //    return myShortMonths[date.getMonth()] + ' ' + date.getFullYear();
    //};

    // In addition to date display, date components also need localized messages
    // for aria-labels for screen-reader users.

    $mdDateLocaleProvider.weekNumberFormatter = function(weekNumber) {
        return 'Semaine ' + weekNumber;
    };

    $mdDateLocaleProvider.msgCalendar = 'Calendrier';
    $mdDateLocaleProvider.msgOpenCalendar = 'Ouvrir le calendrier';

    // You can also set when your calendar begins and ends.
    $mdDateLocaleProvider.firstRenderableDate = new Date(1930, 1, 1);
    //$mdDateLocaleProvider.lastRenderableDate = new Date(2012, 11, 21);
});