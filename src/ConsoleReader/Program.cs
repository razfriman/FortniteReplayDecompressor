using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using FortniteReplayReader;
using FortniteReplayReader.Extensions;
using FortniteReplayReader.Models;
using FortniteReplayReader.Models.NetFieldExports;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Buffers;
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

        [Params(/*ParseType.Minimal, ParseType.Normal, */ParseType.Full)]
        public ParseType Type;

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
        }
        */

        //[Benchmark]
        public FortniteReplay ReadMassiveReplay()
        {
            return _reader.ReadReplay("Replays/massive.replay", Type);
        }

        [Benchmark]
        public FortniteReplay ReadLongReplay()
        {
            return _reader.ReadReplay("Replays/newSeason.replay", Type);
        }
        
        [Benchmark]
        public FortniteReplay ReadShortReplay()
        {
            return _reader.ReadReplay("Replays/replay_Bow.replay", Type);
        }
        
        [Benchmark]
        public FortniteReplay ReadOldReplay()
        {
            return _reader.ReadReplay("Replays/season11.11.replay", Type);
        }

        [Benchmark]
        public FortniteReplay ReadRoundReplay()
        {
            return _reader.ReadReplay("Replays/rounds.replay", Type);
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
            

            var b = a.ReadLongReplay();
            /*
            ReplayReader reader2 = a._reader;

            Console.WriteLine($"Total Groups Read: {reader2?.TotalGroupsRead}. Failed Bunches: {reader2?.TotalFailedBunches}. Failed Replicator: {reader2?.TotalFailedReplicatorReceives} Null Exports: {reader2?.NullHandles} Property Errors: {reader2?.PropertyError} Failed Property Reads: {reader2?.FailedToRead}");
            Console.WriteLine($"Pins: {FBitArray.Pins}");
            */
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
            var replayFile = "Replays/newSeason.replay"; //Used for testing
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
                     
                    Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds}ms. Total Groups Read: {reader?.TotalGroupsRead}. Failed Bunches: {reader?.TotalFailedBunches}. Failed Replicator: {reader?.TotalFailedReplicatorReceives} Null Exports: {reader?.NullHandles} Property Errors: {reader?.PropertyError} Failed Property Reads: {reader?.FailedToRead}. Missing Properties: {reader?.MissingProperty}. Success Properties: {reader?.SuccessProperties}");
                    //Console.Write($"Pins: {MemoryBuffer.Pins}");

#if  DEBUG
                    var asdfa = String.Join("\n", ReplayReader._failedTypes.OrderByDescending(x => x.Value).Select(x => $"{x.Key}: {x.Value}"));

#endif
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

[MemoryDiagnoser]
[Config(typeof(DontForceGcCollectionsConfig))] // we don't want to interfere with GC, we want to include it's impact
public class Pooling
{
    [Params(//(int)1E+2, // 100 bytes
            //(int)1E+3, // 1 000 bytes = 1 KB
            //(int)1E+4, // 10 000 bytes = 10 KB
        (int)1E+6) // 100 000 bytes = 100 KB
        ]
    public int SizeInBytes { get; set; }

    private FBitArray fBitArrray;
    private Type UIntType = typeof(UInt32);
    private Dictionary<Type, RepLayoutCmdType> typeDict = new Dictionary<Type, RepLayoutCmdType>();
    private Dictionary<int, RepLayoutCmdType> intDict = new Dictionary<int, RepLayoutCmdType>();
    private Dictionary<string, RepLayoutCmdType> stringDict = new Dictionary<string, RepLayoutCmdType>();


    [GlobalSetup]
    public void GlobalSetup()
    {
        typeDict.Add(typeof(bool), RepLayoutCmdType.PropertyBool);
        typeDict.Add(typeof(byte), RepLayoutCmdType.PropertyByte);
        typeDict.Add(typeof(ushort), RepLayoutCmdType.PropertyUInt16);
        typeDict.Add(typeof(int), RepLayoutCmdType.PropertyInt);
        typeDict.Add(typeof(uint), RepLayoutCmdType.PropertyUInt32);
        typeDict.Add(typeof(ulong), RepLayoutCmdType.PropertyUInt64);
        typeDict.Add(typeof(float), RepLayoutCmdType.PropertyFloat);
        typeDict.Add(typeof(string), RepLayoutCmdType.PropertyString);
        typeDict.Add(typeof(object), RepLayoutCmdType.Ignore);

        stringDict.Add(typeof(bool).FullName, RepLayoutCmdType.PropertyBool);
        stringDict.Add(typeof(byte).FullName, RepLayoutCmdType.PropertyByte);
        stringDict.Add(typeof(ushort).FullName, RepLayoutCmdType.PropertyUInt16);
        stringDict.Add(typeof(int).FullName, RepLayoutCmdType.PropertyInt);
        stringDict.Add(typeof(uint).FullName, RepLayoutCmdType.PropertyUInt32);
        stringDict.Add(typeof(ulong).FullName, RepLayoutCmdType.PropertyUInt64);
        stringDict.Add(typeof(float).FullName, RepLayoutCmdType.PropertyFloat);
        stringDict.Add(typeof(string).FullName, RepLayoutCmdType.PropertyString);
        stringDict.Add(typeof(object).FullName, RepLayoutCmdType.Ignore);

        intDict.Add(typeof(bool).MetadataToken, RepLayoutCmdType.PropertyBool);
        intDict.Add(typeof(byte).MetadataToken, RepLayoutCmdType.PropertyByte);
        intDict.Add(typeof(ushort).MetadataToken, RepLayoutCmdType.PropertyUInt16);
        intDict.Add(typeof(int).MetadataToken, RepLayoutCmdType.PropertyInt);
        intDict.Add(typeof(uint).MetadataToken, RepLayoutCmdType.PropertyUInt32);
        intDict.Add(typeof(ulong).MetadataToken, RepLayoutCmdType.PropertyUInt64);
        intDict.Add(typeof(float).MetadataToken, RepLayoutCmdType.PropertyFloat);
        intDict.Add(typeof(string).MetadataToken, RepLayoutCmdType.PropertyString);
        intDict.Add(typeof(object).MetadataToken, RepLayoutCmdType.Ignore);
    }


    [Benchmark]
    public RepLayoutCmdType GetByType()
    {
        if(typeDict.TryGetValue(UIntType, out var val))
        {

        }

        return val;
    }

    [Benchmark]
    public RepLayoutCmdType GetByInt()
    {
        if (intDict.TryGetValue(UIntType.MetadataToken, out var val))
        {

        }

        return val;
    }

    [Benchmark]
    public RepLayoutCmdType GetByName()
    {
        if (stringDict.TryGetValue(UIntType.FullName, out var val))
        {

        }

        return val;
    }
}

public class DontForceGcCollectionsConfig : ManualConfig
{
    public DontForceGcCollectionsConfig()
    {
        Add(Job.Default
            .With(new GcMode()
            {
                Force = false // tell BenchmarkDotNet not to force GC collections after every iteration
            }));
    }
}
