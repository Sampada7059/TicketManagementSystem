using TicketManagementSystem.DTOs;
using TicketManagementSystem.Models;

namespace TicketManagementSystem.Interface
{
    public interface ITicketService
    {
        Task CreateTicketAsync(int userId, CreateTicketDto dto);
        Task<IEnumerable<TicketDTO>> GetTicketsAsync(string role, int userId);
        Task UpdateTicketAsync(int ticketId, int userId, UpdateTicketDto dto);
        Task UpdateStatusAsync(int ticketId, string status);
        Task DeleteTicketAsync(int ticketId);
    }
}
