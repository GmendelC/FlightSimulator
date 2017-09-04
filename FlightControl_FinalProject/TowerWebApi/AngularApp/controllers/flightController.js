angular.module('TowerControl')
    .controller('flightController',
    function ($scope, dataService, emergencyService, myHubService, $log) {
            $scope.flights;
            $scope.search = { $: '' };

            //myHubService.setHub(function (message) {
            //    $log.log(message);
            //}); 

            var emegencyFunk = function (plane) {
                emergencyService.setPlaneEmergency(plane);
            };

            $scope.emegencyFunk = emegencyFunk;

            var refresh = function () {
                dataService.getPlanes().then(function (planes) {
                    $scope.flights = planes;
                });
            };

            $scope.refresh = refresh;

            refresh();
});