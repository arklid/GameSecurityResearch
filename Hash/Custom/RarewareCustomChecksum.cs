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
    /// <summary>
    /// This is an implementation of Rareware's custom checksum that is used by some games on the Nintendo 64 system.
    /// 
    /// Known games that use this checksum are:
    /// 
    /// Banjo Kazooie (type A)
    /// Banjo Tooie(type B)
    /// Goldeneye 007 (type B)
    /// 
    /// Example save game data 
    /// 
    /// </summary>
    public static class RarewareCustomChecksum
    {
        /// <summary>
        /// Calculates the checksum used in Rareware games.
        /// </summary>
        /// <param name="buffer">Save data</param>
        /// <param name="size">Size of buffer</param>
        /// <param name="isBT">Type A = false, Type B = true</param>
        /// <returns>Checksum</returns>
        public static ulong Calculate(byte[] buffer, int size, bool isBT)
        {
            ulong value = 0x13108B3C1;
            ulong value2, checksum1 = 0, checksum2 = 0;
            int bp, sd;
            sd = 0;

            for (bp = 0; bp < size; bp++, sd = (sd + 7) & 0xF)
            {
                value = value + (ulong)(buffer[bp] << (sd & 0x0F)) & 0x1FFFFFFFF;
                value2 = (value >> 1 | value << 32) ^ (value << 44) >> 32;
                value = value2 ^ (value2 >> 20) & 0x0FFF;
                checksum1 = (value ^ checksum1) & 0xFFFFFFFF;
            }
            for (bp--; bp >= 0; bp--, sd = (sd + 3) & 0xF)
            {
                value = value + (ulong)(buffer[bp] << (sd & 0x0F)) & 0x1FFFFFFFF;
                value2 = (value >> 1 | value << 32) ^ (value << 44) >> 32;
                value = value2 ^ (value2 >> 20) & 0x0FFF;
                checksum2 = (value ^ checksum2) & 0xFFFFFFFF;
            }
            return isBT ? (checksum1 << 32) + checksum2 : checksum1 ^ checksum2;
        }
    }
}
