angular.module('TowerControl')
    .service('myHubService', function (Hub) {

        var setHub = function (logFunc) {
            return new Hub('ClientHub', {

                //client side methods
                listeners: {
                    'onLog': logFunc
                },
                //handle connection error
                errorHandler: function (error) {
                    console.error(error);
                }
                //specify a non default root
                //rootPath: '/api
                //stateChanged: function (state) {
                //    switch (state.newState) {
                //        case $.signalR.connectionState.connecting:
                //            //your code here
                //            break;
                //        case $.signalR.connectionState.connected:
                //            //your code here
                //            break;
                //        case $.signalR.connectionState.reconnecting:
                //            //your code here
                //            break;
                //        case $.signalR.connectionState.disconnected:
                //            //your code here
                //            break;
                //    }
                //}
            });
        };

        return { 'setHub': setHub };
    });

