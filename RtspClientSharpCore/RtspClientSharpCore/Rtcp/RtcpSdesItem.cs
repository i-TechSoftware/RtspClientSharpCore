using System.IO;

namespace RtspClientSharpCore.Rtcp
{
    abstract class RtcpSdesItem
    {
        public abstract int SerializedLength { get; }

        public abstract void Serialize(Stream stream);
    }
}