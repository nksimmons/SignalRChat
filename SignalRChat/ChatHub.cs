using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using SignalRChat.DAL.Repository;
using SignalRChat.Models.ViewModels;

namespace SignalRChat
{
    public class ChatHub : Hub
    {
        readonly IUserMessageRepository _userMessageRepository;

        public ChatHub()
        {
            _userMessageRepository = new UserMessageRepository();
        }

        public void Login(string name)
        {
            _userMessageRepository.AddUserToDbIfNotAlreadyExisting(name);

            Clients.Caller.manipulateDOMForLoggedInStatus(name);

            SendRecentMessagesToClient();
        }

        public void Send(string name, string message)
        {
            Clients.All.sendMessage(name, message);

            _userMessageRepository.SendMessageToDb(name, message);
        }

        #region Helpers

        void SendRecentMessagesToClient()
        {
            var recentUserMessages = GetRecentMessagesFromDb();

            foreach (var recentUserMessage in recentUserMessages)
            {
                Clients.Caller.sendMessage(recentUserMessage.UserName, recentUserMessage.Message);
            }
        }

        IEnumerable<UserMessageVm> GetRecentMessagesFromDb()
        {
            return _userMessageRepository.GetRecentMessages().Select(message => new UserMessageVm
            {
                Message = message.Content,
                UserName = _userMessageRepository.GetUserNameFromUserId(message.UserId)
            }).Reverse().ToList();
        }

        #endregion

    }
}