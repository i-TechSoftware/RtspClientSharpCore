using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using FrameDecoderCore;
using FrameDecoderCore.DecodedFrames;

namespace TestRtspClient
{
  public interface IFrameTransformer
  {
    Bitmap TransformToBitmap(IDecodedVideoFrame decodedFrame);
  }

  public class FrameTransformer : IFrameTransformer
  {
    private readonly Size _pictureSize;

    public FrameTransformer(int pictureWidth, int pictureHeight)
    {
      _pictureSize = new Size(pictureWidth, pictureHeight);
    }

    public Bitmap TransformToBitmap(IDecodedVideoFrame decodedFrame)
    {
      var managedArray = TransformFrame(decodedFrame, _pictureSize);
      var im = CopyDataToBitmap(managedArray, _pictureSize);
      return im;
    }

    private static byte[] TransformFrame(IDecodedVideoFrame decodedFrame, Size pictureSize)
    {
      var transformParameters = new TransformParameters(
        RectangleF.Empty,
        pictureSize,
        ScalingPolicy.Stretch, FrameDecoderCore.PixelFormat.Bgra32, ScalingQuality.FastBilinear);

      var pictureArraySize = pictureSize.Width * pictureSize.Height * 4;
      var unmanagedPointer = Marshal.AllocHGlobal(pictureArraySize);

      decodedFrame.TransformTo(unmanagedPointer, pictureSize.Width * 4, transformParameters);
      var managedArray = new byte[pictureArraySize];
      Marshal.Copy(unmanagedPointer, managedArray, 0, pictureArraySize);
      Marshal.FreeHGlobal(unmanagedPointer);
      return managedArray;
    }
    
    private static Bitmap CopyDataToBitmap(byte[] data, Size pictureSize)
    {
      var bmp = new Bitmap(pictureSize.Width, pictureSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

      var bmpData = bmp.LockBits(
        new Rectangle(0, 0, bmp.Width, bmp.Height),
        ImageLockMode.WriteOnly, bmp.PixelFormat);

      Marshal.Copy(data, 0, bmpData.Scan0, data.Length);

      bmp.UnlockBits(bmpData);

      return bmp;
    }

  }
}
