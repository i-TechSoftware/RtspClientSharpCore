using System;

namespace FrameDecoderCore.DecodedFrames
{
    public interface IDecodedVideoFrame
    {
        void TransformTo(IntPtr buffer, int bufferStride, TransformParameters transformParameters);
    }
}