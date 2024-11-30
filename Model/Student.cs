using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace STFREYA.Model
{
    public class Student
    {
        public int student_id { get; set; }

        public string name { get; set; }

        public string lastname { get; set; }

        public string age { get; set; }

        public string email { get; set; }

        public string contactno { get; set; }

        public string course { get; set; }


        public string FullName => $"{name} {lastname}";
        public bool IsSelected { get; set; } = false; // Default to false
    }
}
