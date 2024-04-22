using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Bid;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotifService.Hubs;

namespace NotifService.Consumer
{
    public class BidPlacedConsumer : IConsumer<BidPlace>
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public BidPlacedConsumer(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task Consume(ConsumeContext<BidPlace> context)
        {
            Console.WriteLine("======> Notif Hub : Bid placed message received");

            await _hubContext.Clients.All.SendAsync("Bid Placed", context.Message);
        }
    }
}