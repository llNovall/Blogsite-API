using BlogsiteDomain.Entities.AppContent;
using BlogsiteDomain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogsiteAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _repo;
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(IProjectRepository repo, ILogger<ProjectController> logger)
        {
            _repo = repo ?? throw new NullReferenceException(nameof(repo));
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Project>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            if (_repo == null)
            {
                _logger.LogCritical("Failed to find repo.");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(await _repo.GetAllEntitiesAsync());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Project))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Project>> Get(string id)
        {
            if (_repo == null)
            {
                _logger.LogCritical("Failed to find repo.");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            Project? Project = await _repo.GetEntityAsync(id);

            if (Project == null)
                return NotFound();

            return Ok(Project);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(Project project)
        {
            if (_repo == null)
            {
                _logger.LogCritical("Failed to find repo.");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            bool result = await _repo.AddEntityAsync(project);

            if (!result)
            {
                _logger.LogError("Failed to add Project.");
                return BadRequest();
            }

            return CreatedAtAction("Get", project);
        }

        [HttpPut]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(Project project)
        {
            if (_repo == null)
            {
                _logger.LogCritical("Failed to find repo.");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            bool result = await _repo.UpdateEntityAsync(project);

            if (!result)
            {
                _logger.LogError("Failed to update project.");
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
                _logger.LogError("Failed to delete project.");
                return BadRequest();
            }

            return Ok();
        }
    }
}