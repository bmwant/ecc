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
    public static class PointMultiplication
    {
        public delegate void AddDelegate(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger x2, BigInteger y2, BigInteger z2, BigInteger a, BigInteger p,
            out BigInteger x3, out BigInteger y3, out BigInteger z3);

        public delegate void DoubleDelegate(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger p,
            out BigInteger x3, out BigInteger y3, out BigInteger z3);

        public delegate void TernaryDelegate(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger p,
            out BigInteger x3, out BigInteger y3, out BigInteger z3);

        private static List<DoubleDelegate> DoubleList;
        private static List<AddDelegate> AddList;
        private static List<TernaryDelegate> TernaryList;

        static PointMultiplication()
        {
            DoubleList = new List<DoubleDelegate>();
            DoubleList.Add(Double_Affine_Coord);
            DoubleList.Add(Double_Projective_Coord);
            DoubleList.Add(Double_Jacoby_Coord);
            AddList = new List<AddDelegate>();
            AddList.Add(Add_Affine_Coord);
            AddList.Add(Add_Projective_Coord);
            AddList.Add(Add_Jacoby_Coord);
            TernaryList = new List<TernaryDelegate>();
            TernaryList.Add(Ternary.Ternary_Affine_Coord);
            TernaryList.Add(Ternary.Ternary_Projective_Coord);
            TernaryList.Add(Ternary.Ternary_Jacoby_Coord);
        }
        public static void Add_Affine_Coord(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger x2, BigInteger y2, BigInteger z2, BigInteger a, BigInteger p,
            out BigInteger x3, out BigInteger y3, out BigInteger z3)
        {
            while (y1 < 0)
                y1 = y1 + p;

            while (y2 < 0)
                y2 = y2 + p;

            BigInteger d, inv;

            if (z1 == 0)
            {
                x3 = x2 % p;
                y3 = y2 % p;
                z3 = z2 % p;
            }
            else
            {
                if (z2 == 0)
                {
                    x3 = x1 % p;
                    y3 = y1 % p;
                    z3 = z1;
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
                    BigInteger s, u, t;
                    s = (x2 * z1 - x1 * z2) % p;
                    if (s <= 0)
                        s += p;
                    t = (y2 * z1 - y1 * z2) % p;
                    if (t <= 0)
                        t += p;
                    u = (Functions.Pow(s, 2) * (x1 * z2 + x2 * z1) - Functions.Pow(t, 2) * z1 * z2) % p;
                    if (u <= 0)
                        u += p;

                    x3 = (-s * u) % p;
                    if (x3 <= 0)
                        x3 += p;

                    y3 = (t * (u + Functions.Pow(s, 2) * x1 * z2) - Functions.Pow(s, 3) * y1 * z2) % p;
                    if (y3 <= 0)
                        y3 += p;

                    z3 = (Functions.Pow(s, 3) * z1 * z2) % p;
                    if (z3 <= 0)
                        z3 += p;
                }
            }

            /*
            BigInteger u, v, w;
            if ((y2 * z1 - y1 * z2) % p < 0)
                u = (y2 * z1 - y1 * z2) % p + p;
            else u = (y2 * z1 - y1 * z2) % p;
            if ((x2 * z1 - x1 * z2) % p < 0)
                v = (x2 * z1 - x1 * z2) % p + p;
            else v = (x2 * z1 - x1 * z2) % p;
            if ((Functions.Pow(u, 2) * z1 * z2 - Functions.Pow(v, 3) - 2 * Functions.Pow(v, 2) * x1 * z2) % p < 0)
                w = (Functions.Pow(u, 2) * z1 * z2 - Functions.Pow(v, 3) - 2 * Functions.Pow(v, 2) * x1 * z2) % p + p;
            else w = (Functions.Pow(u, 2) * z1 * z2 - Functions.Pow(v, 3) - 2 * Functions.Pow(v, 2) * x1 * z2) % p;

            if ((v * w) % p < 0)
                x3 = (v * w) % p + p;
            else x3 = (v * w) % p;

            if ((u * (Functions.Pow(v, 2) * x1 * z2 - w) - Functions.Pow(v, 3) * y1 * z2) % p < 0)
                y3 = (u * (Functions.Pow(v, 2) * x1 * z2 - w) - Functions.Pow(v, 3) * y1 * z2) % p + p;
            else y3 = (u * (Functions.Pow(v, 2) * x1 * z2 - w) - Functions.Pow(v, 3) * y1 * z2) % p;

            if ((Functions.Pow(v, 3) * z1 * z2) % p < 0)
                z3 = (Functions.Pow(v, 3) * z1 * z2) % p + p;
            else z3 = (Functions.Pow(v, 3) * z1 * z2) % p;
             
            */
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
            out BigInteger x3, out BigInteger y3, out BigInteger z3)
        {
            BigInteger t, u, v, w;
            if ((a * Functions.Pow(z1, 2) + 3 * Functions.Pow(x1, 2)) % p < 0)
                t = (a * Functions.Pow(z1, 2) + 3 * Functions.Pow(x1, 2)) % p + p;
            else t = (a * Functions.Pow(z1, 2) + 3 * Functions.Pow(x1, 2)) % p;

            if ((y1 * z1) % p < 0)
                u = (y1 * z1) % p + p;
            else u = (y1 * z1) % p;
            if ((u * x1 * y1) % p < 0)
                v = (u * x1 * y1) % p + p;
            else v = (u * x1 * y1) % p;
            if ((Functions.Pow(t, 2) - 8 * v) % p < 0)
                w = (Functions.Pow(t, 2) - 8 * v) % p + p;
            else w = (Functions.Pow(t, 2) - 8 * v) % p;

            if ((2 * u * w) % p < 0)
                x3 = (2 * u * w) % p + p;
            else x3 = (2 * u * w) % p;

            if ((t * (4 * v - w) - 8 * Functions.Pow(y1, 2) * Functions.Pow(u, 2)) % p < 0)
                y3 = (t * (4 * v - w) - 8 * Functions.Pow(y1, 2) * Functions.Pow(u, 2)) % p + p;
            else y3 = (t * (4 * v - w) - 8 * Functions.Pow(y1, 2) * Functions.Pow(u, 2)) % p;

            if ((8 * Functions.Pow(u, 3)) % p < 0)
                z3 = (8 * Functions.Pow(u, 3)) % p + p;
            else z3 = (8 * Functions.Pow(u, 3)) % p;
        }

        public static void Double_Jacoby_Coord(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger p,
            out BigInteger x3, out BigInteger y3, out BigInteger z3)
        {
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
        }

        public static void Projective_to_Affine(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger p, out BigInteger x3, out BigInteger y3, out BigInteger z3)
        {
            BigInteger d, inv;
            Functions.Extended_Euclid(p, z1, out d, out inv);
            z3 = 1;

            if ((x1 * inv) % p < 0)
                x3 = (x1 * inv) % p + p;
            else x3 = (x1 * inv) % p;

            if ((y1 * inv) % p < 0)
                y3 = (y1 * inv) % p + p;
            else y3 = (y1 * inv) % p;

        }

        public static void Jacoby_to_Affine(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger p, out BigInteger x3, out BigInteger y3, out BigInteger z3)
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
        

        public static void Point_Multiplication_Affine_Coord_1(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, int type)
        {
            BigInteger x3 = 0;
            BigInteger y3 = 1;
            BigInteger z3 = 0;

            x2 = 0;
            y2 = 1;
            z2 = 0;
            string str = Functions.ToBin(k);

            //BigInteger t = (BigInteger)Math.Floor(BigInteger.Log(k, 2)) + 1;
            int t = str.Length;
            for (int i = t - 1; i >= 0; i--)
            {

                if (str[i] == '1')
                    AddList[type](x1, y1, z1, x2, y2, z2, a, p, out x2, out y2, out z2);

                DoubleList[type](x1, y1, z1, a, p, out x1, out y1, out z1);
            }
            if (x2 < 0) x2 += p;
            if (y2 < 0) y2 += p;

            if (x2 == 0 && y2 == 1)
                z2 = 0;
            else
                z2 = 1;

            if (type == 1)
            {
                Projective_to_Affine(x2, y2, z2, a, p, out x2, out y2, out z2);
            }

        }

        public static void Point_Multiplication_Affine_Coord_2(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, int type)
        {

            x2 = 0;
            y2 = 1;
            z2 = 0;
            string str = ToBin(k);

            int t = str.Length;
            for (int i = 0; i < t; i++)
            {
                DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2);

                if (str[i] == '1')
                   AddList[type](x1, y1, z1, x2, y2, z2, a, p, out x2, out y2, out z2);
            }

            if (x2 == 0 && y2 == 1)
                z2 = 0;
            else
                z2 = 1;
        }

        private static string ToBin(BigInteger k)
        {
            return Functions.ToBin(k);
        }
               
        public static void Point_Multiplication_Affine_Coord_3(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, int w, BigInteger p, 
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            int ind = (int)Math.Pow(2, w) - 1;
            BigInteger[,] PreComputation = new BigInteger[ind, 3];
            BigInteger x3 = 0, y3 = 0, z3 = 0;

            for (int i = 1; i <= ind; i++)
            {
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, i, p, out x2, out y2, out z2, type);
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

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();


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
                // int temp_1 = arr[0];

                for (int d = 1; d < legth_temp; d++)
                {
                    pow = pow * 2;
                    arr[0] = arr[0] + pow * arr[d];
                }

                if (arr[0] > 0)
                {
                    AddList[type](PreComputation[arr[0] - 1, 0], PreComputation[arr[0] - 1, 1], PreComputation[arr[0] - 1, 2], x2, y2, z2, a, p, out x3, out y3, out z3);
                    x2 = x3;
                    y2 = y3;
                    z2 = z3;
                }

                for (int d = 1; d <= w; d++)
                {
                    for (int j = 0; j < h; j++)
                    {
                        DoubleList[type](PreComputation[j, 0], PreComputation[j, 1], PreComputation[j, 2], a, p, out x3, out y3, out z3);
                        PreComputation[j, 0] = x3;
                        PreComputation[j, 1] = y3;
                        PreComputation[j, 2] = z3;
                    }
                }
            }

            if (x2 == 0 && y2 == 1)
                z2 = 0;
            else
                z2 = 1;
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;

        }


        public static void Point_Multiplication_Affine_Coord_4(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, int w, BigInteger p, 
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {

            int ind = (int)Math.Pow(2, w) - 1;
            BigInteger[,] PreComputation = new BigInteger[ind, 3];
            BigInteger x3 = 0, y3 = 0, z3 = 0;

            for (int i = 1; i <= ind; i++)
            {
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, i, p, out x2, out y2, out z2, type);
                PreComputation[i - 1, 0] = x2;
                PreComputation[i - 1, 1] = y2;
                PreComputation[i - 1, 2] = z2;
            }

            x2 = 0;
            y2 = 1;
            z2 = 0;

            Stopwatch stopWatch = new Stopwatch();
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
                    DoubleList[type](x2, y2, z2, a, p, out x3, out y3, out z3);
                    x2 = x3;
                    y2 = y3;
                    z2 = z3;
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
                    AddList[type](PreComputation[temp_1 - 1, 0], PreComputation[temp_1 - 1, 1], PreComputation[temp_1 - 1, 2], x2, y2, z2, a, p, out x3, out y3, out z3);
                    x2 = x3;
                    y2 = y3;
                    z2 = z3;

                }

            }

            if (x2 == 0 && y2 == 1)
                z2 = 0;
            else
                z2 = 1;

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;

        }

        public static void Point_Multiplication_Affine_Coord_5(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, int w, BigInteger p, 
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            int temp_step = (int)(BigInteger.Pow(2, w - 1));
            int count = (int)(BigInteger.Pow(2, w) - temp_step);
            BigInteger[,] PreComputation = new BigInteger[count, 3];
            BigInteger x3 = 0, y3 = 0, z3 = 0;

            for (int i = 0; i < count; i++)
            {
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, temp_step + i, p, out x2, out y2, out z2, type);
                PreComputation[i, 0] = x2;
                PreComputation[i, 1] = y2;
                PreComputation[i, 2] = z2;
            }

            x2 = 0;
            y2 = 1;
            z2 = 0;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            BigInteger x_temp = x1;
            BigInteger y_temp = y1;
            BigInteger z_temp = z1;

            int m = 0;
            int h = PreComputation.GetLength(0);
            string str = new string(ToBin(k).Reverse().ToArray());
            int count_bit = str.Length;
            // bitget(k, 1:count_bit)
            while (m < count_bit)
            {
                if (m + w <= count_bit)
                {
                    if (str[m + w - 1] == '0')
                    {
                        if (str[m] == '1')
                        {
                            AddList[type](x_temp, y_temp, z_temp, x2, y2, z2, a, p, out x3, out y3, out z3);
                            x2 = x3;
                            y2 = y3;
                            z2 = z3;
                        }

                        DoubleList[type](x_temp, y_temp, z_temp, a, p, out x_temp, out y_temp, out z_temp);
                        for (int j = 0; j < h; j++)
                        {
                            DoubleList[type](PreComputation[j, 0], PreComputation[j, 1], PreComputation[j, 2], a, p, out PreComputation[j, 0], out PreComputation[j, 1], out PreComputation[j, 2]);
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
                        }

                        if (temp_1 > 0)
                            AddList[type](PreComputation[temp_1 - temp_step, 0], PreComputation[temp_1 - temp_step, 1], PreComputation[temp_1 - temp_step, 2], x2, y2, z2, a, p, out x2, out y2, out z2);


                        for (int d = 0; d < w; d++)
                        {
                            DoubleList[type](x_temp, y_temp, z_temp, a, p, out x_temp, out y_temp, out z_temp);
                            for (int j = 0; j < h; j++)
                            {
                                DoubleList[type](PreComputation[j, 0], PreComputation[j, 1], PreComputation[j, 2], a, p, out  PreComputation[j, 0], out PreComputation[j, 1], out PreComputation[j, 2]);
                            }
                        }

                        m += w;
                    }
                }
                else
                {
                    if (str[m] == '1')
                    {
                        AddList[type](x_temp, y_temp, z_temp, x2, y2, z2, a, p, out x3, out y3, out z3);
                        x2 = x3;
                        y2 = y3;
                        z2 = z3;
                    }

                    DoubleList[type](x_temp, y_temp, z_temp, a, p, out x_temp, out y_temp, out z_temp);
                    for (int j = 0; j < h; j++)
                    {
                        DoubleList[type](PreComputation[j, 0], PreComputation[j, 1], PreComputation[j, 2], a, p, out PreComputation[j, 0], out PreComputation[j, 1], out PreComputation[j, 2]);
                    }

                    m++;
                }

            }

            if (x2 == 0 && y2 == 1)
                z2 = 0;
            else
                z2 = 1;

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;

        }


        public static void Point_Multiplication_Affine_Coord_6(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, int w, BigInteger p, 
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {

            int temp_step = (int)(BigInteger.Pow(2, w - 1));
            int count = (int)(BigInteger.Pow(2, w) - temp_step);
            BigInteger[,] PreComputation = new BigInteger[count, 3];
            BigInteger x3 = 0, y3 = 0, z3 = 0;

            for (int i = 0; i < count; i++)
            {
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, temp_step + i, p, out x2, out y2, out z2, type);
                PreComputation[i, 0] = x2;
                PreComputation[i, 1] = y2;
                PreComputation[i, 2] = z2;
            }

            x2 = 0;
            y2 = 1;
            z2 = 0;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            int count_bit = (int)(Math.Floor(BigInteger.Log(k, 2))) + 1;

            int m = count_bit;
            while (m > 0)
            {
                string str = new string(ToBin(k).Reverse().ToArray());
                if (str[m - 1] == '0')
                {
                    DoubleList[type](x2, y2, z2, a, p, out x3, out y3, out z3);
                    x2 = x3;
                    y2 = y3;
                    z2 = z3;
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
                            DoubleList[type](x2, y2, z2, a, p, out x3, out y3, out z3);
                            x2 = x3;
                            y2 = y3;
                            z2 = z3;
                            string temp_str = new string(ToBin(temp_1).Reverse().ToArray());
                            if (temp_str[j - 1] == '1')
                            {

                                AddList[type](x1, y1, z1, x2, y2, z2, a, p, out x2, out y2, out z2);
                            }
                        }
                    }

                    else
                    {

                        for (int d = 1; d <= w; d++)
                        {
                            DoubleList[type](x2, y2, z2, a, p, out x3, out y3, out z3);
                            x2 = x3;
                            y2 = y3;
                            z2 = z3;
                        }
                        AddList[type](PreComputation[temp_1 - temp_step, 0], PreComputation[temp_1 - temp_step, 1], PreComputation[temp_1 - temp_step, 2], x2, y2, z2, a, p, out x3, out y3, out z3);
                        x2 = x3;
                        y2 = y3;
                        z2 = z3;


                    }

                    m -= w;
                }

            }
            if (x2 == 0 && y2 == 1)
                z2 = 0;
            else
                z2 = 1;

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;

        }

        public static void Point_Multiplication_Affine_Coord_7_1(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, int w, BigInteger p, 
            out BigInteger x2, out BigInteger y2, out BigInteger z2, int type)
        {
            x2 = 0;
            y2 = 1;
            z2 = 0;

            List<BigInteger> mas_k = Functions.NAF(k);
            int t = mas_k.Count;

            for (int i = 0; i < t; i++)
            {
                if (mas_k[i] == 1)
                    AddList[type](x1, y1, z1, x2, y2, z2, a, p, out x2, out y2, out z2);
                else
                    if (mas_k[i] == -1)
                        AddList[type](x1, -y1, z1, x2, y2, z2, a, p, out x2, out y2, out z2);

                DoubleList[type](x1, y1, z1, a, p, out x1, out y1, out z1);
            }

            if (x2 == 0 && y2 == 1)
                z2 = 0;
            else
                z2 = 1;

        }


        public static void Point_Multiplication_Affine_Coord_7_2(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, int w, BigInteger p, 
            out BigInteger x2, out BigInteger y2, out BigInteger z2, int type)
        {

            x2 = 0;
            y2 = 1;
            z2 = 0;
            BigInteger temp = 0;

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
                    AddList[type](x1, y1, z1, x2, y2, z2, a, p, out x2, out y2, out z2);
                else
                    if (temp == -1)
                        AddList[type](x1, -y1, z1, x2, y2, z2, a, p, out x2, out y2, out z2);


                k = k / 2;
                DoubleList[type](x1, y1, z1, a, p, out x1, out y1, out z1);
            }

            if (x2 == 0 && y2 == 1)
                z2 = 0;
            else
                z2 = 1;
        }


        public static void Point_Multiplication_Affine_Coord_8(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, int w, BigInteger p, 
            out BigInteger x2, out BigInteger y2, out BigInteger z2, int type)
        {
            BigInteger x3 = 0, y3 = 0, z3 = 0;

            x2 = 0;
            y2 = 1;
            z2 = 0;

            List<BigInteger> mas_k = Functions.NAF(k);
            int t = mas_k.Count;

            for (int i = t; i > 0; i--)
            {
                DoubleList[type](x2, y2, z2, a, p, out x3, out y3, out z3);
                x2 = x3;
                y2 = y3;
                z2 = z3;

                if (mas_k[i - 1] == 1)
                    AddList[type](x1, y1, z1, x2, y2, z2, a, p, out x2, out y2, out z2);
                else

                    if (mas_k[i - 1] == -1)

                        AddList[type](x1, -y1, z1, x2, y2, z2, a, p, out x2, out y2, out z2);


            }

            if (x2 == 0 && y2 == 1)
                z2 = 0;
            else
                z2 = 1;
        }

        public static void Point_Multiplication_Affine_Coord_9(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, int w, BigInteger p, 
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {

            int count = (int)((2 * (BigInteger.Pow(2, w) - BigInteger.Pow(-1, w)) / 3) / 2);
            BigInteger[,] PreComputation = new BigInteger[count, 3];
            BigInteger x3 = 0, y3 = 0, z3 = 0;

            for (int i = 0; i < count * 2; i += 2)
            {
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, i + 1, p, out x2, out y2, out z2, type);
                PreComputation[i / 2, 0] = x2;
                PreComputation[i / 2, 1] = y2;
                PreComputation[i / 2, 2] = z2;
            }

            x2 = 0;
            y2 = 1;
            z2 = 0;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<BigInteger> mas_k =Functions.NAF(k);
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

                    AddList[type](PreComputation[max_div - 1, 0], PreComputation[max_div - 1, 1], PreComputation[max_div - 1, 2], x2, y2, z2, a, p, out x2, out y2, out z2);
                }
                else
                    if (max < 0)
                    {
                        int max_abs = Math.Abs(max);
                        int max_abs_div = max_abs / 2;
                        if (max_abs_div * 2 < max_abs) max_abs_div++;

                        AddList[type](PreComputation[max_abs_div - 1, 0], -PreComputation[max_abs_div - 1, 1], PreComputation[max_abs_div - 1, 2], x2, y2, z2, a, p, out x2, out y2, out z2);
                    }

                for (int d = 0; d < max_j; d++)
                {
                    for (int l = 0; l < h; l++)
                    {
                        DoubleList[type](PreComputation[l, 0], PreComputation[l, 1], PreComputation[l, 2], a, p, out PreComputation[l, 0], out PreComputation[l, 1], out PreComputation[l, 2]);
                    }
                }

                j = j + max_j;
            }

            if (x2 == 0 && y2 == 1)
                z2 = 0;
            else
                z2 = 1;

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_10(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, int w, BigInteger p, 
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            int count = (int)((2 * (BigInteger.Pow(2, w) - BigInteger.Pow(-1, w)) / 3) / 2);
            BigInteger[,] PreComputation = new BigInteger[count, 3];
            BigInteger x3 = 0, y3 = 0, z3 = 0;

            for (int i = 0; i < count * 2; i += 2)
            {
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, i + 1, p, out x2, out y2, out z2, type);
                PreComputation[i / 2, 0] = x2;
                PreComputation[i / 2, 1] = y2;
                PreComputation[i / 2, 2] = z2;
            }

            x2 = 0;
            y2 = 1;
            z2 = 0;

            Stopwatch stopWatch = new Stopwatch();
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
                    DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2);
                }

                if (max > 0)
                {
                    AddList[type](PreComputation[max / 2, 0], PreComputation[max / 2, 1], PreComputation[max / 2, 2], x2, y2, z2, a, p, out x2, out y2, out z2);
                }
                else
                    if (max < 0)
                    {
                        int max_abs = Math.Abs(max);
                        AddList[type](PreComputation[max_abs / 2, 0], -PreComputation[max_abs / 2, 1], PreComputation[max_abs / 2, 2], x2, y2, z2, a, p, out x2, out y2, out z2);
                    }

                j = j - max_j;
            }

            if (x2 == 0 && y2 == 1)
                z2 = 0;
            else
                z2 = 1;

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_11_1(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, int w, BigInteger p, 
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            int count = (int)(BigInteger.Pow(2, w - 2));
            BigInteger[,] PreComputation = new BigInteger[count, 3];
            BigInteger x3 = 0, y3 = 0, z3 = 0;

            for (int i = 0; i < count * 2; i += 2)
            {
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, i + 1, p, out x2, out y2, out z2, type);
                PreComputation[i / 2, 0] = x2;
                PreComputation[i / 2, 1] = y2;
                PreComputation[i / 2, 2] = z2;
            }

            x2 = 0;
            y2 = 1;
            z2 = 0;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<BigInteger> mas_k = Functions.NAFw(k, w);
            int t = mas_k.Count;
            int h = PreComputation.GetLength(0);

            for (int i = 0; i < t; i++)
            {
                if (mas_k[i] > 0)
                    AddList[type](PreComputation[(int)(mas_k[i] / 2), 0], PreComputation[(int)(mas_k[i] / 2), 1], PreComputation[(int)(mas_k[i] / 2), 2], x2, y2, z2, a, p, out x2, out y2, out z2);
                else
                    if (mas_k[i] < 0)
                    {
                        int mas_k_abs = (int)BigInteger.Abs(mas_k[i]);
                        AddList[type](PreComputation[mas_k_abs / 2, 0], -PreComputation[mas_k_abs / 2, 1], PreComputation[mas_k_abs / 2, 2], x2, y2, z2, a, p, out x2, out y2, out z2);
                    }
                for (int j = 0; j < h; j++)
                    DoubleList[type](PreComputation[j, 0], PreComputation[j, 1], PreComputation[j, 2], a, p, out PreComputation[j, 0], out PreComputation[j, 1], out PreComputation[j, 2]);

            }
            if (x2 == 0 && y2 == 1)
                z2 = 0;
            else
                z2 = 1;

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_11_2(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, int w, BigInteger p, 
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            int count = (int)(Math.Pow(2, w - 2));
            int count1 = (int)(Math.Pow(2, w - 1)) - 1;
            BigInteger[,] PreComputation = new BigInteger[count, 3];
            for (int i = 0; i < count1; i += 2)
            {
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, i + 1, p, out x2, out y2, out z2, type);
                PreComputation[i / 2, 0] = x2;
                PreComputation[i / 2, 1] = y2;
                PreComputation[i / 2, 2] = z2;
            }

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
                    AddList[type](PreComputation[(int)Math.Ceiling(Math.Abs((double)temp) / 2) - 1, 0], PreComputation[(int)Math.Ceiling(Math.Abs((double)temp) / 2) - 1, 1], PreComputation[(int)Math.Ceiling(Math.Abs((double)temp) / 2) - 1, 2], x2, y2, z2, a, p, out x2, out y2, out z2);
                }
                if (temp < 0)
                {
                    AddList[type](PreComputation[(int)Math.Ceiling(Math.Abs((double)temp) / 2 - 1), 0], -PreComputation[(int)Math.Ceiling(Math.Abs((double)temp) / 2) - 1, 1], PreComputation[(int)Math.Ceiling(Math.Abs((double)temp) / 2) - 1, 2], x2, y2, z2, a, p, out x2, out y2, out z2);
                }

                for (int j = 0; j < PreComputation.GetLength(0); j++)
                {
                    DoubleList[type](PreComputation[j, 0], PreComputation[j, 1], PreComputation[j, 2], a, p, out PreComputation[j, 0], out PreComputation[j, 1], out PreComputation[j, 2]);
                }
                k = k / 2;
                DoubleList[type](x1, y1, z1, a, p, out x1, out y1, out z1);

                if (x2 == 0 && y2 != 0)
                {
                    z2 = 0;
                }
                else z2 = 1;
            }

            if (x2 == 0 && y2 == 1)
                z2 = 0;
            else
                z2 = 1;

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_12(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, int w, BigInteger p, 
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            int count = (int)(BigInteger.Pow(2, w - 2));
            BigInteger[,] PreComputation = new BigInteger[count, 3];
            BigInteger x3 = 0, y3 = 0, z3 = 0;

            for (int i = 0; i < count * 2; i += 2)
            {
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, i + 1, p, out x2, out y2, out z2, type);
                PreComputation[i / 2, 0] = x2;
                PreComputation[i / 2, 1] = y2;
                PreComputation[i / 2, 2] = z2;
            }

            x2 = 0;
            y2 = 1;
            z2 = 0;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<BigInteger> mas_k = Functions.NAFw(k, w);
            int t = mas_k.Count;
            int h = PreComputation.GetLength(0);

            for (int i = t - 1; i >= 0; i--)
            {
                DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2);
                if (mas_k[i] > 0)
                    AddList[type](PreComputation[(int)(mas_k[i] / 2), 0], PreComputation[(int)(mas_k[i] / 2), 1], PreComputation[(int)(mas_k[i] / 2), 2], x2, y2, z2, a, p, out x2, out y2, out z2);
                else
                    if (mas_k[i] < 0)
                    {
                        int mas_k_abs = (int)BigInteger.Abs(mas_k[i]);
                    AddList[type](PreComputation[mas_k_abs / 2, 0], -PreComputation[mas_k_abs / 2, 1], PreComputation[mas_k_abs / 2, 2], x2, y2, z2, a, p, out x2, out y2, out z2);
                    }
            }
            if (x2 == 0 && y2 == 1)
                z2 = 0;
            else
                z2 = 1;

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_13(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, int w, BigInteger p, 
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            int count = (int)((BigInteger.Pow(2, w) - BigInteger.Pow(-1, w)) / 3);
            BigInteger[,] PreComputation = new BigInteger[count, 3];
            for (int u = 0; u < count * 2; u += 2)
            {
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, u + 1, p, out x2, out y2, out z2, type);
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
                        DoubleList[type](PreComputation[l, 0], PreComputation[l, 1], PreComputation[l, 2], a, p, out PreComputation[l, 0], out PreComputation[l, 1], out PreComputation[l, 2]);
                    }
                }
                i = i + max_j;
            }

            if (x2 == 0 && y2 != 0)
                z2 = 0;
            else
                z2 = 1;

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_14(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, int w, BigInteger p, 
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            int count = (int)((BigInteger.Pow(2, w) - BigInteger.Pow(-1, w)) / 3);
            BigInteger[,] PreComputation = new BigInteger[count, 3];
            for (int u = 0; u < count * 2; u += 2)
            {
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, u + 1, p, out x2, out y2, out z2, type);
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
                    DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2);
                }
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
            else
                z2 = 1;

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_15(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {
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
                    AddList[type](x2, y2, z2, x1, y1, z1, a, p, out x2, out y2, out z2);
                }
                else if (str1[i] == '0' && str[i] == '1')
                {
                    AddList[type](x2, y2, z2, x1, -y1, z1, a, p, out x2, out y2, out z2);
                }

                DoubleList[type](x1, y1, z1, a, p, out x1, out y1, out z1);
            }

            AddList[type](x2, y2, z2, x1, y1, z1, a, p, out x2, out y2, out z2);

            if (x2 == 0 && y2 != 0)
            {
                z2 = 0;
            }
            else
            {
                z2 = 1;
            }

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_16(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {
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
                DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2);
                if (str1[i] == '1' && str[i] == '0')
                {
                    AddList[type](x2, y2, z2, x1, y1, z1, a, p, out x2, out y2, out z2);
                }
                else if (str1[i] == '0' && str[i] == '1')
                {
                    AddList[type](x2, y2, z2, x1, -y1, z1, a, p, out x2, out y2, out z2);
                }
            }

            if (x2 == 0 && y2 != 0)
            {
                z2 = 0;
            }
            else
            {
                z2 = 1;
            }

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_17(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            x2 = 0;
            y2 = 1;
            z2 = 0;

            BigInteger x3 = x1;
            BigInteger y3 = y1;
            BigInteger z3 = z1;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            string str = Functions.ToBin(k);

            int t = str.Length;
            for (int i = t - 1; i >= 0; i--)
            {
                if (str[i] == '1')
                {
                    DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2);
                    AddList[type](x2, y2, z2, x3, y3, z3, a, p, out x2, out y2, out z2);
                }
                else
                {
                    DoubleList[type](x3, y3, z3, a, p, out x3, out y3, out z3);
                    AddList[type](x2, y2, z2, x3, y3, z3, a, p, out x3, out y3, out z3);
                }
            }
            if (x2 == 0 && y2 != 0)
            {
                z2 = 0;
            }
            else
            {
                z2 = 1;
            }

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_18(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            x2 = 0;
            y2 = 1;
            z2 = 0;

            BigInteger x3 = x1;
            BigInteger y3 = y1;
            BigInteger z3 = z1;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            string str = Functions.ToBin(k);

            int t = str.Length;
            for (int i = 0; i < t; i++)
            {
                if (str[i] == '0')
                {
                    AddList[type](x2, y2, z2, x3, y3, z3, a, p, out x3, out y3, out z3);
                    DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2);
                }
                else
                {
                    AddList[type](x2, y2, z2, x3, y3, z3, a, p, out x2, out y2, out z2);
                    DoubleList[type](x3, y3, z3, a, p, out x3, out y3, out z3);
                }
            }
            if (x2 == 0 && y2 != 0)
            {
                z2 = 0;
            }
            else
            {
                z2 = 1;
            }

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_19(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x3, out BigInteger y3, out BigInteger z3, out double time, int type, BigInteger a_max, BigInteger b_max)
        {
            //BigInteger a_max = 15;
            //BigInteger b_max = 17;
            BigInteger[,] mas_k;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            mas_k = Functions.Convert_to_DBNS_1(k, a_max, b_max);
            //mas_k = Convert_to_DBNS_2(k, a_max, b_max);
            BigInteger sum = 0;
            for (int i = 0; i < mas_k.GetLength(0); i++)
            {
                sum = sum + mas_k[i, 0] * Functions.Pow(2, mas_k[i, 1]) * Functions.Pow(3, mas_k[i, 2]);
            }

            if (k != sum)
            {
                //Console.WriteLine("Error");
                throw new Exception("Error");
            }

            BigInteger x2 = x1;
            BigInteger y2 = y1;
            BigInteger z2 = z1;

            for (BigInteger i = 0; i < mas_k[mas_k.GetLength(0) - 1, 2]; i++)
            {
                TernaryList[type](x2, y2, z2, a, p, out x2, out y2, out z2);
            }

            for (BigInteger i = 0; i < mas_k[mas_k.GetLength(0) - 1, 1]; i++)
            {
                DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2);
            }

            x3 = x2;
            y3 = mas_k[mas_k.GetLength(0) - 1, 0] * y2;
            z3 = z2;

            for (int i = mas_k.GetLength(0) - 2; i >= 0; i--)
            {
                BigInteger u = mas_k[i, 1] - mas_k[i + 1, 1];
                BigInteger v = mas_k[i, 2] - mas_k[i + 1, 2];
                for (int j = 0; j < u; j++)
                {
                    DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2);
                }

                for (int j = 0; j < v; j++)
                {
                    TernaryList[type](x2, y2, z2, a, p, out x2, out y2, out z2);
                }

                AddList[type](x2, mas_k[i, 0] * y2, z2, x3, y3, z3, a, p, out x3, out y3, out z3);

            }

            if (x3 == 0 && y3 != 0)
            {
                z3 = 0;
            }
            else z3 = 1;

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_20(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, BigInteger a_max, BigInteger b_max)
        {
            //BigInteger a_max = 15;
            //BigInteger b_max = 17;
            BigInteger[,] mas_k;

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
                //Console.WriteLine("Error");
                throw new Exception("Error");
            }

            x2 = x1;
            y2 = mas_k[0, 0] * y1;
            z2 = z1;

            for (int i = 0; i < mas_k.GetLength(0) - 1; i++)
            {
                BigInteger u = mas_k[i, 1] - mas_k[i + 1, 1];
                BigInteger v = mas_k[i, 2] - mas_k[i + 1, 2];
                for (int j = 0; j < u; j++)
                {
                    DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2);
                }

                for (int j = 0; j < v; j++)
                {
                    TernaryList[type](x2, y2, z2, a, p, out x2, out y2, out z2);
                }

                AddList[type](x1, mas_k[i + 1, 0] * y1, z1, x2, y2, z2, a, p, out x2, out y2, out z2);

            }

            for (BigInteger i = 0; i < mas_k[mas_k.GetLength(0) - 1, 1]; i++)
            {
                DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2);
            }

            for (BigInteger i = 0; i < mas_k[mas_k.GetLength(0) - 1, 2]; i++)
            {
                TernaryList[type](x2, y2, z2, a, p, out x2, out y2, out z2);
            }

            if (x2 == 0 && y2 != 0)
            {
                z2 = 0;
            }
            else z2 = 1;

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_22(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x3, out BigInteger y3, out BigInteger z3, out double time, int type)
        {
            BigInteger[,] mas_k;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            //mas_k = Functions.Convert_to_DBNS_1(k, a_max, b_max);
            mas_k = Functions.Convert_to_DBNS1(k);
            BigInteger sum = 1;
            for (int i = mas_k.GetLength(0) - 1; i >= 0; i--)
            {
                sum = sum * Functions.Pow(2, mas_k[i, 1]) * Functions.Pow(3, mas_k[i, 2]) + mas_k[i, 0];
            }

            if (k != sum)
            {
                //Console.WriteLine("Error");
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
                    DoubleList[type](x3, y3, z3, a, p, out x3, out y3, out z3);
                }

                for (int j = 0; j < v; j++)
                {
                    TernaryList[type](x3, y3, z3, a, p, out x3, out y3, out z3);
                }

                PointMultiplication.AddList[type](x1, mas_k[i, 0] * y1, z1, x3, y3, z3, a, p, out x3, out y3, out z3);

            }

            for (BigInteger i = 0; i < mas_k[0, 1]; i++)
            {
                DoubleList[type](x3, y3, z3, a, p, out x3, out y3, out z3);
            }

            for (BigInteger i = 0; i < mas_k[0, 2]; i++)
            {
                TernaryList[type](x3, y3, z3, a, p, out x3, out y3, out z3);
            }

            if (x3 == 0 && y3 != 0)
            {
                z3 = 0;
            }
            else z3 = 1;

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }
        public static void Point_Multiplication_Affine_Coord_21(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x3, out BigInteger y3, out BigInteger z3, out double time, int type)
        {
            BigInteger[,] mas_k;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            //mas_k = Functions.Convert_to_DBNS_1(k, a_max, b_max);
            mas_k = Functions.Convert_to_DBNS(k);
            BigInteger sum = 0, temp = 1;
            for (int i = 0; i < mas_k.GetLength(0); i++)
            {
                temp = temp * Functions.Pow(2, mas_k[i, 1]) * Functions.Pow(3, mas_k[i, 2]);
                sum = sum + mas_k[i, 0] * temp;
            }

            if (k != sum)
            {
                //Console.WriteLine("Error");
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
                    DoubleList[type](x1, y1, z1, a, p, out x1, out y1, out z1);
                }

                for (int j = 0; j < v; j++)
                {
                    TernaryList[type](x1, y1, z1, a, p, out x1, out y1, out z1);
                }

                AddList[type](x1, mas_k[i, 0] * y1, z1, x3, y3, z3, a, p, out x3, out y3, out z3);

            }

            if (x3 == 0 && y3 != 0)
            {
                z3 = 0;
            }
            else z3 = 1;

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_21m(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
           out BigInteger x3, out BigInteger y3, out BigInteger z3, out double time, int type)
        {
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
            }

            if (k != sum)
            {
                //Console.WriteLine("Error");
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
                    Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, 5, p, out x1, out y1, out z1, type);
                }

                for (int j = 0; j < v; j++)
                {
                    TernaryList[type](x1, y1, z1, a, p, out x1, out y1, out z1);
                }

                for (int j = 0; j < u; j++)
                {
                    DoubleList[type](x1, y1, z1, a, p, out x1, out y1, out z1);
                }
              
                AddList[type](x1, mas_k[i, 0] * y1, z1, x3, y3, z3, a, p, out x3, out y3, out z3);

            }

            if (x3 == 0 && y3 != 0)
            {
                z3 = 0;
            }
            else z3 = 1;

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_22m(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
           out BigInteger x3, out BigInteger y3, out BigInteger z3, out double time, int type)
        {
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
                //Console.WriteLine("Error");
                throw new Exception("Error");
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
                    DoubleList[type](x3, y3, z3, a, p, out x3, out y3, out z3);
                }

                for (int j = 0; j < v; j++)
                {
                    TernaryList[type](x3, y3, z3, a, p, out x3, out y3, out z3);
                }

                for (int l = 0; l < t; l++)
                {
                    Point_Multiplication_Affine_Coord_1(x3, y3, z3, a, 5, p, out x3, out y3, out z3, type);
                }

                   AddList[type](x1, mas_k[i, 0] * y1, z1, x3, y3, z3, a, p, out x3, out y3, out z3);

            }

            for (BigInteger i = 0; i < mas_k[0, 1]; i++)
            {
                DoubleList[type](x3, y3, z3, a, p, out x3, out y3, out z3);
            }

            for (BigInteger i = 0; i < mas_k[0, 2]; i++)
            {
                TernaryList[type](x3, y3, z3, a, p, out x3, out y3, out z3);
            }

            for (BigInteger i = 0; i < mas_k[0, 3]; i++)
            {
                Point_Multiplication_Affine_Coord_1(x3, y3, z3, a, 5, p, out x3, out y3, out z3, type);
            }

            if (x3 == 0 && y3 != 0)
            {
                z3 = 0;
            }
            else z3 = 1;

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_19_2(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x3, out BigInteger y3, out BigInteger z3, out double time, int type, BigInteger a_max, BigInteger b_max)
        {
            //BigInteger a_max = 15;
            //BigInteger b_max = 17;
            BigInteger[,] mas_k;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            mas_k = Functions.Convert_to_DBNS_2(k, a_max, b_max);
            //mas_k = Convert_to_DBNS_2(k, a_max, b_max);
            BigInteger sum = 0;
            for (int i = 0; i < mas_k.GetLength(0); i++)
            {
                sum = sum + mas_k[i, 0] * Functions.Pow(2, mas_k[i, 1]) * Functions.Pow(3, mas_k[i, 2]);
            }

            if (k != sum)
            {
                //Console.WriteLine("Error");
                throw new Exception("Error");
            }

            BigInteger x2 = x1;
            BigInteger y2 = y1;
            BigInteger z2 = z1;

            for (BigInteger i = 0; i < mas_k[mas_k.GetLength(0) - 1, 2]; i++)
            {
                TernaryList[type](x2, y2, z2, a, p, out x2, out y2, out z2);
            }

            for (BigInteger i = 0; i < mas_k[mas_k.GetLength(0) - 1, 1]; i++)
            {
                DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2);
            }

            x3 = x2;
            y3 = mas_k[mas_k.GetLength(0) - 1, 0] * y2;
            z3 = z2;

            for (int i = mas_k.GetLength(0) - 2; i >= 0; i--)
            {
                BigInteger u = mas_k[i, 1] - mas_k[i + 1, 1];
                BigInteger v = mas_k[i, 2] - mas_k[i + 1, 2];
                for (int j = 0; j < u; j++)
                {
                    DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2);
                }

                for (int j = 0; j < v; j++)
                {
                    TernaryList[type](x2, y2, z2, a, p, out x2, out y2, out z2);
                }

                AddList[type](x2, mas_k[i, 0] * y2, z2, x3, y3, z3, a, p, out x3, out y3, out z3);

            }

            if (x3 == 0 && y3 != 0)
            {
                z3 = 0;
            }
            else z3 = 1;

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;

        
        }

        public static void Point_Multiplication_Affine_Coord_20_1(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, BigInteger a_max, BigInteger b_max)
        {
            //BigInteger a_max = 15;
            //BigInteger b_max = 17;
            BigInteger[,] mas_k;

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
                //Console.WriteLine("Error");
                throw new Exception("Error");
            }

            x2 = x1;
            y2 = mas_k[0, 0] * y1;
            z2 = z1;

            for (int i = 0; i < mas_k.GetLength(0) - 1; i++)
            {
                BigInteger u = mas_k[i, 1] - mas_k[i + 1, 1];
                BigInteger v = mas_k[i, 2] - mas_k[i + 1, 2];
                for (int j = 0; j < u; j++)
                {
                    DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2);
                }

                for (int j = 0; j < v; j++)
                {
                    TernaryList[type](x2, y2, z2, a, p, out x2, out y2, out z2);
                }

                AddList[type](x1, mas_k[i + 1, 0] * y1, z1, x2, y2, z2, a, p, out x2, out y2, out z2);

            }

            for (BigInteger i = 0; i < mas_k[mas_k.GetLength(0) - 1, 1]; i++)
            {
                DoubleList[type](x2, y2, z2, a, p, out x2, out y2, out z2);
            }

            for (BigInteger i = 0; i < mas_k[mas_k.GetLength(0) - 1, 2]; i++)
            {
                TernaryList[type](x2, y2, z2, a, p, out x2, out y2, out z2);
            }

            if (x2 == 0 && y2 != 0)
            {
                z2 = 0;
            }
            else z2 = 1;

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        //Tania Drozda

        public static void Point_Multiplication_Affine_Coord_27(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, BigInteger B, BigInteger[] S, BigInteger[] M, int type, out double time)
        {
            ECC e = new ECC();
            BigInteger[,] PreComputation = new BigInteger[S.Length, 3];
            Tree tree;
            List<Tree.DecompositonItem> decomposition = new List<Tree.DecompositonItem>();

            if (e.Flag == true)
            {
                tree = new Tree(k, S, M, B);
                decomposition = tree.GetDecomposition();


                for (int i = 0; i < S.Length; i++)
                {
                    Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, S[i], p, out x2, out y2, out z2, type);
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
                            Point_Multiplication_Affine_Coord_1(PreComputation[kk, 0], PreComputation[kk, 1], PreComputation[kk, 2], a, (int)i.pows[j, 0], p, out PreComputation[kk, 0], out PreComputation[kk, 1], out PreComputation[kk, 2],type);
                        }
                    }
                }
                if (i.offset != 0)
                {
                    Int16 b = (Int16)(BigInteger.Abs(i.offset) - 1);
                    Add_Affine_Coord(x2, y2, z2, PreComputation[b, 0], PreComputation[b, 1] * i.offset.Sign, PreComputation[b, 2], a, p, out x2, out y2, out z2);
                }
            }
            Add_Affine_Coord(x2, y2, z2, PreComputation[0, 0], PreComputation[0, 1], PreComputation[0, 2], a, p, out x2, out y2, out z2);


            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }


        public static void Point_Multiplication_Affine_Coord_28(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
           out BigInteger x2, out BigInteger y2, out BigInteger z2, BigInteger B, BigInteger[] S, BigInteger[] M, int type, out double time)
        {
            
            var tree = new Tree(k, S, M, B);
            var decomposition = tree.GetDecomposition();
            BigInteger[,] PreComputation = new BigInteger[S.Length, 3];



            for (int i = 0; i < S.Length; i++)
            {
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, S[i], p, out x2, out y2, out z2, type);
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
                        Point_Multiplication_Affine_Coord_1(x2, y2, z2, a, (int)i.pows[j, 0], p, out x2, out y2, out z2, type);
                }
                if (i.offset != 0)
                {
                    Int16 b = (Int16)(BigInteger.Abs(i.offset) - 1);
                    Add_Affine_Coord(x2, y2, z2, PreComputation[b, 0], PreComputation[b, 1] * i.offset.Sign, PreComputation[b, 2], a, p, out x2, out y2, out z2);
                }
            }

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        public static void Point_Multiplication_Affine_Coord_29(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
           out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {

            BigInteger x3 = 0; BigInteger y3 = 0; BigInteger z3 = 0;
            int count_bit = (int)(Math.Floor(BigInteger.Log(k, 2))) + 1;
            int m = count_bit;
            int w = m;          

            BigInteger[,] PreComputation = new BigInteger[w, 3];
     
            for (int i = 0; i < w; i++)
            {
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, BigInteger.Pow(2, i), p, out x2, out y2, out z2, type);
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

                    DoubleList[type](x2, y2, z2, a, p, out x3, out y3, out z3);
                    x2 = x3;
                    y2 = y3;
                    z2 = z3;
                                  
                }
            
                AddList[type](PreComputation[sizeNewStr - 1, 0], PreComputation[sizeNewStr - 1, 1], PreComputation[sizeNewStr - 1, 2], x2, y2, z2, a, p, out x3, out y3, out z3);
                x2 = x3;
                y2 = y3;
                z2 = z3;

                m = m - sizeNewStr;

                              

            }
            if (x2 == 0 && y2 == 1)
                z2 = 0;
            else
                z2 = 1;

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
    
        }

        public static void Point_Multiplication_Affine_Coord_30(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
           out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {

            BigInteger x3 = 0; BigInteger y3 = 0; BigInteger z3 = 0;
            int count_bit = (int)(Math.Floor(BigInteger.Log(k, 2))) + 1;
            int m = count_bit;
            int w = m;                     
            BigInteger[,] PreComputation = new BigInteger[w, 3];


            for (int i = 0; i < w; i++)
            {
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, BigInteger.Pow(2, i+1)-1, p, out x2, out y2, out z2, type);
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

                        DoubleList[type](x2, y2, z2, a, p, out x3, out y3, out z3);
                        x2 = x3;
                        y2 = y3;
                        z2 = z3;

                    }
                
                 newStr.ToArray();
                 if (newStr[0] != '0')
                 {
                    AddList[type](PreComputation[sizeNewStr - 1, 0], PreComputation[sizeNewStr - 1, 1], PreComputation[sizeNewStr - 1, 2], x2, y2, z2, a, p, out x3, out y3, out z3);
                    x2 = x3;
                    y2 = y3;
                    z2 = z3;
                 }

                m = m - sizeNewStr;



            }
            if (x2 == 0 && y2 == 1)
                z2 = 0;
            else
                z2 = 1;

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;

        }

        public static void Point_Multiplication_Affine_Coord_31(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
         out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {

            BigInteger x3 = 0; BigInteger y3 = 0; BigInteger z3 = 0;
            int count_bit = (int)(Math.Floor(BigInteger.Log(k, 2))) + 1;
            int m = count_bit;
            int w = m;
            BigInteger[,] PreComputation = new BigInteger[w, 3];

            PreComputation[0, 0] = x1;
            PreComputation[0, 1] = y1;
            PreComputation[0, 2] = z1;

            for (int i = 1; i < w; i++)
            {
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, BigInteger.Pow(2, i) + 1, p, out x2, out y2, out z2, type);
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

                    DoubleList[type](x2, y2, z2, a, p, out x3, out y3, out z3);
                    x2 = x3;
                    y2 = y3;
                    z2 = z3;

                }

                newStr.ToArray();
                if (newStr[0] != '0')
                {
                    AddList[type](PreComputation[sizeNewStr - 1, 0], PreComputation[sizeNewStr - 1, 1], PreComputation[sizeNewStr - 1, 2], x2, y2, z2, a, p, out x3, out y3, out z3);
                    x2 = x3;
                    y2 = y3;
                    z2 = z3;
                }

                m = m - sizeNewStr;
            }

  

            if (x2 == 0 && y2 == 1)
                z2 = 0;
            else
                z2 = 1;

          stopWatch.Stop();
          TimeSpan ts = stopWatch.Elapsed;
          time = ts.TotalMilliseconds;
            

        }

        public static void Point_Multiplication_Affine_Coord_32(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
         out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            string str = new string(ToBin(k).ToArray());
           
            BigInteger x3 = 0; BigInteger y3 = 0; BigInteger z3 = 0;
            int count_bit = (int)(Math.Floor(BigInteger.Log(k, 2))) + 1;
            int m = count_bit;

            BigInteger[,] PreComputationFor30 = new BigInteger[count_bit, 3];
            BigInteger[,] PreComputationFor31 = new BigInteger[count_bit, 3];


            for (int i = 0; i < m; i++)
            {
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, BigInteger.Pow(2, i + 1) - 1, p, out x2, out y2, out z2, type);
                PreComputationFor30[i, 0] = x2;
                PreComputationFor30[i, 1] = y2;
                PreComputationFor30[i, 2] = z2;
            }

            PreComputationFor31[0, 0] = x1;
            PreComputationFor31[0, 1] = y1;
            PreComputationFor31[0, 2] = z1;

            for (int i = 1; i < m; i++)
            {
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, BigInteger.Pow(2, i) + 1, p, out x2, out y2, out z2, type);
                PreComputationFor31[i, 0] = x2;
                PreComputationFor31[i, 1] = y2;
                PreComputationFor31[i, 2] = z2;
            }
     
            x2 = 0;
            y2 = 1;
            z2 = 0;


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
                        DoubleList[type](x2, y2, z2, a, p, out x3, out y3, out z3);
                        x2 = x3;
                        y2 = y3;
                        z2 = z3;
                    }
                    AddList[type](PreComputationFor30[sizeNewStr - 1, 0], PreComputationFor30[sizeNewStr - 1, 1], PreComputationFor30[sizeNewStr - 1, 2], x2, y2, z2, a, p, out x3, out y3, out z3);
                    x2 = x3;
                    y2 = y3;
                    z2 = z3;

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
                            DoubleList[type](x2, y2, z2, a, p, out x3, out y3, out z3);
                            x2 = x3;
                            y2 = y3;
                            z2 = z3;
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
                                DoubleList[type](x2, y2, z2, a, p, out x3, out y3, out z3);
                                x2 = x3;
                                y2 = y3;
                                z2 = z3;
                            }
                            AddList[type](PreComputationFor30[sizeNewStr - 1, 0], PreComputationFor30[sizeNewStr - 1, 1], PreComputationFor30[sizeNewStr - 1, 2], x2, y2, z2, a, p, out x3, out y3, out z3);
                            x2 = x3;
                            y2 = y3;
                            z2 = z3;

                            m = m - sizeNewStr;
                        }

                        // algorythm #31
                        else
                        {
                            newStr += str[0];
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
                            sizeNewStr = newStr.Length;
                            str = str.Substring(sizeNewStr);
                            sizeStr = str.Length;

                            for (int j = 0; j < sizeNewStr; j++)
                            {
                                DoubleList[type](x2, y2, z2, a, p, out x3, out y3, out z3);
                                x2 = x3;
                                y2 = y3;
                                z2 = z3;
                            }
                            AddList[type](PreComputationFor31[sizeNewStr - 1, 0], PreComputationFor31[sizeNewStr - 1, 1], PreComputationFor31[sizeNewStr - 1, 2], x2, y2, z2, a, p, out x3, out y3, out z3);
                            x2 = x3;
                            y2 = y3;
                            z2 = z3;

                            m = m - sizeNewStr;
                        }
                    }
                }
            }
            if (x2 == 0 && y2 == 1)
                z2 = 0;
            else
                z2 = 1;

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

    }
}
