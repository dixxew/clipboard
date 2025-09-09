using Microsoft.AspNetCore.Mvc;
using clipboard_api.Models;
using clipboard_api.Services;

namespace clipboard_api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DataController : ControllerBase
{
    private readonly PasswordService _passwordService;

    public DataController(PasswordService passwordService)
    {
        _passwordService = passwordService;
    }

    [HttpGet]
    public IActionResult GetPassword()
    {
        return Ok(_passwordService.GetAll());
    }

    [HttpPost]
    public IActionResult UploadPassword([FromBody] string value)
    {
        var item = _passwordService.Add(value);
        return Ok(item);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var result = _passwordService.Delete(id);
        if (!result)
            return NotFound($"Пароль с id {id} не найден");

        return Ok($"Пароль {id} удалён");
    }
}