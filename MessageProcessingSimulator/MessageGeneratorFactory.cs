using MessageProcessingSimulator.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageProcessingSimulator
{
    public class MessageGeneratorFactory : IProducerFactory<Message>
    {
        private IChannel<Message> MessageChannel { get; }
        private ILoggerFactory LoggerFactory { get; }

        public MessageGeneratorFactory(IChannel<Message> messageChannel, ILoggerFactory loggerFactory)
        {
            MessageChannel = messageChannel;
            LoggerFactory = loggerFactory;
        }

        public IEnumerable<IProducer<Message>> Build(int numGenerators)
        {
            var generators = new List<MessageGenerator>();

            for (var i = 0; i < numGenerators; ++i)
            {
                generators.Add(new MessageGenerator(((char)(65 + i)).ToString(), MessageChannel, LoggerFactory.CreateLogger<MessageGenerator>()));
            }
            return generators;
        }
    }
}
