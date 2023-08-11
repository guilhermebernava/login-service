
using LoginMicroservice.Api.Dtos;
using LoginMicroservice.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LoginMicroservice.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{

    [HttpPost]
    [Route("CreateUser")]
    public async Task<IActionResult> CreateUserAsync([FromServices] ICreateUserServices createUserServices, [FromBody] UserDto dto)
    {
        var result = await createUserServices.Call(dto);
        return result ? Created("/User/Login", null) : BadRequest(); 
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> LoginAsync([FromServices] ILoginServices loginServices, [FromBody] LoginDto dto)
    {
        var result = await loginServices.Call(dto);
        return result.IsTwoFactorLogin ? Accepted(Url.Link("DefaultApi", null) + "/User/TwoFactorLogin") : Ok(result.Token);
    }

    [HttpPost]
    [Route("TwoFactorLogin")]
    public async Task<IActionResult> TwoFactorLoginAsync([FromServices] ITwoFactorLoginServices loginServices, [FromQuery] string code)
    {
        var result = await loginServices.Call(code);
        return Ok(result);
    }
}
