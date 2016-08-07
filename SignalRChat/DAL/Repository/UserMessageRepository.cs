using System;
using System.Linq;
using SignalRChat.DAL.Models;

namespace SignalRChat.DAL.Repository
{
    public interface IUserMessageRepository
    {
        bool SendMessageToDb(string name, string message);
        bool AddUserToDbIfNotAlreadyExisting(string name);
    }
    public class UserMessageRepository : IUserMessageRepository
    {
        public bool SendMessageToDb(string name, string message)
        {
            using (var userMessageContext = new UserMessageContext())
            {
                try
                {
                    var userId = userMessageContext.Users.Where(x => x.Name == name).Select(x => x.Id).FirstOrDefault();

                    userMessageContext.Messages.Add(new Message
                    {
                        Content = message,
                        UserId = userId
                    });

                    userMessageContext.SaveChanges();
                    return true;
                }

                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
            }
        }

        public bool AddUserToDbIfNotAlreadyExisting(string name)
        {
            using (var userMessageContext = new UserMessageContext())
            {
                try
                {
                    if (!userMessageContext.Users.Any(x => x.Name == name))
                        userMessageContext.Users.Add(new User { Name = name });

                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
            }
        }
    }
}