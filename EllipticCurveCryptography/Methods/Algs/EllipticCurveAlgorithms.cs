using System;
using System.Numerics;
using System.Security.Cryptography;
using EllipticCurveCryptography;

namespace Methods
{
    public class EllipticCurveAlgorithms
    {
        protected MultiplyPoint Multiplier;
        protected PointMultiplication.AddDelegate Adder;
        protected BigInteger A;
        protected BigInteger B;
        protected BigInteger p;
        protected BigInteger xP;
        protected BigInteger yP;
        protected int n;
        protected int h;
        protected HashAlgorithm HA;
        protected Random rand;

        public EllipticCurveAlgorithms(BigInteger a, BigInteger b, BigInteger p,
            BigInteger xP, BigInteger yP, int n, int h, MultiplyPoint multiplier = null, PointMultiplication.AddDelegate adder = null, HashAlgorithm ha = null)
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
            Multiplier = multiplier ?? PointMultiplication.Point_Multiplication_Affine_Coord_1;
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
            //add counter and timer
            Multiplier(xP, yP, 1, A, d, p, out x, out y, out z, 0);
            return new Point(x, y);
        }

    }
}