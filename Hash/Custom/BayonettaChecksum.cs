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
    public static class BayonettaChecksum
    {
        /// <summary>
        /// Returns int[] holding 3 hashes. 
        /// 
        /// Hashes are written together beginning from 0xC (ie: int[0] = 0xC, int[1] = 0x10, int[2] = 0x14) in big endian byte order. 
        /// Pass all save data to the function Calculate(ref Data, 0, Data.Length);
        /// </summary>
        /// <param name="Buffer">Buffer</param>
        /// <param name="Index">Index</param>
        /// <param name="Length">Size</param>
        /// <returns>Array with hashes</returns>
        public static int[] Calculate(ref byte[] Buffer, int Index, int Length)
        {
            uint a = 0, b = 0, c = 0, d = 0, e = 0, f = 0, g = 0, h = 0, i = 0, j = 0, k = 0, l = 0, m = 0, n = 0, o = 0;
            a = (uint)(Length >> 2);
            if (a > 8)
            {
                if ((a - 8) > 2)
                {
                    k = (((a - 0xA) >> 1) + 1);
                    i = ((k + 4) << 1);
                    Index += 0x1C;
                    for (int x = 0; x < k; x++)
                    {
                        m = (uint)BitConverter.ToInt32(Buffer, Index);
                        k = (uint)BitConverter.ToInt32(Buffer, (Index + 4));
                        n = (m ^ f);
                        f = (k & 0xFFFF);
                        l = (m & 0xFFFF);
                        m >>= 16;
                        o = (k >> 16);
                        c += f;
                        e += l;
                        d += m;
                        b += o;
                        f = (k ^ n);
                        Index += 8;
                    }
                }
            }
            if (i < a)
            {
                k = (i << 2);
                j = (uint)BitConverter.ToInt32(Buffer, (int)(Index + k));
                g = (j & 0xFFFFFF);
                h = (j >> 16);
                f ^= j;
            }
            int[] Hashes = new int[] { (int)g, (int)h, (int)f };
            return Hashes;
        }
    }
}
