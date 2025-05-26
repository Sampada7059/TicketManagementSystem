using System;
using System.Collections.Generic;

namespace TicketManagementSystem.Models;

public partial class Ticket
{
    public int TicketId { get; set; }

    public int UserId { get; set; }

    public string Subject { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Priority { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<TicketComment> TicketComments { get; set; } = new List<TicketComment>();

    public virtual User User { get; set; } = null!;
}
