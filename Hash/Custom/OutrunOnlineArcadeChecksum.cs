using System;
using System.IO;

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
    public static class OutrunOnlineArcadeChecksum
    {
        /// <summary>
        /// Calculates Outrun Online Arcade custom checksum.
        /// 
        /// There are actually two checksums in the save game, each calculated using the same function but uses data from different parts of the file.
        /// 
        /// - 0x0 = checksum1; data1 start:0x4; data1 length:filelen-4.
        /// - 0x8 = checksum2; data2 start:0x10; data2 length:filelen-16.
        /// </summary>
        /// <param name="bytes">Buffer</param>
        /// <returns>Signature</returns>
        public static byte[] Calculate(byte[] bytes)
        {
            MemoryStream memoryStream = new MemoryStream(bytes);
            uint r0 = 0;
            byte[] buffer = new byte[4];
            memoryStream.Position = 0L;
            int r1 = 0;
            int r3 = (int)(memoryStream.Length - 1L);
            int r4 = r1;
            while (r4 <= r3)
            {
                memoryStream.Read(buffer, 0, buffer.Length);
                Array.Reverse(buffer);
                uint num5 = BitConverter.ToUInt32(buffer, 0);
                r0 = (uint)((int)r0 + (int)num5 & -1);
                r4 += 4;
            }
            memoryStream.Close();
            memoryStream.Dispose();
            byte[] signature = BitConverter.GetBytes(r0);
            Array.Reverse(signature);
            return signature;
        }
    }
}
