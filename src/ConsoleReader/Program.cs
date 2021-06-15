using FortniteReplayReader;
using FortniteReplayReader.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Unreal.Core.Models.Enums;

namespace ConsoleReader
{
    unsafe class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection()
                .AddLogging(loggingBuilder => loggingBuilder
                    .AddConsole()
                    .SetMinimumLevel(LogLevel.Information));
            var provider = serviceCollection.BuildServiceProvider();
            var logger = provider.GetService<ILogger<Program>>();
            var replayFile = "Replays/newSeason.replay"; 
            var reader = new ReplayReader(logger, new FortniteReplaySettings
            {
                PlayerLocationType = LocationTypes.All,
            });
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(replayFile);
                var replay = reader.ReadReplay(replayFile, ParseType.Full);
            }

            Console.WriteLine();
            Console.WriteLine("---- done ----");
        }
    }
}