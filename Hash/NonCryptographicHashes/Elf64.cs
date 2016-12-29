namespace GameSecurityResearch.Hash.NonCryptographicHashes
{
    public static class Elf64
    {
        /// <summary>
        /// Calculates the ELF64 hash
        /// </summary>
        /// <param name="buffer">Data</param>
        /// <param name="size">Size</param>
        /// <returns>Hash</returns>
        public static long Calculate(this byte[] buffer, int size)
        {
            long hash = 0;

            for (var i = 0; i < size; ++i)
            {
                hash <<= 4;
                hash += buffer[i] & 0xFF;

                var tmp = hash & 0xF0000000;

                if (tmp != 0)
                    hash ^= tmp >> 24;

                hash &= ~hash;
                //hash &= 0x0FFFFFFF;
            }

            return hash;
        }
    }
}
