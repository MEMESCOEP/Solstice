using System;
using System.IO;

namespace Solstice.Audio.Utilities.Decoders;

public class WaveDecoder : IAudioDecoder
{
    public int SampleRate { get; private set; }
    public int Channels { get; private set; }
    public int BitDepth { get; private set; }
    public ulong Duration { get; private set; }  // Duration in milliseconds
    public byte[] RawData { get; private set; }

    public WaveDecoder(byte[] rawData)
    {
        RawData = rawData;
    }

    public float[] Decode()
    {
        using var reader = new BinaryReader(new MemoryStream(RawData));

        // RIFF header
        reader.ReadChars(4);                       // "RIFF"
        reader.ReadInt32();                        // chunk size
        var waveId = new string(reader.ReadChars(4));
        if (waveId != "WAVE")
            throw new InvalidDataException("Invalid WAVE header.");

        // fmt chunk
        var fmtId = new string(reader.ReadChars(4));
        if (fmtId != "fmt ")
            throw new InvalidDataException("Missing 'fmt ' chunk.");

        int fmtSize = reader.ReadInt32();          // 16, 18 or 40
        short formatTag = reader.ReadInt16();      // 1 = PCM
        if (formatTag != 1)
            throw new InvalidDataException("Only PCM format supported.");

        Channels   = reader.ReadInt16();
        SampleRate = reader.ReadInt32();
        reader.ReadInt32();                        // byte rate
        short blockAlign = reader.ReadInt16();
        BitDepth   = reader.ReadInt16();

        // skip any remaining fmt extension bytes
        int fmtExtra = fmtSize - 16;
        if (fmtExtra > 0)
            reader.ReadBytes(fmtExtra);

        // find "data" chunk
        string chunkId;
        int chunkSize;
        while (true)
        {
            chunkId   = new string(reader.ReadChars(4));
            chunkSize = reader.ReadInt32();
            if (chunkId == "data")
                break;
            // skip non‐data chunk
            reader.ReadBytes(chunkSize);
        }

        // read raw sample bytes
        byte[] sampleBytes = reader.ReadBytes(chunkSize);

        int bytesPerSample = BitDepth / 8;
        int totalSamples    = chunkSize / bytesPerSample;
        int frameCount      = totalSamples / Channels;

        float[] data = new float[totalSamples];

        using var ms = new MemoryStream(sampleBytes);
        using var br = new BinaryReader(ms);

        for (int i = 0; i < totalSamples; i++)
        {
            switch (BitDepth)
            {
                case 8:
                    // unsigned PCM 0–255 → –1.0…+1.0
                    byte u = br.ReadByte();
                    data[i] = (u - 128) / 128f;
                    break;

                case 16:
                    short s16 = br.ReadInt16();
                    data[i] = s16 / 32768f;
                    break;

                case 24:
                    // read 3 bytes, sign‐extend to 32 bits
                    int b1 = br.ReadByte();
                    int b2 = br.ReadByte();
                    int b3 = br.ReadByte();
                    int s24 = (b3 << 24) | (b2 << 16) | (b1 << 8);
                    data[i] = s24 / 2147483648f;
                    break;

                case 32:
                    int s32 = br.ReadInt32();
                    data[i] = s32 / 2147483648f;
                    break;

                default:
                    throw new InvalidDataException($"Unsupported bit depth: {BitDepth}");
            }
        }

        Duration = (UInt64)frameCount;

        return data;
    }
}