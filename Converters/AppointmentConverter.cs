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
                Date = ADVM.Date,
                Duration = ADVM.Duration,
                //DoctorID = AccountRepo.GetAccountByEmail(ADVM.DoctorEmail).AccountID,
                DoctorEmail = ADVM.DoctorEmail,
            };
            return appointment;
        }

        public AppointmentDetailViewModel ViewModelFromAppointment(Appointment appointment)
        {
            AppointmentDetailViewModel ADVM = new AppointmentDetailViewModel()
            {
                AppointmentID = appointment.AppointmentID,
                Date = appointment.Date,
                Duration = appointment.Duration,
                //Doctor = AccountRepo.GetAccountByID(appointment.DoctorID).Name,
                DoctorEmail = appointment.DoctorEmail
            };
            return ADVM;
        }
    }
}
