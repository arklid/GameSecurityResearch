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
namespace GameSecurityResearch.Hash.CyclicRedundancyCheck
{
    public static class Xor8Checksum
    {
        /// <summary>
        /// XOR8 checksum
        /// </summary>
        /// <param name="buffer">Buffer</param>
        /// <param name="length">Length</param>
        /// <returns>Checksum</returns>
        public static byte Xor8<T>(this T[] buffer, int length)
        {
            byte chksum = 0;

            for (var i = 0; i < length; i++)
            {
                chksum ^= (dynamic)buffer[i] & 0xFF;
            }

            return chksum;
        }
    }
}
