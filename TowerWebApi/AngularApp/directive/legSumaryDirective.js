angular.module('TowerControl')
    .directive('legSumaryDirective', function () {
        return {
            restric: 'E',
            scope: {
                currentleg: '=leg'
            },
            templateUrl: 'AngularApp/template/leg_sumary_directive.html'
        };
    });