using Kolokwium.Models.DTOs;
using Microsoft.Data.SqlClient;

namespace Kolokwium.Services;

public class DbService : IDbService
{
    private readonly string _connectionString;

    public DbService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default") ?? string.Empty;
    }

    public async Task<AppointmentDTO> GetAppointment(int appointmentId)
    {
        await using SqlConnection connection = new SqlConnection(_connectionString);
        await using SqlCommand command = new SqlCommand();
        
        command.Connection = connection;
        await connection.OpenAsync();
        
        command.Parameters.Clear();
        command.CommandText = "SELECT 1 FROM Appointment WHERE appoitment_id = @appointmentId;";
        command.Parameters.AddWithValue("@appointmentId", appointmentId);
        
        var appointmentIdResult = await command.ExecuteScalarAsync();
        if (appointmentIdResult is null)
        {
            throw new Exception("Appointment not found");
        }

        command.Parameters.Clear();
        command.CommandText =
            @"SELECT Appointment.date, Patient.first_name, Patient.last_name, Patient.date_of_birth, Doctor.doctor_id, Doctor.PWZ,
                    Service.name, Service.base_fee
                FROM Appointment
                LEFT JOIN Patient ON Patient.patient_id = Appointment.patient_id
                LEFT JOIN Doctor ON Doctor.doctor_id = Appointment.doctor_id
                LEFT JOIN Appointment_Service ON Appointment_Service.appoitment_id = Appointment.appoitment_id
                LEFT JOIN Service ON Service.service_id = Appointment_Service.service_id
                WHERE Appointment.appoitment_id = @appointmentId";
        
        command.Parameters.AddWithValue("@appointmentId", appointmentId);
        
        var reader = await command.ExecuteReaderAsync();
        
        AppointmentDTO? appointment = null;
        
        while (await reader.ReadAsync())
        {
            if (appointment is null)
            {
                appointment = new AppointmentDTO
                {
                    Date = reader.GetDateTime(0),
                    Patient = new PatientDTO()
                    {
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        DateOfBirth = reader.GetDateTime(3)
                    },
                    Doctor = new DoctorDTO()
                    {
                        DoctorId = reader.GetInt32(4),
                        Pwz = reader.GetString(5)
                    },
                    AppointmentServices = new List<ServiceDTO>()
                };
            }
            
            string serviceName = reader.GetString(6);
            var service = appointment.AppointmentServices.FirstOrDefault(e => e.Name.Equals(serviceName));
            if (service is null)
            {
                service = new ServiceDTO()
                {
                    Name = serviceName,
                    ServiceFee = reader.GetInt32(7)
                };
                appointment.AppointmentServices.Add(service);
            }
        }
        
        return appointment;
    }
}