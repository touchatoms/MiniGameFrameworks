public static class SwapXOR
{
    public static byte[] Key =
    {
        0xe4, 0x86, 0xa6, 0xe4, 0x9d, 0xb5, 0xec, 0x83, 0xb4, 0xe9, 0xac, 0x87, 0xee, 0x95, 0xa7, 0xe3, 0xaf,
        0xb5, 0xe7, 0x8c, 0x92, 0xe2, 0xb5, 0x8d, 0xed, 0x9a, 0x83, 0xed, 0x8a, 0xa6, 0xe4, 0x81, 0x97, 0xe5,
        0xa1, 0xa9, 0xe8, 0x8b, 0x95, 0xe3, 0xbd, 0x9a, 0xe8, 0x99, 0x98, 0xee, 0xb5, 0xbb, 0xed, 0x8f, 0xa4,
        0xe5, 0x9e, 0xab, 0xe7, 0x9b, 0x90, 0xe1, 0xad, 0x98
    };

    public static void Encrypt(byte[] input, bool encrypt)
    {
        if (encrypt)
        {
            Encrypt(input, 0, input.Length);
        }
        else
        {
            Decrypt(input, 0, input.Length);
        }
    }

    private static void Encrypt(byte[] input, int offset, int length)
    {
        int ki = 0;
        int kl = Key.Length;
        for (int i = offset; i < offset + length; i++)
        {
            byte kb = Key[ki];
            input[i] = (byte)(input[i] ^ kb);
            if (ki == kl - 1)
            {
                (input[i], input[i - 1]) = (input[i - 1], input[i]);
                ki = 0;
            }
            else
            {
                ki++;
            }
        }
    }

    private static void Decrypt(byte[] input, int offset, int length)
    {
        int ki = 0;
        int kl = Key.Length;
        for (int i = offset; i < offset + length; i++)
        {
            byte kb = Key[ki];
            input[i] = (byte)(input[i] ^ kb);
            if (ki == kl - 1)
            {
                (input[i], input[i - 1]) = (input[i - 1], input[i]);

                input[i] = (byte)(input[i] ^ kb ^ Key[ki - 1]);
                input[i - 1] = (byte)(input[i - 1] ^ kb ^ Key[ki - 1]);

                ki = 0;
            }
            else
            {
                ki++;
            }
        }
    }
}