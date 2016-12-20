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
    public static class PokemonGeneration3
    {
        /// <summary>
        /// Generation 3 Structure:
        /// 0xE000 per save file
        /// 14 blocks @ 0x1000 each.
        /// Blocks do not use all 0x1000 bytes allocated.
        /// Via: http://bulbapedia.bulbagarden.net/wiki/Save_data_structure_in_Generation_III
        /// </summary>
        private static readonly int[] chunkLength = {
            0xf2c, // 0 | Trainer info
            0xf80, // 1 | Team / items
            0xf80, // 2 | Unknown
            0xf80, // 3 | Unknown
            0xf08, // 4 | Rival info
            0xf80, // 5 | PC Block 0
            0xf80, // 6 | PC Block 1
            0xf80, // 7 | PC Block 2
            0xf80, // 8 | PC Block 3
            0xf80, // 9 | PC Block 4
            0xf80, // A | PC Block 5
            0xf80, // B | PC Block 6
            0xf80, // C | PC Block 7
            0x7d0  // D | PC Block 8
        };

        /// <summary>
        /// Checksum for Pokemon Third generation (2002–2006)
        /// 
        /// </summary>
        /// <param name="buffer">Savegame data</param>
        /// <param name="Japanese">Is the savegame from the japanese version</param>
        /// <returns>Checksum</returns>
        public static void Calculate(ref byte[] buffer, bool Japanese = false)
        {
            for (int i = 0; i < 14; i++)
            {
                byte[] chunk = buffer.Skip(0xE000 + i * 0x1000).Take(chunkLength[i]).ToArray();
                ushort chk = check32(chunk);
                BitConverter.GetBytes(chk).CopyTo(buffer, 0xE000 + i * 0x1000 + 0xFF6);
            }
        }
        /// <summary>Calculates the 32bit checksum over an input byte array. Used in GBA save files.</summary>
        /// <param name="data">Input byte array</param>
        /// <returns>Checksum</returns>
        private static ushort check32(byte[] data)
        {
            uint val = 0;
            for (int i = 0; i < data.Length; i += 4)
                val += BitConverter.ToUInt32(data, i);
            return (ushort)((val & 0xFFFF) + (val >> 16));
        }
    }
}
