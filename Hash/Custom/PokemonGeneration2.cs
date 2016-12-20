using System;

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
    public static class PokemonGeneration2
    {
        /// <summary>
        /// Checksum for Pokemon Second generation (1999–2002)
        /// 
        /// </summary>
        /// <param name="buffer">Savegame data</param>
        /// <param name="Japanese">Is the savegame from the japanese version</param>
        /// <param name="GameVersionCrystal">Is the savegame from the Crystal version</param>
        /// <param name="GameVersionGoldSilver">Is the savegame from the Gold and Silver version</param>
        /// <returns>Checksum</returns>
        public static ushort Calculate(byte[] buffer, bool Japanese = false, bool GameVersionCrystal = false, bool GameVersionGoldSilver = false)
        {
            ushort accum = 0;
            for (int i = 0x2009; i <= 0x2B3A; i++)
            {
                accum += buffer[i];
            }

            if (GameVersionCrystal && Japanese)
            {
                return accum;
            }

            for (int i = 0x2B3B; i <= 0x2B82; i++)
            {
                accum += buffer[i];
            }

            if (GameVersionCrystal && !Japanese)
            {
                return accum;
            }

            for (int i = 0x2B83; i <= 0x2C8B; i++)
            {
                accum += buffer[i];
            }

            if (GameVersionGoldSilver && Japanese)
            {
                return accum;
            }

            for (int i = 0x2C8C; i <= 0x2D68; i++)
            {
                accum += buffer[i];
            }

            if (GameVersionGoldSilver && !Japanese)
            {
                return accum;
            }

            return 0;
        }
    }
}
