using System;

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
    public static class ResidentEvil6Checksum
    {
        private static bool IsLittle = true;

        /// <summary>
        /// Calculates an RE6 hash value over the specified data
        /// </summary>
        /// <param name="Data">Input data to compute over</param>
        /// <param name="Index">Index to begin calculating</param>
        /// <param name="Count">Count of bytes to run algorithm over</param>
        public static uint Compute(ref byte[] Data, int Index, int Count)
        {
            byte[] Buffer = new byte[Count];
            Array.Copy(Data, Index, Buffer, 0, Count);
            long Hash = 0;
            for (int i = 0; i < Count; i++)
            {
                Hash += ((Data[i] & 0xFF) << (24 - ((i & 3) * 8)));
            }
            Hash &= 0xFFFFFFFF;
            Array.Clear(Buffer, 0, Buffer.Length);

            if (!IsLittle)
            {
                SwapEndian(ref Hash);
            }
            return (uint)Hash;
        }

        /// <summary>
        /// Bitshifting function to reverse endianness byte ordering
        /// </summary>
        private static void SwapEndian(ref uint Value)
        {
            Value = (uint)((Value & 0x000000FF) << 24) | (uint)((Value & 0x0000FF00) << 8) | (uint)((Value & 0x00FF0000) >> 8) | (uint)((Value & 0xFF000000) >> 24);
        }

        /// <summary>
        /// Bitshifting function to reverse endianness byte ordering
        /// </summary>
        private static void SwapEndian(ref long Value)
        {
            uint x = (uint)Value;
            SwapEndian(ref x);
            Value = (long)x;
        }
    }
}
