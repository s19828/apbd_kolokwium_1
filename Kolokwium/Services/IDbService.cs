using Kolokwium.Models.DTOs;

namespace Kolokwium.Services;

public interface IDbService
{
    Task<AppointmentDTO> GetAppointment(int appointmentId);
}