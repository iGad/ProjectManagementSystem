﻿angapp.run(function ($templateCache) {

    $templateCache.put('toastWithAction',
        '<div class="{{toastClass}} {{toastType}}" ng-click="tapToast()">\
            <div ng-switch on= "allowHtml" >\
            <div ng-switch-default ng-if="title" class="{{titleClass}}" aria-label="{{title}}">{{ title }}</div>\
    <div ng-switch-default class="{{messageClass}}" aria-label="{{message}}">{{ message }}</div>\
    <div ng-switch-when="true" ng-if="title" class="{{titleClass}}" ng-bind-html="title"></div>\
    <div ng-switch-when="true" class="{{messageClass}}" ng-bind-html="message"></div>\
    <md-button>More</md-button>\
    </div>\
    <progress-bar ng-if="progressBar"></progress-bar>\
    </div>');
});