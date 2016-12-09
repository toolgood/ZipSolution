using System;
using System.IO;
using SoMain.Common.SharpCompress.Archive.GZip;
using SoMain.Common.SharpCompress.Archive.Rar;
using SoMain.Common.SharpCompress.Archive.Tar;
using SoMain.Common.SharpCompress.Archive.Zip;
using SoMain.Common.SharpCompress.Common;
using SoMain.Common.SharpCompress.Compressor;
using SoMain.Common.SharpCompress.Compressor.BZip2;
using SoMain.Common.SharpCompress.IO;
using SoMain.Common.SharpCompress.Reader.GZip;
using SoMain.Common.SharpCompress.Reader.Rar;
using SoMain.Common.SharpCompress.Reader.Tar;
using SoMain.Common.SharpCompress.Reader.Zip;
using GZipStream = SoMain.Common.SharpCompress.Compressor.Deflate.GZipStream;

namespace SoMain.Common.SharpCompress.Reader
{
    public static class ReaderFactory
    {
        /// <summary>
        /// Opens a Reader for Non-seeking usage
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IReader Open(Stream stream, Options options = Options.KeepStreamsOpen)
        {
            stream.CheckNotNull("stream");

            RewindableStream rewindableStream = new RewindableStream(stream);
            rewindableStream.StartRecording();
            if (ZipArchive.IsZipFile(rewindableStream, null))
            {
                rewindableStream.Rewind(true);
                return ZipReader.Open(rewindableStream, null, options);
            }
            rewindableStream.Rewind(false);
            if (GZipArchive.IsGZipFile(rewindableStream))
            {
                rewindableStream.Rewind(false);
                GZipStream testStream = new GZipStream(rewindableStream, CompressionMode.Decompress);
                if (TarArchive.IsTarFile(testStream))
                {
                    rewindableStream.Rewind(true);
                    return new TarReader(rewindableStream, CompressionType.GZip, options);
                }
                rewindableStream.Rewind(true);
                return GZipReader.Open(rewindableStream, options);
            }

            rewindableStream.Rewind(false);
            if (BZip2Stream.IsBZip2(rewindableStream))
            {
                rewindableStream.Rewind(false);
                BZip2Stream testStream = new BZip2Stream(rewindableStream, CompressionMode.Decompress, false);
                if (TarArchive.IsTarFile(testStream))
                {
                    rewindableStream.Rewind(true);
                    return new TarReader(rewindableStream, CompressionType.BZip2, options);
                }
            }

            rewindableStream.Rewind(false);
            if (TarArchive.IsTarFile(rewindableStream))
            {
                rewindableStream.Rewind(true);
                return TarReader.Open(rewindableStream, options);
            }
            rewindableStream.Rewind(false);
            if (RarArchive.IsRarFile(rewindableStream, options))
            {
                rewindableStream.Rewind(true);
                return RarReader.Open(rewindableStream, options);
            }

            throw new InvalidOperationException("Cannot determine compressed stream type.  Supported Reader Formats: Zip, GZip, BZip2, Tar, Rar");
        }
    }
}