using Microsoft.AspNetCore.Mvc;

namespace TodoApp.Controllers;

[ApiController]
[Route("/api")]
public class HealthController : ControllerBase
{
    [HttpGet]
    [Route("health")]
    public ActionResult Get()
    {
        return Ok();
    }
}
