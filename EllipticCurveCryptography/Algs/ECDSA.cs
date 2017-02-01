using System.Numerics;
using System.Security.Cryptography;
using ECC.EllipticCurveCryptography;


namespace EllipticCurveCryptography
{
    public class ECDSA : EllipticCurveAlgorithms
    {
        OperationsCounter ops;

        public ECDSA(BigInteger a, BigInteger b, BigInteger p, BigInteger xP, BigInteger yP, BigInteger n, int w = 0,
            MultiplyPoint multiplier = null, PointMultiplication.AddDelegate adder = null, HashAlgorithm ha = null, OperationsCounter ops = null)
            : base(a, b, p, xP, yP, n, 1, w: w, multiplier: multiplier, adder: adder, ha: ha)
        {
            this.ops = ops;
        }

        public void Sign(byte[] data, BigInteger d, out BigInteger r, out BigInteger s)
        {
            while (true)
            {
                BigInteger x, y, z;
                int k = rand.Next(1, (int)n);
                double iterationTime = 0;
                Multiplier(xP, yP, 1, A, k, p, out x, out y, out z, 0, out iterationTime, w: w, ops: ops);
                this.time += iterationTime;
                r = Utils.mod(x, n);
                if (r == 0)
                {
                    continue;
                }
                BigInteger e = new BigInteger(HA.ComputeHash(data));
                BigInteger invK = Utils.modInverse(k, n);
                s = Utils.mod((e + d*r)*invK, n);
                if (s ==0 )
                {
                    continue;
                }
                break;
            }
        }

        public bool Verify(byte[] data, Point publicKey, BigInteger r, BigInteger s)
        {
            if (r < 1 || r > n - 1 || s < 1 || s > n - 1)
            {
                return false;
            }
            BigInteger e = new BigInteger(HA.ComputeHash(data));
            var w = s.modInverse(n);
            BigInteger u1 = Utils.mod(e*w, n);
            BigInteger u2 = Utils.mod(r * w, n);
            BigInteger x, y, z;
            BigInteger xX, yX, zX;
            double time1 = 0;
            Multiplier(xP, yP, 1, A, u1, p, out x, out y, out z, 0, out time1, w: this.w, ops: ops);
            this.time += time1;
            Point u1P = new Point(x, y);

            double time2 = 0;
            Multiplier(publicKey.X, publicKey.Y, 1, A, u2, p, out x, out y, out z, 0, out time2, w: this.w, ops: ops);
            this.time += time2;
            Point u2Q = new Point(x, y);
            Adder(u1P.X, u1P.Y, 1,
                u2Q.X, u2Q.Y, 1, A, p, out xX, out yX, out zX);
            BigInteger v = Utils.mod(xX, n);
            return v == r;
        }
    }
}