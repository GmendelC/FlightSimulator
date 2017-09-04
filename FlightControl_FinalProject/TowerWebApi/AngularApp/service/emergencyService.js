angular.module('TowerControl').service('emergencyService', function ($q, $http, $log) {

    var baseUrl = 'http://localhost:6462/api/';

    var deleteLegEmergency = function (leg) {
        $http.delete(baseUrl + 'leg/emergency/' + leg.Id)
            .then(function () { },
            function (reason) { $log.error.logs.push(reason); });
    };

    var setLegEmergency = function (leg) {
        $http.get(baseUrl + 'leg/emergency/' + leg.Id)
            .then(function () { },
            function (reason) { $log.error.logs.push(reason); });
    };

    var setPlaneEmergency = function (plane) {
        $http.get(baseUrl + 'flight/emergency/' + plane.Id)
            .then(function () { },
            function (reason) { $log.error.logs.push(reason); });
    };

    return {
        setPlaneEmergency: setPlaneEmergency,
        setLegEmergency: setLegEmergency,
        deleteLegEmergency: deleteLegEmergency
    }

});