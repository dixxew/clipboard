using Microsoft.AspNetCore.Mvc;

namespace clipboard_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DataController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok("Привет, жопа, Новый год 🎄");
}