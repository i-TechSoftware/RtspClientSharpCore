using System;

namespace RtspClientSharpCore.Rtsp
{
    [Serializable]
    public class RtspParseResponseException : RtspClientException
    {
        public RtspParseResponseException(string message) : base(message)
        {
        }
    }
}