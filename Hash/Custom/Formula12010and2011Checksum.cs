using System;
using System.IO;
using System.Security.Cryptography;

/// <summary>
/// Copyright (c) 2016 Fredric Baeckström Arklid
/// All rights reserved.
/// 
/// THIS SOFTWARE IS PROVIDED "AS IS" AND ANY EXPRESSED OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
/// FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE REGENTS OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
/// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
/// 
/// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE 
/// USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
/// 
/// 0101010001101000011001010111001001100101001000000110100101110011001000000110111001101111001000000111001101110000011011110110111101101110
/// </summary>
namespace GameSecurityResearch.Hash.Custom
{
    public static class Formula12010and2011Checksum
    {
        /// <summary>
        /// Decrypt save game data for F1 2010 and F1 2011.
        /// </summary>
        /// <param name="buffer">Buffer</param>
        /// <returns>Decrypted data</returns>
        public static byte[] Decrypt(ref byte[] buffer)
        {
            BinaryReader Reader = new BinaryReader(new MemoryStream(buffer));
            Stream Output = new MemoryStream();
            ICryptoTransform Decryptor = InitAES().CreateDecryptor();
            byte[] Hash, Block, Buffer; int Magic, Size, BlockSize;
            Hash = Reader.ReadBytes(4);
            Magic = SwapEndianess(Reader.ReadInt32());
            Size = SwapEndianess(Reader.ReadInt32());
            for (;;)
            {
                if (Reader.BaseStream.Position >= Reader.BaseStream.Position) break;
                BlockSize = SwapEndianess(Reader.ReadInt32());
                Block = Reader.ReadBytes(BlockSize);
                using (CryptoStream Crypto = new CryptoStream(new MemoryStream(Block), Decryptor, CryptoStreamMode.Read))
                {
                    Buffer = new Byte[BlockSize];
                    Crypto.Read(Buffer, 0, BlockSize);
                    Output.Write(Buffer, 0, BlockSize);
                    Output.Flush();
                    Array.Clear(Buffer, 0, BlockSize);
                }
                Array.Clear(Block, 0, BlockSize);
            }
            Decryptor.Dispose();
            Reader.Close();
            Reader = new BinaryReader(Output);
            Reader.BaseStream.Seek(0, 0);
            Size = SwapEndianess(Reader.ReadInt32());
            Reader.BaseStream.Seek(0, 0);
            Buffer = Reader.ReadBytes((Size + 4));
            Reader.Close();
            Output.Dispose();
            Array.Clear(Hash, 0, 4);
            return Buffer;
        }

        /// <summary>
        /// Encrypt save game data for F1 2010 and F1 2011.
        /// </summary>
        /// <param name="buffer">Buffer</param>
        /// <returns>Encrypted data</returns>
        public static byte[] Encrypt(ref byte[] buffer)
        {
            Stream Input = new MemoryStream(buffer), Output = new MemoryStream();
            BinaryWriter Writer = new BinaryWriter(Output);
            ICryptoTransform Encryptor = InitAES().CreateEncryptor();
            int BlockCount = (buffer.Length / 1024), LeftOver = (buffer.Length % 1024);
            uint Hash; int Size; byte[] Buffer, BlockSize = new byte[] { 0, 0, 4, 0 }; //1024 byte blocks
            Writer.Write((uint)0); //Hash placeholder
            Writer.Write(SwapEndianess((int)809)); //Magic
            Writer.Write((int)0); //Size placeholder
            using (CryptoStream Crypto = new CryptoStream(Input, Encryptor, CryptoStreamMode.Read))
            {
                Hash = 0; Size = 0;
                for (int i = 0; i < BlockCount; i++)
                {
                    Buffer = new byte[1024];
                    TransformHash(ref BlockSize, ref Hash);
                    Crypto.Read(Buffer, 0, 1024);
                    TransformHash(ref Buffer, ref Hash);
                    Writer.Write(BlockSize, 0, 4);
                    Writer.Write(Buffer, 0, 1024);
                    Writer.Flush();
                    Size += 1024;
                    Array.Clear(Buffer, 0, 1024);
                }
                if (LeftOver != 0)
                {
                    Buffer = new byte[1024];
                    TransformHash(ref BlockSize, ref Hash);
                    Crypto.Read(Buffer, 0, LeftOver);
                    TransformHash(ref Buffer, ref Hash);
                    Writer.Write(BlockSize, 0, 4);
                    Writer.Write(Buffer, 0, 1024);
                    Writer.Flush();
                    Size += 1024;
                    Array.Clear(Buffer, 0, 1024);
                }
            }
            Encryptor.Dispose();
            Writer.BaseStream.Position = 0;
            Writer.Write((uint)Hash);
            Writer.BaseStream.Position = 8;
            Writer.Write((int)Size);
            Writer.Flush();
            Buffer = new byte[Output.Length];
            Output.Position = 0;
            Output.Read(Buffer, 0, (int)Output.Length);
            Writer.Close();
            Output.Dispose();
            Input.Dispose();
            Array.Clear(BlockSize, 0, 4);
            return Buffer;

        }


        private static RijndaelManaged InitAES()
        {
            RijndaelManaged AES = new RijndaelManaged();
            AES.Mode = CipherMode.ECB;
            AES.Padding = PaddingMode.None;
            AES.BlockSize = 128;
            AES.Key = new byte[] { 0xBF, 0x23, 0x8E, 0x52, 0x20, 0x82, 0x61, 0xB1, 0x1F, 0xB5, 0x09, 0x01, 0xE7, 0x8E, 0x45, 0xAC, 0x46, 0x60, 0x15, 0x35, 0x65, 0xF0, 0x92, 0x95, 0x30, 0x54, 0x84, 0xE1, 0xF0, 0x51, 0x66, 0xEC };
            AES.IV = new byte[16];
            return AES;
        }

        private static void TransformHash(ref byte[] Data, ref uint Hash)
        {
            if (Data.Length % 4 != 0) return;
            BinaryReader Reader = new BinaryReader(new MemoryStream(Data));
            for (int i = 0; i < (Data.Length / 4); i++)
            {
                Hash = ((Hash + SwapEndianess(Reader.ReadUInt32())) & 0xFFFFFFFF);
            }
            Reader.Close();
        }

        private static int SwapEndianess(int value)
        {
            return Convert.ToInt32((uint)SwapEndianess(Convert.ToUInt32(value)));
        }

        private static uint SwapEndianess(uint value)
        {
            ushort x = (ushort)((((value & 0xFF) << 8) | ((value >> 8) & 0xFF)));
            return (uint)(((x & 0xFFFF) << 16) | ((x >> 16) & 0xFFFF));
        }
    }
}
