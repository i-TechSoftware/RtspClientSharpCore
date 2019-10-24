using System;

namespace RtspClientSharpCore
{
    interface ITransportStream
    {
        void Process(ArraySegment<byte> payloadSegment);
    }
}