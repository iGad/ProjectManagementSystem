angapp.config(function ($mdIconProvider) {
    $mdIconProvider
        .iconSet('content', 'assets/images/material-icons/iconsets/content-icons.svg', 24)
        .icon('clear', 'assets/images/material-icons/ic_clear_black_24px.svg')
        .iconSet('image', 'assets/images/material-icons/iconsets/image-icons.svg', 24)
        .icon('exposure_neg_1', 'assets/images/material-icons/ic_exposure_neg_1_black_24px.svg')
        .icon('exposure_plus_1', 'assets/images/material-icons/ic_exposure_plus_1_black_24px.svg')
        .icon('edit', 'assets/images/material-icons/ic_edit_black_24px.svg')
        .iconSet('editor', 'assets/images/material-icons/iconsets/editor-icons.svg', 24)
        .iconSet('navigation', 'assets/images/material-icons/iconsets/navigation-icons.svg', 24)
        .iconSet('hardware', 'assets/images/material-icons/iconsets/hardware-icons.svg', 24)
        .iconSet('action', 'assets/images/material-icons/iconsets/action-icons.svg', 24)
        .icon('delete', 'assets/images/material-icons/delete.svg')
        .iconSet('av', 'assets/images/material-icons/iconsets/av-icons.svg', 24)
        .icon('fast_forward', 'assets/images/material-icons/ic_fast_forward_black_24px.svg')
        .iconSet('communication', 'assets/images/material-icons/iconsets/communication-icons.svg', 24)
        .iconSet('toggle', 'assets/images/material-icons/iconsets/toggle-icons.svg', 24);
});

angapp.config(function ($stateProvider, $routeProvider) {
    $routeProvider.when('/main', { redirectTo: '/main/all' });
    $routeProvider.when('/events', { redirectTo: '/events/new' });
    $routeProvider.when('/profile', { redirectTo: '/profile/info' });
    $routeProvider.when('/', { redirectTo: '/main/all' });
    // An array of state definitions
    var states = [
        {
            name: 'login',
            url:'/login?returnUrl',
            templateUrl: 'app/login/login.html',
            controller: 'LoginController'
        },
        {
            name: 'base',
            abstract: true,
            templateUrl: 'app/main/index/base.html'
        },
        {
            name: 'base.edit',
            url: '/edit/{workItemId}',
            params: { returnStates: null },
            templateUrl: 'app/main/base/workItem.html'
        },
        {
            name: 'base.add',
            url: '/add',
            params: { returnStates: null, projectId: null, stageId: null, partitionId: null, type: null },
            templateUrl: 'app/main/base/workItem.html'
        },
        {
            name: 'base.main',
            url: '/main',
            templateUrl: 'app/main/base/workboard.html'
        },
    {
        name: 'base.main.all',
        url: '/all',
        templateUrl: 'app/main/base/allItems.html'
    },
    {
        name: 'base.main.users',
        url: '/users',
        templateUrl: 'app/main/base/itemsPerUsers.html'
    },
        {
            name: 'base.tasks',
            url: '/tasks',
            templateUrl: 'app/main/test.html'
        },
        {
            name: 'base.about',
            url: '/about',
            template: '<h1>about</h1>'
        },
        {
            name: 'base.events',
            url: '/events',
            templateUrl: 'app/main/events/eventsIndex.html'
        },
        {
            name: 'base.events.new',
            url: '/new',
            templateUrl: 'app/main/events/newEvents.html'
        },
        {
            name: 'base.events.seen',
            url: '/seen?PageNumber&SortDirection&SortField&ItemsIds&DateStart&DateEnd&IsFavorite&UserIds',
            templateUrl: 'app/main/events/seenEvents.html'
        },
        {
            name: 'base.test',
            url: '/test',
            templateUrl: 'app/main/test.html'
        },
        {
            name: 'base.projects',
            url: '/projects',
            templateUrl: 'app/main/projects/index.html'
        },
        {
            name: 'base.users',
            url: '/users',
            templateUrl: 'app/main/users/index.html'
        },
        {
            name: 'base.profile',
            abstract: true,
            url: '/profile',
            templateUrl: 'app/main/profile/index.html'
        },
        {
            name: 'base.profile.info',
            url: '/info',
            templateUrl: 'app/main/profile/info.html'
        },
        {
            name: 'base.profile.settings',
            url: '/settings',
            templateUrl: 'app/main/profile/settings.html'
        },
        {
            name: 'base.settings',
            url: '/settings',
            templateUrl: 'app/main/settings/settings.html'
        },
        {
            name: 'base.search',
            url: '/serach?UserIds&SearchText&States&Types',
            templateUrl: 'app/main/search/search.html'
        },
    ];

    // Loop over the state definitions and register them
    states.forEach(function (state) {
        $stateProvider.state(state);
    });
});