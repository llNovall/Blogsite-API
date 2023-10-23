using BlogsiteAPI.DataTransferObjects;
using BlogsiteDomain.Entities.AppContent;
using BlogsiteDomain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogsiteAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogRepository _repo;
        private readonly ILogger<BlogController> _logger;

        public BlogController(IBlogRepository repo, ILogger<BlogController> logger)
        {
            _repo = repo ?? throw new NullReferenceException(nameof(repo));
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Blog>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Getting all blogs.");

            if (_repo == null)
            {
                _logger.LogCritical("Failed to find repo.");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            IEnumerable<Blog> blogs = await _repo.GetAllEntitiesAsync();
            _logger.LogInformation("Found blogs : {count}", blogs.Count());
            return Ok(blogs);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Blog))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Blog>> Get(string id)
        {
            if (_repo == null)
            {
                _logger.LogCritical("Failed to find repo.");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            Blog? Blog = await _repo.GetEntityAsync(id);

            if (Blog == null)
            {
                _logger.LogInformation("Failed to find blog : {id}", id);
                return NotFound();
            }

            return Ok(Blog);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Blog>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IList<Blog>>> FindBlogsByTagsAndYears([FromBody] BlogSearchWithTagsAndYearsDTO search)
        {
            if (_repo == null)
            {
                _logger.LogCritical("Failed to find repo.");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            IEnumerable<Blog> blogs = await _repo.FindBlogsByTagsAndYearsAsync(search.TagIds, search.Years);

            return Ok(blogs);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(Blog Blog)
        {
            if (_repo == null)
            {
                _logger.LogCritical("Failed to find repo.");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            bool result = await _repo.AddEntityAsync(Blog);

            if (!result)
            {
                _logger.LogError("Failed to add Blog.");
                return BadRequest();
            }

            return CreatedAtAction("Get", Blog);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddViewToBlog([FromBody] BlogAddViewDTO addViewDTO)
        {
            if (_repo == null)
            {
                _logger.LogCritical("Failed to find repo.");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            if (addViewDTO.Id == null)
                return BadRequest();

            bool result = await _repo.AddViewOfBlogByAsync(addViewDTO.Id, addViewDTO.ViewsToAdd);

            if (!result)
            {
                _logger.LogError("Failed to add Blog.");
                return BadRequest();
            }

            return Ok();
        }

        [HttpPut]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(Blog Blog)
        {
            if (_repo == null)
            {
                _logger.LogCritical("Failed to find repo.");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            bool result = await _repo.UpdateEntityAsync(Blog);

            if (!result)
            {
                _logger.LogError("Failed to update Blog.");
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
                _logger.LogError("Failed to delete Blog.");
                return BadRequest();
            }

            return Ok();
        }
    }
}