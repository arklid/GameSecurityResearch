using System.Diagnostics;

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
    /// All archives are obfuscated with atleast one pass of the Xor cipher, with the source position as the file position of the block. The file names and data in the Local File Header, and the file name in the Central Directory Header are obfuscated.
    /// The key is constant, but encrypted in game files, available in the tool "VZipFlip" as XorTruths.bin
    /// If the Second-pass checksum in the End of Central Directory Header exists, then a second pass of the Xor Cipher is performed with the 32-bit key: EndOfCentralDirectory.CentralDirectorySize* EndOfCentralDirectory.OffsetOfCentralDirectory.
    /// The second pass checksum uses a key that is located within the game files, available in the tool "hfssign" as ChecksumTruths.bin.The algorithm is a xor/shift checksum, described as
    /// </summary>
    public static class VindictusChecksum
    {
        public static void XorBlockWithKey(byte[] buffer, byte[] key, int src_position)
        {
            Debug.Assert(key.Length == 4 || key.Length == 4096);

            for (int x = 0; x < buffer.Length; x++)
            {
                buffer[x] ^= key[(src_position + x) & (key.Length - 1)];
            }
        }

        public static uint XorRollWithKey(byte[] buffer, int limit, uint[] key, uint checksum)
        {
            for (int x = 0; x < limit; x++)
            {
                checksum = key[buffer[x] ^ (byte)(checksum & 0xFF)] ^ (checksum >> 8);
            }

            return checksum;
        }
    }
}
