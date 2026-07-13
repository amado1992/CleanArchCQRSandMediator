using CleanArchCQRSandMediator.Application.Permissions.Commands.AssignPermissionToUser;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchCQRSandMediator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ApiControllerBase
    {
        [HttpPost("assign")]
        public async Task<IActionResult> AssignPermissions(AssignPermissionToUserCommand command)
        {
            await Mediator.Send(command);
            return Ok(new { message = $"Permissions assigned to the user {command.UserId}." });
        }
    }
}
