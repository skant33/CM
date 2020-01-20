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
        readonly AccountConverter accountConverter = new AccountConverter();

        public Appointment ViewModelToAppointment(AppointmentDetailViewModel ADVM)
        {
            Appointment appointment = new Appointment()
            {
                AppointmentID = ADVM.AppointmentID,
                DateTime = ADVM.DateTime,
                Duration = ADVM.Duration,
                Description = ADVM.Description,
                patient = accountConverter.ViewModelToAccount(ADVM.Patient),
                doctor = accountConverter.ViewModelToAccount(ADVM.Doctor)
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
                Patient = accountConverter.ViewModelFromAccount(appointment.patient),
                Doctor = accountConverter.ViewModelFromAccount(appointment.doctor)
            };

            return ADVM;
        }
    }
}
