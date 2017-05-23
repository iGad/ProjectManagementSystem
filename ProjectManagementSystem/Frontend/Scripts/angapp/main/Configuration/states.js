angapp.config(function ($mdIconProvider) {
    $mdIconProvider
        .iconSet('content', 'Frontend/Content/Images/material-icons/iconsets/content-icons.svg', 24)
        .icon('clear', 'Frontend/Content/Images/material-icons/ic_clear_black_24px.svg')
        .iconSet('image', 'Frontend/Content/Images/material-icons/iconsets/image-icons.svg', 24)
        .icon('exposure_neg_1', 'Frontend/Content/Images/material-icons/ic_exposure_neg_1_black_24px.svg')
        .icon('exposure_plus_1', 'Frontend/Content/Images/material-icons/ic_exposure_plus_1_black_24px.svg')
        .icon('edit', 'Frontend/Content/Images/material-icons/ic_edit_black_24px.svg')
        .iconSet('editor', 'Frontend/Content/Images/material-icons/iconsets/editor-icons.svg', 24)
        .iconSet('navigation', 'Frontend/Content/Images/material-icons/iconsets/navigation-icons.svg', 24)
        .iconSet('hardware', 'Frontend/Content/Images/material-icons/iconsets/hardware-icons.svg', 24)
        .iconSet('action', 'Frontend/Content/Images/material-icons/iconsets/action-icons.svg', 24)
        .icon('delete', 'Frontend/Content/Images/material-icons/delete.svg')
        .iconSet('av', 'Frontend/Content/Images/material-icons/iconsets/av-icons.svg', 24)
        .icon('fast_forward', 'Frontend/Content/Images/material-icons/ic_fast_forward_black_24px.svg')
        .iconSet('communication', 'Frontend/Content/Images/material-icons/iconsets/communication-icons.svg', 24)
        .iconSet('toggle', 'Frontend/Content/Images/material-icons/iconsets/toggle-icons.svg', 24);
});

angapp.config(function ($stateProvider, $routeProvider) {
    $routeProvider.when('/main', { redirectTo: '/main/all' });
    $routeProvider.when('/events', { redirectTo: '/events/new' });
    $routeProvider.when('/profile', { redirectTo: '/profile/info' });
    $routeProvider.when('/', { redirectTo: '/main/all' });
    // An array of state definitions
    var states = [
        {
            name: 'base',
            abstract: true,
            templateUrl: 'Frontend/Views/main/base.html'
        },
        {
            name: 'base.edit',
            url: '/edit/{workItemId}',
            params: { returnStates: null },
            templateUrl: 'Frontend/Views/main/base/workItem.html'
        },
        {
            name: 'base.add',
            url: '/add',
            params: { returnStates: null, projectId: null, stageId: null, partitionId: null, type: null },
            templateUrl: 'Frontend/Views/main/base/workItem.html'
        },
        {
            name: 'base.main',
            url: '/main',
            templateUrl: 'Frontend/Views/main/base/workboard.html'
        },
    {
        name: 'base.main.all',
        url: '/all',
        templateUrl: 'Frontend/Views/main/base/allItems.html'
    },
    {
        name: 'base.main.users',
        url: '/users',
        templateUrl: 'Frontend/Views/main/base/itemsPerUsers.html'
    },
        {
            name: 'base.tasks',
            url: '/tasks',
            templateUrl: 'Frontend/Views/main/test.html'
        },
        {
            name: 'base.about',
            url: '/about',
            template: '<h1>about</h1>'
        },
        {
            name: 'base.events',
            url: '/events',
            templateUrl: 'Frontend/Views/main/events/eventsIndex.html'
        },
        {
            name: 'base.events.new',
            url: '/new',
            templateUrl: 'Frontend/Views/main/events/newEvents.html'
        },
        {
            name: 'base.events.seen',
            url: '/seen?PageNumber&PageSize&SortDirection&SortField&ItemsIds&DateStart&DateEnd',
            templateUrl: 'Frontend/Views/main/events/seenEvents.html'
        },
        {
            name: 'base.test',
            url: '/test',
            templateUrl: 'Frontend/Views/main/test.html'
        },
        {
            name: 'base.projects',
            url: '/projects',
            templateUrl: 'Frontend/Views/main/projects/index.html'
        },
        {
            name: 'base.users',
            url: '/users',
            templateUrl: 'Frontend/Views/main/users/index.html'
        },
        {
            name: 'base.profile',
            abstract: true,
            url: '/profile',
            templateUrl: 'Frontend/Views/main/profile/index.html'
        },
        {
            name: 'base.profile.info',
            url: '/info',
            templateUrl: 'Frontend/Views/main/profile/info.html'
        },
        {
            name: 'base.profile.settings',
            url: '/settings',
            templateUrl: 'Frontend/Views/main/profile/settings.html'
        }
    ];

    // Loop over the state definitions and register them
    states.forEach(function (state) {
        $stateProvider.state(state);
    });
});