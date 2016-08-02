
myapp.controller('crudcontroller', function ($scope, crudservice) {
    debugger;
    //Loads all Employee records when page loads

    initControl();
    function initControl()
    {
        $scope.detailsView = true;
        $scope.AddView = false;
        $scope.EditView = false;
        $scope.DeleteView = false;
        $scope.Student = null;
        $scope.alert = false;
        loadStudents();
        GetCountries();
    }

    function loadStudents() {
        $scope.Students = crudservice.getAllStudents()
            .success(function (response) {
                $scope.Students = response;
            })     
    }

    function GetCountries() {
        $scope.countries = crudservice.GetCountries()
            .success(function (responce) {
                $scope.countries = responce;
            })
    }

    $scope.GetStates = function () {
        $scope.State = null;
        $scope.City = null;
        $scope.states = null;
        $scope.cities = null;
        var Country = $scope.Student.CountryId;
        if (Country != "") {
            crudservice.GetStates(Country)
                .success(function (responce) {
                    $scope.states = responce;
                })
        }
    }

    $scope.GetCities = function () {
        $scope.cities = null;
        $scope.City = null;
        var State = $scope.Student.StateId;
        if (State != "") {
            $scope.cities = crudservice.GetCities(State)
                .success(function (responce) {
                    $scope.cities = responce;
                })
        }
    }

    function GetCourse() {
        $scope.courses = crudservice.GetCourses()
        .success(function (responce) {
            $scope.courses = responce;
        })
    }

    function GetCourseById(StudentId) {
        crudservice.GetCourseById(StudentId)
            .success(function (response) {
                $scope.courses = response;
            })
    }

    function ClearControl() {

        $scope.Student = null;
        $scope.courses = null;
        $scope.states = null;
        $scope.cities = null;
    }

    $scope.AddDiv = function () {
        GetCourse();
        $scope.AddView = true;
    }

    $scope.EditDiv= function (student) {
        $scope.get(student);
        $scope.EditView = true;
    }

    $scope.get = function (student) {

        crudservice.get(student.StudentId)
            .success(function (response) {
                $scope.Student = response[0];
                $scope.Student.BirthDate = $.format.date(response[0].BirthDate, "dd-MMM-yyyy");
                $scope.GetStates();
                $scope.GetCities();
                GetCourseById(response[0].StudentId);
            })
    }

    $scope.Add = function (Student) {

        Courses = $scope.courses;
        $scope.Students = crudservice.save(Student, Courses)
                 .success(function (Status) {
                     initControl();
                     $scope.message = "Student added Successfully...!";
                     $scope.alert = true;
                     ClearControl();
                 }).error(function (response) {
                     var errors = [];
                     for (var key in response.ModelState) {
                         for (var i = 0; i < response.ModelState[key].length; i++) {
                             errors.push(response.ModelState[key][i]);
                         }
                     }
                     $scope.errors = errors;
                 });
    }

    $scope.Update = function () {

        Courses = $scope.courses;

        if ($scope.Student.StudentId != null) {
            var StudentId = $scope.Student.StudentId;
            var Student = $scope.Student;
            $scope.Students = crudservice.update(Student, Courses)
                .success(function (status) {
                    $scope.message = "Student Updated Successfully...!";
                    $scope.alert = true;
                    ClearControl();
                    initControl();
                }).error(function (data, status, headers, config) {
                    $scope.errors = data;
                });
        }
    }

    $scope.Cancel = function () {
        ClearControl();
        initControl();
    }

    function parseErrors(response) {
        var errors = [];
        for (var key in response.ModelState) {
            for (var i = 0; i < response.ModelState[key].length; i++) {
                errors.push(response.ModelState[key][i]);
            }
        }
        return errors;
    }

    $scope.formats = ['dd/MM/yyyy', 'dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];
    $scope.format = $scope.formats[0];
    $scope.maxDate = new Date();
    $scope.minDate = new Date("01/01/1965");

    $scope.dpOpenStatus = {};

    $scope.setDpOpenStatus = function (id) {
        $scope.dpOpenStatus[id] = true
    };

});

