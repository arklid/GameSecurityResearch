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
    public static class CastlevaniaLordsofShadowChecksum
    {
        /// <summary>
        /// Calculates the signature for Castlevania Lords of Shadow 1 & 2 save games.
        /// </summary>
        /// <param name="buffer">Buffer</param>
        /// <returns>Signature</returns>
        public static uint Calculate(byte[] buffer)
        {
            var crca = 0;
            var crcb = 0;
            var count = 0;

            do
            {
                crcb = crcb + (buffer[count] ^ count & 0xFF);
                crca = crca + (buffer[count + 1] ^ ++count & 0xFF);
                count++;
            } while (count < buffer.Length - 4); //checksum is eof-4;

            return BSwap32((uint)(crca + crcb));
        }

        private static uint BSwap32(uint Value) { return (uint)(((Value & 0x000000FF) << 24) | ((Value & 0x0000FF00) << 8) | ((Value & 0x00FF0000) >> 8) | ((Value & 0xFF000000) >> 24)); }
    }
}
