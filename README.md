# C# RTSP Client for .NET Core 3.0
This repo contains fork (https://github.com/BogdanovKirill/RtspClientSharp  for .NET Standard 2.0) of C# RTSP client implementation for .Net Core 3.0

Please read the original documentation at: https://github.com/BogdanovKirill/RtspClientSharp/blob/master/README.md

## Differences from the original version:

- ffmpeghelper_nix folder contains a version project of ffmpeghelper built under Ubuntu 18.06 x64 (cmake)
- FFmpegVideoPInvoke has two implementations - for Windows and Linux (FFmpegVideoPInvokeWin, FFmpegVideoPInvokeLinux)
- TestRtspClient - test client for Linux x64 and Windows x64 platforms
- The compiled library ffmpeghelper.so is included in the project TestRtspClient

## Linux dependencies

- Installed package ffmpeg version 4.2.1
- Installed package dotnet-sdk 3.0
- Installed package dotnet-runtime 3.0

## Test on Linux

Type:
`$ dotnet TestRtspClient.dll`

![](https://github.com/i-TechSoftware/RtspClientSharpCore/blob/master/LinuxTestConsole.jpg)
