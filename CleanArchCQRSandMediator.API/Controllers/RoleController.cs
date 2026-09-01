using CleanArchCQRSandMediator.Application.Roles.Queries.GetUserRoles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchCQRSandMediator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ApiControllerBase
    {
        /// <summary>
        /// Retrieves the roles of a specific user
        /// </summary>
        [HttpGet("user-roles")]
        [Authorize]
        public async Task<ActionResult<IList<string>>> GetUserRoles()
        {
            var roles = await Mediator.Send(new GetUserRolesQuery());
            return Ok(roles);
        }
    }
}
