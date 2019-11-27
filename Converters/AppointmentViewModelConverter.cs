using CM.Models;
using CM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CM.Converters
{
    public class AppointmentViewModelConverter
    {
        public Appointment ViewModelToAccount(AppointmentDetailViewModel viewmodel)
        {
            Appointment appointment = new Appointment()
            {
                AppointmentID = viewmodel.AppointmentID,
                LinkID = viewmodel.LinkID,
                Duration = viewmodel.Duration,
                Date = viewmodel.Date,
                Coords = viewmodel.Coords,
                Description = viewmodel.Description,
            };
            return appointment;
        }
        public AppointmentDetailViewModel AccountToViewModel(Appointment appointment)
        {
            AppointmentDetailViewModel viewmodel = new AppointmentDetailViewModel()
            {
                AppointmentID = appointment.AppointmentID,
                LinkID = appointment.LinkID,
                Duration = appointment.Duration,
                Date = appointment.Date,
                Coords = appointment.Coords,
                Description = appointment.Description,
            };
            return viewmodel;
        }
    }
}
