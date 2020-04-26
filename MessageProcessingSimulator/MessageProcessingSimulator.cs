using MessageProcessingSimulator.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;


namespace MessageProcessingSimulator
{
    public class MessageProcessingSimulator
    {
        private IOptions<MessageProcessingSimulatorOptions> Options { get; }
        private IProducerFactory<Message> MessageGeneratorFactory { get; }

        public MessageProcessingSimulator(IOptions<MessageProcessingSimulatorOptions> options, 
            IConsumer<Message> messageConsumer,
            IProducerFactory<Message> messageGeneratorFactory)
        {
            Options = options;
            MessageGeneratorFactory = messageGeneratorFactory;
        }
        public void Run()
        {
            var messageGenerators = MessageGeneratorFactory.Build(Options.Value.NumGenerators);

            while (true)
            {

            }
        }
    }
}
