﻿angular.module('TowerControl')
    .controller('legsController',
    function ($scope, $interval, dataService, emergencyService, $log, myHubService) {

        var legs;
        $scope.towerLogs = [];
        $scope.airEmergency;

            function refresh() {
                dataService.getLegs().then(function (dataLegs) {
                    legs = dataLegs;
                    legs.sort(function (a, b) {
                        return a.Id - b.Id;
                    });
                    $scope.legs = legs;
                });
                dataService.getPlanes().then(function (dataPlanes) {
                    var havePlaneEmergency = dataPlanes.some(
                        function (plane) { return plane.Emergency; });
                    $scope.airEmergency = havePlaneEmergency;
                });
            };

            var stopRefresh = $interval(refresh,500);

            myHubService.setHub(function (message) {
                $scope.towerLogs.unshift(message);
                if ($scope.towerLogs.length > 10)
                    $scope.towerLogs.pop(); 
            }); 

            refresh();

            var emergencyClick = function (leg) {
                if (!leg.InEmergency)
                    emergencyService.setLegEmergency(leg);
                else
                    emergencyService.deleteLegEmergency(leg);
            };

            $scope.emergencyClick = emergencyClick;

            $scope.$on('$destroy', function () {
                $interval.cancel(stopRefresh);
            });

});