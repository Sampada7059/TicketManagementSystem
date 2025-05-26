using System.Net.Sockets;
using TicketManagementSystem.DTOs;
using TicketManagementSystem.Interface;
using TicketManagementSystem.Models;


namespace TicketManagementSystem.Services
{
    // Services/TicketService.cs
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task CreateTicketAsync(int userId, CreateTicketDto dto)
        {
            var ticket = new Ticket
            {
                UserId = userId,
                Subject = dto.Subject,
                Description = dto.Description,
                Priority = dto.Priority,
                Status = "Open",
                CreatedAt = DateTime.UtcNow
            };

            await _ticketRepository.AddAsync(ticket);
            await _ticketRepository.SaveAsync();
        }

        public async Task<IEnumerable<TicketDTO>> GetTicketsAsync(string role, int userId)
        {
            IEnumerable<Ticket> tickets;

            if (role == "Admin")
                tickets = await _ticketRepository.GetAllAsync();
            else
                tickets = await _ticketRepository.GetByUserIdAsync(userId);

            return tickets.Select(t => new TicketDTO
            {
                Id = t.TicketId,
                Subject = t.Subject,
                Description = t.Description,
                Priority = t.Priority,
                Status = t.Status,
                CreatedAt = t.CreatedAt,
                UserName = t.User?.Name ?? "Unknown"
            });
        }

        public async Task UpdateTicketAsync(int ticketId, int userId, UpdateTicketDto dto)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);

            if (ticket == null)
                throw new Exception("Ticket not found.");

            if (ticket.UserId != userId)
                throw new UnauthorizedAccessException("You are not allowed to edit this ticket.");

            if (ticket.Status == "Resolved" || ticket.Status == "Closed")
                throw new InvalidOperationException("Cannot edit a resolved or closed ticket.");

            ticket.Subject = dto.Subject;
            ticket.Description = dto.Description;
            ticket.Priority = dto.Priority;

            await _ticketRepository.UpdateAsync(ticket);
            await _ticketRepository.SaveAsync();
        }

        public async Task UpdateStatusAsync(int ticketId, string status)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            if (ticket == null) throw new InvalidOperationException("Ticket not found.");

            ticket.Status = status;
            await _ticketRepository.UpdateAsync(ticket);
            await _ticketRepository.SaveAsync();
        }

        public async Task DeleteTicketAsync(int ticketId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            if (ticket == null) throw new InvalidOperationException("Ticket not found.");

            await _ticketRepository.DeleteAsync(ticket);
            await _ticketRepository.SaveAsync();
        }
    }

}
