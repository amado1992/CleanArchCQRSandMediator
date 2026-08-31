using CleanArchCQRSandMediator.Application.Dtos.Permissions;
using CleanArchCQRSandMediator.Application.Permissions.Commands.AssignPermissionsToRole;
using CleanArchCQRSandMediator.Application.Permissions.Commands.AssignPermissionToUser;
using CleanArchCQRSandMediator.Application.Permissions.Queries.GetUserUnifiedPermissionsQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace CleanArchCQRSandMediator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ApiControllerBase
    {
        private readonly IStringLocalizer<PermissionController> _localizer;
        public PermissionController(IStringLocalizer<PermissionController> localizer) 
        {
            _localizer = localizer;
        }

        /// <summary>
        /// Assign a list of permissions to a specific user
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("user-permissions")]
        public async Task<IActionResult> AssignPermissionsToUser([FromBody] AssignPermissionsToUserCommand command)
        {
            var message = _localizer.GetString("AssignPermissionsToUser", command.UserId).Value;
            await Mediator.Send(command);
            return Ok(new { message = message });
        }

        /// <summary>
        /// List of user permissions
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("user-permissions")]
        public async Task<ActionResult<PermissionsResponse>> GetUserUnifiedPermissions()
        {
            var permissions = await Mediator.Send(new GetUserUnifiedPermissionsQuery());
            return Ok(permissions);
        }

        /// <summary>
        /// Assign a list of permissions to a specific role
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("role-permissions")]
        public async Task<IActionResult> AssignPermissionsToRole([FromBody] AssignPermissionsToRoleCommand command)
        {
            var message = _localizer.GetString("AssignPermissionsToRole", command.RoleId).Value;
            await  Mediator.Send(command);
            return Ok(new { message = message });
        }

    }
}
