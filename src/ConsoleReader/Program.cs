using FortniteReplayReader;
using FortniteReplayReader.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unreal.Core;
using Unreal.Core.Models;
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
                    .SetMinimumLevel((LogLevel)5));
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

            //var replayFile = "Replays/shootergame.replay";
            //var replayFile = "Replays/season6.10.replay";
            //var replayFile = "Replays/12-5.replay";
            //var replayFile = "Replays/season11.11.replay";
            var replayFile = "Replays/tournament2.replay";
            //var replayFile = "Replays/creative-season11.21.replay";
            //var replayFile = "Replays/creative.replay";
            //var replayFile = "Replays/season11.replay";
            //var replayFile = "Replays/UnsavedReplay-2018.10.06-22.00.32.replay";
            //var replayFile = "Replays/UnsavedReplay-2019.04.05-20.22.58.replay";
            //var replayFile = "Replays/UnsavedReplay-2019.05.03-21.24.46.replay";
            //var replayFile = "Replays/UnsavedReplay-2019.05.22-16.58.41.replay";
            //var replayFile = "Replays/UnsavedReplay-2019.06.30-20.39.37.replay";
            //var replayFile = "Replays/UnsavedReplay-2019.09.12-21.39.37.replay";
            //var replayFile = "Replays/UnsavedReplay-2019.12.11-02.43.14.replay";
            //var replayFile = "Replays/UnsavedReplay-2019.12.10-17.47.46.replay";
            // var replayFile = "Replays/123.replay";
            //var replayFile = "Replays/WCReplay.replay";
            //var replayFile = "Replays/00769AB3D5F45A5ED7B01553227A8A82E07CC592.replay";
            //var replayFile = "Replays/creative3.replay";

            Stopwatch sw = new Stopwatch();

            long totalTime = 0;

            NetFieldParser.IncludeOnlyMode = false;

            foreach (string path in Directory.GetFiles("Replays"))
            {
                sw.Restart();

                var reader = new ReplayReader(logger);
                var replay = reader.ReadReplay(replayFile, ParseType.Normal);

                sw.Stop();

                Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds}ms. Total Groups Read: {reader?.TotalGroupsRead}. Failed Bunches: {reader?.TotalFailedBunches}. Failed Replicator: {reader?.TotalFailedReplicatorReceives} Null Exports: {reader?.NullHandles} Property Errors: {reader?.PropertyError} Failed Property Reads: {reader?.FailedToRead}");
                Console.WriteLine($"\t - Properties Read: {reader.TotalPropertiesRead}");

                /*foreach(var k in reader.ExportGroups)
                {
                    Console.WriteLine($"\t\tIgnored: {reader.Channels[k.Key].Ignore} {String.Join(", ", k.Value.Select(x => x.GetType().ToString()).Distinct())} Entries: {k.Value.Count}");
                }*/

                //Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds}ms. Total Llamas: {reader.GameInformation.Llamas.Count}. Unknown Fields: {NetFieldParser.UnknownNetFields.Count}");

                foreach (Llama llama in replay.GameInformation.Llamas)
                {
                    //Console.WriteLine($"\t -{llama}");
                }

                totalTime += sw.ElapsedMilliseconds;

                var a = replay.GameInformation.Players.OrderByDescending(x => x.Placement);


                var asdf = replay.GameInformation.Players.Where(x => x.IsPlayersReplay);

                var b = NetFieldParser.UnknownNetFields;
                var c = b.Where(x => x.Key.ToLower().Contains("shield"));
            }

            Console.WriteLine($"Total Time: {totalTime}ms. Average: {((double)totalTime / Directory.GetFiles("Replays").Length):0.00}ms");

            Console.WriteLine("---- done ----");
            //Console.WriteLine($"Total Errors: {reader?.TotalErrors}");
            Console.ReadLine();
        }
    }
}
