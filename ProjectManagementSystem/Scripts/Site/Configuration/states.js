angapp.config(function ($stateProvider) {
    // An array of state definitions
    var states = [
    {
        name: 'base',
        url: '/',
        templateUrl: 'Views/'
    },
        {
            name: 'base.main',
            url: 'main',
            template: '<h1>Main</h1>'
        },
    {
        name: 'base.tasks',
        url: 'tasks',
        template: '<h1>Tasks</h1>'
    },
        {
            name: 'base.about',
            url: 'about',
            template: '<h1>about</h1>'
        }
    ];

    // Loop over the state definitions and register them
    states.forEach(function (state) {
        $stateProvider.state(state);
    });
});