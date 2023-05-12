namespace HC_256
{
    public static class Functions
    {
        public static uint F1(uint x)
        {
            uint r7 = Operations.RotateRight(x, 7);
            uint r18 = Operations.RotateRight(x, 18);
            uint sr3 = Operations.ShiftRight(x, 3);

            uint temp = Operations.XOR(r7, r18);
            return Operations.XOR(temp, sr3);
        }

        public static uint F2(uint x)
        {
            uint r17 = Operations.RotateRight(x, 17);
            uint r19 = Operations.RotateRight(x, 19);
            uint sr10 = Operations.ShiftRight(x, 10);

            uint temp = Operations.XOR(r17, r19);
            return Operations.XOR(temp, sr10);
        }

        public static uint G1(uint x, uint y, uint[] Q) 
        {
            uint xr10 = Operations.RotateRight(x, 10);
            uint yr23 = Operations.RotateRight(y, 23);
            uint xXORy = Operations.XOR(x, y);
            uint element = Q[xXORy % 1024];

            uint temp = Operations.XOR(xr10, yr23);
            return Operations.Plus(temp, element);
        }

        public static uint G2(uint x, uint y, uint[] P)
        {
            uint xr10 = Operations.RotateRight(x, 10);
            uint yr23 = Operations.RotateRight(y, 23);
            uint xXORy = Operations.XOR(x, y);
            uint element = P[xXORy % 1024];

            uint temp = Operations.XOR(xr10, yr23);
            return Operations.Plus(temp, element);
        }

        public static uint H1(uint x, uint[] Q)
        {
            byte x0 = (byte)x;
            byte x1 = (byte)(x >> 8);
            byte x2 = (byte)(x >> 16);
            byte x3 = (byte)(x >> 24);

            uint temp1 = Operations.Plus(Q[x0], Q[256 + x1]);
            uint temp2 = Operations.Plus(temp1, Q[512 + x2]);
            return Operations.Plus(temp2, Q[768 + x3]);
        }

        public static uint H2(uint x, uint[] P)
        {
            byte x0 = (byte)x;
            byte x1 = (byte)(x >> 8);
            byte x2 = (byte)(x >> 16);
            byte x3 = (byte)(x >> 24);

            uint temp1 = Operations.Plus(P[x0], P[256 + x1]);
            uint temp2 = Operations.Plus(temp1, P[512 + x2]);
            return Operations.Plus(temp2, P[768 + x3]);
        }
    }
}
