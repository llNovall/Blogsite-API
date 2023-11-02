using BlogsiteAPI.DataTransferObjects;
using BlogsiteDomain.Entities.AppContent;
using BlogsiteDomain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogsiteAPI.Controllers.AppContent
{
    [Route("[controller]")]
    [ApiController]
    public class BlogCommentController : ControllerBase
    {
        private readonly IBlogCommentRepository _repo;
        private readonly ILogger<BlogCommentController> _logger;

        public BlogCommentController(IBlogCommentRepository repo, ILogger<BlogCommentController> logger)
        {
            _repo = repo ?? throw new NullReferenceException(nameof(repo));
            _logger = logger;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BlogComment>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BlogTag>> Get([FromQuery]string blogId, string? parentCommentId = null)
        {
            if (_repo == null)
            {
                _logger.LogCritical("Failed to find repo.");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            if (blogId == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            IEnumerable<BlogComment>? blogComment = await _repo.GetBlogComments(blogId, parentCommentId);

            return Ok(blogComment);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(BlogComment blogComment)
        {
            if (_repo == null)
            {
                _logger.LogCritical("Failed to find repo.");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            bool result = await _repo.AddEntityAsync(blogComment);

            if (!result)
            {
                _logger.LogError("Failed to add blogComment.");
                return BadRequest();
            }

            return CreatedAtAction("Get", blogComment);
        }
    }
}