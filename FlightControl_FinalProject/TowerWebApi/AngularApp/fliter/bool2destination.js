angular.module('TowerControl')
    .filter('bool2destination', function () {
        return function (input) {
            if (input)
                return 'Deperting';
            else
                return 'Landing';
        };
    });