using CM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CM.Context.Interfaces
{
    public interface IAppointmentContext
    {
        List<Appointment> GetAllAppointments();

        List<Appointment> GetAppointmentsByUserId(Account account);
        Appointment GetAppointmentByID(int id);
        List<Appointment> AppointmentsCurrentWeek(int id);
    }
}
