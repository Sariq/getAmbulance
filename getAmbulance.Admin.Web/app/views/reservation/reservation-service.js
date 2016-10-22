'use strict';
angular.module('sbAdminApp').factory('ReservationService', ['$http', 'ngAuthSettings','authService', function ($http, ngAuthSettings, authService) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var ReservationServiceFactory = {};
    
    var _getReservations = function () {
        var data={ status : "0",
            whiteLabelId: authService.authentication.WhiteLabelId
        }
        return $http.post(serviceBase + 'api/Reservation/GetReservationsListByWhiteLabelId', data);
    };

    ReservationServiceFactory.getReservations = _getReservations;

    return ReservationServiceFactory;

}]);