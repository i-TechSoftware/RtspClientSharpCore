cmake_minimum_required(VERSION 3.15)
project(ffmpeghelper)

set(CMAKE_CXX_STANDARD 14)

include_directories(include)
include_directories(include/libavcodec)
include_directories(include/libavdevice)
include_directories(include/libavfilter)
include_directories(include/libavformat)
include_directories(include/libavutil)
include_directories(include/libpostproc)
include_directories(include/libswresample)
include_directories(include/libswscale)

add_library(ffmpeghelper SHARED
        include/libavcodec/ac3_parser.h
        include/libavcodec/adts_parser.h
        include/libavcodec/avcodec.h
        include/libavcodec/avdct.h
        include/libavcodec/avfft.h
        include/libavcodec/d3d11va.h
        include/libavcodec/dirac.h
        include/libavcodec/dv_profile.h
        include/libavcodec/dxva2.h
        include/libavcodec/jni.h
        include/libavcodec/mediacodec.h
        include/libavcodec/qsv.h
        include/libavcodec/vaapi.h
        include/libavcodec/vdpau.h
        include/libavcodec/version.h
        include/libavcodec/videotoolbox.h
        include/libavcodec/vorbis_parser.h
        include/libavcodec/xvmc.h
        include/libavdevice/avdevice.h
        include/libavdevice/version.h
        include/libavfilter/avfilter.h
        include/libavfilter/buffersink.h
        include/libavfilter/buffersrc.h
        include/libavfilter/version.h
        include/libavformat/avformat.h
        include/libavformat/avio.h
        include/libavformat/version.h
        include/libavutil/adler32.h
        include/libavutil/aes.h
        include/libavutil/aes_ctr.h
        include/libavutil/attributes.h
        include/libavutil/audio_fifo.h
        include/libavutil/avassert.h
        include/libavutil/avconfig.h
        include/libavutil/avstring.h
        include/libavutil/avutil.h
        include/libavutil/base64.h
        include/libavutil/blowfish.h
        include/libavutil/bprint.h
        include/libavutil/bswap.h
        include/libavutil/buffer.h
        include/libavutil/camellia.h
        include/libavutil/cast5.h
        include/libavutil/channel_layout.h
        include/libavutil/common.h
        include/libavutil/cpu.h
        include/libavutil/crc.h
        include/libavutil/des.h
        include/libavutil/dict.h
        include/libavutil/display.h
        include/libavutil/downmix_info.h
        include/libavutil/encryption_info.h
        include/libavutil/error.h
        include/libavutil/eval.h
        include/libavutil/ffversion.h
        include/libavutil/fifo.h
        include/libavutil/file.h
        include/libavutil/frame.h
        include/libavutil/hash.h
        include/libavutil/hmac.h
        include/libavutil/hwcontext.h
        include/libavutil/hwcontext_cuda.h
        include/libavutil/hwcontext_d3d11va.h
        include/libavutil/hwcontext_drm.h
        include/libavutil/hwcontext_dxva2.h
        include/libavutil/hwcontext_mediacodec.h
        include/libavutil/hwcontext_qsv.h
        include/libavutil/hwcontext_vaapi.h
        include/libavutil/hwcontext_vdpau.h
        include/libavutil/hwcontext_videotoolbox.h
        include/libavutil/imgutils.h
        include/libavutil/intfloat.h
        include/libavutil/intreadwrite.h
        include/libavutil/lfg.h
        include/libavutil/log.h
        include/libavutil/lzo.h
        include/libavutil/macros.h
        include/libavutil/mastering_display_metadata.h
        include/libavutil/mathematics.h
        include/libavutil/md5.h
        include/libavutil/mem.h
        include/libavutil/motion_vector.h
        include/libavutil/murmur3.h
        include/libavutil/opt.h
        include/libavutil/parseutils.h
        include/libavutil/pixdesc.h
        include/libavutil/pixelutils.h
        include/libavutil/pixfmt.h
        include/libavutil/random_seed.h
        include/libavutil/rational.h
        include/libavutil/rc4.h
        include/libavutil/replaygain.h
        include/libavutil/ripemd.h
        include/libavutil/samplefmt.h
        include/libavutil/sha.h
        include/libavutil/sha512.h
        include/libavutil/spherical.h
        include/libavutil/stereo3d.h
        include/libavutil/tea.h
        include/libavutil/threadmessage.h
        include/libavutil/time.h
        include/libavutil/timecode.h
        include/libavutil/timestamp.h
        include/libavutil/tree.h
        include/libavutil/twofish.h
        include/libavutil/version.h
        include/libavutil/xtea.h
        include/libpostproc/postprocess.h
        include/libpostproc/version.h
        include/libswresample/swresample.h
        include/libswresample/version.h
        include/libswscale/swscale.h
        include/libswscale/version.h
        audiodecoding.cpp
        export.h
        libffmpeghelper.cpp
        stdafx.cpp
        stdafx.h
        targetver.h
        videodecoding.cpp)


# Detect architecture

if (CMAKE_SIZEOF_VOID_P MATCHES 8)
    set( PROJECT_ARCH "x86_64-linux-gnu" )
else(CMAKE_SIZEOF_VOID_P MATCHES 8)
    set( PROJECT_ARCH "x86" )
endif(CMAKE_SIZEOF_VOID_P MATCHES 8)

# FFmpeg

set(FFmpeg_ROOT "/usr" CACHE PATH "Root of FFmpeg library (libAV)")
set(FFmpeg_ROOT "/usr")
foreach (lib "libavformat.so.58" "libavcodec.so.58" "libavutil.so.56" "libswscale.so.5" "libavdevice.so.58" "libavfilter.so.7" "libpostproc.so.55" "libswresample.so.3" "libswscale.so.5")

    # Library files

    find_library(${lib}_LIB_RELEASE "${lib}" PATHS ${FFmpeg_ROOT}/lib/${PROJECT_ARCH} NO_DEFAULT_PATH)
    if (NOT ${lib}_LIB_RELEASE)
        message(FATAL_ERROR "Could not find release version of library \"${lib}\"")
    endif()
    find_library(${lib}_LIB_DEBUG "${lib}" PATH ${FFmpeg_ROOT}/lib/${PROJECT_ARCH} ${FFmpeg_ROOT}/lib)
    if (NOT ${lib}_LIB_DEBUG)
        message(FATAL_ERROR "Could not find debug version of library \"${lib}\"")
    endif()

    target_link_libraries(${PROJECT_NAME} PUBLIC
            $<$<CONFIG:Release>:${${lib}_LIB_RELEASE}>
            $<$<CONFIG:Debug>:${${lib}_LIB_DEBUG}>
            )
endforeach()



