using System;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Auction;
using MassTransit;

namespace AuctionService.Consumer
{
    public class AuctionCreatedFaultConsumer : IConsumer<Fault<AuctionCreated>>
    {
        public async Task Consume(ConsumeContext<Fault<AuctionCreated>> context)
        {
            Console.WriteLine("===> Consuming auction created fault: {0}", context.Message.Message.Id);
            var ex = context.Message.Exceptions[0];
            if (ex.ExceptionType == "System.ArgumentException")
            {
                context.Message.Message.Model = "Foobar";
                await context.Publish(context.Message.Message);
            }
            else
            {
                Console.WriteLine("Unknown exception: {0}", ex);
            }
        }
    }
}