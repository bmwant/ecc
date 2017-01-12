using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Diagnostics;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace EllipticCurveCryptography
{
    public class EllipticCC
    {
        public static void Generate_Point_EC(BigInteger a, BigInteger b, BigInteger p, out List<BigInteger[]> AffinePoints)
        {
            AffinePoints = new List<BigInteger[]>();                                         
            for (int x = 0; x < p; x++)
            {
                for (int y = 0; y < p; y++)
                {
                    if ((y * y) % p == (x * x * x + a * x + b) % p)
                    {
                        BigInteger[] coord = new BigInteger[3];
                        coord[0] = x;
                        coord[1] = y;
                        coord[2] = 1;                  
                        AffinePoints.Add(coord);
                    }
                }               
            }
        }
        public static void generateSimplePointInProjectiveCoord(BigInteger a, BigInteger b, BigInteger p, out List<BigInteger[]> ProjectivePoints)
        {
            ProjectivePoints = new List<BigInteger[]>();
            List <BigInteger[]> AffinePoints = new List<BigInteger[]>();
            AffinePoints.ToArray();
            Generate_Point_EC(a,b,p, out AffinePoints);
            foreach (BigInteger[] array in AffinePoints)
            {
                for (BigInteger z = 2; z < p; z++)
                {
                    BigInteger x = array[0] * z % p;
                    BigInteger y = array[1] * z % p;
                    if ((y * y * z) % p == (x * x * x + a * x * z * z + b * z * z * z) % p) // this if allways true
                    {
                        BigInteger[] coord = new BigInteger[3];
                        coord[0] = x;
                        coord[1] = y;
                        coord[2] = z;
                        ProjectivePoints.Add(coord);
                    }
                }
            }
        }
        public static void generateSimplePointInJocobianCoord(BigInteger a, BigInteger b, BigInteger p, out List<BigInteger[]> JacobianPoints)
        {
            JacobianPoints = new List<BigInteger[]>();
            List<BigInteger[]> AffinePoints = new List<BigInteger[]>();
            AffinePoints.ToArray();
            Generate_Point_EC(a, b, p, out AffinePoints);
            foreach (BigInteger[] array in AffinePoints)
            {
                for (BigInteger z = 2; z < p; z++)
                {
                    BigInteger x = array[0] * z % p;
                    BigInteger y = array[1] * z % p;
                    if ((y * y) % p == (BigInteger.Pow(x,3) + a * x * BigInteger.Pow(z,4) + b * BigInteger.Pow(z,6)) % p) // this if allways true
                    {
                        BigInteger[] coord = new BigInteger[3];
                        coord[0] = x;
                        coord[1] = y;
                        coord[2] = z;
                        JacobianPoints.Add(coord);
                    }
                }
            }
        }
        public static void Generate_Point_EC_(BigInteger a, BigInteger b, BigInteger p, int quantity, out List<BigInteger[]> K)
        {
            K = new List<BigInteger[]>();      
            BigInteger x, y=0, right_side, left_side;
            for (int i = 0; i < quantity; i++)
            {
                do
                {
                    do
                    {
                        int N = (int)Math.Ceiling(Math.Ceiling(BigInteger.Log(p - 1, 2)) / 8);
                        int r = Functions.rand(1, N);
                        x = Functions.random_max(r) + 1;
                        right_side = (x * x * x + a * x + b) % p;
                        if (right_side != 0)
                        {
                            y = Functions.square_root_mod(right_side, p);
                        }
                    }
                    while (y == 0);
                    left_side = (y * y) % p;
                }
                while (left_side != right_side);//поиск корня не всегда правильно работает, поэтому проверяем дополнительно
                
                BigInteger[] coord = new BigInteger[3];
                coord[0] = x;
                coord[1] = y;
                coord[2] = 1;
                K.Add(coord);
            }        
        }

        public static void generatePointEcInProjecriveCoord(BigInteger a, BigInteger b, BigInteger p, int quantity, out List<BigInteger[]> P)
        {
            P = new List<BigInteger[]>();
            BigInteger x, y = 0, z, rightSide, leftSide, d, inv;
            for (int i = 0; i < quantity; i++)
            {
                do
                {
                    do
                    {
                        int N = (int)Math.Ceiling(Math.Ceiling(BigInteger.Log(p - 1, 2)) / 8);
                        int r = Functions.rand(1, N);
                        x = Functions.random_max(r) + 1;
                        z = Functions.random_max(r) + 1;
                        Functions.Extended_Euclid(p, z, out d, out inv);
                        rightSide = (BigInteger.Pow(x, 3) * inv + a * x * z + b * BigInteger.Pow(z, 2)) % p;
                        if (rightSide != 0)
                        {
                            y = Functions.square_root_mod(rightSide, p);
                        }
                    } while (y == 0);
                    leftSide = (y * y) % p;

                } while (leftSide != rightSide);
                BigInteger[] coord = new BigInteger[3];
                coord[0] = x;
                coord[1] = y;
                coord[2] = z;
                P.Add(coord);
            }
        }

        public static BigInteger[,] ReadFromFile(int count, out BigInteger a, out BigInteger p)
        {
            BigInteger[,] K = new BigInteger[count, 3];
            a = 0; p = 0;
            ECC form = new ECC();
            form.openFileDialog1.Filter = "txt файли(*.txt)|*.txt";
            form.openFileDialog1.FileName = "Points";
            form.openFileDialog1.Title = "Оберіть файл з точками ЕК";
            if (form.openFileDialog1.ShowDialog() == DialogResult.Cancel) { }
            else
            {                
                string filename = form.openFileDialog1.FileName;
                FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                for (int j = 0; j <= count; j++)
                {
                    //Значения выделенных точек в виде строки
                    string str = "";
                    str = sr.ReadLine();
                    char[] delimeters = new char[] { '(', ',', ')', '\n', '=', 'a', 'p', ' ' };

                    //Выделяем подстроки, которые содержат координаты точек (массив строк)
                    string[] str2 = str.Split(delimeters, StringSplitOptions.RemoveEmptyEntries);

                    if (j == 0) { a = BigInteger.Parse(str2[0]); p = BigInteger.Parse(str2[1]); }
                    else if (str2.Count() == 2)
                    {
                        K[j - 1, 0] = BigInteger.Parse(str2[0]);
                        K[j - 1, 1] = BigInteger.Parse(str2[1]);
                        K[j - 1, 2] = 1;
                    }
                    else
                    {
                        K[j - 1, 0] = BigInteger.Parse(str2[0]);
                        K[j - 1, 1] = BigInteger.Parse(str2[1]);
                        K[j - 1, 2] = BigInteger.Parse(str2[2]);
                    }

                }
                sr.Close();                
            }
            return K;
        }

        
    }
}
