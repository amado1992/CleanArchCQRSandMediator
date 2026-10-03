using CleanArchCQRSandMediator.Application.Auth.Commands.Login;
using CleanArchCQRSandMediator.Application.Auth.Commands.Logout;
using CleanArchCQRSandMediator.Application.Auth.Commands.RefreshToken;
using CleanArchCQRSandMediator.Application.Auth.Commands.Register;
using CleanArchCQRSandMediator.Application.Dtos.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace CleanArchCQRSandMediator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiControllerBase
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public AuthController(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("register")]
        // [Authorize(Roles = "Super administrador")]
        [AllowAnonymous]
        public async Task<ActionResult<int>> Register([FromBody] RegisterCommand command)
        {
            var userId = await Mediator.Send(command);
            return Ok(userId);
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Refresh([FromBody] RefreshTokenCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var message = _localizer["SessionSuccessfullyClosed"].Value;
            await Mediator.Send(new LogoutCommand());
            return Ok(new { message = message });
        }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
    }
}
