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
    /// <summary>
    /// Decryption for My Little Pony Android and IOS game.
    /// 
    /// To generate the secret key that is used to encrypt the savegame. On iOS this
    /// is quite a complex process and involves 5 different keys (3 of them
    /// are static, 1 is extracted from your Gameloft "Glun" id and one is
    /// extracted from the fte.dat file.
    /// 
    /// Process IOS:
    /// 1) Decode GLUN by using Base64 decode
    /// 2) Decrypt GLUN by using decrypt(decodedGlun, glunDecryptionKey);
    /// 3) Decrypt fte.dat data by using decrypt(ftedatContent, ftedatDecryptionKey);
    /// 4) Get the fte key like this byte[] ftedatKey = copyOfRange(decryptedFtedat, 13, 13+16);
    /// 5) Merge ftedatKey with decryptedGLUN by using byte[] mergedKey = xor(decryptedGlun, ftedatKey);
    /// 6) Merge result with decryptionKeyModifier by using byte[] finalKey = xor(mergedKey, decryptionKeyModifier);
    /// 7) Use the final key to decrypt the save game data.
    /// 
    /// </summary>
    public static class MyLittlePonyCrypto
    {
        // The 3 static keys used.
        private static readonly byte[] glunDecryptionKey = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00 };
        private static readonly byte[] ftedatDecryptionKey = new byte[] { 0x81, 0x40, 0x55, 0x08, 0x1B, 0xB5, 0xD6, 0x2F, 0x4B, 0x45, 0xED, 0x19, 0xE1, 0x54, 0x4A, 0x04 };
        private static readonly byte[] decryptionKeyModifier = new byte[] { 0xDD, 0xA2, 0x68, 0x2A, 0x5F, 0x2C, 0xB2, 0x0C, 0xA3, 0xDC, 0xEA, 0x1E, 0x21, 0xDF, 0xC0, 0x06 };

        /// <summary>
        /// Decrypts My Little Pony Android and IOS game. Crypto looks like XXTEA.
        /// 
        /// NOTE that the save games are also compressed, so decompress after decrypting the save game.
        /// </summary>
        /// <param name="data">Save game data</param>
        /// <param name="key">Key to decrypt save game data with</param>
        /// <returns>Decrypted save game data</returns>
        public static byte[] decrypt(byte[] data, byte[] key)
        {

            if (data.Length == 0)
            {
                return data;
            }

            int[] s = bytesToInts(data);
            int[] g = bytesToInts(copyOfRange(key, 0, 16));

            int d = s.Length;
            int j = s[d - 1], l = s[0];
            uint o = 0x9E3779B9;

            int m, i, a = (int)Math.Floor((double)6 + 52 / d);
            int h = (int)(a * o);

            bool printed = false;
            while (h != 0)
            {
                i = (h >> 2 & 3);
                for (int c = d - 1; c >= 0; c--)
                {
                    j = s[c > 0 ? c - 1 : d - 1];
                    m = (j >> 5 ^ l << 2) + (l >> 3 ^ j << 4) ^ (h ^ l) + (g[(c & 3) ^ i] ^ j);
                    if (!printed)
                    {
                        printed = true;
                    }
                    l = s[c] -= m;
                }
                h -= (int)o;
            }

            return intsToBytes(s);
        }

        private static int[] bytesToInts(byte[] data)
        {

            int[] a = new int[data.Length / 4];

            for (int i = 0; i < a.Length; i++)
            {
                a[i] = (data[i * 4] & 0xFF) | ((data[i * 4 + 1] & 0xFF) << 8) | ((data[i * 4 + 2] & 0xFF) << 16) | ((data[i * 4 + 3] & 0xFF) << 24);
            }

            return a;
        }

        public static byte[] intsToBytes(int[] data)
        {
            byte[] b = new byte[data.Length * 4];

            for (int i = 0; i < data.Length; i++)
            {
                b[i * 4] = (byte)(data[i] & 0xFF);
                b[i * 4 + 1] = (byte)((data[i] >> 8) & 0xFF);
                b[i * 4 + 2] = (byte)((data[i] >> 16) & 0xFF);
                b[i * 4 + 3] = (byte)((data[i] >> 24) & 0xFF);
            }

            return b;
        }

        private static byte[] copyOfRange(byte[] src, int start, int end)
        {
            int len = end - start;
            byte[] dest = new byte[len];
            // note i is always from 0
            for (int i = 0; i < len; i++)
            {
                dest[i] = src[start + i]; // so 0..n = 0+x..n+x
            }
            return dest;
        }

        public static byte[] xor(byte[] a, byte[] b)
        {
            byte[] c = new byte[Math.Min(a.Length, b.Length)];
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = (byte)(a[i] ^ b[i]);
            }
            return c;
        }
    }
}
