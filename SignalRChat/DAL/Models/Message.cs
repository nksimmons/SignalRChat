namespace SignalRChat.DAL.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}