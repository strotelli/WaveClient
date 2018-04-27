using System;
using System.Collections.Generic;

namespace WaveClient
{
    public class RiffChunk
    {
        /// <summary>
        ///Contains the letters "RIFF" in ASCII form
        /// </summary>
        public Int32 ID;
        /// <summary>
        /// 4 + (8 + SubChunk1Size) + (8 + SubChunk2Size) This is the size of the rest of the chunk following this number.  This is the size of the entire file in bytes minus 8 bytes for the two fields not included in this count: ChunkID and ChunkSize.
        /// </summary>
        public Int32 Size;

        /// <summary>
        /// Contains the letters "WAVE"
        /// </summary>
        public Int32 Format;
    }
    public class FormatChunk
    {
        /// <summary>
        /// Contains the letters "fmt"
        /// </summary>
        public Int32 ID;
        /// <summary>
        /// 16 for PCM.  This is the size of the rest of the Subchunk which follows this number.
        /// </summary>
        public Int32 Size;
        /// <summary>
        /// PCM = 1 (i.e. Linear quantization) values other than 1 indicate some form of compression.
        /// </summary>
        public Int16 AudioFormat;
        /// <summary>
        /// Mono = 1, Stereo = 2, etc.
        /// </summary>
        public Int16 NumberOfChannels;
        /// <summary>
        /// 8000, 44100, etc.
        /// </summary>
        public Int32 SampleRate;
        /// <summary>
        /// == NumChannels * BitsPerSample/8 
        /// </summary>
        public Int32 ByteRate;
        /// <summary>
        /// == NumChannels * BitsPerSample/8 
        /// </summary>
        public Int16 BlockAlign;
        /// <summary>
        /// 8 bits = 8, 16 bits = 16, etc.
        /// </summary>
        public Int16 BitsPerSample;

    }
    public class DataChunk
    {
        public Int32 ID;
        public Int32 size;
        public List<Int16> data;
        
    }
    public class WaveFile

    {
        public RiffChunk riffChunk;
        public FormatChunk formatChunk;
        public DataChunk dataChunk;
    }

    public class CreateWave
    {
        /// <summary>
        /// Reads the bytes from the file into an understandable 
        /// </summary>
        /// <param name="filepath">Path to the .wav file</param>
        /// <returns>WaveClient.WaveFile</returns>
        public WaveFile FromFile(string filepath)
        {
            byte[] obj = System.IO.File.ReadAllBytes(filepath);
            GC.Collect();
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            GC.Collect();
            System.IO.BinaryReader binreader = new System.IO.BinaryReader(memoryStream);
            WaveFile waveFile = new WaveFile();
            #region initialize
            waveFile.riffChunk.ID = binreader.ReadInt32();
            waveFile.riffChunk.Format = binreader.ReadInt32();
            waveFile.riffChunk.Size = binreader.ReadInt32();
            waveFile.formatChunk.ID = binreader.ReadInt32();
            waveFile.formatChunk.Size = binreader.ReadInt32();
            waveFile.formatChunk.AudioFormat = binreader.ReadInt16();
            waveFile.formatChunk.NumberOfChannels = binreader.ReadInt16();
            waveFile.formatChunk.SampleRate = binreader.ReadInt32();
            waveFile.formatChunk.ByteRate = binreader.ReadInt32();
            waveFile.formatChunk.BlockAlign = binreader.ReadInt16();
            waveFile.formatChunk.BitsPerSample = binreader.ReadInt16();
            waveFile.dataChunk.ID = binreader.ReadInt32();
            waveFile.dataChunk.size = binreader.ReadInt32();
            for (int i = 0; i == waveFile.dataChunk.size - 1; i++)
            {
                waveFile.dataChunk.data.Add(binreader.ReadInt16());
            }
            #endregion
            return waveFile;
        }
        public System.IO.MemoryStream FromData(List<Int16> data)
        {
            System.IO.MemoryStream memoryStream = null; 
            WaveFile waveFile = new WaveFile();
            return memoryStream;
        }
    }

}
