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
                PatientID = viewmodel.PatientID,
                DoctorID = viewmodel.DoctorID,
                Duration = viewmodel.Duration,
                Date = viewmodel.Date,
                Coords = viewmodel.Coords,
                Done = viewmodel.Done,
            };
            return appointment;
        }
        public AppointmentDetailViewModel AccountToViewModel(Appointment appointment)
        {
            AppointmentDetailViewModel viewmodel = new AppointmentDetailViewModel()
            {
                AppointmentID = appointment.AppointmentID,
                PatientID = appointment.PatientID,
                DoctorID = appointment.DoctorID,
                Duration = appointment.Duration,
                Date = appointment.Date,
                Coords = appointment.Coords,
                Done = appointment.Done,
            };
            return viewmodel;
        }
    }
}
