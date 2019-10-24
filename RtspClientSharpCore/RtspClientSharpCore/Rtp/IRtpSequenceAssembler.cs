using RtspClientSharpCore.Utils;

namespace RtspClientSharpCore.Rtp
{
    internal interface IRtpSequenceAssembler
    {
        RefAction<RtpPacket> PacketPassed { get; set; }

        void ProcessPacket(ref RtpPacket rtpPacket);
    }
}