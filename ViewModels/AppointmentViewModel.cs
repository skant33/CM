using CM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CM.ViewModels
{
    public class AppointmentViewModel
    {
        public List<AppointmentDetailViewModel> appointments { get; set; } = new List<AppointmentDetailViewModel>();

        public AppointmentViewModel()
        {

        }
    }
}
