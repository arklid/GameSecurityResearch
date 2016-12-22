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
namespace GameSecurityResearch.Cryptography
{

    public static class DungeonFighterCrypto
    {

        public static void DecryptBlock(ref byte[] fileBlock, int lengthOfBlock, uint decryptionKey)
        {
            uint each32bit;
            int i;

            for (i = 0; i < lengthOfBlock; i += 4)
            {
                // mov  eax, [ecx]
                each32bit = (uint)fileBlock[i];

                // xor  eax, ebx
                each32bit = each32bit ^ decryptionKey;

                // mov  edx, eax
                // shl  edx, 1Ah
                // shr  eax, 6
                // or   eax, edx
                each32bit = RotateLeft(each32bit, 0x1A);

                // mov  [ecx], eax
                fileBlock[i] = (byte)each32bit;
                fileBlock[i + 1] = (byte)(each32bit >> 8);
                fileBlock[i + 2] = (byte)(each32bit >> 0x10);
                fileBlock[i + 3] = (byte)(each32bit >> 0x18);

                // add  ecx, 4
                // Ignored, handled in for loop, advance 4 bytes.
            }
        }
        public static uint GetEncryptionKey(uint pathLength, uint encryptedPathLength)
        {
            uint encryptionKey;

            encryptionKey = RotateRight(pathLength, 0x1A);
            encryptionKey ^= encryptedPathLength;

            return encryptionKey;
        }

        public static uint RotateLeft(this uint value, int count)
        {
            return (value << count) | (value >> (32 - count));
        }

        public static uint RotateRight(this uint value, int count)
        {
            return (value >> count) | (value << (32 - count));
        }

    }
}
