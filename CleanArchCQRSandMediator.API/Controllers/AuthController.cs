using CleanArchCQRSandMediator.Application.Auth.Commands.Login;
using CleanArchCQRSandMediator.Application.Auth.Commands.RefreshToken;
using CleanArchCQRSandMediator.Application.Auth.Commands.Register;
using CleanArchCQRSandMediator.Application.Dtos.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CleanArchCQRSandMediator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("register")]
        // [Authorize(Roles = "Super administrador")]
        [AllowAnonymous]
        public async Task<ActionResult<int>> Register(RegisterCommand command)
        {
            var userId = await Mediator.Send(command);
            return Ok(userId);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<LoginResponse>> Refresh(RefreshTokenCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("logout")]
        [Authorize]
        public Task<IActionResult> Logout([FromBody] string refreshToken)
        {
            // Revocar refresh token
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // Implementar lógica de revocación en repositorio
            return Task.FromResult<IActionResult>(Ok());
        }
    }
}
