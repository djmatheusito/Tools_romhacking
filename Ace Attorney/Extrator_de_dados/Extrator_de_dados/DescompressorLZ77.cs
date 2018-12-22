using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Extrator_de_dados
{
   class DescompressorLZ77
    {

        /// <summary>
        /// Decompresses LZ77-compressed data from the given input stream.
        /// </summary>
        /// <param name="input">The input stream to read from.</param>
        /// <returns>The decompressed data.</returns>
        public static MemoryStream Decompress(string input)
        {
            BinaryReader reader = new BinaryReader(File.Open(input, FileMode.Open));

            // Check LZ77 type.
            if (reader.ReadByte() != 0x10)
                throw new ArgumentException("Input stream does not contain LZ77-compressed data.", "input");

            // Read the size.
            int size = reader.ReadUInt16() | (reader.ReadByte() << 16);

            // Create output stream.
            MemoryStream output = new MemoryStream(size);

            // Begin decompression.
            while (output.Length < size)
            {
                // Load flags for the next 8 blocks.
                int flagByte = reader.ReadByte();

                // Process the next 8 blocks.
                for (int i = 0; i < 8; i++)
                {
                    // Check if the block is compressed.
                    if ((flagByte & (0x80 >> i)) == 0)
                    {
                        // Uncompressed block; copy single byte.
                        output.WriteByte(reader.ReadByte());
                    }
                    else
                    {
                        // Compressed block; read block.
                        ushort block = reader.ReadUInt16();
                        // Get byte count.
                        int count = ((block >> 4) & 0xF) + 3;
                        // Get displacement.
                        int disp = ((block & 0xF) << 8) | ((block >> 8) & 0xFF);

                        // Save current position and copying position.
                        long outPos = output.Position;
                        long copyPos = output.Position - disp - 1;

                        // Copy all bytes.
                        for (int j = 0; j < count; j++)
                        {
                            // Read byte to be copied.
                            output.Position = copyPos++;
                            byte b = (byte)output.ReadByte();

                            // Write byte to be copied.
                            output.Position = outPos++;
                            output.WriteByte(b);
                        }
                    }

                    // If all data has been decompressed, stop.
                    if (output.Length >= size)
                    {
                        reader.Close();
                        byte[] bytes = output.ToArray();
                        File.WriteAllBytes(input, bytes);

                        break;
                    }
                }
            }

            output.Position = 0;
            return output;

        }


    }
}
