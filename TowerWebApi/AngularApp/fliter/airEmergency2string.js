angular.module('TowerControl')
    .filter('airEmergency2string', function ($sce) {
        return function (input) {
            if (input)
                return 'Was an plane in emergency' +
                    '<i class="fa fa-exclamation-triangle style="font-size:48px;color:red"></i>';
            else
                return $sce.trustAsHtml('<strong>Clean Air</strong> <i class="fa fa-cloud"></i>');
        };
    });