using CleanArchCQRSandMediator.Application.Blogs.Commands.CreateBlog;
using CleanArchCQRSandMediator.Application.Blogs.Commands.UpdateBlog;
using CleanArchCQRSandMediator.Application.Blogs.Queries.GetBlogById;
using CleanArchCQRSandMediator.Application.Blogs.Queries.GetBlogs;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchCQRSandMediator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var blogs = await Mediator.Send(new GetBlogQuery());
            return Ok(blogs);
        }

        [HttpGet("{id}", Name = "GetBlogById")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var blog = await Mediator.Send(new GetBlogByIdQuery() { BlogId = id });

            if (blog == null)
                return NotFound();

            return Ok(blog);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateBlogCommand command)
        {
            var createBlog = await Mediator.Send(command);

            // return CreatedAtAction(nameof(GetByIdAsync), new { id = createBlog.Id }, createBlog);
            return CreatedAtRoute("GetBlogById", new { id = createBlog.Id }, createBlog);
        }

        /*[HttpPut("id")]
        public async Task<IActionResult> UpdateAsync(int id, UpdateBlogCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }*/

        [HttpPut]
        public async Task<ActionResult<int>> UpdateAsync(UpdateBlogCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
