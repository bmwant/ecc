using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Numerics;
using System.IO;
using System.Diagnostics;

namespace EllipticCurveCryptography
{
    class Quadrupling
    {
        public static void Quadrupling_Affine_Coord_1(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger p,
                   out BigInteger x4, out BigInteger y4, out BigInteger z4)
        {
            BigInteger x2, y2, z2;


            PointMultiplication.Double_Affine_Coord(x1, y1, z1, a, p, out x2, out y2, out z2);
           PointMultiplication.Double_Affine_Coord(x2, y2, z2, a, p, out x4, out y4, out z4);

        }

        public static void Quadrupling_Affine_Coord_2(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger parametr_a, BigInteger p,
                    out BigInteger x4, out BigInteger y4, out BigInteger z4)
        {

            BigInteger d = 0, d_, inv, t1, t2, t3, t5, t6, t7, t8;

            if (x1 == 0)
            {

                x4 = 0;
                y4 = 1;
                z4 = 0;
            }
            else
            {

                t1 = x1;
                t2 = (3 * x1 * x1 + parametr_a) % p;
                t3 = y1;
                if (t2 < 0) t2 += p;                
                t5 = (t2 * t2 - 8 * t1 * t3 * t3) % p;
                if (t5 < 0) t5 += p;

                
                t6 = (3 * t5 * t5 + 16 * parametr_a * BigInteger.Pow(t3, 4)) % p;
                if (t6 < 0) t6 += p;
                
                t7 = (t2 * (4 * t1 * t3 * t3 - t5) - 8 * BigInteger.Pow(t3, 4)) % p;
                if (t7 < 0) t7 += p;
                
                t8 = (12 * t5 * t7 * t7 - t6 * t6) % p;
                if (t8 < 0) t8 += p;

                if (t3 == 0 || t7 == 0)
                {
                    x4 = 0;
                    y4 = 1;
                    z4 = 0;
                }
                else
                {
                    Functions.Extended_Euclid(p, 4 * t3 * t7 % p, out d_, out inv);
                                       
                    x4 = ((t6 * t6 - 8 * t5 * t7 * t7) * inv * inv) % p;                                       
                    y4 = ((t6 * t8 - 8 * t7 * t7 * t7 * t7) * inv * inv * inv) % p;
                    z4 = 1;
                }

            }
            if (x4 < 0) x4 += p;
            if (y4 < 0) y4 += p;
        }



        public static void Quadrupling_Affine_Coord_3(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger parametr_a, BigInteger p,
                    out BigInteger x4, out BigInteger y4, out BigInteger z4)
        {
            BigInteger d = 0, d_, inv, lambda_1, lambda_2, t1 = 0, t2 = 0, t3 = 0, t4 = 0, t5 = 0, t6 = 0, t7 = 0, x3, y3;

            if (y1 == 0)
            {
                x4 = x1;
                y4 = y1;
                z4 = z1;
            }
            else
            {
                t1 = BigInteger.ModPow(x1, 2, p);
                t2 = (3 * x1 * x1 + parametr_a) % p;
                t3 = (2 * y1 * y1) % p;
                t4 = (t3 * t3) % p;
                t5 = ((x1 + t3) * (x1 + t3) - t1 - t4) % p;
                d = (t2 * (3 * t5 - t2 * t2) - 2 * t4) % p;
            }

            if (d == 0)
            {
                x4 = 0;
                y4 = 1;
                z4 = 0;
            }
            else
            {
                t6 = (2 * y1 * d) % p;
               Functions.Extended_Euclid(p, t6, out d_, out inv);

                lambda_1 = (d * inv * t2) % p;
                x3 = (lambda_1 * lambda_1 - 2 * x1) % p;
                y3 = (lambda_1 * (x1 - x3) - y1) % p;
                t7 = (3 * x3 * x3 + parametr_a) % p;
                lambda_2 = (2 * t4 * inv * t7) % p;

                x4 = (lambda_2 * lambda_2 - 2 * x3) % p;
                y4 = (lambda_2 * (x3 - x4) - y3) % p;
                z4 = 1;
            }

            if (x4 < 0) x4 += p;
            if (y4 < 0) y4 += p;
        }


    }
}
