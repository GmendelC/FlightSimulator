angular.module('TowerControl')
    .filter('bool2airState', function () {
        return function (input) {
            if (input)
                return 'in land';
            else
                return 'in Air';
        };
    });