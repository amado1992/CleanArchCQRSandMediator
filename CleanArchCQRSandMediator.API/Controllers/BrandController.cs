using CleanArchCQRSandMediator.Application.Authorization;
using CleanArchCQRSandMediator.Application.Brands.Commands.CreateBrand;
using CleanArchCQRSandMediator.Application.Brands.Commands.DeleteBrand;
using CleanArchCQRSandMediator.Application.Brands.Commands.UpdateBrand;
using CleanArchCQRSandMediator.Application.Brands.Queries.GetBrandById;
using CleanArchCQRSandMediator.Application.Dtos.Brands;
using CleanArchCQRSandMediator.Application.Organizations.Commands.DeleteOrganization;
using CleanArchCQRSandMediator.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace CleanArchCQRSandMediator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ApiControllerBase
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        public BrandController(IStringLocalizer<SharedResources> localizer) 
        { 
            _localizer = localizer;
        }

        [HttpPost]
        // [PermissionAuthorize(PermissionAction.Create, "Brand")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateBrandCommand command)
        {
            var id = await Mediator.Send(command);
            return CreatedAtRoute("GetBrandById", new { id }, new { id });
        }

        /// <summary>
        /// Retrieves an brand by its ID.
        /// </summary>
        /// <param name="id">Brand ID</param>
        [HttpGet("{id}", Name = "GetBrandById")]
        // [PermissionAuthorize(PermissionAction.View, "Brand")]
        public async Task<ActionResult<BrandDto>> GetByIdAsync([FromRoute] int id)
        {
            var brand = await Mediator.Send(new GetBrandByIdQuery { BrandId = id });
            return Ok(brand);
        }

        /// <summary>
        /// Update an existing brand
        /// </summary>
        /// <param name="id">Brand ID</param>
        /// <param name="command">Data to be updated</param>
        [HttpPut("{id}")]
        // [PermissionAuthorize(PermissionAction.Update, "Brand")]
        public async Task<IActionResult> UpdateAsync(
            [FromRoute] int id,
            [FromBody] UpdateBrandCommand command)
        {
            // Ensure that the route ID matches the one in the body
            if (id != command.Id)
                return BadRequest(_localizer["IdMismatch"].Value);

            await Mediator.Send(command);
            return Ok(new { id });
        }

        /// <summary>
        /// Deletes an brand by its ID.
        /// </summary>
        /// <param name="id">Brand ID</param>
        [HttpDelete("{id}")]
        // [PermissionAuthorize(PermissionAction.Delete, "Brand")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            await Mediator.Send(new DeleteBrandCommand { BrandId = id });

            return Ok(new { id });
        }
    }
}
