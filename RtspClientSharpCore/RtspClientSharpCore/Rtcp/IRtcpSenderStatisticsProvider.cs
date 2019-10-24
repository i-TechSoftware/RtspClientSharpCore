using System;

namespace RtspClientSharpCore.Rtcp
{
    interface IRtcpSenderStatisticsProvider
    {
        DateTime LastTimeReportReceived { get; }
        long LastNtpTimeReportReceived { get; }
    }
}