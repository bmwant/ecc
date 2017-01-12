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
        public static void Generate_Point_EC(BigInteger a, BigInteger b, BigInteger p, out List<BigInteger[]> K)
        {
            
            K = new List<BigInteger[]>();
                                          
            for (int x = 0; x < p; x++)
            {
                for (int y = 0; y < p; y++)
                {
                    if ((y * y - x * x * x - a * x - b) % p == 0)
                    {

                        BigInteger[] coord = new BigInteger[2];
                        coord[0] = x;
                        coord[1] = y;
                        K.Add(coord);
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
                        x = Functions.random_max(r) +1;
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
                
                BigInteger[] coord = new BigInteger[2];
                coord[0] = x;
                coord[1] = y;
                K.Add(coord);
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
