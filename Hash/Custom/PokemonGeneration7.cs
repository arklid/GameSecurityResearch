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
    public static class PokemonGeneration7
    {
        /// <summary>
        /// Checksum for Pokemon Seventh generation (2016–present)
        /// 
        /// </summary>
        /// <param name="buffer">Savegame data</param>
        /// <returns>Checksum</returns>
        public static ushort Calculate(byte[] buffer)
        {
            return check32(buffer);
        }

        /// <summary>Calculates the 32bit checksum over an input byte array. Used in GBA save files.</summary>
        /// <param name="data">Input byte array</param>
        /// <returns>Checksum</returns>
        private static ushort check32(byte[] data)
        {
            uint val = 0;
            for (int i = 0; i < data.Length; i += 4)
            {
                val += BitConverter.ToUInt32(data, i);
            }

            return (ushort)((val & 0xFFFF) + (val >> 16));
        }
    }
}
