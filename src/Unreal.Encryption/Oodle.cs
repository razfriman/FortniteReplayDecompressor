using OozSharp;
using System.IO;
using System.Runtime.InteropServices;
using Unreal.Encryption.Exceptions;

namespace Unreal.Encryption
{
    /// <summary>  
    /// see https://github.com/EpicGames/UnrealEngine/blob/release/Engine/Plugins/Runtime/PacketHandlers/CompressionComponents/Oodle/Source/OodleHandlerComponent/Private/OodleUtils.cpp 
    /// </summary>
    public unsafe class Oodle
    {
        static Kracken kraken = new Kracken();

        /*
        [DllImport("oo2core_5_win64")]
        private static extern int OodleLZ_Decompress(byte[] buffer, long bufferSize, byte[] outputBuffer, long outputBufferSize, uint a, uint b, ulong c, uint d, uint e, uint f, uint g, uint h, uint i, uint threadModule);
        */

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Plugins/Runtime/PacketHandlers/CompressionComponents/Oodle/Source/OodleHandlerComponent/Private/OodleUtils.cpp#L14
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        /// <param name="uncompressedSize"></param>
        /// <returns>byte[]</returns>
        public static void DecompressReplayData(byte* buffer, int compressedSize, byte* decompressedBuffer, int uncompressedSize)
        {
            kraken.Decompress(buffer, compressedSize, decompressedBuffer, uncompressedSize);
        }
    }
}
