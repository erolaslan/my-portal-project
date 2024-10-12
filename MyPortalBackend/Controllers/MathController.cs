using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize] // JWT koruması ekliyoruz
public class MathController : ControllerBase
{
    [HttpGet("multiply")]
    public IActionResult Multiply([FromQuery] int a, [FromQuery] int b)
    {
        int result = a * b;
        return Ok(new { Result = result });
    }
}
