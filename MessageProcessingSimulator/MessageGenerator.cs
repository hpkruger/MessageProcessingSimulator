using MessageProcessingSimulator.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessageProcessingSimulator
{
    public class MessageGenerator : IProducer<Message>
    {
        public string MessageType { get; }

        // @Jun: I am using C# properties also for private/protected members instead of fields. Just a personal preference. It also shouldn't come with performance penalties as the compiler will do inlining
        private IChannel<Message> MessageChannel { get; }
        private ILogger<MessageGenerator> Logger { get; }
        private long SeqCount { get; set; } = 1;
        private Random Random { get; set; } = new Random(DateTime.Now.Millisecond);

        public MessageGenerator(string messageType, IChannel<Message> messageChannel, ILogger<MessageGenerator> logger)
        {
            MessageType = messageType;
            MessageChannel = messageChannel;
            Logger = logger;

            Task.Run(async () =>
            {
                while (true)
                {
                    var message = await ProduceAsync();
                    //@Jun: The problem is that we cannot guarantee that the consumers always run faster than the producers and
                    //      potentially causing undesired memory usage. So what is @todo here is to create backpressure for the producers
                    //      in the event the channel is full. We could extend the MessageChannel class here to use an underlying BoundedChannel
                    //      with a fixed channel size and return to the producer if the channel is full
                    await MessageChannel.PostAsync(message);
                }
            });
        }

        public async Task<Message> ProduceAsync()
        {
            var message = new Message
            {
                MessageType = MessageType,
                SeqNum = SeqCount++
            };
            await Task.Delay(Random.Next(5, 10));

            //Logger.LogInformation("Produced Message=[{@Message}]", message);

            return message;
        }
    }
}
