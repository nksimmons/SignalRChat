using System.Collections.Generic;

namespace SignalRChat.DAL.Models
{
    public sealed class User
    {
        public User()
        {
            Messages = new HashSet<Message>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}