using System.Numerics;
using System.Security.Cryptography;

namespace EllipticCurveCryptography
{
    public class ECDSA : EllipticCurveAlgorithms
    {
        public ECDSA(BigInteger a, BigInteger b, BigInteger p, BigInteger xP, 
            BigInteger yP, BigInteger n, MultiplyPoint multiplier = null, PointMultiplication.AddDelegate adder = null, HashAlgorithm ha = null)
            : base(a, b, p, xP, yP, n, 1, multiplier, adder, ha)
        {
        }

        public void Sign(byte[] data, BigInteger d, out BigInteger r, out BigInteger s)
        {
            while (true)
            {
                BigInteger x, y, z;
                int k = rand.Next(1, (int)n);
                Multiplier(xP, yP, 1, A, k, p, out x, out y, out z, 0);
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
            //add counter and timer
            Multiplier(xP, yP, 1, A, u1, p, out x, out y, out z, 0);
            Point u1P = new Point(x, y);
            //add counter and timer
            Multiplier(publicKey.X, publicKey.Y, 1, A, u2, p, out x, out y, out z, 0);
            Point u2Q = new Point(x, y);
            Adder(u1P.X, u1P.Y, 1,
                u2Q.X, u2Q.Y, 1, A, p, out xX, out yX, out zX);
            BigInteger v = Utils.mod(xX, n);
            return v == r;
        }
    }
}