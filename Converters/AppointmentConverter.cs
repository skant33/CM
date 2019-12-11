using CM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CM.Repositories;
using CM.Context.SQL;
using CM.ViewModels;

namespace CM.Converters
{
    public class AppointmentConverter
    {
        AppointmentRepo AppointmentRepo = new AppointmentRepo(new AppointmentMsSqlContext());
        AccountRepo AccountRepo = new AccountRepo(new AccountMsSqlContext());

        public Appointment ViewModelToAppointment(AppointmentDetailViewModel ADVM)
        {
            Appointment appointment = new Appointment()
            {
                AppointmentID = ADVM.AppointmentID,
                DateTime = ADVM.DateTime,
                Duration = ADVM.Duration,
                Description = ADVM.Description,
                patient = ADVM.Patient,
                doctor = ADVM.Doctor
            };
            return appointment;
        }

        public AppointmentDetailViewModel ViewModelFromAppointment(Appointment appointment)
        {
            AppointmentDetailViewModel ADVM = new AppointmentDetailViewModel()
            {
                AppointmentID = appointment.AppointmentID,
                DateTime = appointment.DateTime,
                Duration = appointment.Duration,
                Description = appointment.Description,
                Patient = appointment.patient,
                Doctor = appointment.doctor
            };
            return ADVM;
        }
    }
}
