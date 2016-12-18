using System.Security.Cryptography;
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
    public static class RageChecksum
    {
        /// <summary>
        /// Calculates Rage custom checksum.
        /// 
        /// Basically MD5 hashes split into 4 little endian ints and then xored together.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static uint Calculate(byte[] buffer)
        {
            byte[] md5 = new MD5CryptoServiceProvider().ComputeHash(buffer);

            uint A = (uint)(md5[15] << 24) | (uint)(md5[14] << 16) | (uint)(md5[13] << 8) | md5[12];
            uint B = (uint)(md5[11] << 24) | (uint)(md5[10] << 16) | (uint)(md5[9] << 8) | md5[8];
            uint C = (uint)(md5[7] << 24) | (uint)(md5[6] << 16) | (uint)(md5[5] << 8) | md5[4];
            uint D = (uint)(md5[3] << 24) | (uint)(md5[2] << 16) | (uint)(md5[1] << 8) | md5[0];

            return A ^ B ^ C ^ D;
        }
    }
}
