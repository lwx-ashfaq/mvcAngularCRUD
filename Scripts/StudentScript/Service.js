
myapp.service('crudservice', function ($http) {
    debugger;
    this.getAllStudents = function () {
        var request = $http({
            async: true,
            method: 'get',
            url: 'api/Students/GetStudents'
        });
        return request;
        //return $http.get("/api/Student/GetStudents");
    }

    this.save = function (Student, Courses) {
        var request = $http({
            method: 'post',
            url: 'api/Students/PostStudents',
            data: JSON.stringify({ Student: Student, Courses: Courses }),
        });
        return request;
    }

    this.get = function (StudentId) {
        var request = $http({
            async: true,
            method: 'get',
            url: 'api/Students/GetStudentById',
            params: {
            StudentId: StudentId
        }
        });
        return request;
    }

    this.update =function (Student, Courses) {
        var request = $http({
            method: 'put',
            url: "/api/Students/PutStudents",
            data: JSON.stringify({ Student: Student, Courses: Courses }),
        });
        return request;
    }

    this.delete = function (StudentId) {
        var deleterecord= $http({
            method: 'delete',
            url: "/api/Students/DeleteStudents" + StudentId
        });
        return deleterecord;
    }

    this.GetCountries = function () {
        var request = $http({
            method: 'get',
            url: "/api/Students/GetCountries"
        });
        return request;
    }

    this.GetStates = function (CountryId) {
        var request = $http({
            method: 'get',
            url: "/api/Students/GetStates",
            params: {
                CountryId: CountryId
            }
   
        });
        return request;
    }

    this.GetCities = function (StateId) {
        var request = $http({
            method: 'get',
            url: "/api/Students/GetCities" , 
            params: {
                StateId: StateId
            }
        });
        return request;
    }

    this.GetCourses = function () {
        var request = $http({
            method: 'get',
            url: "/api/Students/GetCourses"
        });
        return request;
    }

    this.GetCourseById = function (StudentId) {
        var request = $http({
            method: 'get',
            url: "/api/Students/GetCourseDataById",
            params: {
                StudentId: StudentId
            }
        });
        return request;
    }

});