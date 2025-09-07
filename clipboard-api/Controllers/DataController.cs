using Microsoft.AspNetCore.Mvc;

namespace clipboard_api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DataController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok("Привет, жопа, Новый год 🎄");

    [HttpGet]
    public IActionResult UploadPasswords()
    {
        return Ok(1 + 1);
    }
}