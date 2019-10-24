using System;

namespace RtspClientSharpCore.RawFrames.Video
{
    public abstract class RawH264Frame : RawVideoFrame
    {
        public static readonly byte[] StartMarker = {0, 0, 0, 1};

        protected RawH264Frame(DateTime timestamp, ArraySegment<byte> frameSegment)
            : base(timestamp, frameSegment)
        {
        }
    }
}