using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
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
using System.Runtime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unreal.Core;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;
using Unreal.Encryption;

namespace ConsoleReader
{
    [MemoryDiagnoser]
    [SimpleJob]
    public class Benchmark
    {
        public ReplayReader _reader = new ReplayReader(null, null);

        private byte[] test = new byte[100000];

        public Benchmark()
        {
            Random rand = new Random();
            rand.NextBytes(test);
        }

        /*
        [Benchmark]
        public void BitArrayTest()
        {
            using(FBitArray arr = new FBitArray(test))
            {

            }
        }*/

        [Benchmark]
        public FortniteReplay ReadLongReplay()
        {
            return _reader.ReadReplay("Replays/newSeason.replay", ParseType.Full);
        }

        [Benchmark]
        public FortniteReplay ReadShortReplay()
        {
            return _reader.ReadReplay("Replays/replay_Bow.replay", ParseType.Full);
        }

        [Benchmark]
        public FortniteReplay ReadOldReplay()
        {
            return _reader.ReadReplay("Replays/season11.11.replay", ParseType.Full);
        }

        [Benchmark]
        public FortniteReplay ReadRoundReplay()
        {
            return _reader.ReadReplay("Replays/rounds.replay", ParseType.Full);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
#if !DEBUG
            var summary = BenchmarkRunner.Run<Benchmark>();

            Console.WriteLine(summary);

            Benchmark a = new Benchmark();
            

            var b = a.ReadShortReplay();
            ReplayReader reader2 = a._reader;

            Console.WriteLine($"Total Groups Read: {reader2?.TotalGroupsRead}. Failed Bunches: {reader2?.TotalFailedBunches}. Failed Replicator: {reader2?.TotalFailedReplicatorReceives} Null Exports: {reader2?.NullHandles} Property Errors: {reader2?.PropertyError} Failed Property Reads: {reader2?.FailedToRead}");
            Console.WriteLine($"Pins: {FBitArray.Pins}");

            return;
#endif
            var serviceCollection = new ServiceCollection()
                .AddLogging(loggingBuilder => loggingBuilder
                    .AddConsole()
                    .SetMinimumLevel(LogLevel.Information));
            var provider = serviceCollection.BuildServiceProvider();
            var logger = provider.GetService<ILogger<Program>>();

            //var localAppDataFolder = GetFolderPath(SpecialFolder.LocalApplicationData);
            ////var replayFilesFolder = Path.Combine(localAppDataFolder, @"FortniteGame\Saved\Demos");
            //var replayFilesFolder = @"D:\Projects\FortniteReplayCollection";
            //var replayFiles = Directory.EnumerateFiles(replayFilesFolder, "*.replay");

            //foreach (var replayFile in replayFiles)
            //{
            //    var reader = new ReplayReader(logger);
            //    var replay = reader.ReadReplay(replayFile);
            //}

            //var replayFile = "Replays/season12_arena.replay";
            //var replayFile = "Replays/season11.31.replay
            var replayFile = "Replays/replay_Bow.replay"; //Used for testing
            //var replayFile = @"C:\Users\TnT\Source\Repos\FortniteReplayDecompressor_Shiqan\src\ConsoleReader\bin\Release\netcoreapp3.1\Replays\collectPickup.replay";

            //var replayFile = "Replays/season11.11.replay"; //Used for testing
            //var replayFile = "Replays/Test.replay"; //Used for testing
            //var replayFile = "Replays/shoottest.replay"; 
            //var replayFile = "Replays/tournament2.replay";
            //var replayFile = "Replays/creative-season11.21.replay";
            //var replayFile = "Replays/creative.replay";
            //var replayFile = "Replays/UnsavedReplay-2019.12.11-02.43.14.replay";
            //var replayFile = "Replays/UnsavedReplay-2019.12.10-17.47.46.replay";
            //var replayFile = "Replays/UnsavedReplay-2019.12.11-01.49.22.replay";
            //var replayFile = "Replays/123.replay";
            //var replayFile = "Replays/WCReplay.replay";
            //var replayFile = "Replays/00769AB3D5F45A5ED7B01553227A8A82E07CC592.replay";
            //var replayFile = "Replays/season12_arena.replay";
            //var replayFile = "Replays/1.replay";
            //var replayFile = "Replays/weapons2.replay";
            //var replayFile = "Replays/iceblocks2.replay";
            //var replayFile = "Replays/creativeShooting.replay";

            Stopwatch sw = new Stopwatch();

            double totalTime = 0;

            string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            int count = 0;

            List<double> times = new List<double>();

            var reader = new ReplayReader(null, new FortniteReplaySettings
            {
                PlayerLocationType = LocationTypes.None,
            });

            string demoPath = Path.Combine(appData, "FortniteGame", "Saved", "Demos");

            foreach (string path in Directory.GetFiles(@"C:\Users\TnT\Source\Repos\FortniteReplayDecompressor_Shiqan\src\ConsoleReader\bin\Release\netcoreapp3.1\Replays"))
            {
                Console.WriteLine(path);

                for (int i = 0; i < 200; i++)
                {
                    ++count;

                    sw.Restart();
                    var replay = reader.ReadReplay(replayFile, ParseType.Full);

                    sw.Stop();

                    Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds}ms. Total Groups Read: {reader?.TotalGroupsRead}. Failed Bunches: {reader?.TotalFailedBunches}. Failed Replicator: {reader?.TotalFailedReplicatorReceives} Null Exports: {reader?.NullHandles} Property Errors: {reader?.PropertyError} Failed Property Reads: {reader?.FailedToRead}");

                    Console.WriteLine($"Pins: {FBitArray.Pins}");
                    totalTime += sw.Elapsed.TotalMilliseconds;
                    times.Add(sw.Elapsed.TotalMilliseconds);


                }

                Console.WriteLine();
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
