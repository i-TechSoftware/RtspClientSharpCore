using System;

namespace RtspClientSharpCore.RawFrames.Audio
{
    public class RawG711UFrame : RawG711Frame
    {
        public RawG711UFrame(DateTime timestamp, ArraySegment<byte> frameSegment)
            : base(timestamp, frameSegment)
        {
        }
    }
}