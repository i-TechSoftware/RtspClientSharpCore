using System;

namespace RtspClientSharpCore.RawFrames.Video
{
    public class RawH264IFrame : RawH264Frame
    {
        public ArraySegment<byte> SpsPpsSegment { get; }

        public RawH264IFrame(DateTime timestamp, ArraySegment<byte> frameSegment, ArraySegment<byte> spsPpsSegment)
            : base(timestamp, frameSegment)
        {
            SpsPpsSegment = spsPpsSegment;
        }
    }
}