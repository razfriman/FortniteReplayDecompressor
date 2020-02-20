using FortniteReplayReader;
using FortniteReplayReader.Extensions;
using FortniteReplayReader.Models;
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

namespace ConsoleReader
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection()
                .AddLogging(loggingBuilder => loggingBuilder
                    .AddConsole()
                    .SetMinimumLevel((LogLevel)3));
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
            //var replayFile = "Replays/12-5.replay";
            //var replayFile = "Replays/season11.31.replay";
            var replayFile = "Replays/season11.11.replay"; //Used for testing
            //var replayFile = "Replays/season12_.replay"; //Used for testing
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
            //var replayFile = "Replays/creative.replay";
            //var replayFile = "Replays/weapons2.replay";
            //var replayFile = "Replays/iceblocks2.replay";
            Stopwatch sw = new Stopwatch();

            long totalTime = 0;

            foreach (string path in Directory.GetFiles("Replays"))
            {
                sw.Restart();
                var reader = new ReplayReader(logger);
                var replay = reader.ReadReplay(replayFile, ParseType.Debug);

                sw.Stop();

                Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds}ms. Total Groups Read: {reader?.TotalGroupsRead}. Failed Bunches: {reader?.TotalFailedBunches}. Failed Replicator: {reader?.TotalFailedReplicatorReceives} Null Exports: {reader?.NullHandles} Property Errors: {reader?.PropertyError} Failed Property Reads: {reader?.FailedToRead}");
                Console.WriteLine($"\t - Properties Read: {reader.TotalPropertiesRead}");

                //Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds}ms. Total Llamas: {reader.GameInformation.Llamas.Count}. Unknown Fields: {NetFieldParser.UnknownNetFields.Count}");

                foreach (Llama llama in replay.GameInformation.Llamas)
                {
                    //Console.WriteLine($"\t -{llama}");
                }

                totalTime += sw.ElapsedMilliseconds;

                var c = replay.GameInformation.Players.Where(x => x.IsPlayersReplay);
                //var a = NetFieldParser.UnknownNetFields;
            }

            Console.WriteLine($"Total Time: {totalTime}ms. Average: {((double)totalTime / Directory.GetFiles("Replays").Length):0.00}ms");

            Console.WriteLine("---- done ----");
            //Console.WriteLine($"Total Errors: {reader?.TotalErrors}");
            Console.ReadLine();
        }

        private static void Reader_OnRender(object sender, FortniteReplay e)
        {
            var a = sender as PlaybackReplayReader<FortniteReplay>;

            Console.WriteLine($"FPS: {a.AverageFPS}. Last Update Time: {a.LastUpdateTime}.");
        }
    }
}
