using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagementSystem.Data;
using TicketManagementSystem.DTOs;
using TicketManagementSystem.Interface;
using TicketManagementSystem.Models;

namespace TicketManagementSystem.Services
{
    public class TicketCommentService : ITicketCommentService
    {
        private readonly TicketManagementDbContext _context;

        public TicketCommentService(TicketManagementDbContext context)
        {
            _context = context;
        }

        public async Task AddCommentAsync(int ticketId, int userId, CreateTicketCommentDto dto)
        {
            var comment = new TicketComment
            {
                TicketId = ticketId,
                UserId = userId,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            _context.TicketComments.Add(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TicketCommentDto>> GetCommentsAsync(int ticketId, int userId, string role)
        {
            var comments = await _context.TicketComments
                .Where(c => c.TicketId == ticketId)
                .OrderBy(c => c.CreatedAt)
                .Select(c => new TicketCommentDto
                {
                    Id = c.Id,
                    TicketId = c.TicketId,
                    UserId = c.UserId,
                    Comment = c.Comment,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync();

            return comments;
        }
    }
}
