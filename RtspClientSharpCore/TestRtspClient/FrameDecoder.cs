using System;
using System.Collections.Generic;
using FrameDecoderCore.DecodedFrames;
using FrameDecoderCore.FFmpeg;
using RtspClientSharpCore.RawFrames.Video;

namespace TestRtspClient
{
  public interface IFrameDecoder
  {
    IDecodedVideoFrame TryDecode(RawVideoFrame rawVideoFrame);
  }

  public class FrameDecoder : IFrameDecoder
  {
    private static readonly Dictionary<FFmpegVideoCodecId, FFmpegVideoDecoder> VideoDecodersMap =
      new Dictionary<FFmpegVideoCodecId, FFmpegVideoDecoder>();
    
    public FFmpegVideoDecoder GetDecoderForFrame(RawVideoFrame videoFrame)
    {
      var codecId = DetectCodecId(videoFrame);
      if (!VideoDecodersMap.TryGetValue(codecId, out FFmpegVideoDecoder decoder))
      {
        decoder = FFmpegVideoDecoder.CreateDecoder(codecId);
        VideoDecodersMap.Add(codecId, decoder);
      }

      return decoder;
    }

    public IDecodedVideoFrame TryDecode(RawVideoFrame rawVideoFrame)
    {
      var decoder = GetDecoderForFrame(rawVideoFrame);
      return decoder.TryDecode(rawVideoFrame);
    }

    private static FFmpegVideoCodecId DetectCodecId(RawVideoFrame videoFrame)
    {
      if (videoFrame is RawJpegFrame)
        return FFmpegVideoCodecId.MJPEG;
      if (videoFrame is RawH264Frame)
        return FFmpegVideoCodecId.H264;

      throw new ArgumentOutOfRangeException(nameof(videoFrame));
    }
  }
}
