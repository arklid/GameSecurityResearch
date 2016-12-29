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
namespace GameSecurityResearch.Hash.NonCryptographicHashes
{
    public static class PearsonHashing
    {
        /// <summary>
        /// Calculates Pearson Hashing generating 64-bit (16 bytes) hash
        /// </summary>
        /// <param name="buffer">Data</param>
        /// <param name="size">Size</param>
        /// <returns>Checksum</returns>
        public static byte[] Calculate(this byte[] buffer, int size)
        {
            byte[] chksum = new byte[8];
            byte[] table = new byte[256] {
                // 256 values 0-255 in any (random) order suffices
                98,  6, 85,150, 36, 23,112,164,135,207,169,  5, 26, 64,165,219,     //  1
                61, 20, 68, 89,130, 63, 52,102, 24,229,132,245, 80,216,195,115,     //  2
                90,168,156,203,177,120,  2,190,188,  7,100,185,174,243,162, 10,     //  3
                237, 18,253,225,  8,208,172,244,255,126,101, 79,145,235,228,121,    //  4
                123,251, 67,250,161,  0,107, 97,241,111,181, 82,249, 33, 69, 55,    //  5
                59,153, 29,  9,213,167, 84, 93, 30, 46, 94, 75,151,114, 73,222,     //  6
                197, 96,210, 45, 16,227,248,202, 51,152,252,125, 81,206,215,186,    //  7
                39,158,178,187,131,136,  1, 49, 50, 17,141, 91, 47,129, 60, 99,     //  8
                154, 35, 86,171,105, 34, 38,200,147, 58, 77,118,173,246, 76,254,    //  9
                133,232,196,144,198,124, 53,  4,108, 74,223,234,134,230,157,139,    // 10
                189,205,199,128,176, 19,211,236,127,192,231, 70,233, 88,146, 44,    // 11
                183,201, 22, 83, 13,214,116,109,159, 32, 95,226,140,220, 57, 12,    // 12
                221, 31,209,182,143, 92,149,184,148, 62,113, 65, 37, 27,106,166,    // 13
                3, 14,204, 72, 21, 41, 56, 66, 28,193, 40,217, 25, 54,179,117,      // 14
                238, 87,240,155,180,170,242,212,191,163, 78,218,137,194,175,110,    // 15
                43,119,224, 71,122,142, 42,160,104, 48,247,103, 15, 11,138,239      // 16
            };

            for (int j = 0; j < 8; j++)
            {
                byte tmp = table[(buffer[0] + j) % 256];
                for (int i = 1; i < size; i++)
                {
                    tmp = table[tmp ^ buffer[i]];
                }

                chksum[j] = tmp;
            }

            return chksum;
        }
    }
}
