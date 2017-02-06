using System;

namespace GameSecurityResearch.Hash.CyclicRedundancyChecks
{
    public static class Dynamic
    {
        /// <summary>
        /// Available methods to calculate the CRC checksum:
        /// 
        /// 0 = table[(data[i] ^ crc) & 0xff] ^ (crc >> 8)
        /// 1 = table[(data[i] ^ (crc >> (bits - 8))) & 0xff] ^ (crc << 8)
        /// 2 = ((crc << 8) | data[i]) ^ table[(crc >> (bits - 8)) & 0xff]
        /// 3 = ((crc >> 1) + ((crc & 1) << (bits - 1))) + data[i]
        /// 4 = IP checksum
        /// 5 = crc ^ data[i]
        /// 6 = crc + data[i]
        /// 7 = table[(data[i] ^ crc) & 0xff] ^ crc
        /// 8 = table[(data[i] ^ crc) & 0xff] ^ (crc >> (bits - 8))
        /// 9 = (crc << 1) ^ data [i]
        /// 10 = (crc << 1) + data [i]
        /// 11 = RotateLeft(crc, 1) ^ data[i];
        /// 12 = RotateLeft(crc, 1) + data[i];
        /// 13 = RotateRight(crc, 1) ^ data[i];
        /// 14 = RotateRight(crc, 1) + data[i];
        /// 15 = (crc << 5) + crc + data[i];
        /// 16 = (crc * config.Poly) + data[i];
        /// 17 = (crc * config.Poly) ^ data[i];
        /// 18 = (crc ^ data[i]) * config.Poly;
        /// 19 = data[i] + (crc << 6) + (crc << 16) - crc
        /// 20 = config.Poly * (crc + data[i] * (i + 1))
        /// </summary>
        public enum Types {
            Type0,
            Type1,
            Type2,
            Type3,
            Type4,
            Type5,
            Type6,
            Type7,
            Type8,
            Type9,
            Type10,
            Type11,
            Type12,
            Type13,
            Type14,
            Type15,
            Type16,
            Type17,
            Type18,
            Type19,
            Type20,
        };

        /// <summary>
        /// Configuration
        /// </summary>
        public struct Config
        {
            public ulong[] Table { get; set; }
            public Types Type { get; set; }
            public ulong Poly { get; set; }
            public int Bits { get; set; }
            public ulong Init { get; set; }
            public ulong Final { get; set; }
            public int Rever { get; set; }
            public int BitmaskSide { get; set; }
        }

        private static ulong CrcBitmask(int bits, int mask)
        {
            ulong bitmask;

            if (bits < 0)
            {
                bitmask = 0;
            }
            else if (bits >= 64)
            {
                bitmask = 0;
            }
            else
            {
                bitmask = (ulong)(1 << bits);
            }

            if (Convert.ToBoolean(mask))
            {
                bitmask--;
            }

            return bitmask;
        }

        private static ulong CrcSafeLimit(ulong crc, int bits)
        {
            if (bits < 64)
            {
                crc &= CrcBitmask(bits, 1);
            }
            return (crc);
        }

        public static ulong RotateLeft(this ulong value, int count)
        {
            return (value << count) | (value >> (32 - count));
        }

        public static ulong RotateRight(this ulong value, int count)
        {
            return (value >> count) | (value << (32 - count));
        }


        public static ulong Calculate(Config config, byte[] data, int length)
        {
            ulong crc;

            crc = config.Init;     // Init

            for (var i = 0; i < length; i++)
            {
                if (config.Type == Types.Type0)
                {
                    crc = config.Table[(data[i] ^ (CrcSafeLimit(crc, config.Bits))) & 0xff] ^ ((CrcSafeLimit(crc, config.Bits)) >> 8);
                }
                else if (config.Type == Types.Type1)
                {
                    crc = config.Table[(data[i] ^ ((CrcSafeLimit(crc, config.Bits)) >> (config.Bits - 8))) & 0xff] ^ ((CrcSafeLimit(crc, config.Bits)) << 8);
                }
                else if (config.Type == Types.Type2)
                {
                    crc = (((CrcSafeLimit(crc, config.Bits)) << 8) | data[i]) ^ config.Table[((CrcSafeLimit(crc, config.Bits)) >> (config.Bits - 8)) & 0xff];
                }
                else if (config.Type == Types.Type3)
                {
                    crc = (((CrcSafeLimit(crc, config.Bits)) >> 1) + (((CrcSafeLimit(crc, config.Bits)) & 1) << (config.Bits - 1))) + data[i];
                }
                else if (config.Type == Types.Type4)
                {
                    throw new NotSupportedException();
                }
                else if (config.Type == Types.Type5)
                {
                    crc = (CrcSafeLimit(crc, config.Bits)) ^ data[i];
                }
                else if (config.Type == Types.Type6)
                {
                    crc = (CrcSafeLimit(crc, config.Bits)) + data[i];      // lose lose
                }
                else if (config.Type == Types.Type7)
                {
                    crc = config.Table[(data[i] ^ (CrcSafeLimit(crc, config.Bits))) & 0xff] ^ (CrcSafeLimit(crc, config.Bits));
                }
                else if (config.Type == Types.Type8)
                {
                    crc = config.Table[(data[i] ^ (CrcSafeLimit(crc, config.Bits))) & 0xff] ^ ((CrcSafeLimit(crc, config.Bits)) >> (config.Bits - 8));
                }
                else if (config.Type == Types.Type9)
                {
                    crc = ((CrcSafeLimit(crc, config.Bits)) << 1) ^ data[i];
                }
                else if (config.Type == Types.Type10)
                {
                    crc = ((CrcSafeLimit(crc, config.Bits)) << 1) + data[i];
                }
                else if (config.Type == Types.Type11)
                {
                    crc = RotateLeft((CrcSafeLimit(crc, config.Bits)), 1) ^ data[i];
                }
                else if (config.Type == Types.Type12)
                {
                    crc = RotateLeft((CrcSafeLimit(crc, config.Bits)), 1) + data[i];
                }
                else if (config.Type == Types.Type13)
                {
                    crc = RotateRight((CrcSafeLimit(crc, config.Bits)), 1) ^ data[i];
                }
                else if (config.Type == Types.Type14)
                {
                    crc = RotateRight((CrcSafeLimit(crc, config.Bits)), 1) + data[i];
                }
                else if (config.Type == Types.Type15)
                {
                    crc = ((CrcSafeLimit(crc, config.Bits)) << 5) + (CrcSafeLimit(crc, config.Bits)) + data[i];   // djb2 5381
                }
                else if (config.Type == Types.Type16)
                {
                    crc = ((CrcSafeLimit(crc, config.Bits)) * config.Poly) + data[i]; // djb2 and sdbm
                }
                else if (config.Type == Types.Type17)
                {
                    crc = ((CrcSafeLimit(crc, config.Bits)) * config.Poly) ^ data[i]; // djb2 and FNV-1
                }
                else if (config.Type == Types.Type18)
                {
                    crc = ((CrcSafeLimit(crc, config.Bits)) ^ data[i]) * config.Poly; // FNV-1a
                }
                else if (config.Type == Types.Type19)
                {
                    crc = data[i] + ((CrcSafeLimit(crc, config.Bits)) << 6) + ((CrcSafeLimit(crc, config.Bits)) << 16) - (CrcSafeLimit(crc, config.Bits));   // sdbm 65599
                }
                else if (config.Type == Types.Type20)
                {
                    crc = config.Poly * ((CrcSafeLimit(crc, config.Bits)) + (ulong)data[i] * (ulong)(i + 1));
                }
                else
                {
                    throw new NotSupportedException();
                }
            }

            crc ^= config.Final;  // XorOut
            crc = (CrcSafeLimit(crc, config.Bits));

            return (crc);
        }
    }
}
