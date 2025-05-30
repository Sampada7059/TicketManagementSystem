using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TicketManagementSystem.DTOs;
using TicketManagementSystem.Interface;

namespace TicketManagementSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TicketCommentsController : ControllerBase
    {
        private readonly ITicketCommentService _commentService;

        public TicketCommentsController(ITicketCommentService commentService)
        {
            _commentService = commentService;
        }

        // POST: api/TicketComments/{ticketId}
        [HttpPost("{ticketId}")]
        public async Task<IActionResult> AddComment(int ticketId, [FromBody] CreateTicketCommentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                await _commentService.AddCommentAsync(ticketId, userId, dto);
                return Ok(new { message = "Comment added successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while adding the comment", details = ex.Message });
            }
        }

        // GET: api/TicketComments/{ticketId}
        [HttpGet("{ticketId}")]
        public async Task<IActionResult> GetComments(int ticketId)
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                string role = User.FindFirstValue(ClaimTypes.Role);

                var comments = await _commentService.GetCommentsAsync(ticketId, userId, role);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while retrieving comments", details = ex.Message });
            }
        }
    }
}