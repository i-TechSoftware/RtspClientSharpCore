using System;

namespace RtspClientSharpCore.RawFrames.Audio
{
    public class RawG711AFrame : RawG711Frame
    {
        public RawG711AFrame(DateTime timestamp, ArraySegment<byte> frameSegment)
            : base(timestamp, frameSegment)
        {
        }
    }
}