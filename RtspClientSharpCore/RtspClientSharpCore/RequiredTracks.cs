using System;

namespace RtspClientSharpCore
{
    [Flags]
    public enum RequiredTracks
    {
        Video = 1,
        Audio = 2,
        All = Video | Audio
    }
}