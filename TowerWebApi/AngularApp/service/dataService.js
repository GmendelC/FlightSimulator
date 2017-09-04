angular.module('TowerControl').service('dataService', function ($q, $http) {
    var baseUrl = 'http://localhost:6462/api/';

    var getLegs = function () {

        var deffeder = $q.defer();

        $http.get(baseUrl + 'leg')
            .then(function (response) {
                deffeder.resolve(response.data);
            });

        return deffeder.promise;
    };

    var getPlanes = function () {
        var deffeder = $q.defer();

        $http.get(baseUrl + 'flight')
            .then(function (response) {
                deffeder.resolve(response.data);
            });

        return deffeder.promise;
    };

    var getAllTower = function () {
        var tower = {};

        var legPromise = getLegs()
            .then(function (legs) { tower.legs = legs; });
        var planePromise = getPlanes()
            .then(function (planes) { tower.planes = planes });

        return $q.all([legPromise, planePromise])
            .then(function () { return tower });
    }

    return {
        getAllTower: getAllTower,
        getPlanes: getPlanes,
        getLegs: getLegs
    };
});