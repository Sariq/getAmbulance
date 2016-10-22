'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:MainCtrl
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */
angular.module('sbAdminApp')
  .controller('ReservationCtrl', function ($scope, ReservationService) {
      $scope.reservationsList = ReservationService.getReservations();
  });
