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
    public static class PokemonGeneration6
    {
        /// <summary>
        /// Checksum for Pokemon Sixth generation (2013–2016)
        /// 
        /// </summary>
        /// <param name="buffer">Savegame data</param>
        /// <returns>Checksum</returns>
        public static ushort Calculate(byte[] buffer)
        {
            return ccitt16(buffer);
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
