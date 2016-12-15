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
    public static class ChaserChecksum
    {
        /// <summary>
        /// This function calculates the 16bit checksum of the packets used in the game Chaser http://www.chasergame.com/
        /// A Chaser packet is composed as the following:
        ///
        /// 00 a1 c7 61 61 61 61
        /// |  |     |
        /// |  |     buffer
        /// |  16bit checksum of buffer (little endian)
        /// first byte (NULL)
        /// 
        /// </summary>
        /// <param name="buffer">Data buffer</param>
        /// <param name="length">Length</param>
        /// <returns>Checksum</returns>
        public static ushort ChaserCrc(byte[] buffer, int length)
        {
            ushort ax = 0;

            for (var i = 0; i < length; ++i)
            {
                ax = (ushort)(buffer[i] ^ ((ax << 8) | (ax >> 8)));
                ax ^= (ushort)((ax & 0xff) >> 4);
                ax ^= (ushort)(ax << 0xc);
                ax ^= (ushort)((ax & 0xff) << 5);
            }

            return(ax);
        }
    }
}
