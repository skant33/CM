﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CM.ViewModels
{
    public class AppointmentDetailViewModel
    {
        public int AppointmentID { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        public int Duration { get; set; }
        public DateTime Date { get; set; }
        public int Coords { get; set; }
        public bool Done { get; set; }
    }
}