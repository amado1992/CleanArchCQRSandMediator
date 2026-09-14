using CleanArchCQRSandMediator.Application.Dtos.Users;
using CleanArchCQRSandMediator.Application.Users.Queries.GetUserProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchCQRSandMediator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ApiControllerBase
    {
        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<UserProfileResponse>> GetUserProfile()
        {
            var profile = await Mediator.Send(new GetUserProfileQuery());
            return Ok(profile);
        }
    }
}
