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
    class Ternary
    {
        public static void Ternary_Projective_Coord(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger p, out BigInteger x3, out BigInteger y3, out BigInteger z3)
        {
            BigInteger x2, y2, z2;

            PointMultiplication.Double_Projective_Coord(x1, y1, z1, a, p, out x2, out y2, out z2);
            PointMultiplication.Add_Projective_Coord(x1, y1, z1, x2, y2, z2, a, p, out x3, out y3, out z3);
        }

        public static void Ternary_Jacoby_Coord(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger p, out BigInteger x3, out BigInteger y3, out BigInteger z3)
        {
            BigInteger x2, y2, z2;

            PointMultiplication.Double_Jacoby_Coord(x1, y1, z1, a, p, out x2, out y2, out z2);
            PointMultiplication.Add_Jacoby_Coord(x1, y1, z1, x2, y2, z2, a, p, out x3, out y3, out z3);
        }

        public static void Ternary_Affine_Coord_1(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger p, out BigInteger x3, out BigInteger y3, out BigInteger z3)
        {
            BigInteger x2, y2, z2;

           PointMultiplication.Double_Affine_Coord(x1, y1, z1, a, p, out x2, out y2, out z2);
           PointMultiplication.Add_Affine_Coord(x1, y1, z1, x2, y2, z2, a, p, out x3, out y3, out z3);

        }

        public static void Ternary_Affine_Coord_2(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger p, out BigInteger x3, out BigInteger y3, out BigInteger z3)
        {
            BigInteger x_temp, z_temp, y_temp, d, d_, inv, d_temp, lambda_1, lambda_2;

            if (y1 == 0)
            {
                x3 = x1;
                y3 = y1;
                z3 = z1;
            }
            else
            {
                x_temp = (4 * y1 * y1) % p;
                z_temp = (3 * x1 * x1 + a) % p;
                y_temp = (3 * x1 * x1 + a) * (3 * x1 * x1 + a) % p;
                //d = (4 * y1 * y1 * 3 * x1 - (3 * x1 * x1 + a) * (3 * x1 * x1 + a)) % p;
                d = (x_temp * 3 * x1 - y_temp) % p; if (d < 0) d += p;


                if (d == 0)
                {
                    x3 = 0;
                    y3 = 1;
                    z3 = 0;
                }
                else
                {
                    d_temp = (d * 2 * y1) % p;
                   Functions.Extended_Euclid(p, d_temp, out d_, out inv);

                    lambda_1 = (d * inv * z_temp) % p;
                    lambda_2 = (x_temp * x_temp * inv - lambda_1) % p;

                    x3 = ((lambda_2 - lambda_1) * (lambda_1 + lambda_2) + x1) % p;
                    y3 = (lambda_2 * (x1 - x3) - y1) % p;
                    z3 = 1;
                }
            }

            if (x3 < 0) x3 += p;
            if (y3 < 0) y3 += p;

        }

        public static void Ternary_Affine_Coord_3(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger p, out BigInteger x3, out BigInteger y3, out BigInteger z3)
        {
            BigInteger d, inv1, inv2, lambda_1, lambda_2, temp, temp_1, temp_2;

            if (y1 == 0)
            {
                x3 = x1;
                y3 = y1;
                z3 = z1;
            }
            else
            {
               Functions.Extended_Euclid(p, 2 * y1 % p, out d, out inv1);
                temp_1 = (3 * x1 * x1 + a) % p;
                lambda_1 = (temp_1 * inv1) % p;
                temp_2 = (temp_1 * temp_1 - 12 * x1 * y1 * y1) % p; if (temp_2 < 0) temp_2 += p;

                if (temp_2 == 0)
                {
                    x3 = 0;
                    y3 = 0;
                    z3 = 0;
                }
                else
                {
                   Functions.Extended_Euclid(p, temp_2, out d, out inv2);
                    temp = (y1 * y1 * y1) % p;
                    lambda_2 = (-lambda_1 - 8 * temp * inv2) % p;
                    temp_1 = ((lambda_2 - lambda_1) * (lambda_2 + lambda_1)) % p;

                    x3 = (temp_1 + x1) % p;
                    y3 = (-lambda_2 * temp_1 - y1) % p;
                    z3 = 1;
                }
            }
            if (x3 < 0) x3 += p;
            if (y3 < 0) y3 += p;

        }

        public static void Ternary_Affine_Coord(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger p, out BigInteger x3, out BigInteger y3, out BigInteger z3)
        {
            if (y1 == 0 || z1 == 0)
            {
                x3 = x1;
                y3 = y1;
                z3 = z1;
            }
            else
            {
                BigInteger x_temp = Functions.Pow(2 * y1, 2) % p;
                BigInteger z_temp = (3 * Functions.Pow(x1, 2) + a) % p;
                BigInteger y_temp = Functions.Pow(z_temp, 2) % p;
                BigInteger d;
                if ((x_temp * 3 * x1 - y_temp) % p < 0)
                {
                    d = (x_temp * 3 * x1 - y_temp) % p + p;
                }
                else d = (x_temp * 3 * x1 - y_temp) % p;
                if (d == 0)
                {
                    x3 = 0;
                    y3 = 1;
                    z3 = 0;
                }
                else
                {
                    BigInteger d_temp = (d * 2 * y1) % p;
                    BigInteger garb, inv;
                    Functions.Extended_Euclid(p, d_temp, out garb, out inv);
                    BigInteger lambda_1 = (d * inv * z_temp) % p;
                    BigInteger lambda_2 = (Functions.Pow(x_temp, 2) * inv - lambda_1) % p;
                    if (((lambda_2 - lambda_1) * (lambda_1 + lambda_2) + x1) < 0)
                    {
                        x3 = ((lambda_2 - lambda_1) * (lambda_1 + lambda_2) + x1) % p + p;
                    }
                    else
                    {
                        x3 = ((lambda_2 - lambda_1) * (lambda_1 + lambda_2) + x1) % p;
                    }
                    if (lambda_2 * (x1 - x3) - y1 < 0)
                    {
                        y3 = (lambda_2 * (x1 - x3) - y1) % p + p;
                    }
                    else y3 = (lambda_2 * (x1 - x3) - y1) % p;
                    z3 = 1;

                    if (y3 == 0)
                    {
                        x3 = 0;
                        y3 = 1;
                        z3 = 0;
                    }
                }

            }
        }
    
    }
}
