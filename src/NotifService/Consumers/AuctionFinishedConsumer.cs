using System;
using System.Threading.Tasks;
using Contracts.Auction;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotifService.Hubs;

namespace NotifService.Consumer
{
    public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public AuctionFinishedConsumer(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task Consume(ConsumeContext<AuctionFinished> context)
        {
            Console.WriteLine("======> Notif Hub : Auction Finished message received");

            await _hubContext.Clients.All.SendAsync("Auction Finished", context.Message);
        }
    }
}