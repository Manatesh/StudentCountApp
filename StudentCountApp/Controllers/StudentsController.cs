using Microsoft.AspNetCore.Mvc;
using StudentCountApp.Models;
using StudentCountApp.Services;

namespace StudentCountApp.Controllers;

// REST API
// GET /api/students/count?year=2025&divnCode=0&stream=1&frRptNoc=0&sexCd=2
[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _service;

    public StudentsController(IStudentService service)
    {
        _service = service;
    }

    [HttpGet("count")]
    public async Task<ActionResult<StudentCountResponse>> GetCount([FromQuery] StudentFilter filter)
    {
        var count = await _service.GetStudentCountAsync(filter);
        // get a friendly message (calls service; this may perform another DB call)
        var message = await _service.GetStudentCountMessageAsync(filter);
        return Ok(new StudentCountResponse { StudentCount = count, Message = message });
    }
}
