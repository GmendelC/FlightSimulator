angular.module('TowerControl')
    .filter('int2state', function () {
        return function (input) {
            switch (input) {
                case 0:
                    return 'InAir';
                case 1:
                    return 'AirWaiting1';
                case 2:
                    return 'AirWaiting2';
                case 3:
                    return 'AirWaiting3';
                case 4:
                    return 'LandingRoad';
                case 5:
                    return 'AccessAfterLanding';
                case 6:
                    return 'Boarding';
                case 7:
                    return 'AccessToDeparting';
                case 8:
                    return 'Terminal';
            }
        };
    }); 