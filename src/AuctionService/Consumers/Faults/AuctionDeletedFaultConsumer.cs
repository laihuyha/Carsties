using System;
using System.Threading.Tasks;
using Contracts.Auction;
using MassTransit;

namespace AuctionService.Consumer
{
    public class AuctionDeletedFaultConsumer : IConsumer<Fault<AuctionDeleted>>
    {
        public async Task Consume(ConsumeContext<Fault<AuctionDeleted>> context)
        {
            Console.WriteLine("===> Consuming auction created fault: {0}", context.Message.Message.Id);
            foreach (var ex in context.Message.Exceptions)
            {
                Console.WriteLine("Exception: {0}", !string.IsNullOrEmpty(ex.Message) ? ex.Message : !string.IsNullOrEmpty(ex.InnerException.Message) ? ex.InnerException.Message : ex.ToString());
            }
            await Task.CompletedTask;
        }
    }
}