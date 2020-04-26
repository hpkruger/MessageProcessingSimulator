using MessageProcessingSimulator.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MessageProcessingSimulator
{
    public class MessageChannel : IChannel<Message>
    {
        public System.Threading.Channels.Channel<Message> Channel { get; set; }
        
        public MessageChannel()
        {
            Channel = System.Threading.Channels.Channel.CreateUnbounded<Message>();
        }
        public Task PostAsync(Message message)
        {
            return Channel.Writer.WriteAsync(message).AsTask();
        }

        public Task<Message> ReadAsync()
        {
            return Channel.Reader.ReadAsync().AsTask();
        }

        public Task<bool> WaitToReadAsync()
        {
            return Channel.Reader.WaitToReadAsync().AsTask();
        }
    }
}
