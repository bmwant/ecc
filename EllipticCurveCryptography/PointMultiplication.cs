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
using System.Threading;
using System.Diagnostics;
using ECC.EllipticCurveCryptography;

namespace EllipticCurveCryptography
{
    public static class PointMultiplication
    {
        public delegate void AddDelegate(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger x2, BigInteger y2, BigInteger z2, BigInteger a, BigInteger p,
            out BigInteger x3, out BigInteger y3, out BigInteger z3);
        public delegate void DoubleDelegate(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger p,
            out BigInteger x3, out BigInteger y3, out BigInteger z3);
        public delegate void TernaryDelegate(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger p,
            out BigInteger x3, out BigInteger y3, out BigInteger z3);
        public delegate void QuintupleDelegate(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger p,
            out BigInteger x3, out BigInteger y3, out BigInteger z3);
        public static List<DoubleDelegate> DoubleList;
        public static List<AddDelegate> AddList;
        public static List<TernaryDelegate> TernaryList;
        public static List<QuintupleDelegate> QuintupleList;

        static PointMultiplication()
        {
            DoubleList = new List<DoubleDelegate>();
            DoubleList.Add(Double_Affine_Coord);
            DoubleList.Add(Double_Projective_Coord);
            DoubleList.Add(Double_Jacoby_Coord);
            DoubleList.Add(Double_Jacoby_Quartic);
            AddList = new List<AddDelegate>();
            AddList.Add(Add_Affine_Coord);
            AddList.Add(Add_Projective_Coord);
            AddList.Add(Add_Jacoby_Coord);
            AddList.Add(Add_Jacoby_Quartic);
            TernaryList = new List<TernaryDelegate>();
            TernaryList.Add(Ternary.Ternary_Affine_Coord);
            TernaryList.Add(Ternary.Ternary_Projective_Coord);
            TernaryList.Add(Ternary.Ternary_Jacoby_Coord);
            TernaryList.Add(Ternary.Ternary_Jacoby_Qurtic);
            QuintupleList = new List<QuintupleDelegate>();
            QuintupleList.Add(Quintuple.Quintuple_Affine_Coord);
            QuintupleList.Add(Quintuple.Quintuple_Projective_Coord);
            QuintupleList.Add(Quintuple.Quintuple_Jacobi_Coord);
        }
        #region Adding
        public static void Add_Affine_Coord(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger x2, BigInteger y2, BigInteger z2, BigInteger a, BigInteger p,
            out BigInteger x3, out BigInteger y3, out BigInteger z3)
        {
            while (y1 < 0)
                y1 = y1 + p;
            while (y2 < 0)
                y2 = y2 + p;

            BigInteger d, inv;
            if (x1 == 0 && y1 == 1 && z1 == 0)
            {
                x3 = x2 % p;
                y3 = y2 % p;
                z3 = z2 % p;
            }
            else
            {
                if (x2 == 0 && y2 == 1 && z2 == 0)
                {
                    x3 = x1 % p;
                    y3 = y1 % p;
                    z3 = z1 % p;
                }
                else
                {
                    if ((x1 == x2) && ((y1 + y2) % p) == 0)
                    {
                        x3 = 0;
                        y3 = 1;
                        z3 = 0;
                    }
                    else
                    {
                        if ((x1 == x2) && (y1 % p) == (y2 % p))
                            Double_Affine_Coord(x1, y1, z1, a, p, out x3, out y3, out z3);
                        else
                        {
                            BigInteger temp = x2 - x1;
                            if (temp < 0)
                                temp = temp + p;
                            Functions.Extended_Euclid(p, temp, out d, out inv);
                            BigInteger lambda = (((y2 - y1) * inv) % p);
                            x3 = ((lambda * lambda - x1 - x2) % p);
                            y3 = ((-y1 - lambda * (x3 - x1)) % p);
                            z3 = 1;
                        }
                    }
                }
            }
            if (x3 < 0) x3 += p;
            if (y3 < 0) y3 += p;
            if (y3 == 0)
            {
                x3 = 0;
                y3 = 1;
                z3 = 0;
            }
        }
        public static void Add_Projective_Coord(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger x2, BigInteger y2, BigInteger z2, BigInteger a, BigInteger p,
            out BigInteger x3, out BigInteger y3, out BigInteger z3)
        {
            if (x1 == 0 && y1 == 1 && z1 == 0)
            {
                x3 = x2;
                y3 = y2;
                z3 = z2;
            }
            else
            {
                if (x2 == 0 && y2 == 1 && z2 == 0)
                {
                    x3 = x1;
                    y3 = y1;
                    z3 = z1;
                }
                else
                {
                    BigInteger u1 = (y2 * z1) % p;
                    BigInteger u2 = (y1 * z2) % p;

                    BigInteger v1 = (x2 * z1) % p;
                    BigInteger v2 = (x1 * z2) % p;

                    if (v1 == v2)
                    {
                        if (u1 != u2)
                        {
                            x3 = 0;
                            y3 = 1;
                            z3 = 0; // POINT_AT_INFINITY
                        }
                        else Double_Projective_Coord(x1, y1, z1, a, p, out x3, out y3, out z3);
                    }
                    else
                    {                   
                        BigInteger u = (u1 - u2) % p;
                        BigInteger v = (v1 - v2) % p;
                        BigInteger w = (z1 * z2) % p;
                        BigInteger A = (BigInteger.Pow(u, 2) * w - BigInteger.Pow(v, 3) - 2 * BigInteger.Pow(v, 2) * v2) % p;

                        x3 = (v * A) % p;
                        y3 = (u * (BigInteger.Pow(v, 2) * v2 - A) - BigInteger.Pow(v, 3) * u2) % p;
                        z3 = (BigInteger.Pow(v, 3) * w) % p;

                        if (x3 < 0) x3 += p;
                        if (y3 < 0) y3 += p;
                        if (z3 < 0) z3 += p;
                    }
                }
            }        
        }
        public static void Add_Projective_Coord_2(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger x2, BigInteger y2, BigInteger z2, BigInteger a, BigInteger p,
            out BigInteger x3, out BigInteger y3, out BigInteger z3)
        {
            BigInteger u, v, w, t1, t2, t3, v2, v3, t4;
            t1 = y1 * z2;
            t2 = x1 * z2;
            t3 = z1 * z2;

            if ((y2 * z1 - t1) % p < 0)
                u = (y2 * z1 - t1) % p + p;
            else u = (y2 * z1 - t1) % p;

            if ((x2 * z1 - t2) % p < 0)
                v = (x2 * z1 - t2) % p + p;
            else v = (x2 * z1 - t2) % p;

            v2 = Functions.Pow(v, 2);
            v3 = v2 * v;
            t4 = v2 * t2;

            if ((Functions.Pow(u, 2) * t3 - v3 - 2 * v2 * t2) % p < 0)
                w = (Functions.Pow(u, 2) * t3 - v3 - 2 * v2 * t2) % p + p;
            else w = (Functions.Pow(u, 2) * t3 - v3 - 2 * v2 * t2) % p;

            if ((v * w) % p < 0)
                x3 = (v * w) % p + p;
            else x3 = (v * w) % p;

            if ((u * (v2 * t2 - w) - v3 * t1) % p < 0)
                y3 = (u * (v2 * t2 - w) - v3 * t1) % p + p;
            else y3 = (u * (v2 * t2 - w) - v3 * t1) % p;
      
            if ((v3 * t3) % p < 0)
                z3 = (v3 * t3) % p + p;
            else z3 = (v3 * t3) % p;

        }
        public static void Add_Jacoby_Coord(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger x2, BigInteger y2, BigInteger z2, BigInteger a, BigInteger p,
            out BigInteger x3, out BigInteger y3, out BigInteger z3)
        {
            BigInteger r, s, t, u, v, w;
            if (x1 == 0 && y1 == 1 && z1 == 0)
            {
                x3 = x2;
                y3 = y2;
                z3 = z2;
            }
            else
            {
                if (x2 == 0 && y2 == 1 && z2 == 0)
                {
                    x3 = x1;
                    y3 = y1;
                    z3 = z1;
                }
                else
                {
                    if ((x1 * Functions.Pow(z2, 2)) % p < 0)
                        r = (x1 * Functions.Pow(z2, 2)) % p + p;
                    else r = (x1 * Functions.Pow(z2, 2)) % p;

                    if ((x2 * Functions.Pow(z1, 2)) % p < 0)
                        s = (x2 * Functions.Pow(z1, 2)) % p + p;
                    else s = (x2 * Functions.Pow(z1, 2)) % p;

                    if ((y1 * Functions.Pow(z2, 3)) % p < 0)
                        t = (y1 * Functions.Pow(z2, 3)) % p + p;
                    else t = (y1 * Functions.Pow(z2, 3)) % p;

                    if ((y2 * Functions.Pow(z1, 3)) % p < 0)
                        u = (y2 * Functions.Pow(z1, 3)) % p + p;
                    else u = (y2 * Functions.Pow(z1, 3)) % p;
                    if (r == s)
                    {
                        if (t != u)
                        {
                            x3 = 0;
                            y3 = 1;
                            z3 = 0;
                        }
                        else
                            Double_Jacoby_Coord(x1, y1, z1, a, p, out x3, out y3, out z3);
                    }
                    else
                    {
                        if ((s - r) % p < 0)
                            v = (s - r) % p + p;
                        else v = (s - r) % p;

                        if ((u - t) % p < 0)
                            w = (u - t) % p + p;
                        else w = (u - t) % p;

                        if ((-(Functions.Pow(v, 3)) - 2 * r * Functions.Pow(v, 2) + Functions.Pow(w, 2)) % p < 0)
                            x3 = (-(Functions.Pow(v, 3)) - 2 * r * Functions.Pow(v, 2) + Functions.Pow(w, 2)) % p + p;
                        else x3 = (-(Functions.Pow(v, 3)) - 2 * r * Functions.Pow(v, 2) + Functions.Pow(w, 2)) % p;

                        if ((-t * Functions.Pow(v, 3) + (r * Functions.Pow(v, 2) - x3) * w) % p < 0)
                            y3 = (-t * Functions.Pow(v, 3) + (r * Functions.Pow(v, 2) - x3) * w) % p + p;
                        else y3 = (-t * Functions.Pow(v, 3) + (r * Functions.Pow(v, 2) - x3) * w) % p;

                        if ((v * z1 * z2) % p < 0)
                            z3 = (v * z1 * z2) % p + p;
                        else z3 = (v * z1 * z2) % p;
                    }
                }
            }
        }
        public static void Add_Jacoby_Quartic(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger x2, BigInteger y2, BigInteger z2, BigInteger a, BigInteger p,
            out BigInteger x3, out BigInteger y3, out BigInteger z3)
        {
            BigInteger b = -3; 
            if (x1 == 0 && y1 == 1 && z1 == 0)
            {
                x3 = x2;
                y3 = y2;
                z3 = z2;
            }
            else
            {
                if (x2 == 0 && y2 == 1 && z2 == 0)
                {
                    x3 = x1;
                    y3 = y1;
                    z3 = z1;
                }
                else
                {
                    if (x1 * z2 == x2 * z1)
                    {
                        if (y1 * z2 * z2 != z2 * z1 * z1)
                        {
                            x3 = 0;
                            y3 = 1;
                            z3 = 0;
                        }
                        else
                            Double_Jacoby_Quartic(x1, y1, z1, a, p, out x3, out y3, out z3);
                    }
                    else
                    {
                        BigInteger yy = y1 * y2 % p;
                        BigInteger xz1 = x1 * z1 % p;
                        BigInteger xz2 = x2 * z2 % p;
                        BigInteger zPow = BigInteger.Pow(z1, 2) * BigInteger.Pow(z2, 2) % p;
                        BigInteger xPow = BigInteger.Pow(x1, 2) * BigInteger.Pow(x2, 2) % p;
                        x3 = (xz1 * y2 - xz1) % p;
                        if (x3 < 0) x3 += p;
                        y3 = ((yy - b * xz1 * xz2) * (xPow + zPow) + (BigInteger.Pow(z1, 2) - xz2 - zPow) * 2 * xz1 * xz2) % p;
                        if (y3 < 0) y3 += p;
                        z3 = (zPow - xPow) % p;
                        if (z3 < 0) z3 += p;
                    }

                }
            }
        }
        public static void Add_JacobyChudnovskii_Coord(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger t1, BigInteger r1, BigInteger x2, 
            BigInteger y2, BigInteger z2, BigInteger t2, BigInteger r2, BigInteger a, BigInteger p, out BigInteger x3, out BigInteger y3, 
            out BigInteger z3, out BigInteger t3, out BigInteger r3)
        {
            BigInteger r, s, t, u, v, w;
            t1 = BigInteger.Pow(z1, 2) % p;
            t2 = BigInteger.Pow(z2, 2) % p;
            r1 = BigInteger.Pow(z1, 3) % p;
            r2 = BigInteger.Pow(z2, 3) % p;
            t3 = 0; r3 = 0;
            if (x1 == 0 && y1 == 1 && z1 == 0)
            {
                x3 = x2;
                y3 = y2;
                z3 = z2;
            }
            else
            {
                if (x2 == 0 && y2 == 1 && z2 == 0)
                {
                    x3 = x1;
                    y3 = y1;
                    z3 = z1;
                }
                else
                {
                    if ((x1 * t2) % p < 0)
                        r = (x1 * t2) % p + p;
                    else r = (x1 * t2) % p;

                    if ((x2 * t1) % p < 0)
                        s = (x2 * t1) % p + p;
                    else s = (x2 * t1) % p;

                    if ((y1 * r2) % p < 0)
                        t = (y1 * r2) % p + p;
                    else t = (y1 * r2) % p;

                    if ((y2 * r1) % p < 0)
                        u = (y2 * r1) % p + p;
                    else u = (y2 * r1) % p;

                    if (r == s)
                    {
                        if (t != u)
                        {
                            x3 = 0;
                            y3 = 1;
                            z3 = 0;
                        }
                        else
                            Double_Jacoby_Coord(x1, y1, z1, a, p, out x3, out y3, out z3);
                    }
                    else
                    {
                        if ((s - r) % p < 0)
                            v = (s - r) % p + p;
                        else v = (s - r) % p;

                        if ((u - t) % p < 0)
                            w = (u - t) % p + p;
                        else w = (u - t) % p;

                        if ((-(Functions.Pow(v, 3)) - 2 * r * Functions.Pow(v, 2) + Functions.Pow(w, 2)) % p < 0)
                            x3 = (-(Functions.Pow(v, 3)) - 2 * r * Functions.Pow(v, 2) + Functions.Pow(w, 2)) % p + p;
                        else x3 = (-(Functions.Pow(v, 3)) - 2 * r * Functions.Pow(v, 2) + Functions.Pow(w, 2)) % p;

                        if ((-t * Functions.Pow(v, 3) + (r * Functions.Pow(v, 2) - x3) * w) % p < 0)
                            y3 = (-t * Functions.Pow(v, 3) + (r * Functions.Pow(v, 2) - x3) * w) % p + p;
                        else y3 = (-t * Functions.Pow(v, 3) + (r * Functions.Pow(v, 2) - x3) * w) % p;

                        if ((v * z1 * z2) % p < 0)
                            z3 = (v * z1 * z2) % p + p;
                        else z3 = (v * z1 * z2) % p;

                        t3 = BigInteger.Pow(z3, 2) % p;
                        r3 = BigInteger.Pow(z3, 4) % p;
                    }
                }
            }
        }
        public static void Add_ModifiedJacoby_Coord(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger t1, BigInteger x2, 
            BigInteger y2, BigInteger z2, BigInteger t2,BigInteger a, BigInteger p, out BigInteger x3, out BigInteger y3, 
            out BigInteger z3, out BigInteger t3)
        {
            z3 = 0; 
            t1 = (a * BigInteger.Pow(z1, 4)) % p;
            t2 = (a * BigInteger.Pow(z2, 4)) % p;
            t3 = 0;
            BigInteger h, r;
            if (x1 == 0 && y1 == 1 && z1 == 0)
            {
                x3 = x2;
                y3 = y2;
                z3 = z2;
            }
            else
            {
                if (x2 == 0 && y2 == 1 && z2 == 0)
                {
                    x3 = x1;
                    y3 = y1;
                    z3 = z1;
                }
                else
                {
                    BigInteger u1 = (x1 * BigInteger.Pow(z2,2)) % p;
                    BigInteger u2 = (x2 * BigInteger.Pow(z1,2)) % p;

                    BigInteger s1 = (y1 * BigInteger.Pow(z2,3)) % p;
                    BigInteger s2 = (y2 * BigInteger.Pow(z1,3)) % p;
                    
                    if (u1 == u2)
                    {
                        if (s1 != s2)
                        {
                            x3 = 0;
                            y3 = 1;
                            z3 = 0; // POINT_AT_INFINITY
                        }
                        else Double_ModifiedJacoby_Coord(x1, y1, z1, t1, a, p, out x3, out y3, out z3, out t3);
                    }
                    else
                    {
                        if ((u2 - u1) % p < 0)
                            h = (u2 - u1) % p + p;
                        else h = (u2 - u1) % p;

                        if ((s2 - s1) % p < 0)
                            r = (s2 - s1) % p + p;
                        else r = (s2 - s1) % p;

                        x3 = (-BigInteger.Pow(h, 3) - 2 * u1 * BigInteger.Pow(h, 2) + BigInteger.Pow(r, 2)) % p;
                        if (x3 < 0) x3 += p;
                        y3 = (-s1 * BigInteger.Pow(h, 3) + r * (u1 * BigInteger.Pow(h, 2) - x3)) % p;
                        if (y3 < 0) y3 += p;
                        z3 = z1 * z2 * h % p;
                        if (z3 < 0) z3 += p;
                        t3 = a * BigInteger.Pow(z3, 4) % p;
                        if (t3 < 0) t3 += p;
                    }
                }
            }
        }
        #endregion
        #region Doubling
        public static void Double_Affine_Coord(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger p,
            out BigInteger x3, out BigInteger y3, out BigInteger z3)
        {
            BigInteger d, inv; 
            if (y1 == 0 || z1 == 0)
            {
                x3 = x1;
                y3 = y1;
                z3 = z1;
            }
            else
            {
                Functions.Extended_Euclid(p, 2 * y1, out d, out inv);
                BigInteger lambda = ((3 * x1 * x1 + a) * inv) % p;
                x3 = (lambda * lambda - 2 * x1) % p;
                y3 = (-y1 - lambda * (x3 - x1)) % p;
                z3 = 1;
            }
            if (x3 < 0) x3 += p;
            if (y3 < 0) y3 += p;
            if (y3 == 0)
            {
                x3 = 0;
                y3 = 1;
                z3 = 0;
            }
        }
        public static void Double_Projective_Coord(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2)
        {
            if (y1 % p == 0 || (x1 == 0 && y1 == 1 && z1 == 0))
            {
                x2 = 0;
                y2 = 1;
                z2 = 0; // POINT_AT_INFINITY
            }
            else
            {
                BigInteger A;
                if (a == -3) A = 3 * (x1 + z1) * (x1 - z1) % p;
                else A = (a * BigInteger.Pow(z1, 2) + 3 * BigInteger.Pow(x1, 2)) % p;
                if (A < 0) A += p;
                BigInteger B = (y1 * z1) % p;
                if (B < 0) B += p;
                BigInteger C = (x1 * y1 * B) % p;
                if (C < 0) C += p;
                BigInteger D = (BigInteger.Pow(A, 2) - 8 * C) % p;
                if (D < 0) D += p;

                x2 = (2 * B * D);
                y2 = (A * (4 * C - D) - 8 * BigInteger.Pow(y1, 2) * BigInteger.Pow(B, 2));
                z2 = (8 * BigInteger.Pow(B, 3));

                x2 = x2 % p;
                if (x2 < 0) x2 += p;
                y2 = y2 % p;
                if (y2 < 0) y2 += p;
                z2 = z2 % p;
                if (z2 < 0) z2 += p;
            }           
        }
        public static void Double_Jacoby_Coord(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger p,
            out BigInteger x3, out BigInteger y3, out BigInteger z3)
        {
            if (y1 % p == 0 || (x1 == 0 && y1 == 1 && z1 == 0))
            {
                x3 = 0;
                y3 = 1;
                z3 = 0; // POINT_AT_INFINITY
            }
            else {
                BigInteger v, w;
                if ((4 * x1 * Functions.Pow(y1, 2)) % p < 0)
                    v = (4 * x1 * Functions.Pow(y1, 2)) % p + p;
                else v = (4 * x1 * Functions.Pow(y1, 2)) % p;

                if ((3 * Functions.Pow(x1, 2) + a * Functions.Pow(z1, 4)) % p < 0)
                    w = (3 * Functions.Pow(x1, 2) + a * Functions.Pow(z1, 4)) % p + p;
                else w = (3 * Functions.Pow(x1, 2) + a * Functions.Pow(z1, 4)) % p;

                if ((-2 * v + Functions.Pow(w, 2)) % p < 0)
                    x3 = (-2 * v + Functions.Pow(w, 2)) % p + p;
                else x3 = (-2 * v + Functions.Pow(w, 2)) % p;

                if ((-8 * Functions.Pow(y1, 4) + (v - x3) * w) % p < 0)
                    y3 = (-8 * Functions.Pow(y1, 4) + (v - x3) * w) % p + p;
                else y3 = (-8 * Functions.Pow(y1, 4) + (v - x3) * w) % p;

                if ((2 * y1 * z1) % p < 0)
                    z3 = (2 * y1 * z1) % p + p;
                else z3 = (2 * y1 * z1) % p;
                if (y3 == 0)
                {
                    x3 = 0;
                    y3 = 1;
                    z3 = 0;
                }
            }
        }
        public static void Double_Jacoby_Quartic(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger p,
            out BigInteger x3, out BigInteger y3, out BigInteger z3)
        {           
            BigInteger b = -3;
            if (y1 % p == 0 || (x1 == 0 && y1 == 1 && z1 == 0))
            {
                x3 = 0;
                y3 = 1;
                z3 = 0; // POINT_AT_INFINITY
            }
            else
            {
                BigInteger yy = y1 * y1 % p;
                BigInteger xx = x1 * x1 % p;
                BigInteger zz = z1 * z1 % p;
                BigInteger zPow = BigInteger.Pow(z1, 4) % p;
                BigInteger xPow = BigInteger.Pow(x1, 4) % p;
                x3 = (-2 * xx * zz) % p;
                if (x3 < 0) x3 += p;
                y3 = ((yy - b * xx * zz) * zPow * xPow + 2 * xx * zz * (xPow - x1 * z1 - y1)) % p;
                if (y3 < 0) y3 += p;
                z3 = 2*(zPow - xPow) % p;
                if (z3 < 0) z3 += p;
                if (y3 == 0)
                {
                    x3 = 0;
                    y3 = 1;
                    z3 = 0;
                }
            }
        }
        public static void Double_JacobyChudnovskii_Coord(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger t1, BigInteger r1, BigInteger a, 
            BigInteger p, out BigInteger x3, out BigInteger y3, out BigInteger z3, out BigInteger t3, out BigInteger r3)
        {
            t3 = 0; r3 = 0;
            t1 = BigInteger.Pow(z1, 2) % p;
            r1 = BigInteger.Pow(z1, 3) % p;
            if (y1 % p == 0 || (x1 == 0 && y1 == 1 && z1 == 0))
            {
                x3 = 0;
                y3 = 1;
                z3 = 0; // POINT_AT_INFINITY
                t3 = 0;
                r3 = 0;
            }
            else {
                BigInteger v, w;
                if ((4 * x1 * Functions.Pow(y1, 2)) % p < 0)
                    v = (4 * x1 * Functions.Pow(y1, 2)) % p + p;
                else v = (4 * x1 * Functions.Pow(y1, 2)) % p;

                if ((3 * Functions.Pow(x1, 2) + a * r1 * z1) % p < 0)
                    w = (3 * Functions.Pow(x1, 2) + a * r1 * z1) % p + p;
                else w = (3 * Functions.Pow(x1, 2) + a * r1 * z1) % p;

                if ((-2 * v + Functions.Pow(w, 2)) % p < 0)
                    x3 = (-2 * v + Functions.Pow(w, 2)) % p + p;
                else x3 = (-2 * v + Functions.Pow(w, 2)) % p;

                if ((-8 * Functions.Pow(y1, 4) + (v - x3) * w) % p < 0)
                    y3 = (-8 * Functions.Pow(y1, 4) + (v - x3) * w) % p + p;
                else y3 = (-8 * Functions.Pow(y1, 4) + (v - x3) * w) % p;

                if ((2 * y1 * z1) % p < 0)
                    z3 = (2 * y1 * z1) % p + p;
                else z3 = (2 * y1 * z1) % p;
                if (y3 == 0)
                {
                    x3 = 0;
                    y3 = 1;
                    z3 = 0;
                }
                t3 = BigInteger.Pow(z3, 2) % p;
                r3 = BigInteger.Pow(z3, 3) % p;
            }
        }
        public static void Double_ExtendedProjective_Coord(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger t1, BigInteger a, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out BigInteger t2)
        {
            t2 = 0;
            if (y1 % p == 0 || (x1 == 0 && y1 == 1 && z1 == 0))
            {
                x2 = 0;
                y2 = 1;
                z2 = 0; // POINT_AT_INFINITY
            }
            else
            {
                t1 = (x1 * x1 / z1) % p;
                x2 = (2 * x1 * y1 * (2 * z1 * z1 + 2 * a * x1 * x1 - y1 * y1)) % p;
                y2 = (2 * y1 * y1 * (y1 * y1 - 2 * a * x1 * x1) - BigInteger.Pow((2 * z1 * z1 + 2 * a * x1 * x1 - y1 * y1), 2)) % p;
                z2 = BigInteger.Pow((2 * z1 * z1 + 2 * a * x1 * x1 - y1 * y1), 2) % p;
                t2 = BigInteger.Pow((2 * x1 * y1), 2) % p;

            }
        }
        public static void Double_ModifiedJacoby_Coord(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger t1, 
            BigInteger a, BigInteger p, out BigInteger x3, out BigInteger y3, out BigInteger z3, out BigInteger t3)
        {
            z3 = 0; 
            BigInteger s, u, m;
            t1 = a * BigInteger.Pow(z1, 4);
            t3 = a * BigInteger.Pow(z3, 4);
            if (y1 % p == 0 || (x1 == 0 && y1 == 1 && z1 == 0))
            {
                x3 = 0;
                y3 = 1;
                z3 = 0; // POINT_AT_INFINITY
            }
            else
            {              
                s = (4 * x1 * BigInteger.Pow(y1, 2)) % p;
                u = (8 * BigInteger.Pow(y1, 4)) % p;
                m = (3 * BigInteger.Pow(x1, 2) + t1) % p;

                x3 = (-2 * s + BigInteger.Pow(m, 2)) % p;
                if (x3 < 0) x3 += p;
                y3 = (m * (s - x3) - u) % p;
                if (y3 < 0) y3 += p;
                z3 = (2 * y1 * z1) % p;
                if (z3 < 0) z3 += p;
                t3 = (2 * u * t1) % p;
                if (t3 < 0) t3 += p;
            }
        }
        #endregion

        #region Convertation
        public static void AffineToProjective(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger p, out BigInteger x2, out BigInteger y2, out BigInteger z2)
        {
            x2 = x1 * z1 % p;
            y2 = y1 * z1 % p;
            z2 = z1 % p;
        }
        public static void ProjectiveToAffine(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger p, out BigInteger x3, out BigInteger y3, out BigInteger z3)
        {
            BigInteger d, inv;
            Functions.Extended_Euclid(p, z1, out d, out inv); 

            if ((x1 * inv) % p < 0)
                x3 = (x1 * inv) % p + p;
            else x3 = (x1 * inv) % p;

            if ((y1 * inv) % p < 0)
                y3 = (y1 * inv) % p + p;
            else y3 = (y1 * inv) % p;

            if ((z1 * inv) % p < 0)
                z3 = (z1 * inv) % p + p;
            else z3 = (z1 * inv) % p;
        }
        public static void AffineToJacobi(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger p, out BigInteger x2, out BigInteger y2, out BigInteger z2)
        {
            x2 = x1 * BigInteger.Pow(z1, 2) % p;
            y2 = y1 * BigInteger.Pow(z1, 3) % p;
            z2 = z1 % p;
        }
        public static void JacobyToAffine(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger p, out BigInteger x3, out BigInteger y3, out BigInteger z3)
        {
            BigInteger d, inv;
            Functions.Extended_Euclid(p, z1, out d, out inv);
            z3 = 1;
            if ((x1 * Functions.Pow(inv, 2)) % p < 0)
                x3 = (x1 * Functions.Pow(inv, 2)) % p + p;
            else x3 = (x1 * Functions.Pow(inv, 2)) % p;

            if ((y1 * Functions.Pow(inv, 3)) % p < 0)
                y3 = (y1 * Functions.Pow(inv, 3)) % p + p;
            else y3 = (y1 * Functions.Pow(inv, 3)) % p;
        }
        #endregion
        #region Scalar Multiolication
        public static void Point_Multiplication_Affine_Coord_1(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, int type, out double time, int w = 0, OperationsCounter ops = null)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            BigInteger x3 = 0;
            BigInteger y3 = 1;
            BigInteger z3 = 0;

            x2 = 0;
            y2 = 1;
            z2 = 0;
            BigInteger t1 = a * BigInteger.Pow(z1, 4); BigInteger t2 = 0;
            BigInteger r1 = z1 * z1;
            BigInteger r2 = z1 * z1 * z1;
            BigInteger r3 = 0, r4 = 0;
            string str = Functions.ToBin(k);

            int t = str.Length;
            for (int i = t - 1; i >= 0; i--)
            {
                if (str[i] == '1')
                {
                    ops.opPointsAdd();
                    switch (type)
                    {
                        case 0: case 1: case 2: AddList[type](x1, y1, z1, x2, y2, z2, a, p, out x2, out y2, out z2); break;
                        case 3: Add_JacobyChudnovskii_Coord(x1, y1, z1, r1, r2, x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                        case 4: Add_ModifiedJacoby_Coord(x1, y1, z1, t1, x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                    }
                }
                ops.opPointsDoubling();
                switch (type)
                {
                    case 0: case 1: case 2: DoubleList[type](x1, y1, z1, a, p, out x1, out y1, out z1); break;
                    case 3: Double_JacobyChudnovskii_Coord(x1, y1, z1, r1, r2, a, p, out x1, out y1, out z1, out r1, out r2); break;
                    case 4: Double_ModifiedJacoby_Coord(x1, y1, z1, t1, a, p, out x1, out y1, out z1, out t1); break;                   
                }
            }
            if (x2 < 0) x2 += p;
            if (y2 < 0) y2 += p;
            if (x2 == 0 && y2 == 1)
                z2 = 0;

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_2(BigInteger x1, BigInteger y1, BigInteger z1,BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, int type, out double time, int w = 0, OperationsCounter ops = null)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            x2 = 0;
            y2 = 1;
            z2 = 0;
            BigInteger t1 = a * BigInteger.Pow(z1, 4);
            BigInteger t2 = 0;
            BigInteger r1 = z1 * z1;
            BigInteger r2 = z1 * z1 * z1;
            BigInteger r3 = 0, r4 = 0;
            ops.opElementsMultiply(3);

            string str = ToBin(k);
            int t = str.Length;
            for (int i = 0; i < t; i++)
            {
                ops.opPointsDoubling();
                switch (type)
                {
                    case 0: case 1: case 2: DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2); break;
                    case 3: Double_JacobyChudnovskii_Coord(x2, y2, z2, r2, r3, a, p, out x2, out y2, out z2, out r3, out r4); break;
                    case 4: Double_ModifiedJacoby_Coord(x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                }              
                if (str[i] == '1')
                {
                    ops.opPointsAdd();
                    switch (type)
                    {
                        case 0: case 1: case 2: AddList[type](x1, y1, z1, x2, y2, z2, a, p, out x2, out y2, out z2); break;
                        case 3: Add_JacobyChudnovskii_Coord(x1, y1, z1, r1, r2, x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                        case 4: Add_ModifiedJacoby_Coord(x1, y1, z1, t1, x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                    }
                }
            }
            if (x2 == 0 && y2 == 1)
                z2 = 0;

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        private static string ToBin(BigInteger k)
        {
            return Functions.ToBin(k);
        }
               
        public static void Point_Multiplication_Affine_Coord_3(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p, 
            out BigInteger x2, out BigInteger y2, out BigInteger z2, int type, out double time, int w = 0, OperationsCounter ops = null)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int ind = (int)Math.Pow(2, w) - 1;
            BigInteger[,] PreComputation = new BigInteger[ind, 3];
            BigInteger x3 = 0, y3 = 0, z3 = 0, t3 = 0, r5 = 0, r6 = 0;
            BigInteger t1 = a * BigInteger.Pow(z1, 4); BigInteger t2 = 0;
            BigInteger r1 = z1 * z1;
            BigInteger r2 = z1 * z1 * z1;
            BigInteger r3 = 0, r4 = 0;
            ops.opElementsMultiply(3);
            for (int i = 1; i <= ind; i++)
            {
                double iterationTime = 0;
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, i, p, out x2, out y2, out z2, type, out iterationTime);
                PreComputation[i - 1, 0] = x2;
                PreComputation[i - 1, 1] = y2;
                PreComputation[i - 1, 2] = z2;
            }
            x2 = 0;
            y2 = 1;
            z2 = 0;
            int h = PreComputation.GetLength(0);
            string str = new string(ToBin(k).Reverse().ToArray());
            int t = str.Length / w;
            if (t * w < str.Length) t++;
            string temp;

            // Set timer here not to include precomputations

            for (int i = 1; i <= t; i++)
            {
                if (str.Length % w == 0)
                {
                    temp = str.Substring((i - 1) * w, w);
                }
                else
                {
                    if (i != t)
                    {
                        temp = str.Substring((i - 1) * w, w);
                    }
                    else
                    {
                        int step = str.Length % w;
                        temp = str.Substring((i - 1) * w, step);
                    }
                }
                int legth_temp = temp.Length;
                int[] arr = new int[legth_temp];
                for (int j = 0; j < legth_temp; j++)
                {
                    arr[j] = int.Parse(temp[j].ToString());
                }
                int pow = 1;
                for (int d = 1; d < legth_temp; d++)
                {
                    pow = pow * 2;
                    arr[0] = arr[0] + pow * arr[d];
                    ops.opElementsAdd();
                    ops.opElementsMultiply();
                }
                if (arr[0] > 0)
                {
                    ops.opPointsAdd();
                    switch (type)
                    {
                        case 4:
                                Add_ModifiedJacoby_Coord(PreComputation[arr[0] - 1, 0], PreComputation[arr[0] - 1, 1], PreComputation[arr[0] - 1, 2], t1, x2, y2, z2, t2, a, p, out x3, out y3, out z3, out t3); break;
                        case 3:
                                Add_JacobyChudnovskii_Coord(PreComputation[arr[0] - 1, 0], PreComputation[arr[0] - 1, 1], PreComputation[arr[0] - 1, 2], r1, r2, x2, y2, z2, r3, r4, a, p, out x3, out y3, out z3, out r5, out r6); break;
                        case 0:
                        case 1:
                        case 2:
                                AddList[type](PreComputation[arr[0] - 1, 0], PreComputation[arr[0] - 1, 1], PreComputation[arr[0] - 1, 2], x2, y2, z2, a, p, out x3, out y3, out z3); break;
                    }
                    x2 = x3;
                    y2 = y3;
                    z2 = z3;
                    t2 = t3;
                    r3 = r5;
                    r4 = r6;
                }
                for (int d = 1; d <= w; d++)
                {
                    for (int j = 0; j < h; j++)
                    {
                        ops.opPointsDoubling();
                        switch (type)
                        {
                            case 4:
                                Double_ModifiedJacoby_Coord(PreComputation[j, 0], PreComputation[j, 1], PreComputation[j, 2], t1, a, p, out x3, out y3, out z3, out t3); break;
                            case 3:
                                Double_JacobyChudnovskii_Coord(PreComputation[j, 0], PreComputation[j, 1], PreComputation[j, 2], r1, r2, a, p, out x3, out y3, out z3, out r5, out r6); break;
                            case 0:
                            case 1:
                            case 2:
                                DoubleList[type](PreComputation[j, 0], PreComputation[j, 1], PreComputation[j, 2], a, p, out x3, out y3, out z3); break;
                        }   
                        PreComputation[j, 0] = x3;
                        PreComputation[j, 1] = y3;
                        PreComputation[j, 2] = z3;
                    }
                }
            }
            if (x2 == 0 && y2 == 1)
                z2 = 0;
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_4(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p, 
            out BigInteger x2, out BigInteger y2, out BigInteger z2, int type, out double time, int w = 0, OperationsCounter ops = null)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            int ind = (int)Math.Pow(2, w) - 1;
            BigInteger[,] PreComputation = new BigInteger[ind, 3];
            BigInteger x3 = 0, y3 = 0, z3 = 0, t3 = 0, r5 = 0, r6 = 0;
            BigInteger t1 = a * BigInteger.Pow(z1, 4); BigInteger t2 = 0;
            BigInteger r1 = z1 * z1;
            BigInteger r2 = z1 * z1 * z1;
            BigInteger r3 = 0, r4 = 0;
            Stopwatch stopWatch = new Stopwatch();
            ops.opElementsMultiply(3);

            // Start timer here if you want to include preparation time
            for (int i = 1; i <= ind; i++)
            {
                double iterationTime = 0;
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, i, p, out x2, out y2, out z2, type, out iterationTime);
                PreComputation[i - 1, 0] = x2;
                PreComputation[i - 1, 1] = y2;
                PreComputation[i - 1, 2] = z2;
            }

            x2 = 0;
            y2 = 1;
            z2 = 0;

            stopWatch.Start();

            int count_bit = (int)(Math.Floor(BigInteger.Log(k, 2))) + 1;
            int t = count_bit / w;
            if (t * w < count_bit) t++;
            for (int i = 1; i <= t; i++)
            {
                int left_range = count_bit - i * w + 1;
                int right_range = count_bit - (i - 1) * w;
                if (left_range < 1)
                    left_range = 1;
                if (right_range < 1)
                    right_range = 1;
                string str = new string(ToBin(k).Reverse().ToArray());
                string temp = str.Substring(left_range - 1, right_range - left_range + 1);
                for (int d = 0; d < right_range - left_range + 1; d++)
                {
                    ops.opPointsDoubling();
                    switch (type)
                    {
                        case 4:
                            Double_ModifiedJacoby_Coord(x2, y2, z2, t2, a, p, out x3, out y3, out z3, out t3); break;
                        case 3:
                            Double_JacobyChudnovskii_Coord(x2, y2, z2, r3, r4, a, p, out x3, out y3, out z3, out r5, out r6); break;
                        case 0:
                        case 1:
                        case 2:
                            DoubleList[type](x2, y2, z2, a, p, out x3, out y3, out z3); break;
                    }
                    x2 = x3;
                    y2 = y3;
                    z2 = z3;
                    t2 = t3;
                    r3 = r5;
                    r4 = r6;
                }
                int pow = 1;
                int legth_temp = temp.Length;
                int[] arr = new int[legth_temp];
                for (int j = 0; j < legth_temp; j++)
                {
                    arr[j] = int.Parse(temp[j].ToString());
                }

                int temp_1 = arr[0];

                for (int d = 1; d < legth_temp; d++)
                {
                    pow = pow * 2;
                    temp_1 = temp_1 + pow * arr[d];
                }

                if (temp_1 > 0)
                {
                    ops.opPointsAdd();
                    switch(type)
                    {
                        case 4:
                                Add_ModifiedJacoby_Coord(PreComputation[temp_1 - 1, 0], PreComputation[temp_1 - 1, 1], PreComputation[temp_1 - 1, 2], t1, x2, y2, z2, t2, a, p, out x3, out y3, out z3, out t3); break;
                        case 3:
                                Add_JacobyChudnovskii_Coord(PreComputation[temp_1 - 1, 0], PreComputation[temp_1 - 1, 1], PreComputation[temp_1 - 1, 2], r1, r2, x2, y2, z2, r3, r4, a, p, out x3, out y3, out z3, out r5, out r6); break;
                        case 0:
                        case 1:
                        case 2:
                                AddList[type](PreComputation[temp_1 - 1, 0], PreComputation[temp_1 - 1, 1], PreComputation[temp_1 - 1, 2], x2, y2, z2, a, p, out x3, out y3, out z3); break;
                    }   
                    x2 = x3;
                    y2 = y3;
                    z2 = z3;
                    t2 = t3;
                    r3 = r5;
                    r4 = r6;
                }
            }
           if (x2 == 0 && y2 == 1)
                z2 = 0;
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_5(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p, 
            out BigInteger x2, out BigInteger y2, out BigInteger z2, int type, out double time, int w = 0, OperationsCounter ops = null)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            int temp_step = (int)(BigInteger.Pow(2, w - 1));
            int count = (int)(BigInteger.Pow(2, w) - temp_step);
            BigInteger[,] PreComputation = new BigInteger[count, 3];
            BigInteger x3 = 0, y3 = 0, z3 = 0, t3 = 0, r5 = 0, r6 = 0;
            BigInteger t1 = a * BigInteger.Pow(z1, 4); BigInteger t2 = 0;
            BigInteger r1 = z1 * z1;
            BigInteger r2 = z1 * z1 * z1;
            BigInteger r3 = 0, r4 = 0;
            Stopwatch stopWatch = new Stopwatch();
            ops.opElementsMultiply(3);

            for (int i = 0; i < count; i++)
            {
                double iterationTime = 0;
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, temp_step + i, p, out x2, out y2, out z2, type, out iterationTime);
                PreComputation[i, 0] = x2;
                PreComputation[i, 1] = y2;
                PreComputation[i, 2] = z2;
            }
            
            x2 = 0;
            y2 = 1;
            z2 = 0;

            stopWatch.Start();

            BigInteger x_temp = x1;
            BigInteger y_temp = y1;
            BigInteger z_temp = z1;
            int m = 0;
            int h = PreComputation.GetLength(0);
            string str = new string(ToBin(k).Reverse().ToArray());
            int count_bit = str.Length;
            while (m < count_bit)
            {
                if (m + w <= count_bit)
                {
                    if (str[m + w - 1] == '0')
                    {
                        if (str[m] == '1')
                        {
                            ops.opPointsAdd();
                            switch (type)
                            {
                                case 4:
                                    Add_ModifiedJacoby_Coord(x_temp, y_temp, z_temp, t1, x2, y2, z2, t2, a, p, out x3, out y3, out z3, out t3); break;
                                case 3:
                                    Add_JacobyChudnovskii_Coord(x_temp, y_temp, z_temp, r1, r2, x2, y2, z2, r3, r4, a, p, out x3, out y3, out z3, out r5, out r6); break;
                                case 0:
                                case 1:
                                case 2:
                                    AddList[type](x_temp, y_temp, z_temp, x2, y2, z2, a, p, out x3, out y3, out z3); break;
                            }
                            x2 = x3;
                            y2 = y3;
                            z2 = z3;
                        }

                        ops.opPointsDoubling();
                        switch (type)
                        {
                            case 4:
                                Double_ModifiedJacoby_Coord(x_temp, y_temp, z_temp, t1, a, p, out x_temp, out y_temp, out z_temp, out t1); break;
                            case 3:
                                Double_JacobyChudnovskii_Coord(x_temp, y_temp, z_temp, r1, r1, a, p, out x_temp, out y_temp, out z_temp, out r1, out r2); break;
                            case 0:
                            case 1:
                            case 2:
                                DoubleList[type](x_temp, y_temp, z_temp, a, p, out x_temp, out y_temp, out z_temp); break;
                        }

                        for (int j = 0; j < h; j++)
                        {
                            ops.opPointsDoubling();
                            switch (type)
                            {
                                case 4:
                                    Double_ModifiedJacoby_Coord(PreComputation[j, 0], PreComputation[j, 1], PreComputation[j, 2], t1, a, p, out PreComputation[j, 0], out PreComputation[j, 1], out PreComputation[j, 2], out t1); break;
                                case 3:
                                    Double_JacobyChudnovskii_Coord(PreComputation[j, 0], PreComputation[j, 1], PreComputation[j, 2], r1, r1, a, p, out PreComputation[j, 0], out PreComputation[j, 1], out PreComputation[j, 2], out r1, out r2); break;
                                case 0:
                                case 1:
                                case 2:
                                    DoubleList[type](PreComputation[j, 0], PreComputation[j, 1], PreComputation[j, 2], a, p, out PreComputation[j, 0], out PreComputation[j, 1], out PreComputation[j, 2]); break;
                            }
                        }
                        m++;
                    }
                    else
                    {
                        string temp = str.Substring(m, w);
                        int pow = 1;
                        int legth_temp = temp.Length;
                        int[] arr = new int[legth_temp];
                        for (int j = 0; j < legth_temp; j++)
                        {
                            arr[j] = int.Parse(temp[j].ToString());
                        }
                        int temp_1 = arr[0];

                        for (int d = 1; d < legth_temp; d++)
                        {
                            pow = pow * 2;
                            temp_1 = temp_1 + pow * arr[d];
                            ops.opElementsMultiply(2);
                            ops.opElementsAdd();
                        }
                        if (temp_1 > 0)
                        {
                            ops.opPointsAdd();
                            switch (type)
                            {
                                case 4:
                                    Add_ModifiedJacoby_Coord(PreComputation[temp_1 - temp_step, 0], PreComputation[temp_1 - temp_step, 1], PreComputation[temp_1 - temp_step, 2], t1, x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                                case 3:
                                    Add_JacobyChudnovskii_Coord(PreComputation[temp_1 - temp_step, 0], PreComputation[temp_1 - temp_step, 1], PreComputation[temp_1 - temp_step, 2], r1, r2, x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                                case 0:
                                case 1:
                                case 2:
                                    AddList[type](PreComputation[temp_1 - temp_step, 0], PreComputation[temp_1 - temp_step, 1], PreComputation[temp_1 - temp_step, 2], x2, y2, z2, a, p, out x2, out y2, out z2); break;
                            }
                        }

                        for (int d = 0; d < w; d++)
                        {
                            ops.opPointsDoubling();
                            switch (type)
                            {
                                case 4:
                                    Double_ModifiedJacoby_Coord(x_temp, y_temp, z_temp, t1, a, p, out x_temp, out y_temp, out z_temp, out t1); break;
                                case 3:
                                    Double_JacobyChudnovskii_Coord(x_temp, y_temp, z_temp, r1, r1, a, p, out x_temp, out y_temp, out z_temp, out r1, out r2); break;
                                case 0:
                                case 1:
                                case 2:
                                    DoubleList[type](x_temp, y_temp, z_temp, a, p, out x_temp, out y_temp, out z_temp); break;
                            }

                            for (int j = 0; j < h; j++)
                            {
                                ops.opPointsDoubling();
                                switch (type)
                                {
                                    case 4:
                                        Double_ModifiedJacoby_Coord(PreComputation[j, 0], PreComputation[j, 1], PreComputation[j, 2], t1, a, p, out PreComputation[j, 0], out PreComputation[j, 1], out PreComputation[j, 2], out t1); break;
                                    case 3:
                                        Double_JacobyChudnovskii_Coord(PreComputation[j, 0], PreComputation[j, 1], PreComputation[j, 2], r1, r1, a, p, out PreComputation[j, 0], out PreComputation[j, 1], out PreComputation[j, 2], out r1, out r2); break;
                                    case 0:
                                    case 1:
                                    case 2:
                                        DoubleList[type](PreComputation[j, 0], PreComputation[j, 1], PreComputation[j, 2], a, p, out PreComputation[j, 0], out PreComputation[j, 1], out PreComputation[j, 2]); break;
                                }
                            }
                        }
                        m += w;
                    }
                }
                else
                {
                    if (str[m] == '1')
                    {
                        ops.opPointsAdd();
                        switch (type)
                        {
                            case 4:
                                Add_ModifiedJacoby_Coord(x_temp, y_temp, z_temp, t1, x2, y2, z2, t2, a, p, out x3, out y3, out z3, out t3); break;
                            case 3:
                                Add_JacobyChudnovskii_Coord(x_temp, y_temp, z_temp, r1, r2, x2, y2, z2, r3, r4, a, p, out x3, out y3, out z3, out r5, out r6); break;
                            case 0:
                            case 1:
                            case 2:
                                AddList[type](x_temp, y_temp, z_temp, x2, y2, z2, a, p, out x3, out y3, out z3); break;
                        }
                        x2 = x3;
                        y2 = y3;
                        z2 = z3;
                        t2 = t3;
                        r3 = r5;
                        r4 = r6;
                    }
                    ops.opPointsDoubling();
                    switch (type)
                    {
                        case 4:
                            Double_ModifiedJacoby_Coord(x_temp, y_temp, z_temp, t1, a, p, out x_temp, out y_temp, out z_temp, out t1); break;
                        case 3:
                            Double_JacobyChudnovskii_Coord(x_temp, y_temp, z_temp, r1, r1, a, p, out x_temp, out y_temp, out z_temp, out r1, out r2); break;
                        case 0:
                        case 1:
                        case 2:
                            DoubleList[type](x_temp, y_temp, z_temp, a, p, out x_temp, out y_temp, out z_temp); break;
                    }
                    for (int j = 0; j < h; j++)
                    {
                        ops.opPointsDoubling();
                        switch (type)
                        {
                            case 4:
                                Double_ModifiedJacoby_Coord(PreComputation[j, 0], PreComputation[j, 1], PreComputation[j, 2], t1, a, p, out PreComputation[j, 0], out PreComputation[j, 1], out PreComputation[j, 2], out t1); break;
                            case 3:
                                Double_JacobyChudnovskii_Coord(PreComputation[j, 0], PreComputation[j, 1], PreComputation[j, 2], r1, r1, a, p, out PreComputation[j, 0], out PreComputation[j, 1], out PreComputation[j, 2], out r1, out r2); break;
                            case 0:
                            case 1:
                            case 2:
                                DoubleList[type](PreComputation[j, 0], PreComputation[j, 1], PreComputation[j, 2], a, p, out PreComputation[j, 0], out PreComputation[j, 1], out PreComputation[j, 2]); break;
                        }
                    }
                    m++;
                }
            }
            if (x2 == 0 && y2 == 1)
                z2 = 0;
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_6(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p, 
            out BigInteger x2, out BigInteger y2, out BigInteger z2, int type, out double time, int w = 0, OperationsCounter ops = null)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            int temp_step = (int)(BigInteger.Pow(2, w - 1));
            int count = (int)(BigInteger.Pow(2, w) - temp_step);
            BigInteger[,] PreComputation = new BigInteger[count, 3];
            BigInteger x3 = 0, y3 = 0, z3 = 0, t3 = 0, r5 = 0, r6 = 0;
            BigInteger t1 = a * BigInteger.Pow(z1, 4); BigInteger t2 = 0;
            BigInteger r1 = z1 * z1;
            BigInteger r2 = z1 * z1 * z1;
            BigInteger r3 = 0, r4 = 0;
            Stopwatch stopWatch = new Stopwatch();
            ops.opElementsMultiply(3);
            // Start timer here if you want to include pre computation time
            // todo: you can add this property to OperationsCounter

            for (int i = 0; i < count; i++)
            {
                double iterationTime = 0;
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, temp_step + i, p, out x2, out y2, out z2, type, out iterationTime);
                PreComputation[i, 0] = x2;
                PreComputation[i, 1] = y2;
                PreComputation[i, 2] = z2;
            }

            x2 = 0;
            y2 = 1;
            z2 = 0;
         
            stopWatch.Start();

            int count_bit = (int)(Math.Floor(BigInteger.Log(k, 2))) + 1;
            int m = count_bit;
            while (m > 0)
            {
                string str = new string(ToBin(k).Reverse().ToArray());
                if (str[m - 1] == '0')
                {
                    ops.opPointsDoubling();
                    switch (type)
                    {
                        case 4:
                            Double_ModifiedJacoby_Coord(x2, y2, z2, t2, a, p, out x3, out y3, out z3, out t3); break;
                        case 3:
                            Double_JacobyChudnovskii_Coord(x2, y2, z2, r3, r4, a, p, out x3, out y3, out z3, out r5, out r6); break;
                        case 0:
                        case 1:
                        case 2:
                            DoubleList[type](x2, y2, z2, a, p, out x3, out y3, out z3); break;
                    }
                    x2 = x3;
                    y2 = y3;
                    z2 = z3;
                    t2 = t3;
                    r3 = r5;
                    r4 = r6;
                    m--;
                }
                else
                {
                    int left_range = m - w + 1;
                    if (left_range < 1)
                        left_range = 1;
                    string temp = str.Substring(left_range - 1, m - left_range + 1);
                    int pow = 1;
                    int legth_temp = temp.Length;
                    int[] arr = new int[legth_temp];
                    for (int j = 0; j < legth_temp; j++)
                    {
                        arr[j] = int.Parse(temp[j].ToString());
                    }
                    int temp_1 = arr[0];

                    for (int d = 1; d < legth_temp; d++)
                    {
                        pow = pow * 2;
                        temp_1 = temp_1 + pow * arr[d];
                    }
                    if (m - left_range + 1 < w)
                    {
                        for (int j = legth_temp; j > 0; j--)
                        {
                            ops.opPointsDoubling();
                            switch (type)
                            {
                                case 4:
                                    Double_ModifiedJacoby_Coord(x2, y2, z2, t2, a, p, out x3, out y3, out z3, out t3); break;
                                case 3:
                                    Double_JacobyChudnovskii_Coord(x2, y2, z2, r3, r4, a, p, out x3, out y3, out z3, out r5, out r6); break;
                                case 0:
                                case 1:
                                case 2:
                                    DoubleList[type](x2, y2, z2, a, p, out x3, out y3, out z3); break;
                            }
                            x2 = x3;
                            y2 = y3;
                            z2 = z3;
                            t2 = t3;
                            r3 = r5;
                            r4 = r6;
                            string temp_str = new string(ToBin(temp_1).Reverse().ToArray());
                            if (temp_str[j - 1] == '1')
                            {
                                ops.opPointsAdd();
                                switch (type)
                                {
                                    case 4:
                                        Add_ModifiedJacoby_Coord(x2, y2, z2, t2, x2, y2, z2, t2, a, p, out x3, out y3, out z3, out t3); break;
                                    case 3:
                                        Add_JacobyChudnovskii_Coord(x2, y2, z2, r3, r4, x2, y2, z2, r3, r4, a, p, out x3, out y3, out z3, out r5, out r6); break;
                                    case 0:
                                    case 1:
                                    case 2:
                                        AddList[type](x1, y1, z1, x2, y2, z2, a, p, out x2, out y2, out z2); break;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int d = 1; d <= w; d++)
                        {
                            ops.opPointsDoubling();
                            switch (type)
                            {
                                case 4:
                                    Double_ModifiedJacoby_Coord(x2, y2, z2, t2, a, p, out x3, out y3, out z3, out t3); break;
                                case 3:
                                    Double_JacobyChudnovskii_Coord(x2, y2, z2, r3, r4, a, p, out x3, out y3, out z3, out r5, out r6); break;
                                case 0:
                                case 1:
                                case 2:
                                    DoubleList[type](x2, y2, z2, a, p, out x3, out y3, out z3); break;
                            }
                            x2 = x3;
                            y2 = y3;
                            z2 = z3;
                            t2 = t3;
                            r3 = r5;
                            r4 = r6;
                        }

                        ops.opPointsAdd();
                        switch (type)
                        {
                            case 4:
                                Add_ModifiedJacoby_Coord(PreComputation[temp_1 - temp_step, 0], PreComputation[temp_1 - temp_step, 1], PreComputation[temp_1 - temp_step, 2], t1, x2, y2, z2, t2, a, p, out x3, out y3, out z3, out t3); break;
                            case 3:
                                Add_JacobyChudnovskii_Coord(PreComputation[temp_1 - temp_step, 0], PreComputation[temp_1 - temp_step, 1], PreComputation[temp_1 - temp_step, 2], r1, r2, x2, y2, z2, r3, r4, a, p, out x3, out y3, out z3, out r5, out r6); break;
                            case 0:
                            case 1:
                            case 2:
                                AddList[type](PreComputation[temp_1 - temp_step, 0], PreComputation[temp_1 - temp_step, 1], PreComputation[temp_1 - temp_step, 2], x2, y2, z2, a, p, out x3, out y3, out z3); break;
                        }
                        x2 = x3;
                        y2 = y3;
                        z2 = z3;
                        t2 = t3;
                        r3 = r5;
                        r4 = r6;
                    }
                    m -= w;
                }
            }
            if (x2 == 0 && y2 == 1)
                z2 = 0;
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_7_1(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p, 
            out BigInteger x2, out BigInteger y2, out BigInteger z2, int type, out double time, int w = 0, OperationsCounter ops = null)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            x2 = 0;
            y2 = 1;
            z2 = 0;
            BigInteger t1 = a * BigInteger.Pow(z1, 4); BigInteger t2 = 0;
            BigInteger r1 = z1 * z1;
            BigInteger r2 = z1 * z1 * z1;
            BigInteger r3 = 0, r4 = 0;
            List<BigInteger> mas_k = Functions.NAF(k);
            int t = mas_k.Count;
            ops.opElementsMultiply(3);

            for (int i = 0; i < t; i++)
            {
                if (mas_k[i] == 1)
                {
                    ops.opPointsAdd();
                    switch (type)
                    {
                        case 4:
                            Add_ModifiedJacoby_Coord(x1, y1, z1, t1, x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                        case 3:
                            Add_JacobyChudnovskii_Coord(x1, y1, z1, r1, r2, x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                        case 0:
                        case 1:
                        case 2:
                            AddList[type](x1, y1, z1, x2, y2, z2, a, p, out x2, out y2, out z2); break;
                    }
                }
                else
                {
                    if (mas_k[i] == -1)
                    {
                        ops.opPointsAdd();
                        switch (type)
                        {
                            case 4:
                                Add_ModifiedJacoby_Coord(x1, y1, z1, t1, x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                            case 3:
                                Add_JacobyChudnovskii_Coord(x1, -y1, z1, r1, r2, x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                            case 0:
                            case 1:
                            case 2:
                                AddList[type](x1, -y1, z1, x2, y2, z2, a, p, out x2, out y2, out z2); break;
                        }
                    }
                }
                ops.opPointsDoubling();
                switch (type)
                {
                    case 4:
                        Double_ModifiedJacoby_Coord(x1, y1, z1, t1, a, p, out x1, out y1, out z1, out t1); break;
                    case 3:
                        Double_JacobyChudnovskii_Coord(x1, y1, z1, r1, r2, a, p, out x1, out y1, out z1, out r1, out r2); break;
                    case 0:
                    case 1:
                    case 2:
                        DoubleList[type](x1, y1, z1, a, p, out x1, out y1, out z1); break;
                }
            }
           if (x2 == 0 && y2 == 1)
                z2 = 0;

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_7_2(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p, 
            out BigInteger x2, out BigInteger y2, out BigInteger z2, int type, out double time, int w = 0, OperationsCounter ops = null)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            x2 = 0;
            y2 = 1;
            z2 = 0;
            BigInteger t1 = a * BigInteger.Pow(z1, 4); BigInteger t2 = 0;
            BigInteger r1 = z1 * z1;
            BigInteger r2 = z1 * z1 * z1;
            BigInteger r3 = 0, r4 = 0;
            BigInteger temp = 0;
            ops.opElementsMultiply(3);

            while (k >= 1)
            {
                if (k % 2 != 0)
                {
                    temp = 2 - (k % 4);
                    k = k - temp;
                }
                else
                    temp = 0;
                if (temp == 1)
                {
                    ops.opPointsAdd();
                    switch (type)
                    {
                        case 4:
                            Add_ModifiedJacoby_Coord(x1, y1, z1, t1, x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                        case 3:
                            Add_JacobyChudnovskii_Coord(x1, y1, z1, r1, r2, x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                        case 0:
                        case 1:
                        case 2:
                            AddList[type](x1, y1, z1, x2, y2, z2, a, p, out x2, out y2, out z2); break;
                    }
                }
                else
                {
                    if (temp == -1)
                    {
                        ops.opPointsAdd();
                        switch (type)
                        {
                            case 4:
                                Add_ModifiedJacoby_Coord(x1, y1, z1, t1, x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                            case 3:
                                Add_JacobyChudnovskii_Coord(x1, -y1, z1, r1, r2, x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                            case 0:
                            case 1:
                            case 2:
                                AddList[type](x1, -y1, z1, x2, y2, z2, a, p, out x2, out y2, out z2); break;
                        }
                    }
                }
                k = k / 2;
                ops.opPointsDoubling();
                switch (type)
                {
                    case 4:
                        Double_ModifiedJacoby_Coord(x1, y1, z1, t1, a, p, out x1, out y1, out z1, out t1); break;
                    case 3:
                        Double_JacobyChudnovskii_Coord(x1, y1, z1, r1, r2, a, p, out x1, out y1, out z1, out r1, out r2); break;
                    case 0:
                    case 1:
                    case 2:
                        DoubleList[type](x1, y1, z1, a, p, out x1, out y1, out z1); break;
                }
            }
            if (x2 == 0 && y2 == 1)
                z2 = 0;

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_8(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p, 
            out BigInteger x2, out BigInteger y2, out BigInteger z2, int type, out double time, int w = 0, OperationsCounter ops = null)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            time = 0;
            x2 = 0;
            y2 = 1;
            z2 = 0;
            BigInteger t1 = a * BigInteger.Pow(z1, 4); BigInteger t2 = 0;
            BigInteger r1 = z1 * z1;
            BigInteger r2 = z1 * z1 * z1;
            BigInteger r3 = 0, r4 = 0;
            List<BigInteger> mas_k = Functions.NAF(k);
            int t = mas_k.Count;
            ops.opElementsMultiply(3);

            for (int i = t; i > 0; i--)
            {
                ops.opPointsDoubling();
                switch (type)
                {
                    case 4:
                        Double_ModifiedJacoby_Coord(x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                    case 3:
                        Double_JacobyChudnovskii_Coord(x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                    case 0:
                    case 1:
                    case 2:
                        DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2); break;
                }
                if (mas_k[i - 1] == 1)
                {
                    ops.opPointsAdd();
                    switch (type)
                    {
                        case 4:
                            Add_ModifiedJacoby_Coord(x2, y2, z2, t2, x1, y1, z1, t1, a, p, out x2, out y2, out z2, out t2); break;
                        case 3:
                            Add_JacobyChudnovskii_Coord(x2, y2, z2, r3, r4, x1, y1, z1, r1, r2, a, p, out x2, out y2, out z2, out r3, out r4); break;
                        case 0:
                        case 1:
                        case 2:
                            AddList[type](x2, y2, z2, x1, y1, z1, a, p, out x2, out y2, out z2); break;
                    }
                }
                else
                {
                    if (mas_k[i - 1] == -1)
                    {
                        ops.opPointsAdd();
                        switch (type)
                        {
                            case 4:
                                Add_ModifiedJacoby_Coord(x1, y1, z1, t1, x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                            case 3:
                                Add_JacobyChudnovskii_Coord(x1, -y1, z1, r1, r2, x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                            case 0:
                            case 1:
                            case 2:
                                AddList[type](x1, -y1, z1, x2, y2, z2, a, p, out x2, out y2, out z2); break;
                        }
                    }
                }
            }
            if (x2 == 0 && y2 == 1)
                z2 = 0;

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_9(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p, 
            out BigInteger x2, out BigInteger y2, out BigInteger z2, int type, out double time, int w = 0, OperationsCounter ops = null)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            int count = (int)((2 * (BigInteger.Pow(2, w) - BigInteger.Pow(-1, w)) / 3) / 2);
            BigInteger[,] PreComputation = new BigInteger[count, 3];
            BigInteger x3 = 0, y3 = 0, z3 = 0;
            for (int i = 0; i < count * 2; i += 2)
            {
                double iterationTime = 0;
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, i + 1, p, out x2, out y2, out z2, type, out iterationTime);
                PreComputation[i / 2, 0] = x2;
                PreComputation[i / 2, 1] = y2;
                PreComputation[i / 2, 2] = z2;
            }
            BigInteger t3 = 0, r5 = 0, r6 = 0;
            BigInteger t1 = a * BigInteger.Pow(z1, 4);
            BigInteger t2 = 0;
            BigInteger r1 = z1 * z1;
            BigInteger r2 = z1 * z1 * z1;
            BigInteger r3 = 0, r4 = 0;
            x2 = 0;
            y2 = 1;
            z2 = 0;
            ops.opElementsMultiply(4);

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<BigInteger> mas_k = Functions.NAF(k);
            int t = mas_k.Count;
            int h = PreComputation.GetLength(0);
            int j = 1, max_j;
            int max = 0;
            while (j <= t)
            {
                Functions.Find_the_largest_t(mas_k, j - 1, w, out max, out max_j);
                if (max > 0)
                {
                    int max_div = max / 2;
                    if (max_div * 2 < max) max_div++;
                    ops.opElementsAdd();
                    switch (type)
                    {
                        case 4:
                            Add_ModifiedJacoby_Coord(PreComputation[max_div - 1, 0], PreComputation[max_div - 1, 1], PreComputation[max_div - 1, 2], t1, x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                        case 3:
                            Add_JacobyChudnovskii_Coord(PreComputation[max_div - 1, 0], PreComputation[max_div - 1, 1], PreComputation[max_div - 1, 2], r1, r2, x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                        case 0:
                        case 1:
                        case 2:
                            AddList[type](PreComputation[max_div - 1, 0], PreComputation[max_div - 1, 1], PreComputation[max_div - 1, 2], x2, y2, z2, a, p, out x2, out y2, out z2); break;
                    }
                }
                else if (max < 0)
                {
                    int max_abs = Math.Abs(max);
                    int max_abs_div = max_abs / 2;
                    if (max_abs_div * 2 < max_abs) max_abs_div++;
                    ops.opPointsAdd();
                    switch (type)
                    {
                        case 4:
                            Add_ModifiedJacoby_Coord(PreComputation[max_abs_div - 1, 0], -PreComputation[max_abs_div - 1, 1], PreComputation[max_abs_div - 1, 2], t1, x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                        case 3:
                            Add_JacobyChudnovskii_Coord(PreComputation[max_abs_div - 1, 0], -PreComputation[max_abs_div - 1, 1], PreComputation[max_abs_div - 1, 2], r1, r2, x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                        case 0:
                        case 1:
                        case 2:
                            AddList[type](PreComputation[max_abs_div - 1, 0], -PreComputation[max_abs_div - 1, 1], PreComputation[max_abs_div - 1, 2], x2, y2, z2, a, p, out x2, out y2, out z2); break;
                    }
                }
                for (int d = 0; d < max_j; d++)
                {
                    for (int l = 0; l < h; l++)
                    {
                        ops.opPointsDoubling();
                        switch (type)
                        {
                            case 4:
                                Double_ModifiedJacoby_Coord(PreComputation[l, 0], PreComputation[l, 1], PreComputation[l, 2], t2, a, p, out PreComputation[l, 0], out PreComputation[l, 1], out PreComputation[l, 2], out t3); break;
                            case 3:
                                Double_JacobyChudnovskii_Coord(PreComputation[l, 0], PreComputation[l, 1], PreComputation[l, 2], r3, r4, a, p, out PreComputation[l, 0], out PreComputation[l, 1], out PreComputation[l, 2], out r5, out r6); break;
                            case 0:
                            case 1:
                            case 2:
                                DoubleList[type](PreComputation[l, 0], PreComputation[l, 1], PreComputation[l, 2], a, p, out PreComputation[l, 0], out PreComputation[l, 1], out PreComputation[l, 2]); break;
                        }
                    }
                }
                j = j + max_j;
            }
            if (x2 == 0 && y2 == 1)
                z2 = 0;
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_10(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p, 
            out BigInteger x2, out BigInteger y2, out BigInteger z2, int type, out double time, int w = 0, OperationsCounter ops = null)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            int count = (int)((2 * (BigInteger.Pow(2, w) - BigInteger.Pow(-1, w)) / 3) / 2);
            BigInteger[,] PreComputation = new BigInteger[count, 3];
            BigInteger x3 = 0, y3 = 0, z3 = 0;
            Stopwatch stopWatch = new Stopwatch();
            // Pre computations begin
            for (int i = 0; i < count * 2; i += 2)
            {
                double iterationTime = 0;
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, i + 1, p, out x2, out y2, out z2, type, out iterationTime);
                PreComputation[i / 2, 0] = x2;
                PreComputation[i / 2, 1] = y2;
                PreComputation[i / 2, 2] = z2;
            }

            BigInteger t3 = 0, r5 = 0, r6 = 0;
            BigInteger t1 = a * BigInteger.Pow(z1, 4);
            BigInteger t2 = 0;
            BigInteger r1 = z1 * z1;
            BigInteger r2 = z1 * z1 * z1;
            BigInteger r3 = 0, r4 = 0;
            x2 = 0;
            y2 = 1;
            z2 = 0;
            ops.opElementsMultiply(4);
            stopWatch.Start();

            List<BigInteger> mas_k = Functions.NAF(k);
            int t = mas_k.Count;
            int h = PreComputation.GetLength(0);
            int max, max_j;
            int j = t;
            while (j >= 1)
            {
                if (mas_k[j - 1] == 0)
                {
                    max = 0;
                    max_j = 1;
                }
                else
                   Functions.Find_the_largest_t_10(mas_k, j - 1, w, out max, out max_j);
                for (int l = 0; l < max_j; l++)
                {
                    ops.opPointsDoubling();
                    switch (type)
                    {
                        case 4:
                            Double_ModifiedJacoby_Coord(x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t3); break;
                        case 3:
                            Double_JacobyChudnovskii_Coord(x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r5, out r6); break;
                        case 0:
                        case 1:
                        case 2:
                            DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2); break;
                    }
                }

                if (max > 0)
                {
                    ops.opPointsAdd();
                    switch (type)
                    {
                        case 4:
                            Add_ModifiedJacoby_Coord(PreComputation[max / 2, 0], PreComputation[max / 2, 1], PreComputation[max / 2, 2], t1, x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                        case 3:
                            Add_JacobyChudnovskii_Coord(PreComputation[max / 2, 0], PreComputation[max / 2, 1], PreComputation[max / 2, 2], r1, r2, x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                        case 0:
                        case 1:
                        case 2:
                            AddList[type](PreComputation[max / 2, 0], PreComputation[max / 2, 1], PreComputation[max / 2, 2], x2, y2, z2, a, p, out x2, out y2, out z2); break;
                    }
                }
                else if (max < 0)
                {
                    int max_abs = Math.Abs(max);
                    ops.opPointsAdd();
                    switch (type)
                    {
                        case 4:
                            Add_ModifiedJacoby_Coord(PreComputation[max_abs / 2, 0], -PreComputation[max_abs / 2, 1], PreComputation[max_abs / 2, 2], t1, x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                        case 3:
                            Add_JacobyChudnovskii_Coord(PreComputation[max_abs / 2, 0], -PreComputation[max_abs / 2, 1], PreComputation[max_abs / 2, 2], r1, r2, x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                        case 0:
                        case 1:
                        case 2:
                            AddList[type](PreComputation[max_abs / 2, 0], -PreComputation[max_abs / 2, 1], PreComputation[max_abs / 2, 2], x2, y2, z2, a, p, out x2, out y2, out z2); break;
                    }
                }

                j = j - max_j;
            }
            if (x2 == 0 && y2 == 1)
                z2 = 0;
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_11_1(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p, 
            out BigInteger x2, out BigInteger y2, out BigInteger z2, int type, out double time,  int w = 0, OperationsCounter ops = null)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            int count = (int)(BigInteger.Pow(2, w - 2));
            BigInteger[,] PreComputation = new BigInteger[count, 3];
            BigInteger x3 = 0, y3 = 0, z3 = 0;
            for (int i = 0; i < count * 2; i += 2)
            {
                double iterationTime = 0;
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, i + 1, p, out x2, out y2, out z2, type, out iterationTime);
                PreComputation[i / 2, 0] = x2;
                PreComputation[i / 2, 1] = y2;
                PreComputation[i / 2, 2] = z2;
            }
            BigInteger t3 = 0, r5 = 0, r6 = 0;
            BigInteger t1 = a * BigInteger.Pow(z1, 4);
            BigInteger t2 = 0;
            BigInteger r1 = z1 * z1;
            BigInteger r2 = z1 * z1 * z1;
            BigInteger r3 = 0, r4 = 0;
            x2 = 0;
            y2 = 1;
            z2 = 0;
            ops.opElementsMultiply(4);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<BigInteger> mas_k = Functions.NAFw(k, w);
            int t = mas_k.Count;
            int h = PreComputation.GetLength(0);
            for (int i = 0; i < t; i++)
            {
                if (mas_k[i] > 0)
                {
                    ops.opPointsAdd();
                    switch (type)
                    {
                        case 4:
                            Add_ModifiedJacoby_Coord(PreComputation[(int)(mas_k[i] / 2), 0], PreComputation[(int)(mas_k[i] / 2), 1], PreComputation[(int)(mas_k[i] / 2), 2], t1, x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                        case 3:
                            Add_JacobyChudnovskii_Coord(PreComputation[(int)(mas_k[i] / 2), 0], PreComputation[(int)(mas_k[i] / 2), 1], PreComputation[(int)(mas_k[i] / 2), 2], r1, r2, x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                        case 0:
                        case 1:
                        case 2:
                            AddList[type](PreComputation[(int)(mas_k[i] / 2), 0], PreComputation[(int)(mas_k[i] / 2), 1], PreComputation[(int)(mas_k[i] / 2), 2], x2, y2, z2, a, p, out x2, out y2, out z2); break;
                    }
                }
                else if (mas_k[i] < 0)
                {
                    int mas_k_abs = (int)BigInteger.Abs(mas_k[i]);
                    ops.opPointsAdd();
                    switch (type)
                    {
                        case 4:
                            Add_ModifiedJacoby_Coord(PreComputation[mas_k_abs / 2, 0], -PreComputation[mas_k_abs / 2, 1], PreComputation[mas_k_abs / 2, 2], t1, x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                        case 3:
                            Add_JacobyChudnovskii_Coord(PreComputation[mas_k_abs / 2, 0], -PreComputation[mas_k_abs / 2, 1], PreComputation[mas_k_abs / 2, 2], r1, r2, x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                        case 0:
                        case 1:
                        case 2:
                            AddList[type](PreComputation[mas_k_abs / 2, 0], -PreComputation[mas_k_abs / 2, 1], PreComputation[mas_k_abs / 2, 2], x2, y2, z2, a, p, out x2, out y2, out z2); break;
                    }
                }
                for (int j = 0; j < h; j++)
                {
                    ops.opPointsDoubling();
                    switch (type)
                    {
                        case 4:
                            Double_ModifiedJacoby_Coord(PreComputation[j, 0], PreComputation[j, 1], PreComputation[j, 2], t2, a, p, out PreComputation[j, 0], out PreComputation[j, 1], out PreComputation[j, 2], out t3); break;
                        case 3:
                            Double_JacobyChudnovskii_Coord(PreComputation[j, 0], PreComputation[j, 1], PreComputation[j, 2], r3, r4, a, p, out PreComputation[j, 0], out PreComputation[j, 1], out PreComputation[j, 2], out r5, out r6); break;
                        case 0:
                        case 1:
                        case 2:
                            DoubleList[type](PreComputation[j, 0], PreComputation[j, 1], PreComputation[j, 2], a, p, out PreComputation[j, 0], out PreComputation[j, 1], out PreComputation[j, 2]); break;
                    }
                }
           }
            if (x2 == 0 && y2 == 1)
                z2 = 0;
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_11_2(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p, 
            out BigInteger x2, out BigInteger y2, out BigInteger z2, int type, out double time, int w = 0, OperationsCounter ops = null)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            int count = (int)(Math.Pow(2, w - 2));
            int count1 = (int)(Math.Pow(2, w - 1)) - 1;
            BigInteger[,] PreComputation = new BigInteger[count, 3];
            for (int i = 0; i < count1; i += 2)
            {
                double iterationTime = 0;
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, i + 1, p, out x2, out y2, out z2, type, out iterationTime);
                PreComputation[i / 2, 0] = x2;
                PreComputation[i / 2, 1] = y2;
                PreComputation[i / 2, 2] = z2;
            }
            BigInteger t3 = 0, r5 = 0, r6 = 0;
            BigInteger t1 = a * BigInteger.Pow(z1, 4); BigInteger t2 = 0;
            BigInteger r1 = z1 * z1;
            BigInteger r2 = z1 * z1 * z1;
            BigInteger r3 = 0, r4 = 0;
            x2 = 0;
            y2 = 1;
            z2 = 0;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int h = PreComputation.GetLength(0);
            while (k >= 1)
            {
                BigInteger temp;
                if ((k % 2) != 0)
                {
                    temp = k % Functions.Pow(2, w);
                    if (temp >= Functions.Pow(2, w - 1))
                    {
                        temp = temp - Functions.Pow(2, w);
                    }
                    k = k - temp;
                }
                else temp = 0;
                if (temp > 0)
                {
                    int n = (int)Math.Ceiling(Math.Abs((double)temp) / 2);
                    ops.opPointsAdd();
                    switch (type)
                    {
                        case 4:
                            Add_ModifiedJacoby_Coord(PreComputation[(int)Math.Ceiling(Math.Abs((double)temp) / 2) - 1, 0], PreComputation[(int)Math.Ceiling(Math.Abs((double)temp) / 2) - 1, 1], PreComputation[(int)Math.Ceiling(Math.Abs((double)temp) / 2) - 1, 2], t1, x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                        case 3:
                            Add_JacobyChudnovskii_Coord(PreComputation[(int)Math.Ceiling(Math.Abs((double)temp) / 2) - 1, 0], PreComputation[(int)Math.Ceiling(Math.Abs((double)temp) / 2) - 1, 1], PreComputation[(int)Math.Ceiling(Math.Abs((double)temp) / 2) - 1, 2], r1, r2, x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                        case 0:
                        case 1:
                        case 2:
                            AddList[type](PreComputation[(int)Math.Ceiling(Math.Abs((double)temp) / 2) - 1, 0], PreComputation[(int)Math.Ceiling(Math.Abs((double)temp) / 2) - 1, 1], PreComputation[(int)Math.Ceiling(Math.Abs((double)temp) / 2) - 1, 2], x2, y2, z2, a, p, out x2, out y2, out z2); break;
                    }
                }
                if (temp < 0)
                {
                    ops.opPointsAdd();
                    switch (type)
                    {
                        case 4:
                            Add_ModifiedJacoby_Coord(PreComputation[(int)Math.Ceiling(Math.Abs((double)temp) / 2 - 1), 0], -PreComputation[(int)Math.Ceiling(Math.Abs((double)temp) / 2) - 1, 1], PreComputation[(int)Math.Ceiling(Math.Abs((double)temp) / 2) - 1, 2], t1, x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                        case 3:
                            Add_JacobyChudnovskii_Coord(PreComputation[(int)Math.Ceiling(Math.Abs((double)temp) / 2 - 1), 0], -PreComputation[(int)Math.Ceiling(Math.Abs((double)temp) / 2) - 1, 1], PreComputation[(int)Math.Ceiling(Math.Abs((double)temp) / 2) - 1, 2], r1, r2, x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                        case 0:
                        case 1:
                        case 2:
                            AddList[type](PreComputation[(int)Math.Ceiling(Math.Abs((double)temp) / 2 - 1), 0], -PreComputation[(int)Math.Ceiling(Math.Abs((double)temp) / 2) - 1, 1], PreComputation[(int)Math.Ceiling(Math.Abs((double)temp) / 2) - 1, 2], x2, y2, z2, a, p, out x2, out y2, out z2); break;
                    }
                }

                for (int j = 0; j < PreComputation.GetLength(0); j++)
                {
                    ops.opPointsDoubling();
                    switch (type)
                    {
                        case 4:
                            Double_ModifiedJacoby_Coord(PreComputation[j, 0], PreComputation[j, 1], PreComputation[j, 2], t3, a, p, out PreComputation[j, 0], out PreComputation[j, 1], out PreComputation[j, 2], out t3); break;
                        case 3:
                            Double_JacobyChudnovskii_Coord(PreComputation[j, 0], PreComputation[j, 1], PreComputation[j, 2], r5, r6, a, p, out PreComputation[j, 0], out PreComputation[j, 1], out PreComputation[j, 2], out r5, out r6); break;
                        case 0:
                        case 1:
                        case 2:
                            DoubleList[type](PreComputation[j, 0], PreComputation[j, 1], PreComputation[j, 2], a, p, out PreComputation[j, 0], out PreComputation[j, 1], out PreComputation[j, 2]); break;
                    }
                }
                k = k / 2;
                ops.opPointsDoubling();
                switch (type)
                {
                    case 4:
                        Double_ModifiedJacoby_Coord(x1, y1, z1, t1, a, p, out x1, out y1, out z1, out t1); break;
                    case 3:
                        Double_JacobyChudnovskii_Coord(x1, y1, z1, r1, r2, a, p, out x1, out y1, out z1, out r1, out r2); break;
                    case 0:
                    case 1:
                    case 2:
                        DoubleList[type](x1, y1, z1, a, p, out x1, out y1, out z1); break;
                }
            }
            if (x2 == 0 && y2 == 1)
                z2 = 0;
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_12(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p, 
            out BigInteger x2, out BigInteger y2, out BigInteger z2, int type, out double time, int w = 0, OperationsCounter ops = null)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            int count = (int)(BigInteger.Pow(2, w - 2));
            BigInteger[,] PreComputation = new BigInteger[count, 3];
            BigInteger x3 = 0, y3 = 0, z3 = 0;
            Stopwatch stopWatch = new Stopwatch();
            // Precomputations begin
            for (int i = 0; i < count * 2; i += 2)
            {
                double iterationTime = 0;
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, i + 1, p, out x2, out y2, out z2, type, out iterationTime);
                PreComputation[i / 2, 0] = x2;
                PreComputation[i / 2, 1] = y2;
                PreComputation[i / 2, 2] = z2;
            }

            BigInteger t3 = 0, r5 = 0, r6 = 0;
            BigInteger t1 = a * BigInteger.Pow(z1, 4); BigInteger t2 = 0;
            BigInteger r1 = z1 * z1;
            BigInteger r2 = z1 * z1 * z1;
            BigInteger r3 = 0, r4 = 0;

            x2 = 0;
            y2 = 1;
            z2 = 0;
            
            stopWatch.Start();
            List<BigInteger> mas_k = Functions.NAFw(k, w);
            int t = mas_k.Count;
            int h = PreComputation.GetLength(0);

            for (int i = t - 1; i >= 0; i--)
            {
                ops.opPointsDoubling();
                switch (type)
                {
                    case 4:
                        Double_ModifiedJacoby_Coord(x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                    case 3:
                        Double_JacobyChudnovskii_Coord(x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                    case 0:
                    case 1:
                    case 2:
                        DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2); break;
                }
                if (mas_k[i] > 0)
                {
                    ops.opPointsAdd();
                    switch (type)
                    {
                        case 4:
                            Add_ModifiedJacoby_Coord(PreComputation[(int)(mas_k[i] / 2), 0], PreComputation[(int)(mas_k[i] / 2), 1], PreComputation[(int)(mas_k[i] / 2), 2], t1, x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                        case 3:
                            Add_JacobyChudnovskii_Coord(PreComputation[(int)(mas_k[i] / 2), 0], PreComputation[(int)(mas_k[i] / 2), 1], PreComputation[(int)(mas_k[i] / 2), 2], r1, r2, x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                        case 0:
                        case 1:
                        case 2:
                            AddList[type](PreComputation[(int)(mas_k[i] / 2), 0], PreComputation[(int)(mas_k[i] / 2), 1], PreComputation[(int)(mas_k[i] / 2), 2], x2, y2, z2, a, p, out x2, out y2, out z2); break;
                    }
                }
                else if (mas_k[i] < 0)
                {
                    int mas_k_abs = (int)BigInteger.Abs(mas_k[i]);
                    ops.opPointsAdd();
                    switch (type)
                    {
                        case 4:
                            Add_ModifiedJacoby_Coord(PreComputation[mas_k_abs / 2, 0], -PreComputation[mas_k_abs / 2, 1], PreComputation[mas_k_abs / 2, 2], t1, x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                        case 3:
                            Add_JacobyChudnovskii_Coord(PreComputation[mas_k_abs / 2, 0], -PreComputation[mas_k_abs / 2, 1], PreComputation[mas_k_abs / 2, 2], r1, r2, x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                        case 0:
                        case 1:
                        case 2:
                            AddList[type](PreComputation[mas_k_abs / 2, 0], -PreComputation[mas_k_abs / 2, 1], PreComputation[mas_k_abs / 2, 2], x2, y2, z2, a, p, out x2, out y2, out z2); break;
                    }
                }
            }
            if (x2 == 0 && y2 == 1)
                z2 = 0;
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_13(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p, 
            out BigInteger x2, out BigInteger y2, out BigInteger z2, int type, out double time, int w = 0, OperationsCounter ops = null)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            int count = (int)((BigInteger.Pow(2, w) - BigInteger.Pow(-1, w)) / 3);
            BigInteger[,] PreComputation = new BigInteger[count, 3];
            for (int u = 0; u < count * 2; u += 2)
            {
                double iterationTime = 0;
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, u + 1, p, out x2, out y2, out z2, type, out iterationTime);
                PreComputation[u / 2, 0] = x2;
                PreComputation[u / 2, 1] = y2;
                PreComputation[u / 2, 2] = z2;
            }
            x2 = 0;
            y2 = 1;
            z2 = 0;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<BigInteger> mas_k = Functions.NAFw(k, w);
            int t = mas_k.Count;
            int h = PreComputation.GetLength(0);
            int i = 1, max_j;
            int max = 0;
            while (i <= t)
            {
                Functions.Find_the_largest_t(mas_k, i - 1, w, out max, out max_j);
                ops.opPointsAdd();
                if (max > 0)
                {
                    AddList[type](PreComputation[(int)Math.Ceiling(Math.Abs((double)mas_k[i - 1] / 2)) - 1, 0], PreComputation[(int)Math.Ceiling(Math.Abs((double)mas_k[i - 1] / 2)) - 1, 1], PreComputation[(int)Math.Ceiling(Math.Abs((double)mas_k[i - 1] / 2)) - 1, 2], x2, y2, z2, a, p, out x2, out y2, out z2);
                }
                else if (max < 0)
                {
                    AddList[type](PreComputation[(int)Math.Ceiling(Math.Abs((double)mas_k[i - 1] / 2)) - 1, 0], -PreComputation[(int)Math.Ceiling(Math.Abs((double)mas_k[i - 1] / 2)) - 1, 1], PreComputation[(int)Math.Ceiling(Math.Abs((double)mas_k[i - 1] / 2)) - 1, 2], x2, y2, z2, a, p, out x2, out y2, out z2);
                }
                for (int d = 0; d < max_j; d++)
                {
                    for (int l = 0; l < h; l++)
                    {
                        ops.opPointsDoubling();
                        DoubleList[type](PreComputation[l, 0], PreComputation[l, 1], PreComputation[l, 2], a, p, out PreComputation[l, 0], out PreComputation[l, 1], out PreComputation[l, 2]);
                    }
                }
                i = i + max_j;
            }
            if (x2 == 0 && y2 != 0)
                z2 = 0;
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_14(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p, 
            out BigInteger x2, out BigInteger y2, out BigInteger z2, int type, out double time, int w = 0, OperationsCounter ops = null)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            int count = (int)((BigInteger.Pow(2, w) - BigInteger.Pow(-1, w)) / 3);
            BigInteger[,] PreComputation = new BigInteger[count, 3];
            for (int u = 0; u < count * 2; u += 2)
            {
                double iterationTime = 0;
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, u + 1, p, out x2, out y2, out z2, type, out iterationTime);
                PreComputation[u / 2, 0] = x2;
                PreComputation[u / 2, 1] = y2;
                PreComputation[u / 2, 2] = z2;
            }
            x2 = 0;
            y2 = 1;
            z2 = 0;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<BigInteger> mas_k = Functions.NAFw(k, w);
            int t = mas_k.Count;
            int i = t;
            int max, max_j;
            while (i > 0)
            {
                if (mas_k[i - 1] == 0)
                {
                    max = 0;
                    max_j = 1;
                }
                else
                {
                    Functions.Find_the_largest_t_10(mas_k, i - 1, w, out max, out max_j);
                }
                for (int j = 0; j < max_j; j++)
                {
                    ops.opPointsDoubling();
                    DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2);
                }
                ops.opPointsAdd();
                if (max > 0)
                {
                    AddList[type](PreComputation[(int)Math.Ceiling(Math.Abs((double)max / 2)) - 1, 0], PreComputation[(int)Math.Ceiling(Math.Abs((double)max / 2)) - 1, 1], PreComputation[(int)Math.Ceiling(Math.Abs((double)max / 2)) - 1, 2], x2, y2, z2, a, p, out x2, out y2, out z2);
                }
                else if (max < 0)
                {
                    AddList[type](PreComputation[(int)Math.Ceiling(Math.Abs((double)max / 2)) - 1, 0], -PreComputation[(int)Math.Ceiling(Math.Abs((double)max / 2)) - 1, 1], PreComputation[(int)Math.Ceiling(Math.Abs((double)max / 2)) - 1, 2], x2, y2, z2, a, p, out x2, out y2, out z2);
                }

                i = i - max_j;
            }
            if (x2 == 0 && y2 != 0)
                z2 = 0;
 
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_15(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, int type, out double time, int w = 0, OperationsCounter ops = null)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            x2 = 0;
            y2 = 1;
            z2 = 0;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            string str = Functions.ToBin(k);
            string str1 = Functions.ToBin(k * 3);
            int t = str1.Length;
            int temp_length = t - str.Length;
            while (temp_length > 0)
            {
                str = '0' + str;
                temp_length--;
            }               
            for (int i = t - 2; i >= 1; i--)
            {
                if (str1[i] == '1' && str[i] == '0')
                {
                    ops.opPointsAdd();
                    AddList[type](x2, y2, z2, x1, y1, z1, a, p, out x2, out y2, out z2);
                }
                else if (str1[i] == '0' && str[i] == '1')
                {
                    ops.opPointsAdd();
                    AddList[type](x2, y2, z2, x1, -y1, z1, a, p, out x2, out y2, out z2);
                }
                ops.opPointsDoubling();
                DoubleList[type](x1, y1, z1, a, p, out x1, out y1, out z1);
            }
            ops.opPointsAdd();
            AddList[type](x2, y2, z2, x1, y1, z1, a, p, out x2, out y2, out z2);
            if (x2 == 0 && y2 != 0)
                z2 = 0;
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_16(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, int type, out double time, int w = 0, OperationsCounter ops = null)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            x2 = x1;
            y2 = y1;
            z2 = z1;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            string str = Functions.ToBin(k);
            string str1 = Functions.ToBin(k * 3);
            int t = str1.Length;
            int temp_length = t - str.Length;
            while (temp_length > 0)
            {
                str = '0' + str;
                temp_length--;
            }
            for (int i = 1; i <= t - 2; i++)
            {
                ops.opPointsDoubling();
                DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2);
                if (str1[i] == '1' && str[i] == '0')
                {
                    ops.opPointsAdd();
                    AddList[type](x2, y2, z2, x1, y1, z1, a, p, out x2, out y2, out z2);
                }
                else if (str1[i] == '0' && str[i] == '1')
                {
                    ops.opPointsAdd();
                    AddList[type](x2, y2, z2, x1, -y1, z1, a, p, out x2, out y2, out z2);
                }
            }
            if (x2 == 0 && y2 != 0)
                z2 = 0;           
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_17(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, int type, out double time, int w = 0, OperationsCounter ops = null)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            x2 = 0;
            y2 = 1;
            z2 = 0;
            BigInteger x3 = x1;
            BigInteger y3 = y1;
            BigInteger z3 = z1;
            BigInteger t1 = a * BigInteger.Pow(z1, 4); BigInteger t2 = 0;
            BigInteger r1 = z1 * z1;
            BigInteger r2 = z1 * z1 * z1;
            BigInteger r3 = 0, r4 = 0;
            BigInteger t3 = t1, r5 = r1, r6 = r2;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            string str = Functions.ToBin(k);
            int t = str.Length;
            for (int i = t - 1; i >= 0; i--)
            {
                if (str[i] == '1')
                {
                    ops.opPointsDoubling();
                    ops.opPointsAdd();
                    switch (type)
                    {
                        case 4:
                            Double_ModifiedJacoby_Coord(x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2);
                            Add_ModifiedJacoby_Coord(x3, y3, z3, t3, x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                        case 3:
                            Double_JacobyChudnovskii_Coord(x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4);
                            Add_JacobyChudnovskii_Coord(x3, y3, z3, r5, r6, x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                        case 0:
                        case 1:
                        case 2:
                            DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2);
                            AddList[type](x3, y3, z3, x2, y2, z2, a, p, out x2, out y2, out z2); break;
                    }
                }
                else
                {
                    ops.opPointsDoubling();
                    ops.opPointsAdd();
                    switch (type)
                    {
                        case 4:
                            Double_ModifiedJacoby_Coord(x3, y3, z3, t3, a, p, out x3, out y3, out z3, out t3);
                            Add_ModifiedJacoby_Coord(x3, y3, z3, t3, x2, y2, z2, t2, a, p, out x3, out y3, out z3, out t3); break;
                        case 3:
                            Double_JacobyChudnovskii_Coord(x3, y3, z3, r5, r6, a, p, out x3, out y3, out z3, out r5, out r6);
                            Add_JacobyChudnovskii_Coord(x3, y3, z3, r5, r6, x2, y2, z2, r3, r4, a, p, out x3, out y3, out z3, out r5, out r6); break;
                        case 0:
                        case 1:
                        case 2:
                            DoubleList[type](x3, y3, z3, a, p, out x3, out y3, out z3);
                            AddList[type](x3, y3, z3, x2, y2, z2, a, p, out x3, out y3, out z3); break;
                    }
                }
            }
            if (x2 == 0 && y2 != 0)
            {
                z2 = 0;
            }
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_18(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, int type, out double time, OperationsCounter ops)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            x2 = 0;
            y2 = 1;
            z2 = 0;
            BigInteger x3 = x1;
            BigInteger y3 = y1;
            BigInteger z3 = z1;
            BigInteger t1 = a * BigInteger.Pow(z1, 4);
            BigInteger t2 = 0;
            BigInteger r1 = z1 * z1;
            BigInteger r2 = z1 * z1 * z1;
            BigInteger r3 = 0, r4 = 0;
            BigInteger t3 = t1, r5 = r1, r6 = r2;
            ops.opElementsMultiply(4);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            string str = Functions.ToBin(k);
            int t = str.Length;
            for (int i = 0; i < t; i++)
            {
                if (str[i] == '0')
                {
                    ops.opPointsAdd();
                    ops.opPointsDoubling();
                    switch (type)
                    {
                        case 4:
                            Add_ModifiedJacoby_Coord(x2, y2, z2, t2, x3, y3, z3, t3, a, p, out x3, out y3, out z3, out t3); 
                            Double_ModifiedJacoby_Coord(x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                        case 3:
                            Add_JacobyChudnovskii_Coord(x2, y2, z2, r3, r4, x3, y3, z3, r4, r6, a, p, out x3, out y3, out z3, out r5, out r6);
                            Double_JacobyChudnovskii_Coord(x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                        case 0:
                        case 1:
                        case 2:
                            AddList[type](x2, y2, z2, x3, y3, z3, a, p, out x3, out y3, out z3);
                            DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2); break;
                    }
                }
                else
                {
                    ops.opPointsAdd();
                    ops.opPointsDoubling();
                    switch (type)
                    {
                        case 4:
                            Add_ModifiedJacoby_Coord(x2, y2, z2, t2, x3, y3, z3, t3, a, p, out x2, out y2, out z2, out t2);
                            Double_ModifiedJacoby_Coord(x3, y3, z3, t3, a, p, out x3, out y3, out z3, out t3); break;
                        case 3:
                            Add_JacobyChudnovskii_Coord(x2, y2, z2, r3, r4, x3, y3, z3, r4, r6, a, p, out x2, out y2, out z2, out r3, out r4);
                            Double_JacobyChudnovskii_Coord(x3, y3, z3, r5, r6, a, p, out x3, out y3, out z3, out r5, out r6); break;
                        case 0:
                        case 1:
                        case 2:
                            AddList[type](x2, y2, z2, x3, y3, z3, a, p, out x2, out y2, out z2);
                            DoubleList[type](x3, y3, z3, a, p, out x3, out y3, out z3); break;
                    }
                }
            }
            if (x2 == 0 && y2 != 0)
            {
                z2 = 0;
            }
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_19_1(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x3, out BigInteger y3, out BigInteger z3, out double time, int type, BigInteger a_max, BigInteger b_max, OperationsCounter ops)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            BigInteger[,] mas_k;
            BigInteger t1 = a * BigInteger.Pow(z1, 4);
            BigInteger t2;
            BigInteger r1 = z1 * z1;
            BigInteger r2 = z1 * z1 * z1;
            BigInteger r3, r4;
            BigInteger t3, r5, r6;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            mas_k = Functions.Convert_to_DBNS_1(k, a_max, b_max);
            BigInteger sum = 0;
            for (int i = 0; i < mas_k.GetLength(0); i++)
            {
                sum = sum + mas_k[i, 0] * Functions.Pow(2, mas_k[i, 1]) * Functions.Pow(3, mas_k[i, 2]);
            }
            if (k != sum)
            {
                //throw new Exception("Error");
            }

            BigInteger x2 = x1;
            BigInteger y2 = y1;
            BigInteger z2 = z1;
            t2 = t1;
            r3 = r1;
            r4 = r2;
            for (BigInteger i = 0; i < mas_k[mas_k.GetLength(0) - 1, 2]; i++)
            {
                switch (type)
                {
                    case 4: Ternary.Ternary_ModifiedJacoby(x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                    case 3: Ternary.Ternary_ChudnovskiiJacoby(x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                    case 2:
                    case 1:
                    case 0:
                        TernaryList[type](x2, y2, z2, a, p, out x2, out y2, out z2); break;
                }
            }

            for (BigInteger i = 0; i < mas_k[mas_k.GetLength(0) - 1, 1]; i++)
            {
                ops.opPointsDoubling();
                switch (type)
                {
                    case 4: Double_ModifiedJacoby_Coord(x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                    case 3: Double_JacobyChudnovskii_Coord(x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                    case 2:
                    case 1:
                    case 0:
                        DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2); break;
                }
            }

            x3 = x2;
            y3 = mas_k[mas_k.GetLength(0) - 1, 0] * y2;
            z3 = z2;
            t3 = t2;
            r5 = r3;
            r6 = r4;
            for (int i = mas_k.GetLength(0) - 2; i >= 0; i--)
            {
                BigInteger u = mas_k[i, 1] - mas_k[i + 1, 1];
                BigInteger v = mas_k[i, 2] - mas_k[i + 1, 2];
                ops.opElementsAdd(2);
                for (int j = 0; j < u; j++)
                {
                    ops.opPointsDoubling();
                    switch (type)
                    {
                        case 4: Double_ModifiedJacoby_Coord(x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                        case 3: Double_JacobyChudnovskii_Coord(x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                        case 2:
                        case 1:
                        case 0:
                            DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2); break;
                    }
                }
                for (int j = 0; j < v; j++)
                {
                    switch (type)
                    {
                        case 4: Ternary.Ternary_ModifiedJacoby(x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                        case 3: Ternary.Ternary_ChudnovskiiJacoby(x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                        case 2:
                        case 1:
                        case 0:
                            TernaryList[type](x2, y2, z2, a, p, out x2, out y2, out z2); break;
                    }
                }
                ops.opPointsAdd();
                switch (type)
                {
                    case 4:
                        Add_ModifiedJacoby_Coord(x2, mas_k[i, 0] * y2, z2, t2, x3, y3, z3, t3, a, p, out x3, out y3, out z3, out t3); break;
                    case 3:
                        Add_JacobyChudnovskii_Coord(x2, mas_k[i, 0] * y2, z2, r3, r4, x3, y3, z3, r5, r6, a, p, out x3, out y3, out z3, out r5, out r6); break;
                    case 0:
                    case 1:
                    case 2:
                        AddList[type](x2, mas_k[i, 0] * y2, z2, x3, y3, z3, a, p, out x3, out y3, out z3); break;
                }
            }

            if (x3 == 0 && y3 != 0)
            {
                z3 = 0;
            }
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_19_2(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
           out BigInteger x3, out BigInteger y3, out BigInteger z3, out double time, int type, BigInteger a_max, BigInteger b_max, OperationsCounter ops)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            BigInteger[,] mas_k;
            BigInteger t1 = a * BigInteger.Pow(z1, 4);
            BigInteger t2;
            BigInteger r1 = z1 * z1;
            BigInteger r2 = z1 * z1 * z1;
            BigInteger r3, r4;
            BigInteger t3, r5, r6;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            mas_k = Functions.Convert_to_DBNS_2(k, a_max, b_max);
            BigInteger sum = 0;
            for (int i = 0; i < mas_k.GetLength(0); i++)
            {
                sum = sum + mas_k[i, 0] * Functions.Pow(2, mas_k[i, 1]) * Functions.Pow(3, mas_k[i, 2]);
            }

            if (k != sum)
            {
                //throw new Exception("Error");
            }

            BigInteger x2 = x1;
            BigInteger y2 = y1;
            BigInteger z2 = z1;
            t2 = t1;
            r3 = r1;
            r4 = r2;
            for (BigInteger i = 0; i < mas_k[mas_k.GetLength(0) - 1, 2]; i++)
            {
                switch (type)
                {
                    case 4: Ternary.Ternary_ModifiedJacoby(x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                    case 3: Ternary.Ternary_ChudnovskiiJacoby(x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                    case 2:
                    case 1:
                    case 0:
                        TernaryList[type](x2, y2, z2, a, p, out x2, out y2, out z2); break;
                }
            }

            for (BigInteger i = 0; i < mas_k[mas_k.GetLength(0) - 1, 1]; i++)
            {
                ops.opPointsDoubling();
                switch (type)
                {
                    case 4: Double_ModifiedJacoby_Coord(x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                    case 3: Double_JacobyChudnovskii_Coord(x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                    case 2:
                    case 1:
                    case 0:
                        DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2); break;
                }
            }

            x3 = x2;
            y3 = mas_k[mas_k.GetLength(0) - 1, 0] * y2;
            z3 = z2;
            t3 = t2;
            r5 = r3;
            r6 = r4;
            for (int i = mas_k.GetLength(0) - 2; i >= 0; i--)
            {
                BigInteger u = mas_k[i, 1] - mas_k[i + 1, 1];
                BigInteger v = mas_k[i, 2] - mas_k[i + 1, 2];
                ops.opElementsAdd(2);
                for (int j = 0; j < u; j++)
                {
                    ops.opPointsDoubling();
                    switch (type)
                    {
                        case 4: Double_ModifiedJacoby_Coord(x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                        case 3: Double_JacobyChudnovskii_Coord(x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                        case 2:
                        case 1:
                        case 0:
                            DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2); break;
                    }
                }

                for (int j = 0; j < v; j++)
                {
                    switch (type)
                    {
                        case 4: Ternary.Ternary_ModifiedJacoby(x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                        case 3: Ternary.Ternary_ChudnovskiiJacoby(x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                        case 2:
                        case 1:
                        case 0:
                            TernaryList[type](x2, y2, z2, a, p, out x2, out y2, out z2); break;
                    }
                }
                ops.opPointsAdd();
                switch (type)
                {
                    case 4:
                        Add_ModifiedJacoby_Coord(x2, mas_k[i, 0] * y2, z2, t2, x3, y3, z3, t3, a, p, out x3, out y3, out z3, out t3); break;
                    case 3:
                        Add_JacobyChudnovskii_Coord(x2, mas_k[i, 0] * y2, z2, r3, r4, x3, y3, z3, r5, r6, a, p, out x3, out y3, out z3, out r5, out r6); break;
                    case 0:
                    case 1:
                    case 2:
                        AddList[type](x2, mas_k[i, 0] * y2, z2, x3, y3, z3, a, p, out x3, out y3, out z3); break;
                }

            }
            if (x3 == 0 && y3 != 0)
            {
                z3 = 0;
            }

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_20_1(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, BigInteger a_max, BigInteger b_max, OperationsCounter ops)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            BigInteger[,] mas_k;
            BigInteger t1 = a * BigInteger.Pow(z1, 4);
            BigInteger t2 = 0;
            BigInteger r1 = z1 * z1;
            BigInteger r2 = z1 * z1 * z1;
            BigInteger r3 = 0, r4 = 0;
            ops.opElementsMultiply(4);

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            mas_k = Functions.Convert_to_DBNS_1(k, a_max, b_max);
            BigInteger sum = 0;
            for (int i = 0; i < mas_k.GetLength(0); i++)
            {
                sum = sum + mas_k[i, 0] * Functions.Pow(2, mas_k[i, 1]) * Functions.Pow(3, mas_k[i, 2]);
            }
            if (k != sum)
            {
                //throw new Exception("Error");
            }

            x2 = x1;
            y2 = mas_k[0, 0] * y1;
            z2 = z1;
            t2 = t1;
            r3 = r1;
            r4 = r2;
            for (int i = 0; i < mas_k.GetLength(0) - 1; i++)
            {
                BigInteger u = mas_k[i, 1] - mas_k[i + 1, 1];
                BigInteger v = mas_k[i, 2] - mas_k[i + 1, 2];
                ops.opElementsAdd(2);
                for (int j = 0; j < u; j++)
                {
                    ops.opPointsDoubling();
                    switch (type)
                    {
                        case 4: Double_ModifiedJacoby_Coord(x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                        case 3: Double_JacobyChudnovskii_Coord(x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                        case 2:
                        case 1:
                        case 0:
                            DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2); break;
                    }
                }

                for (int j = 0; j < v; j++)
                {
                    switch (type)
                    {
                        case 4: Ternary.Ternary_ModifiedJacoby(x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                        case 3: Ternary.Ternary_ChudnovskiiJacoby(x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                        case 2:
                        case 1:
                        case 0:
                            TernaryList[type](x2, y2, z2, a, p, out x2, out y2, out z2); break;
                    }
                }
                ops.opElementsAdd();
                switch (type)
                {
                    case 4:
                        Add_ModifiedJacoby_Coord(x1, mas_k[i + 1, 0] * y1, z1, t1, x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                    case 3:
                        Add_JacobyChudnovskii_Coord(x1, mas_k[i + 1, 0] * y1, z1, r1, r2, x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                    case 0:
                    case 1:
                    case 2:
                        AddList[type](x1, mas_k[i + 1, 0] * y1, z1, x2, y2, z2, a, p, out x2, out y2, out z2); break;
                }

            }

            for (BigInteger i = 0; i < mas_k[mas_k.GetLength(0) - 1, 1]; i++)
            {
                ops.opPointsDoubling();
                switch (type)
                {
                    case 4: Double_ModifiedJacoby_Coord(x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                    case 3: Double_JacobyChudnovskii_Coord(x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                    case 2:
                    case 1:
                    case 0:
                        DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2); break;
                }
            }

            for (BigInteger i = 0; i < mas_k[mas_k.GetLength(0) - 1, 2]; i++)
            {
                switch (type)
                {
                    case 4: Ternary.Ternary_ModifiedJacoby(x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                    case 3: Ternary.Ternary_ChudnovskiiJacoby(x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                    case 2:
                    case 1:
                    case 0:
                        TernaryList[type](x2, y2, z2, a, p, out x2, out y2, out z2); break;
                }
            }

            if (x2 == 0 && y2 != 0)
            {
                z2 = 0;
            }

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_20_2(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, BigInteger a_max, BigInteger b_max, OperationsCounter ops)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            BigInteger[,] mas_k;
            BigInteger t1 = a * BigInteger.Pow(z1, 4); BigInteger t2 = 0;
            BigInteger r1 = z1 * z1;
            BigInteger r2 = z1 * z1 * z1;
            BigInteger r3 = 0, r4 = 0;
            ops.opElementsMultiply(4);

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            mas_k = Functions.Convert_to_DBNS_2(k, a_max, b_max);
            BigInteger sum = 0;
            for (int i = 0; i < mas_k.GetLength(0); i++)
            {
                sum = sum + mas_k[i, 0] * Functions.Pow(2, mas_k[i, 1]) * Functions.Pow(3, mas_k[i, 2]);
            }
            if (k != sum)
            {
                //throw new Exception("Error");
            }
            x2 = x1;
            y2 = mas_k[0, 0] * y1;
            z2 = z1;
            t2 = t1;
            r3 = r1;
            r4 = r2;
            for (int i = 0; i < mas_k.GetLength(0) - 1; i++)
            {
                BigInteger u = mas_k[i, 1] - mas_k[i + 1, 1];
                BigInteger v = mas_k[i, 2] - mas_k[i + 1, 2];
                ops.opElementsAdd(2);
                for (int j = 0; j < u; j++)
                {
                    ops.opPointsDoubling();
                    switch (type)
                    {
                        case 4: Double_ModifiedJacoby_Coord(x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                        case 3: Double_JacobyChudnovskii_Coord(x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                        case 2:
                        case 1:
                        case 0:
                            DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2); break;
                    }
                }
                for (int j = 0; j < v; j++)
                {
                    switch (type)
                    {
                        case 4: Ternary.Ternary_ModifiedJacoby(x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                        case 3: Ternary.Ternary_ChudnovskiiJacoby(x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                        case 2:
                        case 1:
                        case 0:
                            TernaryList[type](x2, y2, z2, a, p, out x2, out y2, out z2); break;
                    }
                }
                ops.opElementsAdd();
                switch (type)
                {
                    case 4:
                        Add_ModifiedJacoby_Coord(x1, mas_k[i + 1, 0] * y1, z1, t1, x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                    case 3:
                        Add_JacobyChudnovskii_Coord(x1, mas_k[i + 1, 0] * y1, z1, r1, r2, x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                    case 0:
                    case 1:
                    case 2:
                        AddList[type](x1, mas_k[i + 1, 0] * y1, z1, x2, y2, z2, a, p, out x2, out y2, out z2); break;
                }
            }
            for (BigInteger i = 0; i < mas_k[mas_k.GetLength(0) - 1, 1]; i++)
            {
                ops.opPointsDoubling();
                switch (type)
                {
                    case 4: Double_ModifiedJacoby_Coord(x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                    case 3: Double_JacobyChudnovskii_Coord(x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                    case 2:
                    case 1:
                    case 0:
                        DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2); break;
                }
            }
            for (BigInteger i = 0; i < mas_k[mas_k.GetLength(0) - 1, 2]; i++)
            {
                switch (type)
                {
                    case 4: Ternary.Ternary_ModifiedJacoby(x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                    case 3: Ternary.Ternary_ChudnovskiiJacoby(x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                    case 2:
                    case 1:
                    case 0:
                        TernaryList[type](x2, y2, z2, a, p, out x2, out y2, out z2); break;
                }
            }
            if (x2 == 0 && y2 != 0)
            {
                z2 = 0;
            }
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_22(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x3, out BigInteger y3, out BigInteger z3, int type, out double time, int w = 0, OperationsCounter ops = null)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            BigInteger[,] mas_k;
            BigInteger t1 = a * BigInteger.Pow(z1, 4);
            BigInteger t2 = 0;
            BigInteger r1 = z1 * z1;
            BigInteger r2 = z1 * z1 * z1;
            BigInteger r3 = 0, r4 = 0;
            ops.opElementsMultiply(4);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            mas_k = Functions.Convert_to_DBNS1(k);
            BigInteger sum = 1;
            for (int i = mas_k.GetLength(0) - 1; i >= 0; i--)
            {
                ops.opElementsMultiply(3);
                sum = sum * Functions.Pow(2, mas_k[i, 1]) * Functions.Pow(3, mas_k[i, 2]) + mas_k[i, 0];
            }

            if (k != sum)
            {
                throw new Exception("Error");
            }
            x3 = x1;
            y3 = y1;
            z3 = z1;
            for (int i = mas_k.GetLength(0) - 1; i >= 1; i--)
            {
                BigInteger u = mas_k[i, 1];
                BigInteger v = mas_k[i, 2];
                for (int j = 0; j < u; j++)
                {
                    ops.opPointsDoubling();
                    switch (type)
                    {
                        case 4: Double_ModifiedJacoby_Coord(x3, y3, z3, t2, a, p, out x3, out y3, out z3, out t2); break;
                        case 3: Double_JacobyChudnovskii_Coord(x3, y3, z3, r3, r4, a, p, out x3, out y3, out z3, out r3, out r4); break;
                        case 2:
                        case 1:
                        case 0:
                            DoubleList[type](x3, y3, z3, a, p, out x3, out y3, out z3); break;
                    }
                }
                for (int j = 0; j < v; j++)
                {
                    switch (type)
                    {
                        case 4: Ternary.Ternary_ModifiedJacoby(x3, y3, z3, t2, a, p, out x3, out y3, out z3, out t2); break;
                        case 3: Ternary.Ternary_ChudnovskiiJacoby(x3, y3, z3, r3, r4, a, p, out x3, out y3, out z3, out r3, out r4); break;
                        case 2:
                        case 1:
                        case 0:
                            TernaryList[type](x3, y3, z3, a, p, out x3, out y3, out z3); break;
                    }
                }
                ops.opPointsAdd();
                switch (type)
                {
                    case 4:
                        Add_ModifiedJacoby_Coord(x1, mas_k[i, 0] * y1, z1, t1, x3, y3, z3, t2, a, p, out x3, out y3, out z3, out t2); break;
                    case 3:
                        Add_JacobyChudnovskii_Coord(x1, mas_k[i, 0] * y1, z1, r1, r2, x3, y3, z3, r3, r4, a, p, out x3, out y3, out z3, out r3, out r4); break;
                    case 0:
                    case 1:
                    case 2:
                        AddList[type](x1, mas_k[i, 0] * y1, z1, x3, y3, z3, a, p, out x3, out y3, out z3); break;
                }
            }
            for (BigInteger i = 0; i < mas_k[0, 1]; i++)
            {
                ops.opPointsDoubling();
                switch (type)
                {
                    case 4: Double_ModifiedJacoby_Coord(x3, y3, z3, t2, a, p, out x3, out y3, out z3, out t2); break;
                    case 3: Double_JacobyChudnovskii_Coord(x3, y3, z3, r3, r4, a, p, out x3, out y3, out z3, out r3, out r4); break;
                    case 2:
                    case 1:
                    case 0:
                        DoubleList[type](x3, y3, z3, a, p, out x3, out y3, out z3); break;
                }
            }
            for (BigInteger i = 0; i < mas_k[0, 2]; i++)
            {
                switch (type)
                {
                    case 4: Ternary.Ternary_ModifiedJacoby(x3, y3, z3, t2, a, p, out x3, out y3, out z3, out t2); break;
                    case 3: Ternary.Ternary_ChudnovskiiJacoby(x3, y3, z3, r3, r4, a, p, out x3, out y3, out z3, out r3, out r4); break;
                    case 2:
                    case 1:
                    case 0:
                        TernaryList[type](x3, y3, z3, a, p, out x3, out y3, out z3); break;
                }
            }
            if (x3 == 0 && y3 != 0)
            {
                z3 = 0;
            }
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_21(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x3, out BigInteger y3, out BigInteger z3, int type, out double time, int w = 0, OperationsCounter ops = null)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            BigInteger[,] mas_k;
            BigInteger t1 = a * BigInteger.Pow(z1, 4); BigInteger t2 = 0;
            BigInteger r1 = z1 * z1;
            BigInteger r2 = z1 * z1 * z1;
            BigInteger r3 = 0, r4 = 0;
            ops.opElementsMultiply(4);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            //mas_k = Functions.Convert_to_DBNS_1(k, a_max, b_max);
            mas_k = Functions.Convert_to_DBNS(k);
            BigInteger sum = 0, temp = 1;
            for (int i = 0; i < mas_k.GetLength(0); i++)
            {
                temp = temp * Functions.Pow(2, mas_k[i, 1]) * Functions.Pow(3, mas_k[i, 2]);
                sum = sum + mas_k[i, 0] * temp;
                ops.opElementsMultiply(4);
                ops.opElementsAdd();
            }
            if (k != sum)
            {
                throw new Exception("Error");
            }

            x3 = 0;
            y3 = 1;
            z3 = 0;

            for (int i = 0; i < mas_k.GetLength(0); i++)
            {
                BigInteger u = mas_k[i, 1];
                BigInteger v = mas_k[i, 2];
                for (int j = 0; j < u; j++)
                {
                    ops.opPointsDoubling();
                    switch (type)
                    {
                        case 4:
                            Double_ModifiedJacoby_Coord(x1, y1, z1, t1, a, p, out x1, out y1, out z1, out t1); break;
                        case 3:
                            Double_JacobyChudnovskii_Coord(x1, y1, z1, r1, r2, a, p, out x1, out y1, out z1, out r1, out r2); break;
                        case 2:
                        case 1:
                        case 0:
                            DoubleList[type](x1, y1, z1, a, p, out x1, out y1, out z1); break;
                    }
                }

                for (int j = 0; j < v; j++)
                {
                    switch (type)
                    {
                        case 4:
                            Ternary.Ternary_ModifiedJacoby(x1, y1, z1, t1, a, p, out x1, out y1, out z1, out t1); break;
                        case 3:
                            Ternary.Ternary_ChudnovskiiJacoby(x1, y1, z1, r1, r2, a, p, out x1, out y1, out z1, out r1, out r2); break;
                        case 2:
                        case 1:
                        case 0:
                            TernaryList[type](x1, y1, z1, a, p, out x1, out y1, out z1); break;
                    }
                }
                ops.opPointsAdd();
                switch (type)
                {
                    case 4:
                        Add_ModifiedJacoby_Coord(x1, mas_k[i, 0] * y1, z1, t1, x3, y3, z3, t2, a, p, out x3, out y3, out z3, out t2); break;
                    case 3:
                        Add_JacobyChudnovskii_Coord(x1, mas_k[i, 0] * y1, z1, r1, r2, x3, y3, z3, r3, r4, a, p, out x3, out y3, out z3, out r3, out r4); break;
                    case 0:
                    case 1:
                    case 2:
                        AddList[type](x1, mas_k[i, 0] * y1, z1, x3, y3, z3, a, p, out x3, out y3, out z3); break;
                }
            }
            if (x3 == 0 && y3 != 0)
            {
                z3 = 0;
            }
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_21m(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
           out BigInteger x3, out BigInteger y3, out BigInteger z3, int type, out double time, int w = 0, OperationsCounter ops = null)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            BigInteger[,] mas_k;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            //mas_k = Functions.Convert_to_DBNS_1(k, a_max, b_max);
            mas_k = Functions.Convert_to_MBNR(k);
            BigInteger sum = 0, temp = 1;
            for (int i = 0; i < mas_k.GetLength(0); i++)
            {
                temp = temp * Functions.Pow(5, mas_k[i, 1]) * Functions.Pow(3, mas_k[i, 2]) * Functions.Pow(2, mas_k[i,3]);
                sum = sum + mas_k[i, 0] * temp;
                ops.opElementsMultiply(4);
                ops.opElementsAdd();
            }
            if (k != sum)
            {
                throw new Exception("Error");
            }
            x3 = 0;
            y3 = 1;
            z3 = 0;
            for (int i = 0; i < mas_k.GetLength(0); i++)
            {
                BigInteger t = mas_k[i, 1];
                BigInteger v = mas_k[i, 2];
                BigInteger u = mas_k[i, 3];
                for (int l = 0; l < t; l++)
                {
                    QuintupleList[type](x1, y1, z1, a, p, out x1, out y1, out z1);
                }         
                for (int j = 0; j < v; j++)
                {
                    TernaryList[type](x1, y1, z1, a, p, out x1, out y1, out z1);
                }
                for (int j = 0; j < u; j++)
                {
                    ops.opPointsDoubling();
                    DoubleList[type](x1, y1, z1, a, p, out x1, out y1, out z1);
                }
                ops.opPointsAdd();  
                AddList[type](x1, mas_k[i, 0] * y1, z1, x3, y3, z3, a, p, out x3, out y3, out z3);
            }
            if (x3 == 0 && y3 != 0)
            {
                z3 = 0;
            }
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_22m(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x3, out BigInteger y3, out BigInteger z3, int type, out double time, int w = 0, OperationsCounter ops = null)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            BigInteger[,] mas_k;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            //mas_k = Functions.Convert_to_DBNS_1(k, a_max, b_max);
            mas_k = Functions.Convert_to_MBNR(k);
            BigInteger sum = 1;    
            for (int i = mas_k.GetLength(0) - 1; i >= 0; i--)
            {      
                sum = sum * Functions.Pow(2, mas_k[i, 1]) * Functions.Pow(3, mas_k[i, 2]) * Functions.Pow(5, mas_k[i, 3]) + mas_k[i, 0];               
            }
            if (k != sum)
            {
               // throw new Exception("Error");
            }
            x3 = x1;
            y3 = y1;
            z3 = z1;
            for (int i = mas_k.GetLength(0) - 1; i >= 1; i--)
            {
                BigInteger u = mas_k[i, 1];
                BigInteger v = mas_k[i, 2];
                BigInteger t = mas_k[i, 3];
                for (int j = 0; j < u; j++)
                {
                    ops.opPointsDoubling();
                    DoubleList[type](x3, y3, z3, a, p, out x3, out y3, out z3);
                }
                for (int j = 0; j < v; j++)
                {
                    TernaryList[type](x3, y3, z3, a, p, out x3, out y3, out z3);
                }
                for (int l = 0; l < t; l++)
                {
                    QuintupleList[type](x3, y3, z3, a, p, out x3, out y3, out z3);
                }
                ops.opPointsAdd();
                AddList[type](x1, mas_k[i, 0] * y1, z1, x3, y3, z3, a, p, out x3, out y3, out z3);
            }
            for (BigInteger i = 0; i < mas_k[0, 1]; i++)
            {
                ops.opPointsDoubling();
                DoubleList[type](x3, y3, z3, a, p, out x3, out y3, out z3);
            }
            for (BigInteger i = 0; i < mas_k[0, 2]; i++)
            {
                TernaryList[type](x3, y3, z3, a, p, out x3, out y3, out z3);
            }
            for (BigInteger i = 0; i < mas_k[0, 3]; i++)
            {
                QuintupleList[type](x3, y3, z3, a, p, out x3, out y3, out z3);
            }
            if (x3 == 0 && y3 != 0)
            {
                z3 = 0;
            }
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        #region MBNS methods with trees
        /*Do not work correct*/
        public static void Point_Multiplication_Affine_Coord_27(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, BigInteger B, BigInteger[] S, BigInteger[] M, int type, out double time, OperationsCounter ops = null)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            ECC e = new ECC();
            BigInteger[,] PreComputation = new BigInteger[S.Length, 3];
            Tree tree;
            BigInteger t1 = a * BigInteger.Pow(z1, 4); BigInteger t2 = 0;
            BigInteger r1 = z1 * z1;
            BigInteger r2 = z1 * z1 * z1;
            BigInteger r3 = 0, r4 = 0;
            List<Tree.DecompositonItem> decomposition = new List<Tree.DecompositonItem>();
            if (e.Flag)
            {
                tree = new Tree(k, S, M, B);
                decomposition = tree.GetDecomposition();

                for (int i = 0; i < S.Length; i++)
                {
                    double iterationTime = 0;
                    Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, S[i], p, out x2, out y2, out z2, type, out iterationTime);
                    PreComputation[i, 0] = x2;
                    PreComputation[i, 1] = y2;
                    PreComputation[i, 2] = z2;
                }

                e.Flag = false;
            }
            x2 = 0;
            y2 = 1;
            z2 = 0;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            foreach (var i in decomposition)
            {             
                for (int j = 0; j < M.Length; ++j) // j - рядки pows
                {
                    for (int jj = 0; jj < i.pows[j, 1]; ++jj) // забезпечує множення точки задану к-сть разів на елемент множини М
                    {
                        for (int kk = 0; kk < S.Length; kk++) // перебирає рядки PreComputation
                        {
                            double iterationTime = 0;
                            Point_Multiplication_Affine_Coord_1(PreComputation[kk, 0], PreComputation[kk, 1], PreComputation[kk, 2], a, (int)i.pows[j, 0], p, out PreComputation[kk, 0], out PreComputation[kk, 1], out PreComputation[kk, 2], type, out iterationTime);
                        }
                    }
                }
                if (i.offset != 0)
                {
                    Int16 b = (Int16)(BigInteger.Abs(i.offset) - 1);
                    ops.opPointsAdd();
                    switch (type)
                    {
                        case 4:
                            Add_ModifiedJacoby_Coord(x2, y2, z2, t2, PreComputation[b, 0], PreComputation[b, 1] * i.offset.Sign, PreComputation[b, 2], t1, a, p, out x2, out y2, out z2, out t2); break;
                        case 3:
                            Add_JacobyChudnovskii_Coord(x2, y2, z2, r3, r4, PreComputation[b, 0], PreComputation[b, 1] * i.offset.Sign, PreComputation[b, 2], r1, r2, a, p, out x2, out y2, out z2, out r3, out r4); break;
                        case 0:
                        case 1:
                        case 2:
                            Add_Affine_Coord(x2, y2, z2, PreComputation[b, 0], PreComputation[b, 1] * i.offset.Sign, PreComputation[b, 2], a, p, out x2, out y2, out z2); break;
                    }
                }
            }
            ops.opPointsAdd();
            switch (type)
            {
                case 4:
                    Add_ModifiedJacoby_Coord(x2, y2, z2, t2, PreComputation[0, 0], PreComputation[0, 1], PreComputation[0, 2], t1, a, p, out x2, out y2, out z2, out t2); break;
                case 3:
                    Add_JacobyChudnovskii_Coord(x2, y2, z2, r3, r4, PreComputation[0, 0], PreComputation[0, 1], PreComputation[0, 2], r1, r2, a, p, out x2, out y2, out z2, out r3, out r4); break;
                case 0:
                case 1:
                case 2:
                    Add_Affine_Coord(x2, y2, z2, PreComputation[0, 0], PreComputation[0, 1], PreComputation[0, 2], a, p, out x2, out y2, out z2); break;
            }
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_28(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, BigInteger B, BigInteger[] S, BigInteger[] M, int type, out double time, OperationsCounter ops = null)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            var tree = new Tree(k, S, M, B);
            var decomposition = tree.GetDecomposition();
            BigInteger[,] PreComputation = new BigInteger[S.Length, 3];
            BigInteger t1 = a * BigInteger.Pow(z1, 4);
            BigInteger t2 = 0;
            BigInteger r1 = z1 * z1;
            BigInteger r2 = z1 * z1 * z1;
            BigInteger r3 = 0, r4 = 0;
            ops.opElementsMultiply(4);
            for (int i = 0; i < S.Length; i++)
            {
                double iterationTime = 0;
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, S[i], p, out x2, out y2, out z2, type, out iterationTime);
                PreComputation[i, 0] = x2;
                PreComputation[i, 1] = y2;
                PreComputation[i, 2] = z2;
            }
            x2 = x1;
            y2 = y1;
            z2 = z1;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            foreach (var i in decomposition)
            {
                for (int j = 0; j < M.Length; ++j)
                {
                    for (int jj = 0; jj < i.pows[j, 1]; ++jj)
                    {
                        double iterationTime = 0;
                        Point_Multiplication_Affine_Coord_1(x2, y2, z2, a, (int)i.pows[j, 0], p, out x2, out y2, out z2, type, out iterationTime);
                    }
                }
                if (i.offset != 0)
                {
                    Int16 b = (Int16)(BigInteger.Abs(i.offset) - 1);
                    ops.opPointsAdd();
                    switch (type)
                    {
                        case 4:
                            Add_ModifiedJacoby_Coord(x2, y2, z2, t2, PreComputation[b, 0], PreComputation[b, 1] * i.offset.Sign, PreComputation[b, 2], t1, a, p, out x2, out y2, out z2, out t2); break;
                        case 3:
                            Add_JacobyChudnovskii_Coord(x2, y2, z2, r3, r4, PreComputation[b, 0], PreComputation[b, 1] * i.offset.Sign, PreComputation[b, 2], r1, r2, a, p, out x2, out y2, out z2, out r3, out r4); break;
                        case 0:
                        case 1:
                        case 2:
                            Add_Affine_Coord(x2, y2, z2, PreComputation[b, 0], PreComputation[b, 1] * i.offset.Sign, PreComputation[b, 2], a, p, out x2, out y2, out z2); break;
                    }
                }
            }
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }
        #endregion

        public static void Point_Multiplication_Affine_Coord_29(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
           out BigInteger x2, out BigInteger y2, out BigInteger z2, int type, out double time, int w = 0, OperationsCounter ops = null)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            int count_bit = (int)(Math.Floor(BigInteger.Log(k, 2))) + 1;
            int m = count_bit;
            w = m;
            BigInteger t1 = a * BigInteger.Pow(z1, 4);
            BigInteger t2 = 0;
            BigInteger r1 = z1 * z1;
            BigInteger r2 = z1 * z1 * z1;
            BigInteger r3 = 0, r4 = 0;
            BigInteger[,] PreComputation = new BigInteger[w, 3];
            for (int i = 0; i < w; i++)
            {
                double iterationTime = 0;
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, BigInteger.Pow(2, i), p, out x2, out y2, out z2, type, out iterationTime);
                PreComputation[i, 0] = x2;
                PreComputation[i, 1] = y2;
                PreComputation[i, 2] = z2;
            }
            x2 = 0;
            y2 = 1;
            z2 = 0;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            string str = new string(ToBin(k).ToArray());
            while (m > 0)
            {
                int sizeStr = str.Length;
                string newStr = null;
                for (int i = 0; i < sizeStr; i++)
                {
                    if(str[i] == '1')
                    {
                        newStr += str[i];
                        if (sizeStr == 1) break;
                        else
                        {
                            if (str[i + 1] == '1') break;
                            else
                            {
                                for (int j = i + 1; j < sizeStr; j++)
                                {
                                    if (str[j] == '0')
                                    {
                                        newStr += str[i + 1];
                                    }
                                    else break;
                                }
                                break;
                            }
                        }
                    }
                }
                int sizeNewStr = newStr.Length;
                str = str.Substring(sizeNewStr);
                for (int j = 0; j < sizeNewStr; j++)
                {
                    ops.opPointsDoubling();
                    switch(type)
                    {
                        case 4:
                            Double_ModifiedJacoby_Coord(x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                        case 3:
                            Double_JacobyChudnovskii_Coord(x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                        case 2:
                        case 1:
                        case 0:
                            DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2); break;
                    }              
                }
                ops.opPointsAdd();
                switch(type)
                {
                    case 4:
                        Add_ModifiedJacoby_Coord(x2, y2, z2, t2, PreComputation[sizeNewStr - 1, 0], PreComputation[sizeNewStr - 1, 1], PreComputation[sizeNewStr - 1, 2], t1, a, p, out x2, out y2, out z2, out t2); break;
                    case 3:
                        Add_JacobyChudnovskii_Coord(x2, y2, z2, r3, r4, PreComputation[sizeNewStr - 1, 0], PreComputation[sizeNewStr - 1, 1], PreComputation[sizeNewStr - 1, 2], r1, r2, a, p, out x2, out y2, out z2, out r3, out r4); break;
                    case 2:
                    case 1:
                    case 0:
                        AddList[type](x2, y2, z2, PreComputation[sizeNewStr - 1, 0], PreComputation[sizeNewStr - 1, 1], PreComputation[sizeNewStr - 1, 2], a, p, out x2, out y2, out z2); break;
                }
                m = m - sizeNewStr;
            }
            if (x2 == 0 && y2 == 1)
                z2 = 0;           
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds; 
        }

        public static void Point_Multiplication_Affine_Coord_30(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, int type, out double time, int w = 0, OperationsCounter ops = null)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            int count_bit = (int)(Math.Floor(BigInteger.Log(k, 2))) + 1;
            int m = count_bit;
            w = m;
            BigInteger t1 = a * BigInteger.Pow(z1, 4);
            BigInteger t2 = 0;
            BigInteger r1 = z1 * z1;
            BigInteger r2 = z1 * z1 * z1;
            BigInteger r3 = 0, r4 = 0;
            BigInteger[,] PreComputation = new BigInteger[w, 3];
            ops.opElementsMultiply(4);
            for (int i = 0; i < w; i++)
            {
                double iterationTime = 0;
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, BigInteger.Pow(2, i+1)-1, p, out x2, out y2, out z2, type, out iterationTime);
                PreComputation[i, 0] = x2;
                PreComputation[i, 1] = y2;
                PreComputation[i, 2] = z2;
            }
            x2 = 0;
            y2 = 1;
            z2 = 0;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            string str = new string(ToBin(k).ToArray());
            int sizeNewStr = 0; 
            while (m > 0)
            {
                int sizeStr = str.Length;
                string newStr = null;
                for (int i = 0; i < sizeStr; i++)
                {
                    if (str[i] == '1')
                    {
                        newStr += str[i];
                    }
                    else break;
                   
                }
                if (newStr != null)
                {
                    sizeNewStr = newStr.Length;
                }
                else
                {
                    newStr += '0';
                    sizeNewStr = newStr.Length;
                }

                str = str.Substring(sizeNewStr);

                for (int j = 0; j < sizeNewStr; j++)
                {
                    ops.opPointsDoubling();
                    switch (type)
                    {
                        case 4:
                            Double_ModifiedJacoby_Coord(x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                        case 3:
                            Double_JacobyChudnovskii_Coord(x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                        case 2:
                        case 1:
                        case 0:
                            DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2); break;
                    }
                }         
                newStr.ToArray();
                if (newStr[0] != '0')
                {
                    ops.opPointsAdd();
                    switch (type)
                    {
                        case 4:
                            Add_ModifiedJacoby_Coord(x2, y2, z2, t1, PreComputation[sizeNewStr - 1, 0], PreComputation[sizeNewStr - 1, 1], PreComputation[sizeNewStr - 1, 2], t2, a, p, out x2, out y2, out z2, out t2); break;
                        case 3:
                            Add_JacobyChudnovskii_Coord(x2, y2, z2, r1, r2, PreComputation[sizeNewStr - 1, 0], PreComputation[sizeNewStr - 1, 1], PreComputation[sizeNewStr - 1, 2], r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                        case 0:
                        case 1:
                        case 2:
                            AddList[type](x2, y2, z2, PreComputation[sizeNewStr - 1, 0], PreComputation[sizeNewStr - 1, 1], PreComputation[sizeNewStr - 1, 2], a, p, out x2, out y2, out z2); break;
                    }
                }
                m = m - sizeNewStr;
            }
            if (x2 == 0 && y2 == 1)
                z2 = 0;
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_31(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, int type, out double time, int w = 0, OperationsCounter ops = null)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            int count_bit = (int)(Math.Floor(BigInteger.Log(k, 2))) + 1;
            int m = count_bit;
            w = m;
            BigInteger t1 = a * BigInteger.Pow(z1, 4);
            BigInteger t2 = 0;
            BigInteger r1 = z1 * z1;
            BigInteger r2 = z1 * z1 * z1;
            BigInteger r3 = 0, r4 = 0;
            BigInteger[,] PreComputation = new BigInteger[w, 3];
            PreComputation[0, 0] = x1;
            PreComputation[0, 1] = y1;
            PreComputation[0, 2] = z1;
              
            for (int i = 1; i < w; i++)
            {
                double iterationTime = 0;
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, BigInteger.Pow(2, i) + 1, p, out x2, out y2, out z2, type, out iterationTime);
                PreComputation[i, 0] = x2;
                PreComputation[i, 1] = y2;
                PreComputation[i, 2] = z2;
            }
            x2 = 0;
            y2 = 1;
            z2 = 0;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            string str = new string(ToBin(k).ToArray());
            while (m > 0)
            {    
                int sizeNewStr = 0;
                int sizeStr = str.Length;
                string newStr = null;
                for (int i = 0; i < sizeStr; i++)
                {
                    if (str[i] == '1')
                    {
                        newStr += str[i];
                        for (int j = i + 1; j < sizeStr; j++)
                        {
                            if (str[j] == '0')
                            {
                                newStr += str[j];
                                if (j == sizeStr - 1)
                                {
                                    newStr = null;
                                    newStr += '1';
                                    break;
                                }
                    
                            }
                            else
                            {
                                newStr += str[j];
                                break;
                            }
                        }
                        break;
                    }
                    else
                    {
                        newStr += str[i];
                        break;
                    }
                }
                sizeNewStr = newStr.Length;
               
                str = str.Substring(sizeNewStr);
                sizeStr = str.Length;

                for (int j = 0; j < sizeNewStr; j++)
                {
                    ops.opPointsDoubling();
                    switch (type)
                    {
                        case 4:
                            Double_ModifiedJacoby_Coord(x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                        case 3:
                            Double_JacobyChudnovskii_Coord(x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                        case 2:
                        case 1:
                        case 0:
                            DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2); break;
                    }
                }

                newStr.ToArray();
                if (newStr[0] != '0')
                {
                    ops.opPointsAdd();
                    switch (type)
                    {
                        case 4:
                            Add_ModifiedJacoby_Coord(PreComputation[sizeNewStr - 1, 0], PreComputation[sizeNewStr - 1, 1], PreComputation[sizeNewStr - 1, 2], t1, x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                        case 3:
                            Add_JacobyChudnovskii_Coord(PreComputation[sizeNewStr - 1, 0], PreComputation[sizeNewStr - 1, 1], PreComputation[sizeNewStr - 1, 2], r1, r2, x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                        case 2:
                        case 1:
                        case 0:
                            AddList[type](PreComputation[sizeNewStr - 1, 0], PreComputation[sizeNewStr - 1, 1], PreComputation[sizeNewStr - 1, 2], x2, y2, z2, a, p, out x2, out y2, out z2); break;
                    }
                }
                m = m - sizeNewStr;
            }
            if (x2 == 0 && y2 == 1)
                z2 = 0;

          stopWatch.Stop();
          TimeSpan ts = stopWatch.Elapsed;
          time = ts.TotalMilliseconds;            
        }

        public static void Point_Multiplication_Affine_Coord_32(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, int type, out double time, int w = 0, OperationsCounter ops = null)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            string str = new string(ToBin(k).ToArray());
            int count_bit = (int)(Math.Floor(BigInteger.Log(k, 2))) + 1;
            int m = count_bit;
            BigInteger t1 = a * BigInteger.Pow(z1, 4);
            BigInteger t2 = 0;
            BigInteger r1 = z1 * z1;
            BigInteger r2 = z1 * z1 * z1;
            BigInteger r3 = 0, r4 = 0;
            BigInteger[,] PreComputationFor30 = new BigInteger[count_bit, 3];
            BigInteger[,] PreComputationFor31 = new BigInteger[count_bit, 3];
            ops.opElementsMultiply(4);
            for (int i = 0; i < m; i++)
            {
                double iterationTime = 0;
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, BigInteger.Pow(2, i + 1) - 1, p, out x2, out y2, out z2, type, out iterationTime);
                PreComputationFor30[i, 0] = x2;
                PreComputationFor30[i, 1] = y2;
                PreComputationFor30[i, 2] = z2;
            }
            PreComputationFor31[0, 0] = x1;
            PreComputationFor31[0, 1] = y1;
            PreComputationFor31[0, 2] = z1;
            for (int i = 1; i < m; i++)
            {
                double iterationTime = 0;
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, BigInteger.Pow(2, i) + 1, p, out x2, out y2, out z2, type, out iterationTime);
                PreComputationFor31[i, 0] = x2;
                PreComputationFor31[i, 1] = y2;
                PreComputationFor31[i, 2] = z2;
            }   
            x2 = 0;
            y2 = 1;
            z2 = 0;

            /*Start Time fixing*/
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            while (m > 0)
            {
                int sizeNewStr = 0;
                int sizeStr = str.Length;
                string newStr = null;
                if (sizeStr == 1)
                {
                    newStr += str[sizeStr - 1];
                    sizeNewStr = newStr.Length;
                    str = str.Substring(sizeNewStr);

                    for (int j = 0; j < sizeNewStr; j++)
                    {
                        ops.opPointsDoubling();
                        switch (type)
                        {
                            case 4:
                                Double_ModifiedJacoby_Coord(x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                            case 3:
                                Double_JacobyChudnovskii_Coord(x2, y2, z2, r3, r3, a, p, out x2, out y2, out z2, out r3, out r4); break;
                            case 2:
                            case 1:
                            case 0:
                                DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2); break;
                        }
                    }
                    if (newStr[0] == '1')
                    {
                        ops.opPointsAdd();
                        switch (type)
                        {
                            case 4:
                                Add_ModifiedJacoby_Coord(x2, y2, z2, t1, PreComputationFor30[sizeNewStr - 1, 0], PreComputationFor30[sizeNewStr - 1, 1], PreComputationFor30[sizeNewStr - 1, 2], t2, a, p, out x2, out y2, out z2, out t2); break;
                            case 3:
                                Add_JacobyChudnovskii_Coord(x2, y2, z2, r1, r2, PreComputationFor30[sizeNewStr - 1, 0], PreComputationFor30[sizeNewStr - 1, 1], PreComputationFor30[sizeNewStr - 1, 2], r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                            case 2:
                            case 1:
                            case 0:
                                AddList[type](x2, y2, z2, PreComputationFor30[sizeNewStr - 1, 0], PreComputationFor30[sizeNewStr - 1, 1], PreComputationFor30[sizeNewStr - 1, 2], a, p, out x2, out y2, out z2); break;
                        }
                    }
                    m = m - sizeNewStr;
                }
                else
                {
                    if (str[0] == '0')
                    {
                        for (int i = 0; i < sizeStr; i++)
                        {
                            if (str[i] != '1')
                            {
                                newStr += str[i];
                            }
                            else break;
                        }
                        sizeNewStr = newStr.Length;
                        str = str.Substring(sizeNewStr);

                        for (int j = 0; j < sizeNewStr; j++)
                        {
                            ops.opPointsDoubling();
                            switch (type)
                            {
                                case 4:
                                    Double_ModifiedJacoby_Coord(x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                                case 3:
                                    Double_JacobyChudnovskii_Coord(x2, y2, z2, r3, r3, a, p, out x2, out y2, out z2, out r3, out r4); break;
                                case 2:
                                case 1:
                                case 0:
                                    DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2); break;
                            }
                        }
                        m = m - sizeNewStr;
                    }
                    else
                    {
                        // algorythm # 30
                        if (str[1] == '1')
                        {
                            for (int i = 0; i < sizeStr; i++)
                            {
                                if (str[i] == '1')
                                {
                                    newStr += str[i];
                                }
                                else break;
                            }

                            sizeNewStr = newStr.Length;
                            str = str.Substring(sizeNewStr);

                            for (int j = 0; j < sizeNewStr; j++)
                            {
                                ops.opPointsDoubling();
                                switch (type)
                                {
                                    case 4:
                                        Double_ModifiedJacoby_Coord(x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                                    case 3:
                                        Double_JacobyChudnovskii_Coord(x2, y2, z2, r3, r3, a, p, out x2, out y2, out z2, out r3, out r4); break;
                                    case 2:
                                    case 1:
                                    case 0:
                                        DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2); break;
                                }
                            }
                            ops.opPointsAdd();
                            switch (type)
                            {
                                case 4:
                                    Add_ModifiedJacoby_Coord(x2, y2, z2, t2, PreComputationFor30[sizeNewStr - 1, 0], PreComputationFor30[sizeNewStr - 1, 1], PreComputationFor30[sizeNewStr - 1, 2], t2, a, p, out x2, out y2, out z2, out t2); break;
                                case 3:
                                    Add_JacobyChudnovskii_Coord(x2, y2, z2, r1, r2, PreComputationFor30[sizeNewStr - 1, 0], PreComputationFor30[sizeNewStr - 1, 1], PreComputationFor30[sizeNewStr - 1, 2], r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                                case 2:
                                case 1:
                                case 0:
                                    AddList[type](x2, y2, z2, PreComputationFor30[sizeNewStr - 1, 0], PreComputationFor30[sizeNewStr - 1, 1], PreComputationFor30[sizeNewStr - 1, 2], a, p, out x2, out y2, out z2); break;
                            }
                            m = m - sizeNewStr;
                        }

                        // algorythm #31
                        else
                        {                  
                            newStr += str[0];
                            if (m != 2)
                            {
                                newStr += str[1];
                                for (int j = 2; j < sizeStr; j++)
                                {
                                    if (str[j] == '0')
                                    {
                                        newStr += str[j];
                                        if (j == sizeStr - 1)
                                        {
                                            newStr = null;
                                            newStr += '1';
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        newStr += str[j];
                                        break;
                                    }
                                }
                            }
                            sizeNewStr = newStr.Length;
                            str = str.Substring(sizeNewStr);
                            sizeStr = str.Length;

                            for (int j = 0; j < sizeNewStr; j++)
                            {
                                ops.opPointsDoubling();
                                switch (type)
                                {
                                    case 4:
                                        Double_ModifiedJacoby_Coord(x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                                    case 3:
                                        Double_JacobyChudnovskii_Coord(x2, y2, z2, r3, r3, a, p, out x2, out y2, out z2, out r3, out r4); break;
                                    case 2:
                                    case 1:
                                    case 0:
                                        DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2); break;
                                }
                            }
                            ops.opPointsAdd();
                            switch (type)
                            {
                                case 4:
                                    Add_ModifiedJacoby_Coord(PreComputationFor31[sizeNewStr - 1, 0], PreComputationFor31[sizeNewStr - 1, 1], PreComputationFor31[sizeNewStr - 1, 2], t1, x2, y2, z2, t2, a, p, out x2, out y2, out z2, out t2); break;
                                case 3:
                                    Add_JacobyChudnovskii_Coord(PreComputationFor31[sizeNewStr - 1, 0], PreComputationFor31[sizeNewStr - 1, 1], PreComputationFor31[sizeNewStr - 1, 2], r1, r2, x2, y2, z2, r3, r4, a, p, out x2, out y2, out z2, out r3, out r4); break;
                                case 2:
                                case 1:
                                case 0:
                                    AddList[type](PreComputationFor31[sizeNewStr - 1, 0], PreComputationFor31[sizeNewStr - 1, 1], PreComputationFor31[sizeNewStr - 1, 2], x2, y2, z2, a, p, out x2, out y2, out z2); break;
                            }    
                            m = m - sizeNewStr;
                        }
                    }
                }
            }
            if (x2 == 0 && y2 == 1)
                z2 = 0;

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }
        #endregion
        #region Multi Scalar Multiplication
        public static void Point_Multiplication_33(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger x2, BigInteger y2, BigInteger z2, BigInteger a, BigInteger k, 
            BigInteger l, BigInteger p, out BigInteger x3, out BigInteger y3, out BigInteger z3, int type, out double time, int w, OperationsCounter ops = null)
        {
            if (ops == null) ops = new OperationsCounter();  // Create default unused if not provided
            int count = (int)Math.Pow(2, w);
            BigInteger[,,] PreComputation = new BigInteger[3,count,count];
            x3 = 0; y3 = 1; z3 = 0;
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    double iterationTime = 0;
                    Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, i, p, out x3, out y3, out z3, type, out iterationTime);
                    BigInteger temp1, temp2 , temp3;
                    Point_Multiplication_Affine_Coord_1(x2, y2, z2, a, j, p, out temp1, out temp2, out temp3, type, out iterationTime);
                    AddList[type](x3,y3,z3,temp1, temp2, temp3, a, p, out x3, out y3, out z3);
                    PreComputation[0, i, j] = x3;
                    PreComputation[1, i, j] = y3;
                    PreComputation[2, i, j] = z3;
                }
           }         
           string str1 = new string(ToBin(k).Reverse().ToArray());
           string str2 = new string(ToBin(l).Reverse().ToArray());
           int t1 = str1.Length;
           int t2 = str2.Length;
            /*Add 0 to str1 and str2 when they isn't kratni w*/
           if (t1 % w != 0)
           {        
               for (int i = 0; i < (w - (t1 % w)); i++)
               {
                   str1 += '0';
               }
               str1 = new string (str1.Reverse().ToArray());
           }
           if (t2 % w != 0)
           {
               for (int i = 0; i < (w - (t2 % w)); i++)
               {
                   str2 += '0';
               }
               str2 = new string(str2.Reverse().ToArray());
           }        
            t1 = str1.Length;
            t2 = str2.Length;
            BigInteger x4 = 0, y4 = 1, z4 = 0;
            List<int> tmp1 = new List<int>();
            List<int> tmp2 = new List<int>();
            do
            {
                tmp1.Add((int)(k % (1 << w)));
                k = k >> w;
            } while (k != 0);
            do
            {
                tmp2.Add((int)(l % (1 << w)));
                l = l >> w;
            } while (l != 0);
            int maxSize = tmp1.Count, number = 0;
            if (tmp1.Count != tmp2.Count)
            {
                if (tmp1.Count > tmp2.Count)
                {
                    maxSize = tmp1.Count;
                    number = (maxSize - tmp2.Count) % w;
                    do
                    {
                        tmp2.Add(0);
                        number--;
                    } while (number != 0);

                }
                else
                {
                    maxSize = tmp2.Count;
                    number = (maxSize - tmp1.Count) % w;
                    do
                    {
                        tmp1.Add(0);
                        number--;
                    } while (number != 0);
                }
            }
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 0; i < maxSize; i++)
            {
                for (int j = 0; j < w * i; j++)
                {
                    DoubleList[type](PreComputation[0, tmp1[i], tmp2[i]], PreComputation[1, tmp1[i], tmp2[i]], PreComputation[2, tmp1[i], tmp2[i]], a, p, out x3, out y3, out z3);
                    PreComputation[0, tmp1[i], tmp2[i]] = x3;
                    PreComputation[1, tmp1[i], tmp2[i]] = y3;
                    PreComputation[2, tmp1[i], tmp2[i]] = z3;
                }
                if (tmp1[i] > 0 || tmp2[i] > 0)
                {
                    ops.opPointsAdd();
                    AddList[type](x4, y4, z4, PreComputation[0, tmp1[i], tmp2[i]], PreComputation[1, tmp1[i], tmp2[i]], PreComputation[2, tmp1[i], tmp2[i]],
                             a, p, out x3, out y3, out z3);
                    x4 = x3;
                    y4 = y3;
                    z4 = z3;                        
                }                                                                     
            }
            if (x2 == 0 && y2 == 1)
               z2 = 0;
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        } 
        #endregion
    }
}
