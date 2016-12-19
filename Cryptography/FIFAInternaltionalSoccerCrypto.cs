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
    public static class FIFAInternaltionalSoccerCrypto
    {
		// Data blocks in the game binary (.exe) of FIFA International Soccer are encrypted using
		// a simple XOR encryption with a specific key. Each data section is encrypted individualy
		//
		// To identify if the binary file is encrypted look for the "Hello EA" signature in the header

		private static readonly byte[] encrypt = {
			0x23, 0x91, 0xC8, 0xE4, 0x72, 0x39, 0x9C, 0xCE,
			0x67, 0x33, 0x99, 0xCC, 0xE6, 0x73, 0xB9, 0x5C,
			0x2E, 0x17, 0x8B, 0x45, 0xA2, 0x51, 0xA8, 0x54,
			0x2A, 0x95, 0xCA, 0x65, 0x32, 0x19, 0x8C, 0x46
		};
		
		public static void Decrypt(ref byte[] buffer)
		{
			var c = 0;
			for(var d = 0; d < buffer.Length; ++d)
			{
				buffer[d] ^= encrypt[c & 31];
				
				if (++c >= 64)
				{
					c = 0;
				}
			}
		}
	}
}
