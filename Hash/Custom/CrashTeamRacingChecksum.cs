
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
    public static class CrashTeamRacingChecksum
    {
        /// <summary>
        /// Crash Team Racing PS1 Custom Checksum
        /// </summary>
        /// <param name="buffer">Save Game Data</param>
        /// <returns>Checksum</returns>
        public static uint Calculate(byte[] buffer)
        {
            uint r2 = 0;
            uint r3 = 0;
            uint r4 = 0;
            uint r5 = 0;
            uint r6 = 0;
            uint r7 = 0;

            for (uint i = 0; i < 5760; i++)
            {
                r5 = buffer[i + 0x180];
                r3 = 0x7;
                r7 = 0x10000;
                r6 = r7;
                r6 = r6 | 0x1021;
                
                // Clear checksum
                buffer[0x17FE] = 0x00;
                buffer[0x17FF] = 0x00;

                while (r3 < 0xFFFF)
                {
                    r4 = r4 << 0x1;
                    r2 = r5 >> (int)r3;
                    r2 = r2 & 0x1;
                    r4 = r4 | r2;
                    r2 = r4 & r7;

                    if (r2 != 0) r4 = r4 ^ r6;

                    r3 = (r3 + 0xFFFF) & 0xFFFF;
                }
            }

            return r4;
        }
    }
}
