using System;

namespace RtspClientSharpCore.Rtsp
{
    [Serializable]
    public class RtspBadResponseException : RtspClientException
    {
        public RtspBadResponseException(string message) : base(message)
        {
        }
    }
}