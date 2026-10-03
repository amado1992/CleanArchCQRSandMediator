using CleanArchCQRSandMediator.Application.Authorization;
using CleanArchCQRSandMediator.Application.Common.Models;
using CleanArchCQRSandMediator.Application.Dtos.Organizations;
using CleanArchCQRSandMediator.Application.Organizations.Commands.CreateOrganization;
using CleanArchCQRSandMediator.Application.Organizations.Commands.DeleteOrganization;
using CleanArchCQRSandMediator.Application.Organizations.Commands.UpdateOrganization;
using CleanArchCQRSandMediator.Application.Organizations.Queries.GetOrganizationById;
using CleanArchCQRSandMediator.Application.Organizations.Queries.GetOrganizations;
using CleanArchCQRSandMediator.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchCQRSandMediator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ApiControllerBase
    {
        [HttpPost]
        // [PermissionAuthorize(PermissionAction.Create, "Organizations")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateOrganizationCommand command)
        {
            var id = await Mediator.Send(command);
            return CreatedAtRoute("GetOrganizationById", new { id }, new { id });
        }

        /// <summary>
        /// Retrieves an organization by its ID.
        /// </summary>
        /// <param name="id">Organization ID</param>
        [HttpGet("{id}", Name = "GetOrganizationById")]
        // [PermissionAuthorize(PermissionAction.View, "Organizations")]
        public async Task<ActionResult<OrganizationDto>> GetByIdAsync([FromRoute] int id)
        {
            var organization = await Mediator.Send(new GetOrganizationByIdQuery { OrganizationId = id });
            return Ok(organization);
        }

        /// <summary>
        /// Update an existing organization
        /// </summary>
        /// <param name="id">Organization ID</param>
        /// <param name="command">Data to be updated</param>
        [HttpPut("{id}")]
        // [PermissionAuthorize(PermissionAction.Update, "Organizations")]
        public async Task<IActionResult> UpdateAsync(
            [FromRoute] int id,
            [FromBody] UpdateOrganizationCommand command)
        {
            // Ensure that the route ID matches the command ID.
            if (id != command.Id)
                return BadRequest("The route ID does not match the body ID.");

            await Mediator.Send(command);

            return Ok(new { id });
        }

        /// <summary>
        /// Deletes an organization by its ID.
        /// </summary>
        /// <param name="id">Organization ID</param>
        [HttpDelete("{id}")]
        // [PermissionAuthorize(PermissionAction.Delete, "Organizations")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            await Mediator.Send(new DeleteOrganizationCommand { OrganizationId = id });

            return Ok(new { id });
        }

        /// <summary>
        /// List organizations with filters, sorting, and pagination.
        /// </summary>
        /// <param name="filter">Filtering and pagination parameters</param>
        /// <returns>Paginated list of organizations</returns>
        /// <remarks>
        /// Examples:
        /// 
        ///     GET /api/organizations?page=1&amp;pageSize=10
        ///     GET /api/organizations?search=nina&amp;isActive=true
        ///     GET /api/organizations?sortBy=name&amp;sortOrder=desc
        ///     GET /api/organizations?slug=nina-nails-shop
        /// 
        /// </remarks>
        [HttpGet]
        // [PermissionAuthorize(PermissionAction.View, "Organizations")]
        public async Task<ActionResult<PagedResult<OrganizationDto>>> GetAllAsync(
            [FromQuery] OrganizationFilterDto filter)
        {
            var result = await Mediator.Send(new GetOrganizationsQuery { Filter = filter });
            return Ok(result);
        }
    }
}
