using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STFREYA.Model
{
    public class AcademicHistory
    {
        public string Id { get; set; } // Primary key
        public int StudentId { get; set; } // Foreign key from student_list
        public string Course { get; set; } // Course name
        public string yearlevel { get; set; }
        public DateTime Date { get; set; } // Year or date
    }
}
