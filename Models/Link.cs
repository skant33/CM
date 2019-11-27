using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CM.Models
{
    public class Link
    {
        public int LinkID { get; set; }
        public int DoctorID { get; set; }
        public int PatientID { get; set; }
    }
}
