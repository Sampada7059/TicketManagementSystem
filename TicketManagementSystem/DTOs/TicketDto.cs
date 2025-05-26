namespace TicketManagementSystem.DTOs
{
    public class TicketDTO
    {
        public int Id { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Priority { get; set; } 
        public string Status { get; set; } 
        public DateTime CreatedAt { get; set; }

        public string UserName { get; set; } = string.Empty;
    }
}
