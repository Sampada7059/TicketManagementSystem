using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketManagementSystem.DTOs;
using TicketManagementSystem.Interface;

namespace TicketManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly ILogger<TicketController> _logger;

        public TicketController(ITicketService ticketService, ILogger<TicketController> logger)
        {
            _ticketService = ticketService;
            _logger = logger;
        }

        private int GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst("id") ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            return int.Parse(userIdClaim.Value);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
                await _ticketService.CreateTicketAsync(userId, dto);
                return Ok("Ticket created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating ticket");
                return StatusCode(500, "An error occurred while creating the ticket.");
            }
        }


        [HttpGet]
        //[Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetTickets()
        {
            try
            {
                var role = User.IsInRole("Admin") ? "Admin" : "User";
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

                var tickets = await _ticketService.GetTicketsAsync(role, userId);
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tickets");
                return StatusCode(500, "An error occurred while retrieving tickets.");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "User")]

        public async Task<IActionResult> UpdateTicket(int id, [FromBody] UpdateTicketDto dto)
        {
            try
            {
                int userId = GetUserIdFromToken(); 

                await _ticketService.UpdateTicketAsync(id, userId, dto);
                return Ok("Ticket updated successfully.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeStatus(int id, [FromBody] UpdateTicketStatusDto dto)
        {
            try
            {
                await _ticketService.UpdateStatusAsync(id, dto.Status);
                return Ok("Status updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating status");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            try
            {
                await _ticketService.DeleteTicketAsync(id);
                return Ok("Ticket deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting ticket");
                return StatusCode(500, ex.Message);
            }
        }
        

    }
}
