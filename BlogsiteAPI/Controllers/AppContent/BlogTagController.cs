using BlogsiteDomain.Entities.AppContent;
using BlogsiteDomain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogsiteAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BlogTagController : ControllerBase
    {
        private readonly IBlogTagRepository _repo;
        private readonly ILogger<BlogTagController> _logger;

        public BlogTagController(IBlogTagRepository repo, ILogger<BlogTagController> logger)
        {
            _repo = repo ?? throw new NullReferenceException(nameof(repo));
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BlogTag>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            if (_repo == null)
            {
                _logger.LogCritical("Failed to find repo.");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            IEnumerable<BlogTag> tags = await _repo.GetAllEntitiesAsync();

            return Ok(tags);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BlogTag))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BlogTag>> Get(string id)
        {
            if (_repo == null)
            {
                _logger.LogCritical("Failed to find repo.");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            BlogTag? tag = await _repo.GetEntityAsync(id);

            if (tag == null)
                return NotFound();

            return Ok(tag);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(BlogTag tag)
        {
            if (_repo == null)
            {
                _logger.LogCritical("Failed to find repo.");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            bool result = await _repo.AddEntityAsync(tag);

            if (!result)
            {
                _logger.LogError("Failed to add tag.");
                return BadRequest();
            }

            return CreatedAtAction("Get", tag);
        }

        [HttpPut]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(BlogTag tag)
        {
            if (_repo == null)
            {
                _logger.LogCritical("Failed to find repo.");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            bool result = await _repo.UpdateEntityAsync(tag);

            if (!result)
            {
                _logger.LogError("Failed to update tag.");
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(string id)
        {
            if (_repo == null)
            {
                _logger.LogCritical("Failed to find repo.");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            bool result = await _repo.DeleteEntityAsync(id);

            if (!result)
            {
                _logger.LogError("Failed to delete tag.");
                return BadRequest();
            }

            return Ok();
        }
    }
}