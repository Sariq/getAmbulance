'use strict';
var reservationsCmp = ['$scope', 'ReservationService', function ($scope, ReservationService) {
    var ctrl = this;

    ReservationService.getReservations().then(function (res) {
        ctrl.reservationsList = res.data;
    });


}]
angular.module('sbAdminApp').component('reservationsCmp', {
    bindings: {
        isAllReservations: '=',
        reservationType : '='
    },
    templateUrl: 'components/reservation/reservations-cmp.html',
    controller: reservationsCmp
});