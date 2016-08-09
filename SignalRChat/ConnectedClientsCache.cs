using System.Collections.Generic;
using System.Web;
using SignalRChat.Models;

namespace SignalRChat
{
    public class ConnectedClientsCache
    {
        private static List<TransactionIdentityModel> _connectedClientsList;

        public static List<TransactionIdentityModel> ConnectedClientsList
        {
            get
            {
                if (_connectedClientsList != null) return _connectedClientsList;

                _connectedClientsList = (HttpContext.Current.Cache["ConnectedClientsList"] as List<TransactionIdentityModel>);
                if (_connectedClientsList != null) return _connectedClientsList;

                _connectedClientsList = new List<TransactionIdentityModel>();
                HttpContext.Current.Cache.Insert("ConnectedClientsList", _connectedClientsList);

                return _connectedClientsList;
            }
            set
            {
                _connectedClientsList = value;
                HttpContext.Current.Cache.Insert("ConnectedClientsList", _connectedClientsList);
            }
        }

        public void ClearList()
        {
            HttpContext.Current.Cache.Remove("ConnectedClientsList");
        }
    }
}