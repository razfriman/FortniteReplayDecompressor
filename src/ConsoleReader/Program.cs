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
            //var replayFile = "Replays/creative-season11.21.replay";
            //var replayFile = "Replays/creative.replay";
            //var replayFile = "Replays/season11.replay";
            //var replayFile = "Replays/UnsavedReplay-2018.10.06-22.00.32.replay";
            //var replayFile = "Replays/UnsavedReplay-2019.04.05-20.22.58.replay";
            //var replayFile = "Replays/UnsavedReplay-2019.05.03-21.24.46.replay";
            //var replayFile = "Replays/UnsavedReplay-2019.05.22-16.58.41.replay";
            //var replayFile = "Replays/UnsavedReplay-2019.06.30-20.39.37.replay";
            //var replayFile = "Replays/UnsavedReplay-2019.09.12-21.39.37.replay";
            var replayFile = "Replays/S11-Duos.replay";
            //var replayFile = "Replays/00769AB3D5F45A5ED7B01553227A8A82E07CC592.replay";

            Stopwatch sw = new Stopwatch();

            foreach(string path in Directory.GetFiles("Replays"))
            {
                //Console.WriteLine($"Reading {path}");

                sw.Restart();

                var reader = new ReplayReader(logger);
                var replay = reader.ReadReplay(path);

                sw.Stop();

                //Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds}ms. Total Groups Read: {reader?.TotalGroupsRead}. Failed Bunches: {reader?.TotalFailedBunches}. Failed Replicator: {reader?.TotalFailedReplicatorReceives} Null Exports: {reader?.NullHandles} Property Errors: {reader?.PropertyError} Failed Property Reads: {reader?.FailedToRead}");

                Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds}ms. Total Llamas: {reader.GameInformation.Llamas.Count}. Unknown Fields: {NetFieldParser.UnknownNetFields.Count}");

                foreach(Llama llama in reader.GameInformation.Llamas)
                {
                    //Console.WriteLine($"\t -{llama}");
                }
            }

            Console.WriteLine("---- done ----");
            //Console.WriteLine($"Total Errors: {reader?.TotalErrors}");
            Console.ReadLine();
        }

        private static char[] decrypt(char[] ciphertext)
        {
            char[] plaintext = new char[500];

            int key1 = 0xd9;
            int key2 = 0x26;
            int index = 0;

            plaintext[index] = (char)(ciphertext[index] ^ key1);

            while (plaintext[index] != 0)
            {
                ++index;
                key2 = (index | (2 * key2));
                plaintext[index] = (char)(ciphertext[index] ^ ~key2);
            }

            return plaintext;
        }

        /*private static char[] decrypt(string input)
        {
            char[] decrypted = input.ToCharArray();

            uint length;
            uint value = 0x22;

            for (int i = 0; i < decrypted.Length; i++)
            {
                uint v6 = (uint)i | value;
                length = ~v6;
                value = (2 * v6);


                decrypted[i] ^= (char)length;
            }

            return decrypted;
        }*/
    }
}
