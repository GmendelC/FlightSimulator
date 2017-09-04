angular.module('TowerControl')
    .filter('timespan2seconds', function () {
        return function (input) {
            if (typeof input === 'string') {
                var inputArr = input.split(':');
                var output = inputArr[5] + inputArr[6];
                return output;
            }
            else {
                return input;
            }
        };
    });