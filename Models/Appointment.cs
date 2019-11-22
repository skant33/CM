using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CM.Models
{
    public class Appointment
    {
        public int AppointmentID { get; set; }
        public Account Client { get; set; }
        public Account Doctor { get; set; }
        public int Duration { get; set; }
        public DateTime Date { get; set; }
        public int Coords { get; set; }
        public string Description { get; set; }
        public string DoctorEmail { get; set; }
    }
}
