using System;

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
    public static class DynastyWarriors8Crypto
    {
        public static void DecryptLayer1(ref byte[] buffer, int datasize, uint fileseed)
        {
            uint xorValue = fileseed;  // initial 2 byte value from the save game

            for (var i = 0; i < (datasize / 4); i++)  // loop through every integer of data
            {
                for (var j = 0; j < 3; j++)  // loop advances scramble value 3x
                {
                    xorValue *= 0x5B1A7851;
                    xorValue += 0xCE4E;
                }
                int pos = 4 + (i * 4);  // advance reading/writing position
                
                uint temp = BitConverter.ToUInt32(buffer, pos);

                uint temp2 = temp ^ xorValue;   // xor dword in file with current value
                buffer[pos] = (byte)temp2;
                buffer[pos + 1] = (byte)(temp2 >> 8);
                buffer[pos + 2] = (byte)(temp2 >> 0x10);
                buffer[pos + 3] = (byte)(temp2 >> 0x18);
            }
        }

        public static void DecryptLayer2(ref byte[] buffer, int datasize)
        {
            uint xorValue = 0x13100200;  // fixed initial value

            for (var i = 0; i < datasize; i++)  // loop through every byte of data
            {
                xorValue *= 0x41C64E6D;
                xorValue += 0x3039;
                uint xor8 = xorValue >> 0x10;  // get 3rd lowest byte, like 03 in 0x04030201
                var pos = 4 + i;  // advance reading/writing position
                uint temp = BitConverter.ToUInt32(buffer, pos);

                uint temp2 = temp ^ xor8;   // xor the byte
                buffer[pos] = (byte)temp2;
                buffer[pos + 1] = (byte)(temp2 >> 8);
                buffer[pos + 2] = (byte)(temp2 >> 0x10);
                buffer[pos + 3] = (byte)(temp2 >> 0x18);
            }
        }
    }
}
