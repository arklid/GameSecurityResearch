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
    public static class Nintendo64CICChecksum
    {
        private static uint[] crc_table = new uint[256];

        private static void gen_table()
        {
            uint crc, poly;
            uint i, j;

            poly = 0xEDB88320;
            for (i = 0; i < 256; i++)
            {
                crc = i;
                for (j = 8; j > 0; j--)
                {
                    if (Convert.ToBoolean(crc & 1))
                    {
                        crc = (crc >> 1) ^ poly;
                    }

                    else crc >>= 1;
                }
                crc_table[i] = crc;
            }
        }

        public static uint crc32(byte[] data, int len)
        {
            uint crc = 0xFFFFFFFF;

            int i;

            for (i = 0; i < len; i++)
            {
                crc = (crc >> 8) ^ crc_table[(crc ^ data[i]) & 0xFF];
            }

            return ~crc;
        }


        public static int N64GetCIC(byte[] data)
        {
            switch (crc32(data, 0x1000 - 0x40))
            {
                case 0x6170A4A1: return 6101;
                case 0x90BB6CB5: return 6102;
                case 0x0B050EE0: return 6103;
                case 0x98BC2C86: return 6105;
                case 0xACC8580A: return 6106;
                default: return 6102;
            }
        }

        public static int N64CalcCRC(ref uint[] crc, byte[] data)
        {
            int bootcode, i;
            uint seed;

            uint t1, t2, t3;
            uint t4, t5, t6;
            uint r, d;

            switch ((bootcode = N64GetCIC(data)))
            {
                case 6101:
                case 6102:
                    seed = 0xF8CA4DDC;
                    break;
                case 6103:
                    seed = 0xA3886759;
                    break;
                case 6105:
                    seed = 0xDF26F436;
                    break;
                case 6106:
                    seed = 0x1FEA617A;
                    break;
                default:
                    return 1;
            }

            t1 = t2 = t3 = t4 = t5 = t6 = seed;

            i = 0x00001000;
            while (i < (0x00001000 + 0x00100000))
            {
                d = data[i];
                if ((t6 + d) < t6) t4++;
                t6 += d;
                t3 ^= d;
                r = RotateLeft(d, (d & 0x1F));
                t5 += r;
                if (t2 > d) t2 ^= r;
                else t2 ^= t6 ^ d;

                if (bootcode == 6105) t1 += data[0x40 + 0x0710 + (i & 0xFF)] ^ d;
                else t1 += t5 ^ d;

                i += 4;
            }
            if (bootcode == 6103)
            {
                crc[0] = (t6 ^ t4) + t3;
                crc[1] = (t5 ^ t2) + t1;
            }
            else if (bootcode == 6106)
            {
                crc[0] = (t6 * t4) + t3;
                crc[1] = (t5 * t2) + t1;
            }
            else
            {
                crc[0] = t6 ^ t4 ^ t3;
                crc[1] = t5 ^ t2 ^ t1;
            }

            return 0;
        }

        public static uint RotateLeft(this uint value, uint count)
        {
            return (value << (int)count) | (value >> (32 - (int)count));
        }

    }
}
