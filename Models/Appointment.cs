using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CM.Models
{
    public class Appointment
    {
        public int AppointmentID { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        public int Duration { get; set; }
        public DateTime Date { get; set; }
        public int Coords { get; set; }
        public bool Done { get; set; }
        public string DoctorEmail { get; set; }
    }
}
