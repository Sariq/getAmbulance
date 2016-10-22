angular.module('sbAdminApp')
.factory('Reservations', ['$rootScope', 'Hub', 'localStorageService', '$timeout', 'ReservationService', function ($rootScope, Hub, localStorageService, $timeout, ReservationService) {
   // var Reservations = this;
    var Employees = this;
    var reservationsList = ReservationService.getReservations();

    //Employee ViewModel
    var WL_ID = 1;
    var Employee = function (employee) {
        if (!employee) employee = {};

        var Employee = {
            Id: employee.Id || null,
            Name: employee.Name || 'New',
            Email: employee.Email || 'New',
            Salary: employee.Salary || 1,
            Edit: false,
            Locked: employee.Locked || false,
            displayMode: function () {
                if (this.Locked) return 'lock-template';
                return this.Edit ? 'edit-template' : 'read-template';
            }
        };

        return Employee;
    }

    //Hub setup
    var hub = new Hub('Reservation', {
        rootPath: 'http://localhost:54543/signalr',
        listeners: {
            'newConnection': function (id) {
                Employees.connected.push(id);
                $rootScope.$apply();
                alert(id);
            },
            'removeConnection': function (id) {
                Employees.connected.splice(Employees.connected.indexOf(id), 1);
                $rootScope.$apply();
            },
            'lockEmployee': function (id) {
                var employee = find(id);
                employee.Locked = true;
                $rootScope.$apply();
            },
            'unlockEmployee': function (id) {
                var employee = find(id);
                employee.Locked = false;
                $rootScope.$apply();
            },
            'updatedEmployee': function (id, key, value) {
                var employee = find(id);
                employee[key] = value;
                $rootScope.$apply();
            },
            'addReservation': function (reservation) {
                alert("addReservation")
                reservationsList=ReservationService._getReservations();
            },
            'removeEmployee': function (id) {
                var employee = find(id);
                Employees.all.splice(Employees.all.indexOf(employee), 1);
                $rootScope.$apply();
            }
        },
        methods: ['lock', 'unlock'],
        errorHandler: function (error) {
            console.error(error);
        },
        queryParams: {
            'WL_ID': WL_ID
        },
        token: localStorageService.get('authorizationData').token,
        autoConnect:false
    });
    $timeout(function () {
        hub.connect();
    },2000);

    //Web API setup
   // var webApi = OData('/odata/Employees', { id: '@Id' });

    //Helpers
    var find = function (id) {
        for (var i = 0; i < Employees.all.length; i++) {
            if (Employees.all[i].Id == id) return Employees.all[i];
        }
        return null;
    };

    //Variables
    Employees.all = [];
    Employees.connected = [];
    Employees.loading = true;

    //Methods
    Employees.add = function () {
        webApi.post({ Name: 'New', Email: 'New', Salary: 1 });
    }
    Employees.edit = function (employee) {
        employee.Edit = true;
        hub.lock(employee.Id);
    }
    Employees.delete = function (employee) {
        webApi.remove({ id: employee.Id });
    }
    Employees.patch = function (employee, key) {
        var payload = {};
        payload[key] = employee[key];
        webApi.patch({ id: employee.Id }, payload);
    }
    Employees.done = function (employee) {
        employee.Edit = false;
        hub.unlock(employee.Id);
    }

    //Load
    //Employees.all = webApi.query(function (data) {
    //    var employees = [];
    //    angular.forEach(data.value, function (employee) {
    //        employees.push(new Employee(employee));
    //    });
    //    Employees.all = employees;
    //    Employees.loading = false;
    //});
    return Employees;
}])
