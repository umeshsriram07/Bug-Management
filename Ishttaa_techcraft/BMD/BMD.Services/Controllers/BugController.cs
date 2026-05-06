using BMD.Business.Services.Interface;
using BMD.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace BMD.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugController : ControllerBase
    {
        private readonly IBugService _bugService;

        private readonly ILogger<BugController> _logger;

        public BugController(
            IBugService bugService,
            ILogger<BugController> logger)
        {
            _bugService = bugService;

            _logger = logger;
        }

        // GET ALL BUGS
        [HttpGet]
        public async Task<IActionResult> GetAllBugs()
        {
            try
            {
                _logger.LogInformation(
                    "Fetching all bugs.");

                var bugs =
                    await _bugService.GetAllBugsAsync();

                return Ok(new
                {
                    success = true,
                    message = "Bugs retrieved successfully.",
                    data = bugs
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while fetching bugs.");

                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error."
                });
            }
        }

        // GET BUG BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBugById(int id)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching bug with Id: {Id}",
                    id);

                var bug =
                    await _bugService.GetBugByIdAsync(id);

                if (bug == null)
                {
                    _logger.LogWarning(
                        "Bug not found with Id: {Id}",
                        id);

                    return NotFound(new
                    {
                        success = false,
                        message = "Bug not found."
                    });
                }

                return Ok(new
                {
                    success = true,
                    data = bug
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while fetching bug with Id: {Id}",
                    id);

                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error."
                });
            }
        }

        // GET BUGS BY STATUS
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetBugsByStatus(string status)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching bugs by status: {Status}",
                    status);

                var bugs =
                    await _bugService.GetBugsByStatusAsync(status);

                return Ok(new
                {
                    success = true,
                    data = bugs
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while fetching bugs by status.");

                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error."
                });
            }
        }

        // CREATE BUG
        [HttpPost]
        public async Task<IActionResult> CreateBug([FromBody] Bug bug)
        {
            try
            {
                _logger.LogInformation(
                    "Creating new bug: {Title}",
                    bug.Title);

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning(
                        "Invalid model state while creating bug.");

                    return BadRequest(ModelState);
                }

                var result =
                    await _bugService.CreateBugAsync(bug);

                if (!result)
                {
                    _logger.LogWarning(
                        "Failed to create bug.");

                    return BadRequest(new
                    {
                        success = false,
                        message = "Failed to create bug."
                    });
                }

                _logger.LogInformation(
                    "Bug created successfully.");

                return Ok(new
                {
                    success = true,
                    message = "Bug created successfully."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while creating bug.");

                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error."
                });
            }
        }

        // UPDATE BUG
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBug(
            int id,
            [FromBody] Bug bug)
        {
            try
            {
                _logger.LogInformation(
                    "Updating bug with Id: {Id}",
                    id);

                var result =
                    await _bugService.UpdateBugAsync(id, bug);

                if (!result)
                {
                    _logger.LogWarning(
                        "Bug not found for update. Id: {Id}",
                        id);

                    return NotFound(new
                    {
                        success = false,
                        message = "Bug not found."
                    });
                }

                _logger.LogInformation(
                    "Bug updated successfully. Id: {Id}",
                    id);

                return Ok(new
                {
                    success = true,
                    message = "Bug updated successfully."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while updating bug with Id: {Id}",
                    id);

                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error."
                });
            }
        }

        // DELETE BUG
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBug(int id)
        {
            try
            {
                _logger.LogWarning(
                    "Deleting bug with Id: {Id}",
                    id);

                var result =
                    await _bugService.DeleteBugAsync(id);

                if (!result)
                {
                    _logger.LogWarning(
                        "Bug not found for delete. Id: {Id}",
                        id);

                    return NotFound(new
                    {
                        success = false,
                        message = "Bug not found."
                    });
                }

                _logger.LogInformation(
                    "Bug deleted successfully. Id: {Id}",
                    id);

                return Ok(new
                {
                    success = true,
                    message = "Bug deleted successfully."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while deleting bug with Id: {Id}",
                    id);

                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error."
                });
            }
        }
    }
}