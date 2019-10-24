using System.IO;

namespace RtspClientSharpCore.Rtcp
{
    interface ISerializablePacket
    {
        void Serialize(Stream stream);
    }
}