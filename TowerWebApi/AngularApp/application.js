angular.module('TowerControl', ['angular-loading-bar', 'ngAnimate', 'SignalR', 'ngRoute', 'ngSanitize'])
    .config(function ($routeProvider, $locationProvider) {

        $routeProvider
            .when('/', {
                redirectTo:'/legs'
            })
            .when('/legs', {
                controller: 'legsController',
                templateUrl: 'AngularApp/partial/legs.html'
            })
            .when('/flights', {
                controller: 'flightController',
                templateUrl: 'AngularApp/partial/flights.html'
            })
            .when('/about', {
                templateUrl: 'AngularApp/partial/about.html'
            })
    });
  ;
