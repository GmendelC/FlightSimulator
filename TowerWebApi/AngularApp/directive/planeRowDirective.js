angular.module('TowerControl')
    .directive('planeRowDirective', function (emergencyService) {

        //var emegencyFunk = function (plane) {
        //    emergencyService.setPlaneEmergency(plane);
        //};

        return {
            restric: 'E',
            scope: {
                model: '=dirPlane',
               // emergencyFunck: emegencyFunk
            },
            templateUrl: 'AngularApp/template/plane_row_directive.html'
        };
    });