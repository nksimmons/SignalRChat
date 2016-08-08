using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using SignalRChat.DAL.Repository;
using SignalRChat.Models;
using SignalRChat.Models.ViewModels;

namespace SignalRChat
{
    public class ChatHub : Hub
    {
        readonly IUserMessageRepository _userMessageRepository;
        public ConnectedClientsCache ClientsCache { get; }

        public ChatHub()
        {
            _userMessageRepository = new UserMessageRepository();
            ClientsCache = new ConnectedClientsCache();
        }

        public void Login(string name)
        {
            _userMessageRepository.AddUserToDbIfNotAlreadyExisting(name);

            Clients.Caller.manipulateDOMForLoggedInStatus();

            SendRecentMessagesToClient();

            var transactionIdentity = new TransactionIdentityModel
            {
               ConnectionToken = Context.QueryString.Get("connectionToken"),
                Name = name
            };

            ConnectedClientsCache.ConnectedClientsList.Add(transactionIdentity);
        }

        public void Send(string name, string message)
        {
            Clients.All.sendMessage(name, message);

            _userMessageRepository.SendMessageToDb(name, message);
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var connectionToken = Context.QueryString.Get("connectionToken");

            var indexOfClient = ConnectedClientsCache.ConnectedClientsList.FindIndex(x => x.ConnectionToken == connectionToken);

            try
            {
                ConnectedClientsCache.ConnectedClientsList.RemoveAt(indexOfClient);
            }

            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            return base.OnDisconnected(stopCalled);
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