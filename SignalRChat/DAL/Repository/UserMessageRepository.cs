using System;
using System.Collections.Generic;
using System.Linq;
using SignalRChat.DAL.Models;

namespace SignalRChat.DAL.Repository
{
    public interface IUserMessageRepository
    {
        bool SendMessageToDb(string name, string message);
        bool AddUserToDbIfNotAlreadyExisting(string name);
        List<Message> GetRecentMessages();
        string GetUserNameFromUserId(int userId);
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
                    if (userMessageContext.Users.Any(x => x.Name == name)) return true;

                    userMessageContext.Users.Add(new User { Name = name });
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

        public List<Message> GetRecentMessages()
        {
            try
            {
                using (var userMessageContext = new UserMessageContext())
                {
                    var messages = userMessageContext.Messages.Take(15).OrderByDescending(x => x.Id).ToList();
                    return messages;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public string GetUserNameFromUserId(int userId)
        {
            try
            {
                using (var userMessageContext = new UserMessageContext())
                {
                    return userMessageContext.Users.Where(x => x.Id == userId).Select(x => x.Name).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}