using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace EllipticCurveCryptography
{
    class Lehmer_
    {
        public static void Lehmer_long(BigInteger a, BigInteger b, out BigInteger d, out BigInteger inv)
        {
            int s = 10, k = 4, p = 8;

            BigInteger temp_a = a;
            BigInteger temp_b = b;
            BigInteger u, v, y;

            BigInteger pow = BigInteger.Pow(s, p - k);

            v = b / pow;

            if (v * pow > b)
                v--;

            BigInteger ka = 1, kc = 0;
            BigInteger A, B, C, D, t1, t2, q1, q2;

            while (v != 0)
            {
                u = a / pow;
                if (u * pow > a)
                    u--;
                A = 1; B = 0; C = 0; D = 1;
                
                t1 = v + C;
                if (t1 != 0)
                {
                    t2 = v + D;
                    if (t2 != 0)
                    {
                        q1 = (u + A) / t1;
                        if (q1 * t1 > (u + A)) q1--;
                        q2 = (u + B) / t2;
                        if (q2 * t2 > (u + B)) q2--;

                        while (q1 == q2)
                        {
                            t1 = A - q1 * C;
                            A = C;
                            C = t1;

                            t1 = B - q1 * D;
                            B = D;
                            D = t1;

                            t1 = u - q1 * v;
                            u = v;
                            v = t1;

                            t1 = v + C;

                            if (t1 != 0)
                            {
                                t2 = v + D;
                                if (t2 != 0)
                                {
                                    q1 = (u + A) / t1;
                                    if (q1 * t1 > (u + A)) q1--;
                                    q2 = (u + B) / t2;
                                    if (q2 * t2 > (u + B)) q2--;
                                }
                                else
                                {
                                    q1 = 1;
                                    q2 = 0;
                                }
                            }
                            else
                            {
                                q1 = 1;
                                q2 = 0;
                            }
                        }

                    }

                }
                if (B == 0)
                {
                    q1 = a / b;
                    if (q1 * b > a) q1--;
                    t1 = a % b;
                    a = b;
                    b = t1;
                    t1 = ka - q1 * kc;
                    ka = kc;
                    kc = t1;
                }
                else
                {
                    t1 = A * a + B * b;
                    t2 = C * a + D * b;
                    a = t1;
                    b = t2;

                    t1 = A * ka + B * kc;
                    t2 = C * ka + D * kc;
                    ka = t1;
                    kc = t2;
                }

                v = b / pow;
                if (v * pow > b) v--;
            }
            while (b != 0)
            {
                q1 = a / b;
                if (q1 * b > a) q1--;

                t1 = a - q1 * b;
                t2 = ka - q1 * kc;

                a = b;
                ka = kc;

                b = t1;
                kc = t2;
            }
            d = a;
            y = (a - ka * temp_a) / temp_b;

            if (y > 0) inv = y;
            else
                inv = y + temp_a;
        }
    }
}
