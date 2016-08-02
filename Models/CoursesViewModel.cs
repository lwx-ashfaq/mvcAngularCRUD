using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AngularJSDemo4.Models
{
    public class CoursesViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public bool IsSelected { get; set; }
    }
}