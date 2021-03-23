using FortniteReplayReader;
using FortniteReplayReader.Extensions;
using FortniteReplayReader.Models;
using FortniteReplayReader.Models.NetFieldExports;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Unreal.Core.Models.Enums;

namespace ConsoleReader
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection()
                .AddLogging(loggingBuilder => loggingBuilder
                    .AddConsole()
                    .SetMinimumLevel(LogLevel.Error));
            var provider = serviceCollection.BuildServiceProvider();
            var logger = provider.GetService<ILogger<Program>>();

            
            var replayFile = "Replays/season11.11.replay"; //Used for testing

            Stopwatch sw = new Stopwatch();

            double totalTime = 0;

            string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            int count = 0;

            List<double> times = new List<double>();

            var reader = new ReplayReader(logger, new FortniteReplaySettings
            {
            });

            string demoPath = Path.Combine(appData, "FortniteGame", "Saved", "Demos");

            foreach (string path in Directory.GetFiles("Replays"))
            {
                for (int i = 0; i < 2; i++)
                {
                    Console.WriteLine($"Parsing: {path}");

                    ++count;

                    sw.Restart();
                    var replay = reader.ReadReplay(path, ParseType.Full);

                    sw.Stop();

                    Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds}ms. Total Groups Read: {reader?.TotalGroupsRead}. Failed Bunches: {reader?.TotalFailedBunches}. Failed Replicator: {reader?.TotalFailedReplicatorReceives} Null Exports: {reader?.NullHandles} Property Errors: {reader?.PropertyError} Failed Property Reads: {reader?.FailedToRead}");

                    totalTime += sw.Elapsed.TotalMilliseconds;
                    times.Add(sw.Elapsed.TotalMilliseconds);
                }
            }

            var fastest5 = times.OrderBy(x => x).Take(10);



            Console.WriteLine($"Total Time: {totalTime}ms. Average: {((double)totalTime / count):0.00}ms");
            Console.WriteLine($"Fastest 10 Time: {fastest5.Sum()}ms. Average: {(fastest5.Sum() / fastest5.Count()):0.00}ms");

            Console.WriteLine("---- done ----");
            //Console.WriteLine($"Total Errors: {reader?.TotalErrors}");
            Console.ReadLine();
        }
    }
}
