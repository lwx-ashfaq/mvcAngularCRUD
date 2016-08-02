using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AngularJSDemo4.Models
{
    public class StudentCourseModel
    {
        [Required]
        public Students Student { get; set; }
        public List<CoursesViewModel> Courses { get; set; }
    }
}