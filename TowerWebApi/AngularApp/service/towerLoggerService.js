angular.module('TowerControl').service('towerLoggerService', function ($q, $http) {
    var baseUrl = 'http://localhost:6462/api/';

    var loggs = [];

    var onLog;

    var SetOnLog = function (onLogHandler) {
        onLog = onLogHandler;
    }

    var getLoggs = function () {
        var defender = $q.defer();
        $http.get(baseUrl + 'logger').then(function (response) {
            loggs = response.data;
        });
        return
    }


    function addLoggs(newLoggs) {
        for (var log in responseData) {
            if (onLog)
                onLog(log);

            loggs.push(log);
        };
    }

    return {
        loggs: loggs,
        setOnLog: SetOnLog
    };
});