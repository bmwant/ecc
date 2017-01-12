using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Methods
{
    public static class Utils
    {
        public static List<int> GenerateUniqueList(int num, int min, int max)
        {
            var random = new Random();
            return Enumerable.Range(min, max).OrderBy(x => random.Next()).Take(num).ToList();
        }

        public static BigInteger RandomBigInteger(int length)
        {
            byte[] bytes = new byte[length];
            Random random = new Random();
            BigInteger R;
            random.NextBytes(bytes);
            bytes[bytes.Length - 1] &= (byte)0x7F; //force sign bit to positive
            bytes[bytes.Length - 2] &= (byte)0x7F; // force next to be 1
            R = new BigInteger(bytes);


            return R;
        }

        public static BigInteger mod(BigInteger x, BigInteger y)
        {
            BigInteger res = x % y;
            if (res < 0)
            {
                res += y;
            }
            return res;
        }

        public static BigInteger modInverse(this BigInteger n, BigInteger p)
        {
            BigInteger x = 1;
            BigInteger y = 0;
            BigInteger a = p;
            BigInteger b = n;

            while (b != 0)
            {
                BigInteger t = b;
                BigInteger q = BigInteger.Divide(a, t);
                b = a - q * t;
                a = t;
                t = x;
                x = y - q * t;
                y = t;
            }

            if (y < 0)
                return y + p;
            //else
            return y;
        }
    }
}