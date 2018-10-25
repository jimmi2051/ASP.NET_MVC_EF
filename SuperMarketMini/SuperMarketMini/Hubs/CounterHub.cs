using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SuperMarketMini.Hubs
{
    public class CounterHub : Hub
    {
        static long counter = 0;
        public override Task OnConnected()
        {
            counter = counter + 1;
            Clients.All.UpdateCount(counter);
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            counter = counter - 1;
            Clients.All.UpdateCount(counter);
            return base.OnDisconnected(stopCalled);
        }
    }
}