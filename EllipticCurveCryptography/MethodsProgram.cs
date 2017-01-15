using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using EllipticCurveCryptography;

namespace Methods
{
    class Program
    {
        static void MethodsMain(string[] args)
        {
            var shor = new Shor(2, 6, 17, 2, 1, 11);
            var Ds = new List<BigInteger>() { 8, 5 };
            List<Point> PublicKeys = new List<Point>
            {
                shor.GenerateKey(8),
                shor.GenerateKey(5)
            };
            var data = Encoding.UTF8.GetBytes("TestTestTEst");
            var Ks = new List<BigInteger>() { 3, 4 };
            BigInteger r, s;
            shor.GroupSign(data, Ks, Ds, out r, out s);
            var expectedFalse = shor.VerifyGroupSign(data, PublicKeys, r, s - 1);
            var expectedTrue = shor.VerifyGroupSign(data, PublicKeys, r, s);
            Console.WriteLine("GOST test result : " + (expectedTrue && !expectedFalse));

        }
    }
}
