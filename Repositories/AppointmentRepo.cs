using CM.Context.Interfaces;
using CM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CM.Repositories
{
    public class AppointmentRepo
    {
        private IAppointmentContext context;

        public AppointmentRepo(IAppointmentContext context)
        {
            this.context = context;
        }
        public List<Appointment> GetAllAppointments()
        {
            return context.GetAllAppointments();
        }
        public List<Appointment> GetAppointmentsByUserID(Account account)
        {
            return context.GetAppointmentsByUserId(account);
        }

        public bool MakeAppointment(Appointment appointment)
        {
            return context.MakeAppointment(appointment);
        }
    }
}
