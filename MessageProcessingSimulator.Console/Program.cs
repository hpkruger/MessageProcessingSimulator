using MessageProcessingSimulator.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;

namespace MessageProcessingSimulator.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Map("MessageType", "Other", (messageType, wt) => wt.File($"{messageType}.log"))
                .CreateLogger();

            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            var collection = new ServiceCollection();
            collection.AddOptions();

            collection.Configure<MessageProcessingSimulatorOptions>(configuration.GetSection("MessageProcessingSimulatorOptions"));
            collection.AddLogging(configure => configure.AddSerilog());
            collection.AddSingleton<MessageProcessingSimulator>();
            collection.AddSingleton<IConsumer<Message>, MessageConsumer>();
            collection.AddSingleton<IChannel<Message>, MessageChannel>();
            collection.AddSingleton<IProducerFactory<Message>, MessageGeneratorFactory>();

            var serviceProvider = collection.BuildServiceProvider();

            var simulator = serviceProvider.GetService<MessageProcessingSimulator>();

            simulator.Run();
        }
    }
}
