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
        .icon('keyboard_arrow_right', 'Frontend/Content/Images/material-icons/ic_keyboard_arrow_left_black_24px.svg')
        .icon('keyboard_arrow_left', 'Frontend/Content/Images/material-icons/ic_keyboard_arrow_right_black_24px.svg')
        .iconSet('action', 'Frontend/Content/Images/material-icons/iconsets/action-icons.svg', 24)
        .icon('delete', 'Frontend/Content/Images/material-icons/delete.svg')
        .iconSet('av', 'Frontend/Content/Images/material-icons/iconsets/av-icons.svg', 24)
        .icon('fast_forward', 'Frontend/Content/Images/material-icons/ic_fast_forward_black_24px.svg')
        .iconSet('toggle', 'Frontend/Content/Images/material-icons/iconsets/toggle-icons.svg', 24)
        .icon('check_box', 'Frontend/Content/Images/material-icons/ic_check_box_true_24px.svg')
        .icon('check_box_outline_blank', 'Frontend/Content/Images/material-icons/ic_check_box_false_24px.svg');
});

angapp.config(function ($stateProvider) {
    // An array of state definitions
    var states = [
    {
        name: 'base',
        templateUrl: 'Frontend/Views/main/base.html'
    },
        {
            name: 'base.main',
            url: '/',
            template: '<h1>Main</h1>'
        },
    {
        name: 'base.tasks',
        url: '/tasks',
        template: '<h1>Tasks</h1>'
    },
        {
            name: 'base.about',
            url: '/about',
            template: '<h1>about</h1>'
        },
        {
            name: 'base.projects',
            url: '/projects',
            template: '<h1>Projects</h1>'
        },
        {
            name: 'base.users',
            url: '/users',
            templateUrl:'Frontend/Views/main/users/index.html'
        }
    ];

    // Loop over the state definitions and register them
    states.forEach(function (state) {
        $stateProvider.state(state);
    });
});