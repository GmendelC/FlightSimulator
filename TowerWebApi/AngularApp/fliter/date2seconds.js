angular.module('TowerControl')
    .filter('date2seconds', function () {
        return function (input) {
            if (input) {
                var milliseconds = new Date() - new Date(input);
                return milliseconds / 1000;
            }
            else
                return "";
        };
    });