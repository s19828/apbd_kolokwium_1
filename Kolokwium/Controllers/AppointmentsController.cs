using Kolokwium.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kolokwium.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppointmentsController : ControllerBase
{
    private readonly IDbService _dbService;

    public AppointmentsController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAppointment(int id)
    {
        try
        {
            var appointment = await _dbService.GetAppointment(id);
            return Ok(appointment);
        }
        catch (Exception e)
        {
            return NotFound(e);
        }
    }
}