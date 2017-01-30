using System;
using System.Numerics;
using System.Security.Cryptography;

namespace EllipticCurveCryptography
{
    public class EllipticCurveAlgorithms
    {
        public MultiplyPoint Multiplier;
        public PointMultiplication.AddDelegate Adder;
        public BigInteger A;
        public BigInteger B;
        public BigInteger p;
        public BigInteger xP;
        public BigInteger yP;
        public BigInteger n;
        public BigInteger h;
        public int w;
        public HashAlgorithm HA;
        public Random rand;
        public double time;

        public EllipticCurveAlgorithms(BigInteger a, BigInteger b, BigInteger p,
            BigInteger xP, BigInteger yP, BigInteger n, BigInteger h, int w = 0, 
            MultiplyPoint multiplier = null, PointMultiplication.AddDelegate adder = null, HashAlgorithm ha = null)
        {
            if (b == 0)
            {
                throw new ArgumentOutOfRangeException("b can't be 0");
            }
            A = a;
            B = b;
            this.p = p;
            this.xP = xP;
            this.yP = yP;
            this.n = n;
            this.h = h;
            this.w = w;
            this.time = 0;

            if (multiplier != null)
                Multiplier = multiplier;
            else
                Multiplier = PointMultiplication.Point_Multiplication_Affine_Coord_1;
            Adder = adder ?? PointMultiplication.Add_Affine_Coord;
            rand = new Random();
            HA = ha ?? new SHA1CryptoServiceProvider();
        }

        public BigInteger GeneratePrivateKey(int BitSize)
        {
            BigInteger d = new BigInteger();
            do
            {
                d = Utils.RandomBigInteger(BitSize);
            } while ((d < 1) || (d > n));
            return d;
        }

        public Point GenerateKey(BigInteger d)
        {
            BigInteger x, y, z;
            double time = 0;
            Multiplier(xP, yP, 1, A, d, p, out x, out y, out z, 0, out time, w: w);
            return new Point(x, y);
        }

    }
}