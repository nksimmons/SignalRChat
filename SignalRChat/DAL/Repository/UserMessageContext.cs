using System.Data.Entity;
using SignalRChat.DAL.Models;

namespace SignalRChat.DAL.Repository
{
    public class UserMessageContext : DbContext
    {
        public UserMessageContext() : base("name=UserMessageContext")
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
    }
}