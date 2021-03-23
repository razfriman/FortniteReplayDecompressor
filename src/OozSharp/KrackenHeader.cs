using System;
using System.Collections.Generic;
using System.Text;

namespace OozSharp
{
    public class KrackenHeader
    {
        internal DecoderTypes DecoderType { get; set; }
        internal bool RestartDecoder { get; set; }
        internal bool Uncompressed { get; set; }
        internal bool UseChecksums { get; set; }
    }
}
