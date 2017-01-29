using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using ECC.EllipticCurveCryptography;


namespace EllipticCurveCryptography
{
    public class Shor : EllipticCurveAlgorithms
    {
        OperationsCounter ops;
        public Shor(BigInteger a, BigInteger b, BigInteger p, BigInteger xP, BigInteger yP, BigInteger n,
            MultiplyPoint multiplier = null, PointMultiplication.AddDelegate adder = null, HashAlgorithm ha = null, OperationsCounter ops = null)
            : base(a, b, p, xP, yP, n, 1, multiplier, adder, ha)
        {
            this.ops = ops;
        }

        public new BigInteger GeneratePrivateKey(int BitSize)
        {
            BigInteger d = new BigInteger();
            do
            {
                d = Utils.RandomBigInteger(BitSize);
            } while ((d < 1) || (d > n));
            return d;
        }

        public new Point GenerateKey(BigInteger d)
        {
            BigInteger x, y, z;
            double time = 0;
            Multiplier(xP, yP, 1, A, d, p, out x, out y, out z, 0, out time, ops: ops);
            return new Point(x, y);
        }

        public void GroupSign(byte[] data, List<BigInteger> k, List<BigInteger> d, out BigInteger r, out BigInteger sign)
        {
            ops.opElementsAdd(47);
            while (true)
            {
                if (k.Count != d.Count)
                {
                    throw new ArgumentOutOfRangeException("You should pass equal amount of k, d and public keys");
                }
                sign = 0;
                List<Point> RList = new List<Point>();
                for (int i = 0; i < k.Count; i++)
                {
                    BigInteger x, y, z;
                    double iterationTime = 0;
                    Multiplier(xP, yP, 1, A, k[i], p, out x, out y, out z, 0, out iterationTime, ops: ops);
                    RList.Add(new Point(x, y));
                }
                BigInteger xR, yR, zR;
                xR = RList[0].X;
                yR = RList[0].Y;
                if (RList.Count > 1)
                {
                    for (int i = 1; i < RList.Count; i++)
                    {
                        Adder(xR, yR, 1, RList[i].X, RList[i].Y, 1, A, p, out xR, out yR, out zR);
                    }
                }
                BigInteger h = new BigInteger(HA.ComputeHash((xR | new BigInteger(data)).ToByteArray()));
                r = Utils.mod(h, n);
                if (r == 0)
                {
                    throw new Exception("R can't be 0. Use another list of k");
                }

                for (int i = 0; i < d.Count; i++)
                {
                    sign += Utils.mod(k[i] - d[i] * r, n);
                }
                sign = Utils.mod(sign, n);
                if (sign == 0)
                {
                    continue;
                }
                break;
            }
        }

        public bool VerifyGroupSign(byte[] data, List<Point> PublicKeys, BigInteger r, BigInteger s)
        {
            BigInteger xQ, yQ, zQ;
            xQ = PublicKeys[0].X;
            yQ = PublicKeys[0].Y;
            if (PublicKeys.Count > 1)
            {
                for (int i = 1; i < PublicKeys.Count; i++)
                {
                    Adder(xQ, yQ, 1, PublicKeys[i].X, PublicKeys[i].Y, 1, A, p, out xQ, out yQ, out zQ);
                }
            }
            BigInteger x, y, z;
            BigInteger xR, yR, zR;
            double time1 = 0;
            Multiplier(xP, yP, 1, A, s, p, out x, out y, out z, 0, out time1, ops: ops);
            Point sP = new Point(x, y);
            double time2 = 0;
            Multiplier(xQ, yQ, 1, A, r, p, out x, out y, out z, 0, out time2, ops: ops);
            Point rQ = new Point(x, y);
            Adder(sP.X, sP.Y, 1,
                rQ.X, rQ.Y, 1, A, p, out xR, out yR, out zR);
            BigInteger h = new BigInteger(HA.ComputeHash((xR | new BigInteger(data)).ToByteArray()));
            BigInteger r1 = Utils.mod(h, n);
            return r == r1;
        }
    }
}