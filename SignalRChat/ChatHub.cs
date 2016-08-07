using Microsoft.AspNet.SignalR;
using SignalRChat.DAL.Repository;

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

            Clients.Caller.showLoggedInStatus(name);
        }
         
        public void Send(string name, string message)
        {
            Clients.All.sendMessageToAllClients(name, message);

            _userMessageRepository.SendMessageToDb(name, message);
        }       
    }
}