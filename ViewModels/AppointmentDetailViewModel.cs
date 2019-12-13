using CM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CM.ViewModels
{
    public class AppointmentDetailViewModel
    {
        public int AppointmentID { get; set; }
        public int Duration { get; set; }
        public DateTime DateTime { get; set; }
        public int Coords { get; set; }
        public string Description { get; set; }
        public Account Doctor { get; set; }
        public Account Patient { get; set; }
        public DateTime SendTime { get; set; }

        public AppointmentDetailViewModel()
        {
            Doctor = new Account();

            Patient = new Account();
        }
    }
}
