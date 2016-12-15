using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class CRC4
{
    private byte[] tab_crc4 = new byte[]
        {
            0,
            3,
            6,
            5,
            12,
            15,
            10,
            9,
            11,
            8,
            13,
            14,
            7,
            4,
            1,
            2
        };

    public static byte Crc4(this byte[] buffer)
    {
        byte k = 0, crc = 0;

        for (k = 0; buf[k] != null; k++)
        {
            crc ^= buf[k] >> 4;
            crc = tab_crc4[crc];
            crc ^= buf[k] & 0xF;
            crc = tab_crc4[crc];
        }

        return crc;
    }
}