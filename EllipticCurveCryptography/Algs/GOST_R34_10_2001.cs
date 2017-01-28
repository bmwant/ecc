using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using ECC.EllipticCurveCryptography;

namespace EllipticCurveCryptography
{
    public delegate void MultiplyPoint(
        BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
        out BigInteger x2, out BigInteger y2, out BigInteger z2, int type, out double time, int w = 0, OperationsCounter ops = null);
    public class GOST_R34_10_2001 : EllipticCurveAlgorithms
    {
        private BigInteger Sigma;
        public GOST_R34_10_2001(BigInteger a, BigInteger b, BigInteger p, BigInteger xP, BigInteger yP, BigInteger Sigma, int n,
            MultiplyPoint multiplier = null, PointMultiplication.AddDelegate adder = null, HashAlgorithm ha = null)
            : base(a, b, p, xP, yP, n, 1, multiplier, adder,ha)
        {
            this.Sigma = Sigma;
        }

        public void GroupSign(byte[] data, List<BigInteger> k, List<BigInteger> d, out BigInteger r, out BigInteger sign)
        {
            byte[] hash = HA.ComputeHash(data);
            while (true)
            {
                if (k.Count != d.Count)
                {
                    throw new ArgumentOutOfRangeException("You should pass equal amount of k and d");
                }
                sign = 0;
                BigInteger h = new BigInteger(hash);
                List<Point> RList = new List<Point>();
                for (int i = 0; i < k.Count; i++)
                {
                    BigInteger x, y, z;
                    double iterationTime = 0;
                    Multiplier(xP, yP, 1, A, k[i], p, out x, out y, out z, 0, out iterationTime);
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
                r = Utils.mod(xR * h, Sigma);
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
            byte[] hash = HA.ComputeHash(data);
            BigInteger xQ, yQ, zQ;
            BigInteger h = new BigInteger(hash);
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
            Multiplier(xP, yP, 1, A, s, p, out x, out y, out z, 0, out time1);
            Point sP = new Point(x,y);
            double time2 = 0;
            Multiplier(xQ, yQ, 1, A, r, p, out x, out y, out z, 0, out time2);
            Point rQ = new Point(x, y);
            Adder(sP.X, sP.Y, 1,
                rQ.X, rQ.Y, 1, A, p, out xR, out yR, out zR);
            BigInteger r1 = Utils.mod(h * xR, Sigma);
            return r == r1;
        }
    }
}