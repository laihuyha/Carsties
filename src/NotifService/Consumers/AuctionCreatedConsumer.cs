using System;
using System.Threading.Tasks;
using Contracts.Auction;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotifService.Hubs;

namespace NotifService.Consumer
{
    public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public AuctionCreatedConsumer(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task Consume(ConsumeContext<AuctionCreated> context)
        {
            Console.WriteLine("======> Notif Hub : Auction Created message received");

            await _hubContext.Clients.All.SendAsync("Auction Created", context.Message);
        }
    }
}