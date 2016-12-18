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
    public static class LostOdysseyChecksumEncryption
    {
        /// <summary>
        /// Calculates the signature of Lost Odyssey custom checksum
        /// </summary>
        /// <param name="data">Buffer</param>
        /// <param name="length">Size</param>
        /// <returns>Signature</returns>
        public static uint Calculate(byte[] data, uint length)
        {
            uint signature = 0;
            for (uint i = 0; i < length; i++)
            {
                signature += (data[i] ^ i);
            }
            return signature;
        }

        /// <summary>
        /// Decrypt data for Lost Odyssey custom encryption
        /// </summary>
        /// <param name="data">Buffer</param>
        /// <param name="length">Size</param>
        private static void Decrypt(ref byte[] data, uint length)
        {
            uint i, r8, r9, r10;
            r8 = r9 = r10 = 0;

            for (i = 0; i < length; i++)
            {
                r10 = data[i];
                r8 = r9 & 0xFF;
                r9 = r10;
                r10 ^= 0xBB;
                r10 -= i;
                r10 ^= r8;
                data[i] = (byte)r10;
            }
        }

        /// <summary>
        /// Encrypt data for Lost Odyssey custom encryption
        /// </summary>
        /// <param name="data">Buffer</param>
        /// <param name="length">Size</param>
        private static void Encrypt(ref byte[] data, uint length)
        {
            uint i, r8, r9, r10;
            r8 = r9 = r10 = 0;

            for (i = 0; i < length; i++)
            {
                r8 = data[i];
                r10 &= 0xFF;
                r10 ^= r8;
                r10 += i;
                r10 &= 0xFF;
                r10 ^= 0xBB;
                data[i] = (byte)r10;
            }
        }
    }
}
