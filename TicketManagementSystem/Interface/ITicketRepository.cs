using System.Net.Sockets;
using TicketManagementSystem.Models;

namespace TicketManagementSystem.Interface
{
    public interface ITicketRepository
    {
        Task AddAsync(Ticket ticket);
        Task<Ticket> GetByIdAsync(int id);
        Task<IEnumerable<Ticket>> GetAllAsync();
        Task<List<Ticket>> GetByUserIdAsync(int userId);
        Task UpdateAsync(Ticket ticket);
        Task DeleteAsync(Ticket ticket);
        Task SaveAsync();

    }

}
