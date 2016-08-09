using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using SignalRChat.DAL.Repository;
using SignalRChat.Models;
using SignalRChat.Models.ViewModels;

namespace SignalRChat
{
    public class ChatHub : Hub
    {
        public static int MaxNumberOfUsers { get; } = 20;
        private readonly IUserMessageRepository _userMessageRepository;
        public ConnectedClientsCache ClientsCache { get; }

        public ChatHub()
        {
            _userMessageRepository = new UserMessageRepository();
            ClientsCache = new ConnectedClientsCache();
        }

        public void Login(string name)
        {
            if (!(ConnectedClientsCache.ConnectedClientsList.Count >= MaxNumberOfUsers))
            {
                _userMessageRepository.AddUserToDbIfNotAlreadyExisting(name);

                Clients.Caller.manipulateDOMForLoggedInStatus();

                SendRecentMessagesToClient();

                var transactionIdentity = GetUserTransaction(name);

                AddTransactionToAppCache(transactionIdentity);

                ShowConnectedClients(transactionIdentity);
            }

            else
            {
                Clients.Caller.alertAtMaxCapacity();
            }
        }

        public void Send(string name, string message)
        {
            Clients.All.sendMessage(name, message);

            _userMessageRepository.SendMessageToDb(name, message);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            int indexOfClient;
            string nameOfClient;

            GetCurrentUser(out indexOfClient, out nameOfClient);

            if (nameOfClient == null) return base.OnDisconnected(stopCalled);

            Clients.All.removeDisconnectedClientFromClientList(nameOfClient);

            ConnectedClientsCache.ConnectedClientsList.RemoveAt(indexOfClient);

            return base.OnDisconnected(stopCalled);
        }

        #region Helpers
        private void ShowConnectedClients(TransactionIdentityModel transactionIdentity)
        {
            Clients.Others.showConnectedClients(transactionIdentity.Name);

            foreach (var connectedClient in ConnectedClientsCache.ConnectedClientsList)
            {
                Clients.Caller.showConnectedClients(connectedClient.Name);
            }
        }

        private static void AddTransactionToAppCache(TransactionIdentityModel transactionIdentity)
        {
            ConnectedClientsCache.ConnectedClientsList.Add(transactionIdentity);
        }

        private TransactionIdentityModel GetUserTransaction(string name)
        {
            return new TransactionIdentityModel
            {
                ConnectionToken = Context.QueryString.Get("connectionToken"),
                Name = name
            };
        }

        private void SendRecentMessagesToClient()
        {
            var recentUserMessages = GetRecentMessagesFromDb();

            foreach (var recentUserMessage in recentUserMessages)
            {
                Clients.Caller.sendMessage(recentUserMessage.UserName, recentUserMessage.Message);
            }
        }

        private IEnumerable<UserMessageVm> GetRecentMessagesFromDb()
        {
            return _userMessageRepository.GetRecentMessages().Select(message => new UserMessageVm
            {
                Message = message.Content,
                UserName = _userMessageRepository.GetUserNameFromUserId(message.UserId)
            }).Reverse().ToList();
        }

        private void GetCurrentUser(out int indexOfClient, out string nameOfClient)
        {
            var connectionToken = Context.QueryString.Get("connectionToken");

            indexOfClient = ConnectedClientsCache.ConnectedClientsList.FindIndex(x => x.ConnectionToken == connectionToken);
            nameOfClient = ConnectedClientsCache.ConnectedClientsList.Where(x => x.ConnectionToken == connectionToken).Select(x => x.Name).FirstOrDefault();
        }

        #endregion

    }
}