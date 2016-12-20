using System;
using System.Linq;

namespace GameSecurityResearch.Hash.Custom
{
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
    public static class PokemonGeneration4
    {
        /// <summary>
        /// Checksum for Pokemon Fourth generation (2006–2010)
        /// 
        /// </summary>
        /// <param name="buffer">Savegame data</param>
        /// <param name="GameVersionDiamondandPearl">Is the savegame from the Diamond and Pearl version</param>
        /// <param name="GameVersionPlatinum">Is the savegame from the Platinum version</param>
        /// <param name="GameVersionHeartGoldandSoulSilver">Is the savegame from the Gold and Soul Silver version</param>
        /// <returns>Checksum</returns>
        public static ushort[] Calculate(byte[] buffer, bool GameVersionDiamondandPearl = false, bool GameVersionPlatinum = false, bool GameVersionHeartGoldandSoulSilver = false)
        {
            ushort[] chksum = new ushort[2];
            if (GameVersionDiamondandPearl)
            {
                // 0x0000-0xC0EC @ 0xC0FE
                // 0xc100-0x1E2CC @ 0x1E2DE
                chksum[0] = ccitt16(buffer.Skip(0 + 0x40000).Take(0xC0EC).ToArray());
                chksum[1] = ccitt16(buffer.Skip(0xc100 + 0x40000).Take(0x121CC).ToArray());
            }

            if (GameVersionPlatinum)
            {
                // 0x0000-0xCF18 @ 0xCF2A
                // 0xCF2C-0x1F0FC @ 0x1F10E
                chksum[0] = ccitt16(buffer.Skip(0 + 0x40000).Take(0xCF18).ToArray());
                chksum[1] = ccitt16(buffer.Skip(0xCF2C + 0x40000).Take(0x121D0).ToArray());
            }

            if (GameVersionHeartGoldandSoulSilver)
            {
                // 0x0000-0xF618 @ 0xF626
                // 0xF700-0x21A00 @ 0x21A0E
                chksum[0] = ccitt16(buffer.Skip(0 + 0x40000).Take(0xF618).ToArray());
                chksum[1] = ccitt16(buffer.Skip(0xF700 + 0x40000).Take(0x12300).ToArray());
            }

            return chksum;
        }

        // SAV Manipulation
        /// <summary>Calculates the CRC16-CCITT checksum over an input byte array.</summary>
        /// <param name="data">Input byte array</param>
        /// <returns>Checksum</returns>
        public static ushort ccitt16(byte[] data)
        {
            const ushort init = 0xFFFF;
            const ushort poly = 0x1021;

            ushort crc = init;
            foreach (byte b in data)
            {
                crc ^= (ushort)(b << 8);
                for (int j = 0; j < 8; j++)
                {
                    bool flag = (crc & 0x8000) > 0;
                    crc <<= 1;
                    if (flag)
                    {
                        crc ^= poly;
                    }
                }
            }

            return crc;
        }
    }
}
