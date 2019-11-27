using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CM.Models
{
    public class Appointment
    {
        public int AppointmentID { get; set; }
        public int Duration { get; set; }
        public DateTime Date { get; set; }
        public int Coords { get; set; }
        public string Description { get; set; }
        public Account doctor = new Account();

        public Account patient = new Account();
    }
}
