using System.Security.Cryptography;
using System.Text;

namespace HC_256
{
    public class Program
    {
        private static void InitializationProcess(byte[] key, byte[] IV, ref uint[] P, ref uint[] Q)
        {
            uint[] W = new uint[2560];

            uint[] keyParts = new uint[8];
            for (int i = 0; i < 8; i++)
            {
                keyParts[i] = BitConverter.ToUInt32(key, i * 4);
            }

            uint[] IVParts = new uint[8];
            for (int i = 0; i < 8; i++)
            {
                IVParts[i] = BitConverter.ToUInt32(IV, i * 4);
            }

            for (int i = 0; i <= 2559; i++)
            {
                if (i >= 0 && i <= 7)
                {
                    W[i] = keyParts[i];
                }

                if (i >= 8 && i <= 15)
                {
                    W[i] = IVParts[i - 8];
                }

                if (i >= 16)
                {
                    uint f2 = Functions.F2(W[i - 2]);
                    uint f1 = Functions.F1(W[i - 15]);

                    uint temp1 = Operations.Plus(f2, W[i - 7]);
                    uint temp2 = Operations.Plus(temp1, f1);
                    uint temp3 = Operations.Plus(temp2, W[i - 16]);

                    W[i] = Operations.Plus(temp3, (uint)i);
                }
            }

            for (int i = 0; i <= 1023; i++)
            {
                P[i] = W[i + 512];
                Q[i] = W[i + 1536];
            }
        }

        private static uint[] GenerateKeystream(int n, ref uint[] P, ref uint[] Q)
        {
            uint[] s = new uint[n];
            int tempN = n;
            for (int k = 1; k <= 4096; k++)
            {
                int i = 0;
                while (tempN != 0)
                {
                    int j = i % 1024;
                    if (i % 2048 < 1024)
                    {
                        uint temp1 = Operations.Plus(P[j], P[Operations.Minus((uint)j, 10)]);
                        uint temp2 = Operations.Plus(temp1, Functions.G1(P[Operations.Minus((uint)j, 3)], P[Operations.Minus((uint)j, 1023)], Q));
                        P[j] = temp2;
                        s[i] = Operations.XOR(Functions.H1(P[Operations.Minus((uint)j, 12)], Q), P[j]);
                    }
                    else
                    {
                        uint temp1 = Operations.Plus(Q[j], Q[Operations.Minus((uint)j, 10)]);
                        uint temp2 = Operations.Plus(temp1, Functions.G2(Q[Operations.Minus((uint)j, 3)], Q[Operations.Minus((uint)j, 1023)], P));
                        Q[j] = temp2;
                        s[i] = Operations.XOR(Functions.H2(Q[Operations.Minus((uint)j, 12)], P), Q[j]);
                    }
                    tempN--;
                    i++;
                }
            }
            return s;
        }

        public static byte[] Encrypt(byte[] message, byte[] key, byte[] IV)
        {
            uint[] P = new uint[1024];
            uint[] Q = new uint[1024];

            InitializationProcess(key, IV, ref P, ref Q);

            uint[] keystream = GenerateKeystream(message.Length, ref P, ref Q);

            byte[] ciphertext = new byte[message.Length];
            for (int i = 0; i < message.Length; i++)
            {
                ciphertext[i] = (byte)(message[i] ^ (byte)keystream[i]);
            }

            return ciphertext;
        }

        static void Main(string[] args)
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] key = new byte[32];
            byte[] IV = new byte[32];
            rng.GetBytes(key);
            rng = RandomNumberGenerator.Create();
            rng.GetBytes(IV);

            string plaintext = "Hello, Bob!";
            byte[] cipertext = Encrypt(Encoding.UTF8.GetBytes(plaintext), key, IV);
            byte[] decripted = Encrypt(cipertext, key, IV);

            Console.WriteLine($"Plaintext: {plaintext}");
            Console.WriteLine($"Cipertext: {Encoding.UTF8.GetString(cipertext)}");
            Console.WriteLine($"Decripted: {Encoding.UTF8.GetString(decripted)}");


            //hexadecimals
            Console.WriteLine("Using hexadecimal");

            key = new byte[] { 0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0xaa, 0xbb, 0xcc, 0xdd, 0xee, 0xff,
                          0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0xaa, 0xbb, 0xcc, 0xdd, 0xee, 0xff };
            IV = new byte[] { 0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0xaa, 0xbb, 0xcc, 0xdd, 0xee, 0xff,
                         0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0xaa, 0xbb, 0xcc, 0xdd, 0xee, 0xff };

            byte[] plaintextHex = new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x2C, 0x20, 0x42, 0x6F, 0x62, 0x21};

            cipertext = Encrypt(plaintextHex, key, IV);
            decripted = Encrypt(cipertext, key, IV);

            Console.WriteLine($"Plaintext: {Encoding.UTF8.GetString(plaintextHex)}");
            Console.WriteLine($"Cipertext: {Encoding.UTF8.GetString(cipertext)}");
            Console.WriteLine($"Decripted: {Encoding.UTF8.GetString(decripted)}");
        }
    }
}