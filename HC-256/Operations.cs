using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC_256
{
    public static class Operations
    {
        public static uint Plus(uint x, uint y)
        {
            if (x >= Math.Pow(2, 32) || y >= Math.Pow(2, 32)) throw new ArgumentException("Plus");
            if (x < 0 || y < 0) throw new ArgumentException("");
            return (uint)((x + y) % Math.Pow(2, 32));
        }

        public static uint Minus(uint x, uint y)
        {
            if (x >= Math.Pow(2, 32) || y >= Math.Pow(2, 32)) throw new ArgumentException("Minus");
            if (x < 0 || y < 0) throw new ArgumentException("");
            return (uint)((x - y) % Math.Pow(2, 10));
        }

        public static uint XOR(uint x, uint y)
        {
            if (x >= Math.Pow(2, 32) || y >= Math.Pow(2, 32)) throw new ArgumentException("XOR");
            if (x < 0 || y < 0) throw new ArgumentException("XOR");
            return x ^ y;
        }

        public static uint ShiftRight(uint x, int n)
        {
            if (x >= Math.Pow(2, 32) || x < 0) throw new ArgumentException("ShiftRight");
            return x >> n;
        }

        public static uint ShiftLeft(uint x, int n)
        {
            if (x >= Math.Pow(2, 32) || x < 0) throw new ArgumentException("ShiftLeft");
            return x >> n;
        }

        public static uint RotateRight(uint x, int n)
        {
            if (x >= Math.Pow(2, 32) || x < 0) throw new ArgumentException("RotateRight - x");
            if (n < 0 || n >= 32) throw new ArgumentException("RotateRight - n");
            return XOR(ShiftRight(x, n), ShiftLeft(x, 32 - n));
        }
    }
}
