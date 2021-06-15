using FortniteReplayReader;
using FortniteReplayReader.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Unreal.Core.Models.Enums;

namespace ConsoleReader
{
    public class Program
    {
        public static void Main()
        {
            var logger = new ServiceCollection()
                .AddLogging(loggingBuilder => loggingBuilder
                    .AddConsole()
                    .SetMinimumLevel(LogLevel.Information))
                .BuildServiceProvider()
                .GetService<ILogger<Program>>();
            var reader = new ReplayReader(logger, new FortniteReplaySettings
            {
                PlayerLocationType = LocationTypes.All,
            });
            
            var replayFile = "Replays/newSeason.replay";
            var replay = reader.ReadReplay(replayFile, ParseType.Full);
            Console.WriteLine("---- done ----");
        }
    }
}