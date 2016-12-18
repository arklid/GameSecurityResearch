using System;
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
namespace GameSecurityResearch.Cryptography
{
    public static class Xbox360AccountFile
    {
        private static byte[] RetailKey = new byte[] { 0xE1, 0xBC, 0x15, 0x9C, 0x73, 0xB1, 0xEA, 0xE9, 0xAB, 0x31, 0x70, 0xF3, 0xAD, 0x47, 0xEB, 0xF3 };
        private static byte[] DevelopmentKey = new byte[] { 0xDA, 0xB6, 0x9A, 0xD9, 0x8E, 40, 0x76, 0x4F, 0x97, 0x7E, 0xE2, 0x48, 0x7E, 0x4F, 0x3F, 0x68 };

        /// <summary>
        /// Decrypt Xbox 360 Account file.
        /// </summary>
        /// <param name="Account">Account data</param>
        /// <returns>Decrypted data</returns>
        public static byte[] DecryptAccount(ref byte[] Account)
        {
            if (Account == null || Account.Length != 404)
            {
                throw new ArgumentException();
            }

            RC4 Crypto = new RC4();
            HMACSHA1 SHA = new HMACSHA1(RetailKey); //Initialize HMAC w/ retail or development key
            byte[] Seed = new byte[16], Key = new byte[16], Buffer, Hash;
            Array.Copy(Account, 0, Seed, 0, 16); //Copy first 16 bytes of account used for seeding the crypto key
            Buffer = SHA.ComputeHash(Seed, 0, 16); //Hash the seed for crypto key
            Array.Copy(Buffer, 0, Key, 0, 16); //Use only the first 16 bytes of the hash for the key
            Array.Resize<byte>(ref Buffer, 388); //Size buffer to hold data to decrypt
            Array.Copy(Account, 16, Buffer, 0, 388); //Copy data to decrypt into buffer
            RC4.Key = Key; //Set generated key for use with RC4 crypto
            RC4.TransformBlock(ref Buffer, 0, 388); //Decrypt data in buffer
            Hash = SHA.ComputeHash(Buffer, 0, 388); //Compute hash over decrypted data
            bool Success = true; //boolean to hold whether decryption was successful
            for (int i = 0; i < 16; i++)
            {
                if (Seed[i] != Hash[i]) //Compare seed to computed hash, decrypt success if first 16 bytes match
                {
                    Success = false;
                    break;
                }
            }
            SHA.Clear();
            RC4.Dispose();
            Array.Clear(Seed, 0, Seed.Length);
            Array.Clear(Hash, 0, Hash.Length);
            Array.Clear(Key, 0, Key.Length);
            if (!Success)
            {
                Array.Clear(Buffer, 0, Buffer.Length);
                throw new ArgumentException("Decryption failed");
            }
            return Buffer;
        }

        /// <summary>
        /// Encrypt Xbox 360 Account file.
        /// </summary>
        /// <param name="Account">Account data</param>
        /// <returns>Encrypted data</returns>
        public static byte[] EncryptAccount(ref byte[] Account)
        {
            if (Account == null || Account.Length != 388)
            {
                throw new ArgumentException();
            }

            RC4 Crypto = new RC4();
            HMACSHA1 SHA = new HMACSHA1(RetailKey); //Initialize HMAC w/ retail or development key
            byte[] Seed = new byte[16], Key = new byte[16], Buffer = new byte[404], Hash;
            Hash = SHA.ComputeHash(Account, 0, 388); //Compute HMAC over account data for key seed
            Array.Copy(Hash, 0, Seed, 0, 16); //Copy first 16 bytes of hash of account data to use for key seed
            Hash = SHA.ComputeHash(Seed, 0, 16); //Hash the seed for crypto key
            Array.Copy(Hash, 0, Key, 0, 16); //Use only the first 16 bytes of the hash for the key
            RC4.Key = Key; //Set generated key for use with RC4 crypto
            RC4.TransformBlock(Account, 0, 388); //Encrypt account data
            Array.Copy(Seed, 0, Buffer, 0, 16); //Copy seed to buffer
            Array.Copy(Account, 0, Buffer, 16, 388); //Copy encrypted data to buffer
            SHA.Clear();
            RC4.Dispose();
            Array.Clear(Seed, 0, Seed.Length);
            Array.Clear(Key, 0, Key.Length);
            Array.Clear(Hash, 0, Hash.Length);
            return Buffer;
        }
    }
}
