using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using ProjectManagementSystem.Hubs;

namespace ProjectManagementSystem.Services
{
    public class SignalrClientsProvider
    {
        public IHubConnectionContext<dynamic> GetClients()
        {
            return GlobalHost.ConnectionManager.GetHubContext<NotificationHub>().Clients;
        }
    }
}