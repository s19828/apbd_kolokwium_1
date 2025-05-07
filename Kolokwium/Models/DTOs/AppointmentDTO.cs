using System.ComponentModel.DataAnnotations;

namespace Kolokwium.Models.DTOs;

public class AppointmentDTO
{
    public DateTime Date { get; set; }
    public PatientDTO Patient { get; set; }
    public DoctorDTO Doctor { get; set; }
    public List<ServiceDTO> AppointmentServices { get; set; }
}

public class PatientDTO
{
    [MaxLength(100)]
    public string FirstName { get; set; }
    [MaxLength(100)]
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
}

public class DoctorDTO
{
    [Required]
    public int DoctorId { get; set; }
    [MaxLength(7)]
    public string Pwz { get; set; }
}

public class ServiceDTO
{
    [MaxLength(100)]
    public string Name { get; set; }
    public double ServiceFee { get; set; }
}