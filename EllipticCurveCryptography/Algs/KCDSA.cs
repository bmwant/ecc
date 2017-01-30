using System.Numerics;
using System.Security.Cryptography;
using ECC.EllipticCurveCryptography;


namespace EllipticCurveCryptography
{
    public class KCDSA : EllipticCurveAlgorithms
    {
        OperationsCounter ops;

        public KCDSA(BigInteger a, BigInteger b, BigInteger p, BigInteger xP, BigInteger yP, BigInteger n, ref OperationsCounter ops,
            MultiplyPoint multiplier = null, PointMultiplication.AddDelegate adder = null, HashAlgorithm ha = null)
            : base(a, b, p, xP, yP, n, 1, multiplier: multiplier, adder: adder, ha: ha)
        {
            this.ops = ops;
        }

        public void Sign(byte[] data, byte[] hcert, BigInteger d, out BigInteger r, out BigInteger s)
        {
            hcert = HA.ComputeHash(hcert);
            while (true)
            {
                BigInteger x, y, z;
                int k = rand.Next(1, (int)n);
                double iterationTime = 0;
                Multiplier(xP, yP, 1, A, k, p, out x, out y, out z, 0, out iterationTime, ops: ops);
                r = new BigInteger(HA.ComputeHash((x|y).ToByteArray()));
                var da = new BigInteger(data);
                var hc = new BigInteger(hcert);
                BigInteger e = new BigInteger(HA.ComputeHash((hc | da).ToByteArray()));
                BigInteger w = r ^ e;
                w = Utils.mod(w, n);
                s = Utils.mod(d * (k - w), n);
                if (s ==0 )
                {
                    continue;
                }
                break;
            }
        }

        public bool Verify(byte[] data, byte[] hcert, Point publicKey, BigInteger r, BigInteger s)
        {
            hcert = HA.ComputeHash(hcert);
            if (s < 1 || s > n - 1)
            {
                return false;
            }
            var da = new BigInteger(data);
            var hc = new BigInteger(hcert);
            BigInteger e = new BigInteger(HA.ComputeHash((hc | da).ToByteArray()));
            BigInteger w = r ^ e;
            w = Utils.mod(w, n);
            BigInteger x, y, z;
            double time1 = 0;
            Multiplier(xP, yP, 1, A, w, p, out x, out y, out z, 0, out time1, ops: ops);
            Point wP = new Point(x, y);
            double time2 = 0;
            Multiplier(publicKey.X, publicKey.Y, 1, A, s, p, out x, out y, out z, 0, out time2, ops: ops);
            Point sQ = new Point(x, y);
            Adder(sQ.X, sQ.Y, 1,
                wP.X, wP.Y, 1, A, p, out x, out y, out z);
            BigInteger v = new BigInteger(HA.ComputeHash((x | y).ToByteArray()));
            return v == r;
        }
    }
}