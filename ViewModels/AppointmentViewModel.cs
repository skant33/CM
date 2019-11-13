using CM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CM.ViewModels
{
    public class AppointmentViewModel
    {
        public List<Appointment> appointments;
        public List<AppointmentDetailViewModel> Afspraken = new List<AppointmentDetailViewModel>();
    }
}
