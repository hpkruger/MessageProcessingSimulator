using MessageProcessingSimulator.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MessageProcessingSimulator
{
    public class MessageConsumer : IConsumer<Message>
    {
        protected ILogger<MessageConsumer> Logger { get; }
        protected IChannel<Message> MessageChannel { get; set; }
        protected Dictionary<string, IChannel<Message>> MessageChannelsPerMessageType { get; set; } = new Dictionary<string, IChannel<Message>>();
        private Random Random { get; set; } = new Random(DateTime.Now.Millisecond);

        public MessageConsumer(ILogger<MessageConsumer> logger, IChannel<Message> messageChannel)
        {
            Logger = logger;
            MessageChannel = messageChannel;
            // @Jun: I have one thread here that reads the channel which contains all message types
            Task.Run(async () =>
            {
                while (await MessageChannel.WaitToReadAsync())
                {
                    var message = await MessageChannel.ReadAsync();
                    await ConsumeAsync(message);
                }
            });
        }
        public Task ConsumeAsync(Message message)
        {
            if (!MessageChannelsPerMessageType.ContainsKey(message.MessageType))
            {
                // @Jun: Here I create for every message type a seperate channel. Each of these channels will be read by a seperate thread
                var channel = new MessageChannel();
                MessageChannelsPerMessageType[message.MessageType] = channel;

                Task.Run(async () =>
                {
                    while (await channel.WaitToReadAsync())
                    {
                        var msg = await channel.ReadAsync();
                        await Task.Delay(Random.Next(10, 20));

                        Logger.LogInformation("Consumed MessageType=[{MessageType}], Message=[{@Message}]", msg.MessageType, msg);
                    }
                });
            }
            return MessageChannelsPerMessageType[message.MessageType].PostAsync(message);
        }
    }
}
