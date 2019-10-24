using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using FrameDecoderCore;
using FrameDecoderCore.DecodedFrames;
using RtspClientSharpCore;
using RtspClientSharpCore.RawFrames.Video;
using RtspClientSharpCore.Rtsp;
using FrameDecoderCore.FFmpeg;
using RtspClientSharpCore.RawFrames;
using PixelFormat = FrameDecoderCore.PixelFormat;

namespace TestRtspClient
{
    class Program
    {

        private const int STREAM_WIDTH = 240;
        private const int STREAM_HEIGHT = 160;
        private static readonly Dictionary<FFmpegVideoCodecId, FFmpegVideoDecoder> _videoDecodersMap =
            new Dictionary<FFmpegVideoCodecId, FFmpegVideoDecoder>();
        //public static event EventHandler<IDecodedVideoFrame> FrameReceived;
        private static bool isWindows;
        private static bool isLinux;
        static void Main(string[] args)
        {
            isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            

            Console.WriteLine($"Platform {RuntimeInformation.OSDescription} {RuntimeInformation.OSArchitecture}");
            var serverUri = new Uri("rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mov");
            //var credentials = new NetworkCredential("admin", "admin12345678");

            var connectionParameters = new ConnectionParameters(serverUri/*, credentials*/);
            var cancellationTokenSource = new CancellationTokenSource();

            Task connectTask = ConnectAsync(connectionParameters, cancellationTokenSource.Token);

            Console.WriteLine("Press any key to cancel");
            Console.ReadLine();

            cancellationTokenSource.Cancel();

            Console.WriteLine("Canceling");
            connectTask.Wait(CancellationToken.None);
        }


        private static async Task ConnectAsync(ConnectionParameters connectionParameters, CancellationToken token)
        {
            try
            {
                TimeSpan delay = TimeSpan.FromSeconds(5);

                using (var rtspClient = new RtspClient(connectionParameters))
                {
                    rtspClient.FrameReceived += RtspClient_FrameReceived;

                    while (true)
                    {
                        Console.WriteLine("Connecting...");

                        try
                        {
                            await rtspClient.ConnectAsync(token);
                        }
                        catch (OperationCanceledException)
                        {
                            return;
                        }
                        catch (RtspClientException e)
                        {
                            Console.WriteLine(e.ToString());
                            await Task.Delay(delay, token);
                            continue;
                        }

                        Console.WriteLine("Connected.");

                        try
                        {
                            await rtspClient.ReceiveAsync(token);
                        }
                        catch (OperationCanceledException)
                        {
                            return;
                        }
                        catch (RtspClientException e)
                        {
                            Console.WriteLine(e.ToString());
                            await Task.Delay(delay, token);
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private static void RtspClient_FrameReceived(object sender, RawFrame rawFrame)
        {
            
            if (!(rawFrame is RawVideoFrame rawVideoFrame))
                return;

            FFmpegVideoDecoder decoder = GetDecoderForFrame(rawVideoFrame);
            IDecodedVideoFrame decodedFrame = decoder.TryDecode(rawVideoFrame);

            if (decodedFrame != null) 
            {
                var _FrameType = rawFrame is RawH264IFrame ? "IFrame" : "PFrame";
                TransformParameters _transformParameters = new TransformParameters(RectangleF.Empty,
                    new Size(STREAM_WIDTH, STREAM_HEIGHT),
                    ScalingPolicy.Stretch, PixelFormat.Bgra32, ScalingQuality.FastBilinear);

                var pictureSize = STREAM_WIDTH* STREAM_HEIGHT;
                IntPtr unmanagedPointer = Marshal.AllocHGlobal(pictureSize*4);

                decodedFrame.TransformTo(unmanagedPointer, STREAM_WIDTH*4, _transformParameters);
                byte[] managedArray = new byte[pictureSize*4];
                Marshal.Copy(unmanagedPointer, managedArray, 0, pictureSize*4);
                Marshal.FreeHGlobal(unmanagedPointer);
                Console.WriteLine($"Frame was successfully decoded! {_FrameType } Trying to save to BMP file...");
                try
                {
                    var im = CopyDataToBitmap(managedArray);
                    if (isWindows)
                    {
                        // Change to your path
                        im.Save(@"E:\TestPhoto\image21.bmp");
                        return;
                    }
                    if (isLinux)
                    {
                        // Change to your path
                        im.Save(@"/home/alex/image21.bmp");
                        return;
                    }
                    throw new PlatformNotSupportedException("Not supported OS platform!!");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error saving to file: {e.Message}");
                    Debug.WriteLine($"Error saving to file: {e.Message}");
                    Debug.WriteLine($"Stack trace: {e.StackTrace}");
                }
            }
           
        }


        private static Bitmap CopyDataToBitmap(byte[] data)
        {
            //Here create the Bitmap to the know height, width and format
            Bitmap bmp = new Bitmap( STREAM_WIDTH, STREAM_HEIGHT, System.Drawing.Imaging.PixelFormat.Format32bppArgb);  

            //Create a BitmapData and Lock all pixels to be written 
            BitmapData bmpData = bmp.LockBits(
                new Rectangle(0, 0, bmp.Width, bmp.Height),   
                ImageLockMode.WriteOnly, bmp.PixelFormat);
 
            //Copy the data from the byte array into BitmapData.Scan0
            Marshal.Copy(data, 0, bmpData.Scan0, data.Length);
            //Unlock the pixels
            bmp.UnlockBits(bmpData);
            //Return the bitmap 
            return bmp;
        }

        private static FFmpegVideoDecoder GetDecoderForFrame(RawVideoFrame videoFrame)
        {
            FFmpegVideoCodecId codecId = DetectCodecId(videoFrame);
            if (!_videoDecodersMap.TryGetValue(codecId, out FFmpegVideoDecoder decoder))
            {
                decoder = FFmpegVideoDecoder.CreateDecoder(codecId);
                _videoDecodersMap.Add(codecId, decoder);
            }

            return decoder;
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
