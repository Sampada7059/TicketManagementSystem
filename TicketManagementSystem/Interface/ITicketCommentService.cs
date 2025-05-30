using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagementSystem.DTOs;

namespace TicketManagementSystem.Interface
{
    public interface ITicketCommentService
    {
        Task AddCommentAsync(int ticketId, int userId, CreateTicketCommentDto dto);
        Task<IEnumerable<TicketCommentDto>> GetCommentsAsync(int ticketId, int userId, string role);
    }
}
