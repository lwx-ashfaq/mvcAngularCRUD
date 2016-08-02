using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using AngularJSDemo4.Models;
using AngularJSDemo4.Filter;

namespace AngularJSDemo4.Controllers
{
    public class StudentsController : ApiController
    {
        private StudentDBContext db = new StudentDBContext();

        // GET: api/Students
        [HttpGet]
        public HttpResponseMessage GetStudents()
        {
            var students = db.Students.Select(s => new
            {
                s.StudentId,
                s.FirstName,
                s.LastName,
                s.Gender.GenderName,
                s.Country.CountryName,
                s.State.StateName,
                s.City.CityName,
                s.EmailId,
                s.BirthDate,
                Courses = s.Courses.Select(c => new {c.CourseId, c.CourseName })
            });
             
            if (students != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, students);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, " Student Not Found");
            }
        }

        // GET: api/Students/5
        [HttpGet]
        [ResponseType(typeof(Students))]
        public HttpResponseMessage GetStudentById(int StudentId)
        {
            var student = db.Students.Where(s => s.StudentId == StudentId).Select(i => new
                                                            {
                                                                i.StudentId,
                                                                i.FirstName,
                                                                i.LastName,
                                                                i.GenderId,
                                                                i.Country.CountryId,
                                                                i.State.StateId,
                                                                i.City.CityId,
                                                                i.EmailId,
                                                                i.Password,
                                                                i.BirthDate,
                                                            });

            if (student != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, student);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, " Student Not Found");
            }
        }

        // PUT: api/Students/5
        [HttpPut]
        public HttpResponseMessage PutStudents(StudentCourseModel request)
        {
            Students student = request.Student;
            List<CoursesViewModel> Courses = request.Courses;

            if (ModelState.IsValid)
            {
                if (Courses != null)
                {
                    var studentToUpdate = db.Students
                    .Include(i => i.Courses)
                    .Where(i => i.StudentId == student.StudentId).Single();

                    studentToUpdate.FirstName = student.FirstName;
                    studentToUpdate.LastName = student.LastName;
                    studentToUpdate.GenderId = student.GenderId;
                    studentToUpdate.CountryId = student.CountryId;
                    studentToUpdate.StateId = student.StateId;
                    studentToUpdate.CityId = student.CityId;
                    studentToUpdate.EmailId = student.EmailId;
                    studentToUpdate.Password = student.Password;
                    studentToUpdate.BirthDate = student.BirthDate;

                    string[] selectedCourses = Courses.Where(c => c.IsSelected).Select(i => i.CourseId.ToString()).ToArray();
                    UpdateStudentCourses(selectedCourses, studentToUpdate);
                }
                

                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                return response;
            }
            else
            {
                HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                return response;
            }
        }

        //POST: api/Students
        [HttpPost]
        [ValidateModel]
        [ResponseType(typeof(Students))]
        public HttpResponseMessage PostStudents(StudentCourseModel request)
        {
            Students student = request.Student;
            List<CoursesViewModel> model = request.Courses;
            if (ModelState.IsValid)
            {
                if (model != null)
                {
                    student.Courses = new List<Course>();
                    foreach (var course in model)
                    {
                        if (course.IsSelected == true)
                        {
                            var coursesToAdd = db.Course.Find(Convert.ToInt32(course.CourseId));
                            student.Courses.Add(coursesToAdd);
                        }
                    }
                }

                db.Students.Add(student);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
                return response;
            }
            else
            {
                HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                return response;
            }
        }

        // DELETE: api/Students/5
        [HttpDelete]
        [ResponseType(typeof(Students))]
        public IHttpActionResult DeleteStudents(int id)
        {
            Students students = db.Students.Find(id);
            if (students == null)
            {
                return NotFound();
            }

            db.Students.Remove(students);
            db.SaveChanges();

            return Ok(students);
        }
        [HttpGet]
        public HttpResponseMessage GetCountries()
        {
            var countries = db.Country.Select(c => new { 
                                                c.CountryId,
                                                c.CountryName
                                        });

            if (countries != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, countries);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, " Student Not Found");
            }
        }

        [HttpGet]
        public HttpResponseMessage GetStates(int CountryId)
        {
            var states = db.State.Where(s => s.CountryId == CountryId).Select(s => new
                                                                            {
                                                                                s.StateId,
                                                                                s.StateName
                                                                            }); ;

            if (states != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, states);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, " Student Not Found");
            }
        }

        [HttpGet]
        public HttpResponseMessage GetCities(int StateId)
        {
            var cities = db.City.Where(c => c.StateId == StateId).Select(i => new
            {
                i.CityId,
                i.CityName
            }); ;

            if (cities != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, cities);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, " Student Not Found");
            }
        }
        
        [HttpGet]
        public HttpResponseMessage GetCourses()
        {
            var student = new Students();
            student.Courses = new List<Course>();
            var viewModel = PopulateCourseData(student);

            if (viewModel != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, viewModel);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, " Student Not Found");
            }
        }

        [HttpGet]
        public HttpResponseMessage GetCourseDataById(int studentId)
        {
            Students student = db.Students.Single(s => s.StudentId == studentId);
            if (student != null)
            {
                var viewModel = PopulateCourseData(student);

                if (viewModel != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, viewModel);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, " Student Not Found");
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, " Student Not Found");
            }
        }

        private List<CoursesViewModel> PopulateCourseData(Students student)
        {
            var allCourse = db.Course;
            var studentCourse = new HashSet<int>(student.Courses.Select(c => c.CourseId));
            var viewModel = new List<CoursesViewModel>();
            foreach (var course in allCourse)
            {
                viewModel.Add(new CoursesViewModel
                {
                    CourseId = course.CourseId,
                    CourseName = course.CourseName,
                    IsSelected = studentCourse.Contains(course.CourseId)
                });
            }
            return viewModel;
        }

        private void UpdateStudentCourses(string[] selectedCourses, Students studentToUpdate)
        {
            if (selectedCourses == null)
            {
                studentToUpdate.Courses = new List<Course>();
                return;
            }

            var selectedCoursesHS = new HashSet<string>(selectedCourses);
            var studentCourses = new HashSet<int>
                (studentToUpdate.Courses.Select(c => c.CourseId));
            foreach (var course in db.Course)
            {
                if (selectedCoursesHS.Contains(course.CourseId.ToString()))
                {
                    if (!studentCourses.Contains(course.CourseId))
                    {
                        studentToUpdate.Courses.Add(course);
                    }
                }
                else
                {
                    if (studentCourses.Contains(course.CourseId))
                    {
                        studentToUpdate.Courses.Remove(course);
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StudentsExists(int id)
        {
            return db.Students.Count(e => e.StudentId == id) > 0;
        }
    }
}