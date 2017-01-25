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
using System.Collections;
using System.Threading.Tasks;
using System.Globalization;
using Excel = Microsoft.Office.Interop.Excel;


namespace EllipticCurveCryptography
{
    public partial class ECC : Form
    {
        Dictionary<String, Dictionary<String, BigInteger>> curveValues = new Dictionary<String, Dictionary<String, BigInteger>>
        {
            {
                "Curve P-192", new Dictionary<String, BigInteger> {
                    {"p", BigInteger.Parse("6277101735386680763835789423207666416083908700390324961279")},
                    {"a", BigInteger.Parse("3099d2bbbfcb2538542dcd5fb078b6ef5f3d6fe2c745de65", NumberStyles.AllowHexSpecifier)},
                    {"b", BigInteger.Parse("64210519e59c80e70fa7e9ab72243049feb8deecc146b9b1", NumberStyles.AllowHexSpecifier)},
                    {"N", BigInteger.Parse("6277101735386680763835789423176059013767194773182842284081")},
                }
            },
            {
                "Curve P-224", new Dictionary<String, BigInteger> {
                    {"p", BigInteger.Parse("26959946667150639794667015087019630673557916260026308143510066298881")},
                    {"a", BigInteger.Parse("5b056c7e11dd68f40469ee7f3c7a7d74f7d121116506d031218291fb", NumberStyles.AllowHexSpecifier)},
                    {"b", BigInteger.Parse("b4050a850c04b3abf54132565044b0b7d7bfd8ba270b39432355ffb4", NumberStyles.AllowHexSpecifier)},
                    {"N", BigInteger.Parse("26959946667150639794667015087019625940457807714424391721682722368061")},
                }
            },
            {
                "Curve P-256", new Dictionary<String, BigInteger> {
                    {"p", BigInteger.Parse("115792089210356248762697446949407573530086143415290314195533631308867097853951")},
                    {"a", BigInteger.Parse("7efba1662985be9403cb055c75d4f7e0ce8d84a9c5114abcaf3177680104fa0d", NumberStyles.AllowHexSpecifier)},
                    {"b", BigInteger.Parse("5ac635d8aa3a93e7b3ebbd55769886bc651d06b0cc53b0f63bce3c3e27d2604b", NumberStyles.AllowHexSpecifier)},
                    {"N", BigInteger.Parse("115792089210356248762697446949407573529996955224135760342422259061068512044369")},
                }
            },
            {
                "Curve P-384", new Dictionary<String, BigInteger> {
                    {"p", BigInteger.Parse("39402006196394479212279040100143613805079739270465446667948293404245721771496870329047266088258938001861606973112319")},
                    {"a", BigInteger.Parse("79d1e655f868f02fff48dcdee14151ddb80643c1406d0ca10dfe6fc52009540a495e8042ea5f744f6e184667cc722483", NumberStyles.AllowHexSpecifier)},
                    {"b", BigInteger.Parse("b3312fa7e23ee7e4988e056be3f82d19181d9c6efe8141120314088f5013875ac656398d8a2ed19d2a85c8edd3ec2aef", NumberStyles.AllowHexSpecifier)},
                    {"N", BigInteger.Parse("39402006196394479212279040100143613805079739270465446667946905279627659399113263569398956308152294913554433653942643")},
                }
            },
            {
                "Curve P-521", new Dictionary<String, BigInteger> {
                    {"p", BigInteger.Parse("6864797660130609714981900799081393217269435300143305409394463459185543183397656052122559640661454554977296311391480858037121987999716643812574028291115057151")},
                    {"a", BigInteger.Parse("0b48bfa5f420a34949539d2bdfc264eeeeb077688e44fbf0ad8f6d0edb37bd6b533281000518e19f1b9ffbe0fe9ed8a3c2200b8f875e523868c70c1e5bf55bad637", NumberStyles.AllowHexSpecifier)},
                    {"b", BigInteger.Parse("6051953eb9618e1c9a1f929a21a0b68540eea2da725b99b315f3b8b489918ef109e156193951ec7e937b1652c0bd3bb1bf073573df883d2c34f1ef451fd46b503f00", NumberStyles.AllowHexSpecifier)},
                    {"N", BigInteger.Parse("6864797660130609714981900799081393217269435300143305409394463459185543183397655394245057746333217197532963996371363321113864768612440380340372808892707005449")},
                }
            }
        };
        public bool Flag = true;
        public ECC()
        {
            InitializeComponent();
        }

        private void Quadrupling_Affine_Coord_3(BigInteger bigInteger, BigInteger bigInteger_2, int p, BigInteger a, BigInteger p_2, out BigInteger x3, out BigInteger y3, out BigInteger z3)
        {
            Quadrupling.Quadrupling_Affine_Coord_3(bigInteger, bigInteger_2, p, a, p_2, out x3, out y3, out z3);
        }

        private void Quadrupling_Affine_Coord_2(BigInteger bigInteger, BigInteger bigInteger_2, int p, BigInteger a, BigInteger p_2, out BigInteger x3, out BigInteger y3, out BigInteger z3)
        {
            Quadrupling.Quadrupling_Affine_Coord_2(bigInteger, bigInteger_2, p, a, p_2, out x3, out y3, out z3);
        }

        private void Quadrupling_Affine_Coord_1(BigInteger bigInteger, BigInteger bigInteger_2, int p, BigInteger a, BigInteger p_2, out BigInteger x3, out BigInteger y3, out BigInteger z3)
        {
            Quadrupling.Quadrupling_Affine_Coord_1(bigInteger, bigInteger_2, p, a, p_2, out x3, out y3, out z3);
        }


        private void Ternary_Affine_Coord_3(BigInteger bigInteger, BigInteger bigInteger_2, int p, BigInteger a, BigInteger p_2, out BigInteger x3, out BigInteger y3, out BigInteger z3)
        {
            Ternary.Ternary_Affine_Coord_3(bigInteger, bigInteger_2, p, a, p_2, out x3, out y3, out z3);
        }

        private void Ternary_Affine_Coord_2(BigInteger bigInteger, BigInteger bigInteger_2, int p, BigInteger a, BigInteger p_2, out BigInteger x3, out BigInteger y3, out BigInteger z3)
        {
            Ternary.Ternary_Affine_Coord_2(bigInteger, bigInteger_2, p, a, p_2, out x3, out y3, out z3);
        }

        private void Ternary_Affine_Coord_1(BigInteger bigInteger, BigInteger bigInteger_2, int p, BigInteger a, BigInteger p_2, out BigInteger x3, out BigInteger y3, out BigInteger z3)
        {
            Ternary.Ternary_Affine_Coord_1(bigInteger, bigInteger_2, p, a, p_2, out x3, out y3, out z3);
        }

        private void Point_Multiplication_Affine_Coord_12(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, int w, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, out double tableTime, int type)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #12"));
            PointMultiplication.Point_Multiplication_Affine_Coord_12(bigInteger, bigInteger_2, bigInteger_3, a, k, w, p,
                out x2, out y2, out z2, out time, out tableTime, type);
        }

        private void Point_Multiplication_Affine_Coord_15(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #15"));
            PointMultiplication.Point_Multiplication_Affine_Coord_15(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, out time, type);
        }

        private void Point_Multiplication_Affine_Coord_16(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #16"));
            PointMultiplication.Point_Multiplication_Affine_Coord_16(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, out time, type);
        }

        private void Point_Multiplication_Affine_Coord_17(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #17"));
            PointMultiplication.Point_Multiplication_Affine_Coord_17(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, out time, type);
        }

        private void Point_Multiplication_Affine_Coord_18(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #18"));
            PointMultiplication.Point_Multiplication_Affine_Coord_18(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, out time, type);
        }

        private void Point_Multiplication_Affine_Coord_19(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, BigInteger a_max, BigInteger b_max)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #19"));
            PointMultiplication.Point_Multiplication_Affine_Coord_19_1(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, out time, type, a_max, b_max);
        }

        private void Point_Multiplication_Affine_Coord_20(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, BigInteger a_max, BigInteger b_max)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #20"));
            PointMultiplication.Point_Multiplication_Affine_Coord_20_2(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, out time, type, a_max, b_max);
        }

        private void Point_Multiplication_Affine_Coord_19_2(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, BigInteger a_max, BigInteger b_max)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #19.2"));
            PointMultiplication.Point_Multiplication_Affine_Coord_19_2(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, out time, type, a_max, b_max);
        }

        private void Point_Multiplication_Affine_Coord_20_1(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, BigInteger a_max, BigInteger b_max)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #20.1"));
            PointMultiplication.Point_Multiplication_Affine_Coord_20_1(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, out time, type, a_max, b_max);
        }

        private void Point_Multiplication_Affine_Coord_21(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #21"));
            PointMultiplication.Point_Multiplication_Affine_Coord_21(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, out time, type);
        }

        private void Point_Multiplication_Affine_Coord_21m(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #21m"));
            PointMultiplication.Point_Multiplication_Affine_Coord_21m(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, out time, type);
        }
        private void Point_Multiplication_Affine_Coord_22(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #22"));
            PointMultiplication.Point_Multiplication_Affine_Coord_22(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, out time, type);
        }
        private void Point_Multiplication_Affine_Coord_22m(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p,
          out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #22m"));
            PointMultiplication.Point_Multiplication_Affine_Coord_22m(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, out time, type);
        }
        private void Point_Multiplication_Affine_Coord_13(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, int w, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #13"));
            PointMultiplication.Point_Multiplication_Affine_Coord_13(bigInteger, bigInteger_2, bigInteger_3, a, k, w, p,
                out x2, out y2, out z2, out time, type);
        }

        private void Point_Multiplication_Affine_Coord_14(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, int w, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #14"));
            PointMultiplication.Point_Multiplication_Affine_Coord_14(bigInteger, bigInteger_2, bigInteger_3, a, k, w, p,
                out x2, out y2, out z2, out time, type);
        }

        private void Point_Multiplication_Affine_Coord_11_2(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, int w, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #11.2"));
            PointMultiplication.Point_Multiplication_Affine_Coord_11_2(bigInteger, bigInteger_2, bigInteger_3, a, k, w, p,
                out x2, out y2, out z2, out time, type);
        }

        private void Point_Multiplication_Affine_Coord_11_1(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, int w, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #11.1"));
            PointMultiplication.Point_Multiplication_Affine_Coord_11_1(bigInteger, bigInteger_2, bigInteger_3, a, k, w, p,
                out x2, out y2, out z2, out time, type);
        }


        private void Point_Multiplication_Affine_Coord_10(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, int w, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, out double tableTime, int type)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #10"));
            PointMultiplication.Point_Multiplication_Affine_Coord_10(bigInteger, bigInteger_2, bigInteger_3, a, k, w, p,
                out x2, out y2, out z2, out time, out tableTime, type);
        }

        private void Point_Multiplication_Affine_Coord_9(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, int w, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #9"));
            PointMultiplication.Point_Multiplication_Affine_Coord_9(bigInteger, bigInteger_2, bigInteger_3, a, k, w, p,
                out x2, out y2, out z2, out time, type);
        }

        private void Point_Multiplication_Affine_Coord_8(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, int w, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #8"));
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PointMultiplication.Point_Multiplication_Affine_Coord_8(bigInteger, bigInteger_2, bigInteger_3, a, k, w, p,
                out x2, out y2, out z2, type);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        private void Point_Multiplication_Affine_Coord_7_2(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, int w, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #7.2"));
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PointMultiplication.Point_Multiplication_Affine_Coord_7_2(bigInteger, bigInteger_2, bigInteger_3, a, k, w, p,
                out x2, out y2, out z2, type);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        private void Point_Multiplication_Affine_Coord_7_1(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, int w, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #7.1"));
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PointMultiplication.Point_Multiplication_Affine_Coord_7_1(bigInteger, bigInteger_2, bigInteger_3, a, k, w, p,
                out x2, out y2, out z2, type);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        private void Point_Multiplication_Affine_Coord_6(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, int w, BigInteger p, out BigInteger x2,
            out BigInteger y2, out BigInteger z2, out double time, out double tableTime, int type)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #6"));
            PointMultiplication.Point_Multiplication_Affine_Coord_6(bigInteger, bigInteger_2, bigInteger_3, a, k, w, p,
                out x2, out y2, out z2, out time, out tableTime, type);
        }

        private void Point_Multiplication_Affine_Coord_5(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, int w, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #5"));
            PointMultiplication.Point_Multiplication_Affine_Coord_5(bigInteger, bigInteger_2, bigInteger_3, a, k, w, p,
                out x2, out y2, out z2, out time, type);
        }

        private void Point_Multiplication_Affine_Coord_4(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, int w, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, out double tableTime, int type)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #4"));
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PointMultiplication.Point_Multiplication_Affine_Coord_4(bigInteger, bigInteger_2, bigInteger_3, a, k, w, p,
                out x2, out y2, out z2, out time, out tableTime, type);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        private void Point_Multiplication_Affine_Coord_3(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, int w, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #3"));
            PointMultiplication.Point_Multiplication_Affine_Coord_3(bigInteger, bigInteger_2, bigInteger_3, a, k, w, p,
                out x2, out y2, out z2, out time, type);
        }

        private void Point_Multiplication_Affine_Coord_2(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #2"));
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PointMultiplication.Point_Multiplication_Affine_Coord_2(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, type);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        private void Point_Multiplication_Affine_Coord_1(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #1"));
            PointMultiplication.Point_Multiplication_Affine_Coord_1(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, type);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts.TotalMilliseconds;
        }

        //With trees
        private void Point_Multiplication_Affine_Coord_27(BigInteger bigIntegerX, BigInteger bigIntegerY, BigInteger bigIntegerZ, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, BigInteger B, BigInteger[] S, BigInteger[] M, int type, out double time)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #27"));
            PointMultiplication.Point_Multiplication_Affine_Coord_27(bigIntegerX, bigIntegerY, bigIntegerZ, a, k, p, out x2, out y2, out z2,
                 B, S, M, type, out time);
        }

        private void Point_Multiplication_Affine_Coord_28(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
   out BigInteger x2, out BigInteger y2, out BigInteger z2, BigInteger B, BigInteger[] S, BigInteger[] M, int type, out double time)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #28"));
            PointMultiplication.Point_Multiplication_Affine_Coord_28(x1, y1, z1, a, k, p, out x2, out y2, out z2,
               B, S, M, type, out time);
        }

        private void Point_Multiplication_Affine_Coord_29(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p, out BigInteger x2,
out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #29"));
            PointMultiplication.Point_Multiplication_Affine_Coord_29(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, out time, type);
        }

        private void Point_Multiplication_Affine_Coord_30(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p, out BigInteger x2,
out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #30"));
            PointMultiplication.Point_Multiplication_Affine_Coord_30(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, out time, type);
        }

        private void Point_Multiplication_Affine_Coord_31(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p, out BigInteger x2,
out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #31"));
            PointMultiplication.Point_Multiplication_Affine_Coord_31(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, out time, type);
        }

        private void Point_Multiplication_Affine_Coord_32(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p, out BigInteger x2,
out BigInteger y2, out BigInteger z2, out double time, int type)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #32"));
            PointMultiplication.Point_Multiplication_Affine_Coord_32(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, out time, type);
        }
        private void PointMultiplication33(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger x2, BigInteger y2, BigInteger z2, BigInteger a, BigInteger k, BigInteger l, int w,
            BigInteger p, out BigInteger x3, out BigInteger y3, out BigInteger z3, out double time, int type)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #33"));
            PointMultiplication.Point_Multiplication33(x1, y1, z1, x2, y2, z2, a, k, l, w, p, out x3, out y3, out z3, out time, type);
        }
        private void AffineToProjective(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger p, out BigInteger x2, out BigInteger y2, out BigInteger z2)
        {
            PointMultiplication.AffineToProjective(x1, y1, z1, p, out x2, out y2, out z2);
        }
        private void ProjectiveToAffine(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger p, out BigInteger x2, out BigInteger y2, out BigInteger z2)
        {
            PointMultiplication.ProjectiveToAffine(x1, y1, z1, p, out x2, out y2, out z2);
        }
        private void AffineToJacobi(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger p, out BigInteger x2, out BigInteger y2, out BigInteger z2)
        {
            PointMultiplication.AffineToJacobi(x1, y1, z1, p, out x2, out y2, out z2);
        }
        private void JacobyToAffine(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger p, out BigInteger x2, out BigInteger y2, out BigInteger z2)
        {
            PointMultiplication.JacobyToAffine(x1, y1, z1, p, out x2, out y2, out z2);
        }
        //Время 3P 4P
        /*
        private void button11_Click(object sender, EventArgs e)
        {
            int quantity = 100;//кол-во значение, которые будут считываться из файла
            int count = 0;
            BigInteger[,] points1 = new BigInteger[quantity, 2];
            BigInteger[,] points2 = new BigInteger[quantity, 2];
            BigInteger[,] points3 = new BigInteger[quantity, 2];
            double time_average1 = 0, time_average2 = 0, time_average3 = 0;
            double time1 = 0, time2 = 0, time3 = 0;

            points1 = EllipticCC.ReadFromFile(quantity);
            points2 = EllipticCC.ReadFromFile(quantity);
            points3 = EllipticCC.ReadFromFile(quantity);

            BigInteger p1, p2, p3, x2, y2, z2, a = 79, b = -3, k = 2;
            string str1 = "4367366322425605456197058237053481450109451937736228685109";
            string str2 = "98986071353297530943384365654849202433121130949888574242236548271225054481127";
            string str3 = "28469537186371909788747437996032617313424030831218833714731819646275278632525462855269869004729489084166420204317071";
            p1 = BigInteger.Parse(str1);
            p2 = BigInteger.Parse(str2);
            p3 = BigInteger.Parse(str3);

            

            FileStream fs = new FileStream("I:\\3P_1.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);

            Stopwatch stopWatch = new Stopwatch();


            for (int i = 0; i < quantity; i++)
            {
                stopWatch.Start();
                Ternary.Ternary_Affine_Coord_1(points1[i, 0], points1[i, 1], 1, a, p1, out x2, out y2, out z2);
               // EllipticCC.Quadrupling_Affine_Coord_3(points1[i, 0], points1[i, 1], 1, a, p1, out x2, out y2, out z2);
                stopWatch.Stop();
                TimeSpan ts1 = stopWatch.Elapsed;
                //time1 += ts1.TotalMilliseconds; 
                stopWatch.Reset();

                stopWatch.Start();
                Ternary.Ternary_Affine_Coord_1(points2[i, 0], points2[i, 1], 1, a, p2, out x2, out y2, out z2);
                //EllipticCC.Quadrupling_Affine_Coord_3(points2[i, 0], points2[i, 1], 1, a, p2, out x2, out y2, out z2);
                stopWatch.Stop();
                TimeSpan ts2 = stopWatch.Elapsed;
                time2 += ts2.TotalMilliseconds;
                stopWatch.Reset();

                stopWatch.Start();
                Ternary.Ternary_Affine_Coord_1(points3[i, 0], points3[i, 1], 1, a, p3, out x2, out y2, out z2);
                //EllipticCC.Quadrupling_Affine_Coord_3(points3[i, 0], points3[i, 1], 1, a, p3, out x2, out y2, out z2);
                stopWatch.Stop();
                TimeSpan ts3 = stopWatch.Elapsed;
                time3 += ts3.TotalMilliseconds;
                stopWatch.Reset();

                stopWatch.Start();
                Ternary.Ternary_Affine_Coord_1(points1[i, 0], points1[i, 1], 1, a, p1, out x2, out y2, out z2);
                //EllipticCC.Quadrupling_Affine_Coord_3(points1[i, 0], points1[i, 1], 1, a, p1, out x2, out y2, out z2);
                stopWatch.Stop();
                ts1 = stopWatch.Elapsed;
                time1 += ts1.TotalMilliseconds;
                stopWatch.Reset();

            }

            time_average1 = time1 / quantity;
            time_average2 = time2 / quantity;
            time_average3 = time3 / quantity;
            sw.WriteLine("192: " + time_average1);
            sw.WriteLine("256: " + time_average2);
            sw.WriteLine("348: " + time_average3);

            richTextBox3.Text += "192: " + time_average1 + "\n";
            richTextBox3.Text += "256: " + time_average2 + "\n";
            richTextBox3.Text += "348: " + time_average3 + "\n";

            sw.Close();           


        }
         */
        private void writePointsInFile_Click(object sender, EventArgs e)
        {
            List<BigInteger[]> points = new List<BigInteger[]>();
            BigInteger a, b, p = 0;
            int quantity;

            a = BigInteger.Parse(textBox2.Text);
            b = BigInteger.Parse(textBox3.Text);
            quantity = int.Parse(textBox9.Text);

            if (radioButton43.Checked)
            {
                try
                {
                    p = BigInteger.Parse(dataGridView3.CurrentCell.Value.ToString());

                    int p_bits = Functions.ToBin(p).Length;

                    if (dataGridView3.SelectedRows == null) MessageBox.Show("Оберіть модуль!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        openFileDialog1.Filter = "txt файли(*.txt)|*.txt";
                        openFileDialog1.FileName = quantity + "_Points_" + p_bits + ".txt";
                        string filename = openFileDialog1.FileName;
                        FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
                        StreamWriter sw = new StreamWriter(fs);

                        if (openFileDialog1.ShowDialog() == DialogResult.Cancel) { sw.Close(); }
                        else {
                            EllipticCC.Generate_Point_EC_(a, b, p, quantity, out points);
                            dataGridView2.Visible = true;
                            dataGridView3.Visible = false;
                            int i = 0;
                            sw.WriteLine("a = " + a + ", p = " + p);
                            foreach (BigInteger[] point in points)
                            {
                                dataGridView2.Rows.Add();
                                dataGridView2.Rows[i].Cells[0].Value = point[0];
                                dataGridView2.Rows[i].Cells[1].Value = point[1];
                                i++;
                                sw.WriteLine(point[0] + "," + point[1] + "," + 1);
                            }
                            MessageBox.Show("Записано успішно!", "УСПІШНО!", MessageBoxButtons.OK, MessageBoxIcon.None);
                            sw.Close();
                        }
                    }
                }
                catch (NullReferenceException ex)
                {
                    MessageBox.Show("Оберіть модуль", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (radioButton42.Checked)
            {
                try
                {
                    p = BigInteger.Parse(dataGridView3.CurrentCell.Value.ToString());

                    int p_bits = Functions.ToBin(p).Length;

                    if (dataGridView3.SelectedRows == null) MessageBox.Show("Оберіть модуль!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        openFileDialog1.Filter = "txt файли(*.txt)|*.txt";
                        openFileDialog1.FileName = quantity + "_PointsInProjectiveCoordinate_" + p_bits + ".txt";
                        string filename = openFileDialog1.FileName;
                        FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
                        StreamWriter sw = new StreamWriter(fs);

                        if (openFileDialog1.ShowDialog() == DialogResult.Cancel) { sw.Close(); }
                        else {
                            EllipticCC.generatePointEcInProjecriveCoord(a, b, p, quantity, out points);
                            dataGridView2.Visible = true;
                            dataGridView3.Visible = false;
                            int i = 0;
                            sw.WriteLine("a = " + a + ", p = " + p);
                            foreach (BigInteger[] point in points)
                            {
                                dataGridView2.Rows.Add();
                                dataGridView2.Rows[i].Cells[0].Value = point[0];
                                dataGridView2.Rows[i].Cells[1].Value = point[1];
                                i++;
                                sw.WriteLine(point[0] + "," + point[1] + "," + point[2]);
                            }
                            MessageBox.Show("Записано успішно!", "УСПІШНО!", MessageBoxButtons.OK, MessageBoxIcon.None);
                            sw.Close();
                        }
                    }
                }
                catch (NullReferenceException ex)
                {
                    MessageBox.Show("Оберіть модуль", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        private void generatePoints_Click(object sender, EventArgs e)
        {
            dataGridView2.Visible = true;
            dataGridView3.Visible = false;

            dataGridView2.RowCount = 1;
            dataGridView3.RowCount = 1;
            dataGridView5.RowCount = 1;

            List<BigInteger[]> points = new List<BigInteger[]>();
            BigInteger a, b, p;

            a = BigInteger.Parse(textBox2.Text);
            b = BigInteger.Parse(textBox3.Text);
            p = BigInteger.Parse(textBox1.Text);

            richTextBox4.Text = a.ToString();
            richTextBox5.Text = p.ToString();

            if (radioButton43.Checked)
            {
                EllipticCC.Generate_Point_EC(a, b, p, out points);
            }
            if (radioButton42.Checked)
            {
                EllipticCC.generateSimplePointInProjectiveCoord(a, b, p, out points);
            }
            if (radioButton41.Checked)
            {
                EllipticCC.generateSimplePointInJocobianCoord(a, b, p, out points);
            }
            int i = 0;
            foreach (BigInteger[] point in points)
            {
                dataGridView1.Rows.Add();
                dataGridView2.Rows.Add();
                dataGridView5.Rows.Add();

                dataGridView2.Rows[i].Cells[0].Value = point[0];
                dataGridView2.Rows[i].Cells[1].Value = point[1];
                dataGridView2.Rows[i].Cells[2].Value = point[2];

                dataGridView1.Rows[i].Cells[0].Value = point[0];
                dataGridView1.Rows[i].Cells[1].Value = point[1];
                dataGridView1.Rows[i].Cells[2].Value = point[2];

                dataGridView5.Rows[i].Cells[0].Value = point[0];
                dataGridView5.Rows[i].Cells[1].Value = point[1];
                dataGridView5.Rows[i].Cells[2].Value = point[2];
                i++;
            }
        }

        private void generateModulo_Click(object sender, EventArgs e)
        {
            BigInteger modulo;
            Boolean result = true;
            dataGridView3.Visible = true;
            dataGridView2.Visible = false;
            dataGridView3.RowCount = 1;
            int j = 0;
            while (!dataGridView3.Rows[j].IsNewRow && dataGridView3.Rows[j] != null)
            {
                dataGridView3.Rows.RemoveAt(j);
                j++;
            }
            int from = int.Parse(textBox6.Text);
            int to = int.Parse(textBox7.Text);

            int quantity = int.Parse(textBox8.Text);

            for (int i = 0; i < quantity; i++)
            {
                int temp = Functions.rand(from, to);
                do
                {
                    do
                    {
                        modulo = Functions.random_max(temp);
                    }
                    while (Primality_Tests.Prime_Test_Miller_Rabin(modulo) == false);

                    for (int k = 1; k < 6; k++)
                    {

                        result = Primality_Tests.Prime_Test_Miller_Rabin(modulo);
                        if (!result)
                        {
                            result = false;
                            break;
                        }
                    }
                }
                while (result == false);
                dataGridView3.Rows.Add();
                dataGridView3.Rows[i].Cells[0].Value = modulo;
            }
        }
        private void downloadPointsFromFile_Click(object sender, EventArgs e)
        {
            int quantity = int.Parse(textBox10.Text);
            BigInteger[,] points = new BigInteger[quantity, 3];
            BigInteger a, p;
            points = EllipticCC.ReadFromFile(quantity, out a, out p);
            richTextBox4.Text = a.ToString();
            richTextBox5.Text = p.ToString();
            dataGridView1.RowCount = 1;
            for (int i = 0; i < quantity; i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = points[i, 0];
                dataGridView1.Rows[i].Cells[1].Value = points[i, 1];
                dataGridView1.Rows[i].Cells[2].Value = points[i, 2];
            }
        }

        private BigInteger[] writeToArray(TextBox t)
        {

            List<BigInteger> list = new List<BigInteger>();
            //string[] text = t.Text.Split(','); 

            foreach (char ch in t.Text)
            {
                if (ch != ',') list.Add(BigInteger.Parse(ch.ToString()));
            }

            list.ToArray();
            BigInteger[] array = new BigInteger[list.Count];
            for (int i = 0; i < list.Count; i++)
                array[i] = list[i];

            return array;
        }

        private int Get_Type()
        {
            int type = 0;
            if (radioButton30.Checked)
                type = 0;
            else if (radioButton31.Checked)
                type = 1;
            else if (radioButton32.Checked)
                type = 2;
            else if (radioButton50.Checked)
                type = 3;
            else if (radioButton49.Checked)
                type = 4;
            else MessageBox.Show("Виберіть систему координат!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return type;
        }

        private void ExportToExcel()
        {
            var excelApp = new Excel.Application();
            // Make the object visible.
            excelApp.Visible = true;

            // Create a new, empty workbook and add it to the collection returned 
            // by property Workbooks. The new workbook becomes the active workbook.
            // Add has an optional parameter for specifying a praticular template. 
            // Because no argument is sent in this example, Add creates a new workbook. 
            Excel.Workbook workbook = excelApp.Workbooks.Add();

            // This example uses a single workSheet. The explicit type casting is
            // removed in a later procedure.
            Excel._Worksheet workSheet = (Excel.Worksheet)excelApp.ActiveSheet;
            workSheet.Cells[1, "A"] = "Назва алгоритму шифрування";
            workSheet.Cells[1, "B"] = "Назва алгоритму скалярного множення";
            workSheet.Cells[1, "C"] = "Назва алгоритму пошуку мультиплікативно оберненого елемента";
            workSheet.Cells[1, "D"] = "Час роботи та частка у відсотках від загального часу";

            workSheet.Cells[2, "D"] = "Додавання точок еліптичної кривої";
            workSheet.Cells[2, "E"] = "Подвоєння точок еліптичної кривої";
            workSheet.Cells[2, "F"] = "Інші операції (відсутні в переліку)";
            workSheet.Cells[2, "G"] = "Загалом (з переліку)";
            workSheet.Cells[2, "H"] = "Загалом (всі)";
            //workSheet.Cells[2, "I"] = "some other";
            workSheet.Columns.AutoFit();
            workbook.SaveAs("filename.xlsx");
            //workSheet.Columns[2].AutoFit();
        }

        private void Multiply_Click(object sender, EventArgs e)
        {
            BigInteger a, b, p, x1, y1, z1, x3, y3, z3, x2 = 0, y2 = 0, z2 = 0, k, l, a_max, b_max;
            int w;
            BigInteger[] S;
            BigInteger[] M;
            BigInteger B;
            B = BigInteger.Parse(textBox26.Text);
            S = writeToArray(textBox29);
            M = writeToArray(textBox30);
            a = BigInteger.Parse(richTextBox4.Text);
            b = -3;
            p = BigInteger.Parse(richTextBox5.Text);
            k = BigInteger.Parse(textBox4.Text);
            l = BigInteger.Parse(textBox47.Text);
            w = int.Parse(textBox5.Text);
            a_max = BigInteger.Parse(textBox27.Text);
            b_max = BigInteger.Parse(textBox28.Text);
            double time = 0;
            double tableTime = 0;
            x1 = BigInteger.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
            y1 = BigInteger.Parse(dataGridView1.CurrentRow.Cells[1].Value.ToString());
            z1 = BigInteger.Parse(dataGridView1.CurrentRow.Cells[2].Value.ToString());


            int type = Get_Type();

            if (x1 == 0 || y1 == 0 || z1 == 0)
                MessageBox.Show("Виберіть точку!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                String Coord_Type = comboBox1.Text;
                if (Coord_Type == "1")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "2")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_2(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "3")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_3(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "4")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_4(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, out tableTime, type))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "5")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_5(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "6")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_6(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, out tableTime, type))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "7.1")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_7_1(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "7.2")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_7_2(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "8")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_8(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "9")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_9(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "10")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_10(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, out tableTime, type))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "11.1")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_11_1(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "15")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_15(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "11.2")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_11_2(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "13")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_13(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "14")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_14(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "16")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_16(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "17")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_17(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "18")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_18(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "19")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_19(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "20")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_20(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "19.2")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_19_2(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "20.1")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_20_1(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "21")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_21(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "22")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_22(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "12")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_12(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, out tableTime, type))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "27")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_27(x1, y1, z1, a, k, p, out x2, out y2, out z2, B, S, M, type, out time))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "28")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_28(x1, y1, z1, a, k, p, out x2, out y2, out z2, B, S, M, type, out time))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "29")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_29(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "30")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_30(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "31")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_31(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "32")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_32(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "21m")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_21m(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "22m")
                    Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_22m(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                else if (Coord_Type == "33")
                {
                    int i = 0;
                    BigInteger[] arr = new BigInteger[6];
                    foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
                    {
                        arr[i] = BigInteger.Parse(row.Cells[0].Value.ToString());
                        arr[i + 1] = BigInteger.Parse(row.Cells[1].Value.ToString());
                        arr[i + 2] = BigInteger.Parse(row.Cells[2].Value.ToString());
                        i += 3;
                    }
                    x1 = arr[0]; y1 = arr[1]; z1 = arr[2];
                    x3 = arr[3]; y3 = arr[4]; z3 = arr[5];
                    Task.Factory.StartNew(() => PointMultiplication33(x1, y1, z1, x3, y3, z3, a, k, l, w, p, out x2, out y2, out z2, out time, type))
                        .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                }
                else
                {
                    MessageBox.Show("Выбирете алгоритм умножения!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void updateDataGridValues(BigInteger x2, BigInteger y2, BigInteger z2, double time)
        {
            dataGridView4.RowCount = 1;
            if (x2 != 0 || y2 != 0 || z2 != 0)
            {
                dataGridView4.Rows[0].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[0].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[0].Cells[3].Value = z2.ToString();
            }
            toolStripStatusLabel2.Text = "Виконано за " + time/1000 + "c";
            toolStripProgressBar1.Value = 0;
            
        }
        //ne pravilno rabotaet
        private void writeTimeInFile_Click(object sender, EventArgs e)
        {
            BigInteger p, x2, y2, z2, a, a_max, b_max;
            int quantity = int.Parse(textBox14.Text);
            int w = int.Parse(textBox5.Text);
            a_max = BigInteger.Parse(textBox27.Text);
            b_max = BigInteger.Parse(textBox28.Text);
            int count = 0;
            BigInteger step = 0, max_k = 0, k;
            List<BigInteger> mass_k = new List<BigInteger>();
            BigInteger[] S;
            BigInteger[] M;
            BigInteger B;
            B = BigInteger.Parse(textBox26.Text);
            S = writeToArray(textBox29);
            M = writeToArray(textBox30);
            if (radioButton17.Checked)
            {
                step = BigInteger.Parse(textBox13.Text);
                max_k = BigInteger.Parse(textBox12.Text);
                k = BigInteger.Parse(textBox11.Text);
                while (k <= max_k)
                {
                    mass_k.Add(k);
                    k += step;
                }
            }
            if (radioButton18.Checked)
            {
                max_k = BigInteger.Parse(textBox19.Text);
                step = 1;
                int left = int.Parse(textBox21.Text);
                int right = int.Parse(textBox20.Text);
                for (int j = 0; j < max_k; j++)
                {
                    mass_k.Add(Functions.rand(left, right));
                }
            }
            if (radioButton19.Checked)
            {
                max_k = BigInteger.Parse(textBox22.Text);
                step = 1;
                int left = int.Parse(textBox24.Text);
                int right = int.Parse(textBox23.Text);
                for (int j = 0; j < max_k; j++)
                {
                    int rand = Functions.rand(left, right);
                    mass_k.Add(Functions.random_max(rand));
                }
            }
            BigInteger[,] points = new BigInteger[quantity, 3];
            double[] time_average;
            if (radioButton17.Checked)
            {
                time_average = new double[(int)(max_k - mass_k[0] / step) + 1]; //14 - kol-vo algoritmov realizovanih
            }
            else
            {
                time_average = new double[mass_k.Count]; //14 - kol-vo algoritmov realizovanih
            }
            double time = 0, time1 = 0;
            points = EllipticCC.ReadFromFile(quantity, out a, out p);
            int p_bits = Functions.ToBin(p).Length;
            openFileDialog1.Filter = "txt файли(*.txt)|*.txt";
            openFileDialog1.FileName = "#1_Time_" + p_bits + ".txt";
            /* if (radioButton1.Checked )
                 openFileDialog1.FileName = "#1_Time_" + p_bits + ".txt";
             else if (radioButton2.Checked )
                 openFileDialog1.FileName = "#2_Time_" + p_bits + ".txt";
             else if (radioButton3.Checked )
                 openFileDialog1.FileName = "#3_Time_" + p_bits + ".txt";
             else if (radioButton4.Checked )
                 openFileDialog1.FileName = "#4_Time_" + p_bits + ".txt";
             else if (radioButton5.Checked )
                 openFileDialog1.FileName = "#5_Time_" + p_bits + ".txt";
             else if (radioButton6.Checked )
                 openFileDialog1.FileName = "#6_Time_" + p_bits + ".txt";
             else if (radioButton7.Checked )
                 openFileDialog1.FileName = "#7_1_Time_" + p_bits + ".txt";
             else if (radioButton8.Checked )
                 openFileDialog1.FileName = "#7_2_Time_" + p_bits + ".txt";
             else if (radioButton9.Checked )
                 openFileDialog1.FileName = "#8_Time_" + p_bits + ".txt";
             else if (radioButton10.Checked )
                 openFileDialog1.FileName = "#9_Time_" + p_bits + ".txt";
             else if (radioButton11.Checked )
                 openFileDialog1.FileName = "#10_Time_" + p_bits + ".txt";
             else if (radioButton12.Checked )
                 openFileDialog1.FileName = "#11_1_Time_" + p_bits + ".txt";
             else if (radioButton20.Checked )
                 openFileDialog1.FileName = "#11_2_Time_" + p_bits + ".txt";
             else if (radioButton13.Checked )
                 openFileDialog1.FileName = "#15_Time_" + p_bits + ".txt";
             else if (radioButton14.Checked )
                 openFileDialog1.FileName = "#12_Time_" + p_bits + ".txt";
             else if (radioButton21.Checked )
                 openFileDialog1.FileName = "#13_Time_" + p_bits + ".txt";
             else if (radioButton22.Checked )
                 openFileDialog1.FileName = "#14_Time_" + p_bits + ".txt";
             else if (radioButton23.Checked )
                 openFileDialog1.FileName = "#16_Time_" + p_bits + ".txt";
             else if (radioButton24.Checked )
                 openFileDialog1.FileName = "#17_Time_" + p_bits + ".txt";
             else if (radioButton25.Checked )
                 openFileDialog1.FileName = "#18_Time_" + p_bits + ".txt";
             else if (radioButton26.Checked )
                 openFileDialog1.FileName = "#19_1_Time_" + p_bits + ".txt";
             else if (radioButton15.Checked )
                 openFileDialog1.FileName = "#19_2_Time_" + p_bits + ".txt";
             else if (radioButton16.Checked )
                 openFileDialog1.FileName = "#20_1_Time_" + p_bits + ".txt";
             else if (radioButton27.Checked )
                 openFileDialog1.FileName = "#20_2_Time_" + p_bits + ".txt";
             else if (radioButton28.Checked )
                 openFileDialog1.FileName = "#21_Time_" + p_bits + ".txt";
             else if (radioButton29.Checked )
                 openFileDialog1.FileName = "#22_Time_" + p_bits + ".txt";
             else if (radioButton35.Checked )
                 openFileDialog1.FileName = "#27_Time_" + p_bits + ".txt";
             else if (radioButton36.Checked )
                 openFileDialog1.FileName = "#28_Time_" + p_bits + ".txt";
             else if (radioButton33.Checked )
                 openFileDialog1.FileName = "#29_Time_" + p_bits + ".txt";
             else if (radioButton34.Checked )
                 openFileDialog1.FileName = "#30_Time_" + p_bits + ".txt";
             else if (radioButton37.Checked )
                 openFileDialog1.FileName = "#31_Time_" + p_bits + ".txt";*/
            string filename = openFileDialog1.FileName;
            FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            int type = 0;
            if (radioButton30.Checked)
            {
                type = 0;
                sw.WriteLine("Афінні координати");
            }
            else if (radioButton31.Checked)
            {
                type = 1;
                sw.WriteLine("Проективні координати");
            }
            else if (radioButton32.Checked)
            {
                type = 2;
                sw.WriteLine("Координати Якобі");
            }
            else if (radioButton50.Checked)
            {
                type = 3;
                sw.WriteLine("Jacoby Chudnovskii");
            }
            else if (radioButton49.Checked)
            {
                type = 4;
                sw.WriteLine("Modified Jacoby");
            }
            else
            {
                MessageBox.Show("Виберіть систему координат!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                sw.WriteLine("Афінні координати");
            }

            /*  if (openFileDialog1.ShowDialog() == DialogResult.Cancel) { sw.Close(); }
              else
              {*/
            Stopwatch stopWatch = new Stopwatch();
            for (int l = 0; l < mass_k.Count; l++)
            {
                for (int i = 0; i < quantity; i++)
                {
                    time = 0;
                    stopWatch.Start();
                    String Coord_Type = comboBox1.Text;
                    if (Coord_Type == "1")
                        Point_Multiplication_Affine_Coord_1(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type);
                    /* else if (radioButton2.Checked)
                         Point_Multiplication_Affine_Coord_2(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, type);
                     else if (radioButton3.Checked)
                         Point_Multiplication_Affine_Coord_3(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2, out time, type);
                     else if (radioButton4.Checked)
                         Point_Multiplication_Affine_Coord_4(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2, out time, type);
                     else if (radioButton5.Checked)
                         Point_Multiplication_Affine_Coord_5(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2, out time, type);
                     else if (radioButton6.Checked)
                         Point_Multiplication_Affine_Coord_6(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2, out time, type);
                     else if (radioButton7.Checked)
                         Point_Multiplication_Affine_Coord_7_1(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2, type);
                     else if (radioButton8.Checked)
                         Point_Multiplication_Affine_Coord_7_2(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2, type);
                     else if (radioButton9.Checked )
                         Point_Multiplication_Affine_Coord_8(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2, type);
                     else if (radioButton10.Checked )
                         Point_Multiplication_Affine_Coord_9(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2, out time, type);
                     else if (radioButton11.Checked )
                         Point_Multiplication_Affine_Coord_10(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2, out time, type);
                     else if (radioButton12.Checked)
                         Point_Multiplication_Affine_Coord_11_1(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2, out time, type);
                     else if (radioButton20.Checked)
                         Point_Multiplication_Affine_Coord_11_2(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2, out time, type);
                     else if (radioButton14.Checked)
                         Point_Multiplication_Affine_Coord_12(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2, out time, type);
                     else if (radioButton21.Checked)
                         Point_Multiplication_Affine_Coord_13(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2, out time, type);
                     else if (radioButton22.Checked)
                         Point_Multiplication_Affine_Coord_14(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2, out time, type);
                     else if (radioButton13.Checked)
                         Point_Multiplication_Affine_Coord_15(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type);
                     else if (radioButton23.Checked)
                         Point_Multiplication_Affine_Coord_16(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type);
                     else if (radioButton24.Checked)
                         Point_Multiplication_Affine_Coord_17(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type);
                     else if (radioButton25.Checked)
                         Point_Multiplication_Affine_Coord_18(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type);
                     else if (radioButton26.Checked)
                         Point_Multiplication_Affine_Coord_19(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type, a_max, b_max);
                     else if (radioButton15.Checked)
                         Point_Multiplication_Affine_Coord_19_2(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type, a_max, b_max);
                     else if (radioButton16.Checked)
                         Point_Multiplication_Affine_Coord_20_1(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type, a_max, b_max);
                     else if (radioButton27.Checked)
                         Point_Multiplication_Affine_Coord_20(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type, a_max, b_max);
                     else if (radioButton28.Checked)
                         Point_Multiplication_Affine_Coord_21(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type);
                     else if (radioButton29.Checked)
                         Point_Multiplication_Affine_Coord_22(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type);
                     else if (radioButton35.Checked)
                         Point_Multiplication_Affine_Coord_27(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, B, S, M, type, out time);
                     else if (radioButton36.Checked)
                         Point_Multiplication_Affine_Coord_28(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, B, S, M, type, out time);
                     else if (radioButton33.Checked)
                         Point_Multiplication_Affine_Coord_29(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type);
                     else if (radioButton34.Checked)
                         Point_Multiplication_Affine_Coord_30(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type);
                     else if (radioButton37.Checked)
                         Point_Multiplication_Affine_Coord_31(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type);}}*/
                    stopWatch.Stop();
                    TimeSpan ts = stopWatch.Elapsed;
                    time1 += ts.TotalMilliseconds;
                }
                if (time == 0) time_average[count] = time1 / quantity;
                else time_average[count] = time / quantity;
                sw.WriteLine(time_average[count]);
                count++;
            }
            double sumTime = 0;
            for (int j = 0; j < time_average.Length; j++)
            {
                sumTime += time_average[j];
            }
            //sw.WriteLine("Среднее время = " + sumTime / count);
            MessageBox.Show("Записано успішно!", "УСПІШНО!", MessageBoxButtons.OK, MessageBoxIcon.None);
            sw.Close();
        }

        private void writeTimeOfAllAlgorithmsInFile_Click(object sender, EventArgs e)
        {
            BigInteger p, x2, y2, z2, a, a_max, b_max;
            int quantity = int.Parse(textBox14.Text);
            BigInteger step = 0, max_k = 0, k;
            List<BigInteger> mass_k = new List<BigInteger>();

            BigInteger[] S;
            BigInteger[] M;
            BigInteger B;
            B = BigInteger.Parse(textBox26.Text);
            S = writeToArray(textBox29);
            M = writeToArray(textBox30);
            if (radioButton17.Checked)
            {
                step = BigInteger.Parse(textBox13.Text);
                max_k = BigInteger.Parse(textBox12.Text);
                k = BigInteger.Parse(textBox11.Text);
                while (k <= max_k)
                {
                    mass_k.Add(k);
                    k += step;
                }
            }
            if (radioButton18.Checked)
            {
                max_k = BigInteger.Parse(textBox19.Text);
                step = 1;
                int left = int.Parse(textBox21.Text);
                int right = int.Parse(textBox20.Text);
                for (int j = 0; j < max_k; j++)
                {
                    int random = Functions.rand(left, right);
                    mass_k.Add(random);
                }
            }
            if (radioButton19.Checked)
            {
                max_k = BigInteger.Parse(textBox22.Text);
                step = 1;
                int left = int.Parse(textBox24.Text);
                int right = int.Parse(textBox23.Text);
                for (int j = 0; j < max_k; j++)
                {
                    int rand = Functions.rand(left, right);
                    mass_k.Add(Functions.random_max(rand));
                }
            }
            int w = int.Parse(textBox5.Text);
            a_max = BigInteger.Parse(textBox27.Text);
            b_max = BigInteger.Parse(textBox28.Text);
            BigInteger[,] points = new BigInteger[quantity, 3];
            double[,] time_average;
            if (radioButton17.Checked)
            {
                time_average = new double[(int)(max_k - mass_k[0] / step) + 1, 28]; //32
            }
            else
            {
                time_average = new double[mass_k.Count, 28]; //32
            }
            double time = 0;
            double tableTime = 0;
            points = EllipticCC.ReadFromFile(quantity, out a, out p);
            int p_bits = Functions.ToBin(p).Length;
            int type = 0;
            string sysCoord = "";
            if (radioButton30.Checked)
            {
                type = 0;
                sysCoord = "AffineCoordinate";
            }
            else if (radioButton31.Checked)
            {
                type = 1;
                sysCoord = "ProjectiveCoordinate";
            }
            else if (radioButton32.Checked)
            {
                type = 2;
                sysCoord = "JacobiCoordinate";
            }
            else if (radioButton50.Checked)
            {
                type = 3;
                sysCoord = "JacobiChudnovskyiCoordinate";
            }
            else if (radioButton49.Checked)
            {
                type = 4;
                sysCoord = "ModifiedJacobiCoordinate";
            }
            else
            {
                MessageBox.Show("Виберіть систему координат!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            openFileDialog1.Filter = "txt файли(*.txt)|*.txt";
            openFileDialog1.FileName = "All_Algorithms_Time_" + p_bits + "_" + sysCoord + ".txt"; //All_Algorithms_Time_
            string filename = openFileDialog1.FileName;
            FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(sysCoord);

            if (openFileDialog1.ShowDialog() == DialogResult.Cancel) { sw.Close(); }
            else
            {
                Stopwatch stopWatch = new Stopwatch();
                int j = 0;
                int numberK = 0;
                for (int l = 0; l < mass_k.Count; l++)
                {
                    textBox42.Text = null;
                    numberK++;
                    textBox42.AppendText(numberK.ToString());
                    time = 0;
                    for (int i = 0; i < quantity; i++)
                    {
                        TimeSpan ts = new TimeSpan();
                        
                        stopWatch.Start();
                        Point_Multiplication_Affine_Coord_1(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 0] += ts.TotalMilliseconds;
                        stopWatch.Reset();

                        stopWatch.Start();
                        Point_Multiplication_Affine_Coord_2(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 1] += ts.TotalMilliseconds;
                        stopWatch.Reset();

                        time = 0;
                        Point_Multiplication_Affine_Coord_3(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2, out time, type);
                        time_average[j, 2] += time;
                        stopWatch.Reset();
                        
                        time = 0;
                        Point_Multiplication_Affine_Coord_4(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2, out time, out tableTime, type);
                        time_average[j, 3] += time;
                        stopWatch.Reset();

                        time = 0;
                        Point_Multiplication_Affine_Coord_5(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2, out time, type);
                        time_average[j, 4] += time;
                        stopWatch.Reset();

                        time = 0;
                        Point_Multiplication_Affine_Coord_6(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2, out time, out tableTime, type);
                        time_average[j, 5] += time;
                        stopWatch.Reset();
                        
                        stopWatch.Start();
                        Point_Multiplication_Affine_Coord_7_1(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2, out time, type);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 6] += ts.TotalMilliseconds;
                        stopWatch.Reset();

                        stopWatch.Start();
                        Point_Multiplication_Affine_Coord_7_2(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2, out time, type);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 7] += ts.TotalMilliseconds;
                        stopWatch.Reset();

                        stopWatch.Start();
                        Point_Multiplication_Affine_Coord_8(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2, out time, type);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 8] += ts.TotalMilliseconds;
                        stopWatch.Reset();

                        time = 0;
                        Point_Multiplication_Affine_Coord_9(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2, out time, type);
                        time_average[j, 9] += time;
                        stopWatch.Reset();

                        time = 0;
                        Point_Multiplication_Affine_Coord_10(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2, out time, out tableTime, type);
                        time_average[j, 10] += time;
                        stopWatch.Reset();

                        time = 0;
                        Point_Multiplication_Affine_Coord_11_1(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2, out time, type);
                        time_average[j, 11] += time;
                        stopWatch.Reset();

                        time = 0;
                        Point_Multiplication_Affine_Coord_11_2(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2, out time, type);
                        time_average[j, 12] += time;
                        stopWatch.Reset();

                        time = 0;
                        Point_Multiplication_Affine_Coord_12(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2, out time, out tableTime, type);
                        time_average[j, 13] += time;
                        stopWatch.Reset();
                        /*
                        time = 0;
                        Point_Multiplication_Affine_Coord_13(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2, out time, type);
                        time_average[j, 14] += time;
                        stopWatch.Reset();

                        time = 0;
                        Point_Multiplication_Affine_Coord_14(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2, out time, type);
                        time_average[j, 15] += time;
                        stopWatch.Reset();

                        stopWatch.Start();
                        Point_Multiplication_Affine_Coord_15(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 16] += ts.TotalMilliseconds;
                        stopWatch.Reset();

                        stopWatch.Start();
                        Point_Multiplication_Affine_Coord_16(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 17] += ts.TotalMilliseconds;
                        stopWatch.Reset();
                        */
                        stopWatch.Start();
                        Point_Multiplication_Affine_Coord_17(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 14] += ts.TotalMilliseconds;
                        stopWatch.Reset();

                        stopWatch.Start();
                        Point_Multiplication_Affine_Coord_18(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 15] += ts.TotalMilliseconds;
                        stopWatch.Reset();

                        stopWatch.Start();
                        Point_Multiplication_Affine_Coord_19(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type, a_max, b_max);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 16] += ts.TotalMilliseconds;
                        stopWatch.Reset();

                        stopWatch.Start();
                        Point_Multiplication_Affine_Coord_19_2(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type, a_max, b_max);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 17] += ts.TotalMilliseconds;
                        stopWatch.Reset();

                        stopWatch.Start();
                        Point_Multiplication_Affine_Coord_20_1(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type, a_max, b_max);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 18] += ts.TotalMilliseconds;
                        stopWatch.Reset();

                        stopWatch.Start();
                        Point_Multiplication_Affine_Coord_20(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type, a_max, b_max);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 19] += ts.TotalMilliseconds;
                        stopWatch.Reset();
                        
                        stopWatch.Start();
                        Point_Multiplication_Affine_Coord_21(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 20] += ts.TotalMilliseconds;
                        stopWatch.Reset();

                        stopWatch.Start();
                        Point_Multiplication_Affine_Coord_22(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 21] += ts.TotalMilliseconds;
                        stopWatch.Reset();

                        stopWatch.Start();
                        Point_Multiplication_Affine_Coord_27(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, B, S, M, type, out time);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 22] += ts.TotalMilliseconds;
                        stopWatch.Reset();

                        stopWatch.Start();
                        Point_Multiplication_Affine_Coord_28(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, B, S, M, type, out time);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 23] += ts.TotalMilliseconds;
                        stopWatch.Reset();

                        time = 0;
                        Point_Multiplication_Affine_Coord_29(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type);
                        time_average[j, 24] += time;
                        stopWatch.Reset();

                        time = 0;
                        Point_Multiplication_Affine_Coord_30(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type);
                        time_average[j, 25] += time;
                        stopWatch.Reset();

                        time = 0;
                        Point_Multiplication_Affine_Coord_31(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type);
                        time_average[j, 26] += time;
                        stopWatch.Reset();

                        time = 0;
                        Point_Multiplication_Affine_Coord_32(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type);
                        time_average[j, 27] += time;
                        stopWatch.Reset();
                        /*
                        stopWatch.Start();
                        Point_Multiplication_Affine_Coord_21m(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 30] += ts.TotalMilliseconds;
                        stopWatch.Reset();

                        stopWatch.Start();
                        Point_Multiplication_Affine_Coord_22m(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 31] += ts.TotalMilliseconds;
                        stopWatch.Reset();*/

                    }
                    j++;

                }

                double[] result = getAverageTime(time_average, quantity);
                for (int i = 0; i < result.Length; i++)
                {
                    sw.WriteLine(result[i]);
                }


                MessageBox.Show("Записано успішно!", "УСПІШНО!", MessageBoxButtons.OK, MessageBoxIcon.None);
                sw.Close();
            }

        }

        public static double[] getAverageTime(double[,] timeAverage, int quantity)
        {
            /* double[] result = new double[22];
             int countRows = timeAverage.GetLength(0);
             int countColumns = timeAverage.GetLength(1);*/


            int countRows = timeAverage.GetLength(0);
            int countColumns = timeAverage.GetLength(1);
            double[] result = new double[countColumns];

            double temp;

            for (int j = 0; j < countColumns; j++)
            {
                temp = 0;
                for (int i = 0; i < countRows; i++)
                {
                    temp += timeAverage[i, j];
                }

                result[j] = temp / (quantity * countRows);
            }
            return result;
        }


        private void multiplyAllAlgorithms_Click(object sender, EventArgs e)
        {
            dataGridView4.Rows.Clear();
            BigInteger a, b, p, x1, y1, z1, x2 = 0, y2 = 0, z2 = 0, k, a_max, b_max;
            int w;
            a = BigInteger.Parse(richTextBox4.Text);
            b = -3;
            p = BigInteger.Parse(richTextBox5.Text);
            k = BigInteger.Parse(textBox4.Text);
            w = int.Parse(textBox5.Text);

            BigInteger[] S;
            BigInteger[] M;
            BigInteger B;
            B = BigInteger.Parse(textBox26.Text);
            S = writeToArray(textBox29);
            M = writeToArray(textBox30);
            string[] numOfAlg = new string[] { "1", "2", "3", "4", "5", "6", "7_1", "7_2", "8", "9", "10", "11_1", "11_2", "12", "13", "14", "15", "16", "17", "18", "19_1", "19_2", "20_1", "20_2", "21", "22", "27", "28", "29", "30", "31", "32", "21m", "22m"};  
            double time = 0;
            double tableTime = 0;
            x1 = BigInteger.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
            y1 = BigInteger.Parse(dataGridView1.CurrentRow.Cells[1].Value.ToString());
            z1 = BigInteger.Parse(dataGridView1.CurrentRow.Cells[2].Value.ToString());
            a_max = BigInteger.Parse(textBox27.Text);
            b_max = BigInteger.Parse(textBox28.Text);

            int type = Get_Type();

            if (x1 == 0 || y1 == 0)
                MessageBox.Show("Выбирете точку!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                int i = 0;
                dataGridView4.RowCount = 1;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_2(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_3(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_4(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, out tableTime, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_5(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_6(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, out tableTime, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_7_1(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_7_2(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_8(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_9(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_10(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, out tableTime, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_11_1(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_11_2(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_12(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, out tableTime, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_13(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_14(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_15(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_16(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_17(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_18(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_19(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_19_2(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_20_1(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_20(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_21(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_22(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_27(x1, y1, z1, a, k, p, out x2, out y2, out z2, B, S, M, type, out time);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_28(x1, y1, z1, a, k, p, out x2, out y2, out z2, B, S, M, type, out time);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_29(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_30(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_31(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_32(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_21m(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_22m(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                for (int s = 0; s < numOfAlg.Length; s++)
                {
                    dataGridView4.Rows[s].Cells[0].Value = numOfAlg[s];
                }
            }
        }
        private void ToAffineButton_Click(object sender, EventArgs e)
        {
            BigInteger p, x1, y1, z1, x2 = 0, y2 = 1, z2 = 0;
            foreach (DataGridViewRow row in this.dataGridView5.SelectedRows)
            {
                textBox43.Text = row.Cells[0].Value.ToString();
                textBox44.Text = row.Cells[1].Value.ToString();
                textBox45.Text = row.Cells[2].Value.ToString();
            }
            x1 = BigInteger.Parse(textBox43.Text);
            y1 = BigInteger.Parse(textBox44.Text);
            z1 = BigInteger.Parse(textBox45.Text);
            p = BigInteger.Parse(textBox15.Text);
            if (radioButton45.Checked)
                ProjectiveToAffine(x1, y1, z1, p, out x2, out y2, out z2);
            if (radioButton44.Checked)
                JacobyToAffine(x1, y1, z1, p, out x2, out y2, out z2);
            dataGridView7.RowCount = 1;
            dataGridView7.Rows.Add();
            dataGridView7.Rows[0].Cells[0].Value = x2.ToString();
            dataGridView7.Rows[0].Cells[1].Value = y2.ToString();
            dataGridView7.Rows[0].Cells[2].Value = z2.ToString();
        }
        private void RowHeaderMouse_Click(object sender, DataGridViewCellMouseEventArgs e)
        {           
                /*
                foreach (DataGridViewRow row in this.dataGridView5.SelectedRows)
                {
                    textBox43.Text = row.Cells[0].Value.ToString();
                    textBox44.Text = row.Cells[1].Value.ToString();
                    textBox45.Text = row.Cells[2].Value.ToString();
                }
                foreach (DataGridViewRow row in this.dataGridView6.SelectedRows)
                {
                    textBox43.Text = row.Cells[0].Value.ToString();
                    textBox44.Text = row.Cells[1].Value.ToString();
                    textBox45.Text = row.Cells[2].Value.ToString();
                }*/
            }
        private void Add_Click(object sender, EventArgs e)
        {
            BigInteger a, b, p, x1 = 0, y1 = 1, z1 = 0, x2 = 0, y2 = 1, z2 = 0, x3 = 0, y3 = 1, z3 = 0;
            a = BigInteger.Parse(textBox16.Text);
            //b = BigInteger.Parse(textBox3.Text);
            p = BigInteger.Parse(textBox15.Text);
            int i = 0;
            BigInteger[] arr = new BigInteger[6];
            foreach (DataGridViewRow row in this.dataGridView5.SelectedRows)
            {
                arr[i] = BigInteger.Parse(row.Cells[0].Value.ToString());
                arr[i + 1] = BigInteger.Parse(row.Cells[1].Value.ToString());
                arr[i + 2] = BigInteger.Parse(row.Cells[2].Value.ToString());
                i += 3;
            }
            x1 = arr[0]; y1 = arr[1]; z1 = arr[2];
            x2 = arr[3]; y2 = arr[4]; z2 = arr[5];
            if (arr == null)
            {
                MessageBox.Show("Выберите две точки!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (radioButton43.Checked)
                    PointMultiplication.Add_Affine_Coord(x1, y1, z1, x2, y2, z2, a, p, out x3, out y3, out z3);
                if (radioButton42.Checked)
                    PointMultiplication.Add_Projective_Coord(x1, y1, z1, x2, y2, z2, a, p, out x3, out y3, out z3);
                if (radioButton41.Checked)
                    PointMultiplication.Add_Jacoby_Coord(x1, y1, z1, x2, y2, z2, a, p, out x3, out y3, out z3);
            }
            dataGridView6.RowCount = 1;
            dataGridView6.Rows.Add();
            dataGridView6.Rows[0].Cells[0].Value = x3.ToString();
            dataGridView6.Rows[0].Cells[1].Value = y3.ToString();
            dataGridView6.Rows[0].Cells[2].Value = z3.ToString();
        }
        private void Double_Click(object sender, EventArgs e)
        {
            BigInteger a, b, p, x1, y1, z1, x3 = 0, y3 = 0, z3 = 0;
            a = BigInteger.Parse(textBox16.Text);
            //b = BigInteger.Parse(textBox3.Text);
            p = BigInteger.Parse(textBox15.Text);
            x1 = BigInteger.Parse(dataGridView5.CurrentRow.Cells[0].Value.ToString());
            y1 = BigInteger.Parse(dataGridView5.CurrentRow.Cells[1].Value.ToString());
            z1 = BigInteger.Parse(dataGridView5.CurrentRow.Cells[2].Value.ToString());
            if (x1 == 0 || y1 == 0 || z1 == 0)
            {
                MessageBox.Show("Выберите точку!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (radioButton43.Checked)
                    PointMultiplication.Double_Affine_Coord(x1, y1, 1, a, p, out x3, out y3, out z3);
                if (radioButton42.Checked)
                    PointMultiplication.Double_Projective_Coord(x1, y1, z1, a, p, out x3, out y3, out z3);
                if (radioButton41.Checked)
                    PointMultiplication.Double_Jacoby_Coord(x1, y1, z1, a, p, out x3, out y3, out z3);
            }           
            dataGridView6.RowCount = 1;
            dataGridView6.Rows.Add();
            dataGridView6.Rows[0].Cells[0].Value = x3.ToString();
            dataGridView6.Rows[0].Cells[1].Value = y3.ToString();
            dataGridView6.Rows[0].Cells[2].Value = z3.ToString();
        }

        private void ternary_Click(object sender, EventArgs e)
        {
            BigInteger a, b, p, x1, y1, x3 = 0, y3 = 0, z3 = 0;
            a = BigInteger.Parse(textBox16.Text);
            //b = BigInteger.Parse(textBox3.Text);
            p = BigInteger.Parse(textBox15.Text);

            x1 = BigInteger.Parse(dataGridView5.CurrentRow.Cells[0].Value.ToString());
            y1 = BigInteger.Parse(dataGridView5.CurrentRow.Cells[1].Value.ToString());
            dataGridView6.RowCount = 1;
            int i = 0;
            if (checkBox1.Checked)
            {
                x3 = 0; y3 = 0; z3 = 0;
                Ternary.Ternary_Affine_Coord_1(x1, y1, 1, a, p, out x3, out y3, out z3);
                
                dataGridView6.Rows.Add();
                dataGridView6.Rows[i].Cells[0].Value = x3.ToString();
                dataGridView6.Rows[i].Cells[1].Value = y3.ToString();
                i++;
            }
            if (checkBox2.Checked)
            {
                x3 = 0; y3 = 0; z3 = 0;
                Ternary.Ternary_Affine_Coord_2(x1, y1, 1, a, p, out x3, out y3, out z3);

                dataGridView6.Rows.Add();
                dataGridView6.Rows[i].Cells[0].Value = x3.ToString();
                dataGridView6.Rows[i].Cells[1].Value = y3.ToString();
                i++;
            }
            if (checkBox3.Checked)
            {
                x3 = 0; y3 = 0; z3 = 0;
                Ternary.Ternary_Affine_Coord_3(x1, y1, 1, a, p, out x3, out y3, out z3);

                dataGridView6.Rows.Add();
                dataGridView6.Rows[i].Cells[0].Value = x3.ToString();
                dataGridView6.Rows[i].Cells[1].Value = y3.ToString();
                i++;
            }

        }

        private void quadruple_Click(object sender, EventArgs e)
        {
            BigInteger a, b, p, x1, y1, x3 = 0, y3 = 0, z3 = 0;
            a = BigInteger.Parse(textBox16.Text);
            //b = BigInteger.Parse(textBox3.Text);
            p = BigInteger.Parse(textBox15.Text);

            x1 = BigInteger.Parse(dataGridView5.CurrentRow.Cells[0].Value.ToString());
            y1 = BigInteger.Parse(dataGridView5.CurrentRow.Cells[1].Value.ToString());
            dataGridView6.RowCount = 1;

            int i = 0;

            if (checkBox1.Checked)
            {
                x3 = 0; y3 = 0; z3 = 0;
                Quadrupling.Quadrupling_Affine_Coord_1(x1, y1, 1, a, p, out x3, out y3, out z3);

                dataGridView6.Rows.Add();
                dataGridView6.Rows[i].Cells[0].Value = x3.ToString();
                dataGridView6.Rows[i].Cells[1].Value = y3.ToString();
                i++;
            }
            if (checkBox2.Checked)
            {
                x3 = 0; y3 = 0; z3 = 0;
                Quadrupling.Quadrupling_Affine_Coord_2(x1, y1, 1, a, p, out x3, out y3, out z3);

                dataGridView6.Rows.Add();
                dataGridView6.Rows[i].Cells[0].Value = x3.ToString();
                dataGridView6.Rows[i].Cells[1].Value = y3.ToString();
                i++;
            }
            if (checkBox3.Checked)
            {
                x3 = 0; y3 = 0; z3 = 0;
                Quadrupling.Quadrupling_Affine_Coord_3(x1, y1, 1, a, p, out x3, out y3, out z3);

                dataGridView6.Rows.Add();
                dataGridView6.Rows[i].Cells[0].Value = x3.ToString();
                dataGridView6.Rows[i].Cells[1].Value = y3.ToString();
                i++;
            }
        }

        private void dataGridView3_Click(object sender, EventArgs e)
        {
            BigInteger p = BigInteger.Parse(dataGridView3.CurrentCell.Value.ToString());
            textBox1.Text = p.ToString();
            textBox48.Text = p.ToString();
        }

        private void downloadPointsFromFile2_Click(object sender, EventArgs e)
        {
            int quantity = int.Parse(textBox18.Text);
            BigInteger[,] points = new BigInteger[quantity, 2];
            BigInteger a, p;
            points = EllipticCC.ReadFromFile(quantity, out a, out p);
            textBox16.Text = a.ToString();
            textBox15.Text = p.ToString();
            dataGridView5.RowCount = 1;
            for (int i = 0; i < quantity; i++)
            {
                dataGridView5.Rows.Add();
                dataGridView5.Rows[i].Cells[0].Value = points[i, 0];
                dataGridView5.Rows[i].Cells[1].Value = points[i, 1];
                dataGridView5.Rows[i].Cells[2].Value = points[i, 2];
            }
        }

        private void Bonus1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView2.Visible = true;
            dataGridView3.Visible = false;

            dataGridView2.RowCount = 1;
            dataGridView3.RowCount = 1;
            dataGridView5.RowCount = 1;

            List<BigInteger[]> points = new List<BigInteger[]>();
            BigInteger a, b, p;
            int po;

            a = BigInteger.Parse(textBox2.Text);
            b = BigInteger.Parse(textBox3.Text);
            p = BigInteger.Parse(textBox1.Text);
            if (textBox25.Text != "")
            {
                po = int.Parse(textBox25.Text);
                richTextBox4.Text = a.ToString();
                richTextBox5.Text = p.ToString();

                if (radioButton43.Checked)
                {
                    EllipticCC.Generate_Point_EC(a, b, p, out points);
                }
                if (radioButton42.Checked)
                {
                    EllipticCC.generateSimplePointInProjectiveCoord(a, b, p, out points);
                }
                if (radioButton41.Checked)
                {
                    EllipticCC.generateSimplePointInJocobianCoord(a, b, p, out points);
                }
                
                int i = 0;

                if (points.Count > po)
                {
                    foreach (BigInteger[] point in points)
                    {
                        if (i < po)
                        {
                            dataGridView1.Rows.Add();
                            dataGridView2.Rows.Add();
                            dataGridView5.Rows.Add();

                            dataGridView2.Rows[i].Cells[0].Value = point[0];
                            dataGridView2.Rows[i].Cells[1].Value = point[1];
                            dataGridView2.Rows[i].Cells[2].Value = point[2];

                            dataGridView1.Rows[i].Cells[0].Value = point[0];
                            dataGridView1.Rows[i].Cells[1].Value = point[1];
                            dataGridView1.Rows[i].Cells[2].Value = point[2];

                            dataGridView5.Rows[i].Cells[0].Value = point[0];
                            dataGridView5.Rows[i].Cells[1].Value = point[1];
                            dataGridView5.Rows[i].Cells[2].Value = point[2];                           
                            i++;
                        }
                        else break;
                    }
                }
                else
                {
                    foreach (BigInteger[] point in points)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView2.Rows.Add();
                        dataGridView5.Rows.Add();

                        dataGridView2.Rows[i].Cells[0].Value = point[0];
                        dataGridView2.Rows[i].Cells[1].Value = point[1];
                        dataGridView2.Rows[i].Cells[2].Value = point[2];

                        dataGridView1.Rows[i].Cells[0].Value = point[0];
                        dataGridView1.Rows[i].Cells[1].Value = point[1];
                        dataGridView1.Rows[i].Cells[2].Value = point[2];

                        dataGridView5.Rows[i].Cells[0].Value = point[0];
                        dataGridView5.Rows[i].Cells[1].Value = point[1];
                        dataGridView5.Rows[i].Cells[2].Value = point[2];
                        i++;
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите количество точек!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox15.Text = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox16.Text = textBox2.Text;
        }


        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox17.Text = textBox3.Text;
        }

        private void textBox25_TextChanged(object sender, EventArgs e)
        {
            textBox18.Text = textBox25.Text;
        }

        private void ECC_Load(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();

            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 10000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 5000;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;
            toolTip1.SetToolTip(this.groupBox11, "k береться з GroupBox \"Запис часу у файл\"");
            toolTip1.SetToolTip(this.groupBox12, "k береться з GroupBox \"Запис часу у файл\"");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView4.Rows.Clear();
            BigInteger a, b, p, x1, y1, z1, x2 = 0, y2 = 0, z2 = 0, k, a_max, b_max;
            int w;
            a = BigInteger.Parse(richTextBox4.Text);
            b = -3;
            p = BigInteger.Parse(richTextBox5.Text);

            a_max = BigInteger.Parse(textBox27.Text);
            b_max = BigInteger.Parse(textBox28.Text);

            k = BigInteger.Parse(textBox4.Text);
            w = int.Parse(textBox5.Text);

            BigInteger[] S;
            BigInteger[] M;
            BigInteger B;

            B = BigInteger.Parse(textBox26.Text);
            S = writeToArray(textBox29);
            M = writeToArray(textBox30);

            string[] numOfAlg = new string[] { "1", "1", "2", "2", "3", "3", "4", "4", "5", "5", "6", "6", "7_1", "7_1", "7_2", "7_2", "8", "8", "9", "9", "10", "10", "11_1", "11_1", "11_2", "11_2", "12", "12", 
                "13", "13", "14", "14", "15", "15", "16", "16", "17", "17", "18", "18", "19_1", "19_1", "19_2", "19_2", "20_1", "20_1", "20_2", "20_2", "21", "21", "22", "22", "27", "27", "28", "28", "29", "29", "30", "30", "31","31","32","32"};

            double time = 0;
            double tableTime = 0;
            x1 = BigInteger.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
            y1 = BigInteger.Parse(dataGridView1.CurrentRow.Cells[1].Value.ToString());
            z1 = BigInteger.Parse(dataGridView1.CurrentRow.Cells[2].Value.ToString());

            BigInteger x3, y3, z3;

            int type = 1;

            if (x1 == 0 || y1 == 0)
                MessageBox.Show("Выбирете точку!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                int i = 0;
                dataGridView4.RowCount = 1;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                ProjectiveToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_2(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                ProjectiveToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_3(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                ProjectiveToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_4(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, out tableTime, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                ProjectiveToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_5(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                ProjectiveToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_6(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, out tableTime, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                ProjectiveToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_7_1(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                ProjectiveToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_7_2(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                ProjectiveToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_8(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                ProjectiveToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_9(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                ProjectiveToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_10(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, out tableTime, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                ProjectiveToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;

                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_11_1(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                ProjectiveToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;

                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_11_2(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                ProjectiveToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_12(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, out tableTime, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                ProjectiveToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_13(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                ProjectiveToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_14(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                ProjectiveToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_15(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                ProjectiveToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_16(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                ProjectiveToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_17(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                ProjectiveToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;

                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_18(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                ProjectiveToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_19(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                ProjectiveToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_19_2(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                ProjectiveToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_20_1(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                ProjectiveToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_20(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                ProjectiveToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();



                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_21(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                ProjectiveToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_22(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                ProjectiveToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_27(x1, y1, z1, a, k, p, out x2, out y2, out z2, B, S, M, type, out time);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                ProjectiveToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_28(x1, y1, z1, a, k, p, out x2, out y2, out z2, B, S, M, type, out time);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                ProjectiveToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_29(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                ProjectiveToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_30(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                ProjectiveToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_31(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                ProjectiveToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_32(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                ProjectiveToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();

                for (int s = 0; s < numOfAlg.Length; s++)
                {
                    dataGridView4.Rows[s].Cells[0].Value = numOfAlg[s];
                }

            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView4.Rows.Clear();
            BigInteger a, b, p, x1, y1, z1, x2 = 0, y2 = 0, z2 = 0, k, a_max, b_max;
            int w;
            a = BigInteger.Parse(richTextBox4.Text);
            b = -3;
            p = BigInteger.Parse(richTextBox5.Text);
            a_max = BigInteger.Parse(textBox27.Text);
            b_max = BigInteger.Parse(textBox28.Text);
            k = BigInteger.Parse(textBox4.Text);
            w = int.Parse(textBox5.Text);
            BigInteger[] S;
            BigInteger[] M;
            BigInteger B;
            B = BigInteger.Parse(textBox26.Text);
            S = writeToArray(textBox29);
            M = writeToArray(textBox30);

            string[] numOfAlg = new string[] { "1", "1", "2", "2", "3", "3", "4", "4", "5", "5", "6", "6", "7_1", "7_1", "7_2", "7_2", "8", "8", "9", "9", "10", "10", "11_1", "11_1", "11_2", "11_2", "12", "12", 
                "13", "13", "14", "14", "15", "15", "16", "16", "17", "17", "18", "18", "19_1", "19_1", "19_2", "19_2", "20_1", "20_1", "20_2", "20_2", "21", "21", "22", "22", "27", "27", "28", "28", "29", "29",
                "30", "30", "31", "31", "32", "32"};

            double time = 0;
            double tableTime = 0;
            x1 = BigInteger.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
            y1 = BigInteger.Parse(dataGridView1.CurrentRow.Cells[1].Value.ToString());
            z1 = BigInteger.Parse(dataGridView1.CurrentRow.Cells[2].Value.ToString());

            BigInteger x3, y3, z3;
            int type = 2;
            if (x1 == 0 || y1 == 0)
                MessageBox.Show("Выбирете точку!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                int i = 0;
                dataGridView4.RowCount = 1;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_2(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_3(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_4(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, out tableTime, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_5(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_6(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, out tableTime, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_7_1(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_7_2(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_8(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_9(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_10(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, out tableTime, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;

                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_11_1(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;

                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_11_2(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_12(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, out tableTime, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_13(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_14(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_15(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_16(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_17(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;

                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_18(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_19(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_19_2(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_20_1(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_20(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_21(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_22(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_27(x1, y1, z1, a, k, p, out x2, out y2, out z2, B, S, M, type, out time);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_28(x1, y1, z1, a, k, p, out x2, out y2, out z2, B, S, M, type, out time);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_29(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_30(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_31(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_32(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                for (int s = 0; s < numOfAlg.Length; s++)
                {
                    dataGridView4.Rows[s].Cells[0].Value = numOfAlg[s];
                }
            }
        }

        private void groupBox11_Enter(object sender, EventArgs e)
        {

        }

        private void TestB_Click(object sender, EventArgs e)
        {
            int type = 0;
            if (radioButton30.Checked)
                type = 0;
            else if (radioButton31.Checked)
                type = 1;
            else if (radioButton32.Checked)
                type = 2;
            BigInteger p, x2, y2, z2, a;
            int quantity = int.Parse(textBox14.Text), num;
            BigInteger step = 0;
            string[] name;
            string fname;
            num = 4;// 11;
            name = new string[4];
            name[0] = "2";
            name[1] = "4";
            name[2] = "6";
            name[3] = "8";
            /*name[0] = "3";
            name[1] = "4";
            name[2] = "5";
            name[3] = "6";
            name[4] = "9";
            name[5] = "10";
            name[6] = "11_1";
            name[7] = "11_2";
            name[8] = "12";
            name[9] = "13";
            name[10] = "14";*/
            fname = "W";

            BigInteger Start = BigInteger.Parse(TBWStart.Text);
            BigInteger End = BigInteger.Parse(TBWEnd.Text);
            BigInteger Step = BigInteger.Parse(TBWStep.Text);
            Int32 W = Int32.Parse(textBox5.Text);
            BigInteger a_max = BigInteger.Parse(textBox27.Text);
            BigInteger b_max = BigInteger.Parse(textBox28.Text);
            int wNum = 0;
            for (BigInteger i = Start; i < End; i+=Step)
            {
                wNum++;
            }
            BigInteger[,] points = new BigInteger[quantity, 2];
            double[,] time_average = new double[num, wNum];

            double time = 0;
            double tableTime = 0;
            points = EllipticCC.ReadFromFile(quantity, out a, out p);

            int p_bits = Functions.ToBin(p).Length;

            openFileDialog1.Filter = "txt файли(*.txt)|*.csv";
            openFileDialog1.FileName = fname + p_bits + "_" + Start.ToString() + "_" + End.ToString() + "_" + Step.ToString() + "_" + DateTime.Now.ToString().Replace('/', '-').Replace(' ', '-').Replace('.', '-').Replace(':', '-') + ".csv";

            string filename = openFileDialog1.FileName;
            FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            BigInteger stepK = 0, max_k = 0, k;
            List<BigInteger> mass_k = new List<BigInteger>();


            if (radioButton17.Checked)
            {
                step = BigInteger.Parse(textBox13.Text);
                max_k = BigInteger.Parse(textBox12.Text);
                k = BigInteger.Parse(textBox11.Text);

                while (k <= max_k)
                {
                    mass_k.Add(k);
                    k += step;

                }
            }

            if (radioButton18.Checked)
            {
                max_k = BigInteger.Parse(textBox19.Text);
                step = 1;
                int left = int.Parse(textBox21.Text);
                int right = int.Parse(textBox20.Text);
                for (int i = 0; i < max_k; i++)
                {
                    mass_k.Add(Functions.rand(left, right));
                }
            }


            if (radioButton19.Checked)
            {
                max_k = BigInteger.Parse(textBox22.Text);
                step = 1;
                int left = int.Parse(textBox24.Text);
                int right = int.Parse(textBox23.Text);
                for (int i = 0; i < max_k; i++)
                {
                    int rand = Functions.rand(left, right);
                    mass_k.Add(Functions.random_max(rand));
                }
            }
            Stopwatch stopWatch = new Stopwatch();
            int j; int numberOfK = 0;
            foreach (var k1 in mass_k)
            {               
                j = 0;
                textBox42.Text = null;
                textBox42.AppendText(numberOfK.ToString());
                for (BigInteger w = Start; w < End; w += Step)
                {
                    textBox52.Text = null;                    
                    textBox52.AppendText(w.ToString());
                    time = 0;
                    tableTime = 0;
                    for (int i = 0; i < quantity; i++)
                    {
                        TimeSpan ts = new TimeSpan();

                        stopWatch.Start();
                        /*
                        time = 0;
                        Point_Multiplication_Affine_Coord_3(points[i, 0], points[i, 1], 1, a, k1, (int)w, p, out x2, out y2, out z2, out time, type);
                        time_average[0, j] += time;
                        stopWatch.Reset();
                        */
                        time = 0;
                        tableTime = 0;
                        Point_Multiplication_Affine_Coord_4(points[i, 0], points[i, 1], 1, a, k1, (int)w, p, out x2, out y2, out z2, out time, out tableTime, type);
                        if (checkBox11.Checked) time_average[0, j] += tableTime;
                        else time_average[0, j] += time;
                        stopWatch.Reset();
                        /*
                        time = 0;
                        Point_Multiplication_Affine_Coord_5(points[i, 0], points[i, 1], 1, a, k1, (int)w, p, out x2, out y2, out z2, out time, type);
                        time_average[2, j] += time;
                        stopWatch.Reset();
                        */
                        time = 0;
                        tableTime = 0;
                        Point_Multiplication_Affine_Coord_6(points[i, 0], points[i, 1], 1, a, k1, (int)w, p, out x2, out y2, out z2, out time, out tableTime, type);
                        if (checkBox11.Checked) time_average[0, j] += tableTime;
                        else time_average[1, j] += time;
                        stopWatch.Reset();
                        /*
                        time = 0;
                        Point_Multiplication_Affine_Coord_9(points[i, 0], points[i, 1], 1, a, k1, (int)w, p, out x2, out y2, out z2, out time, type);
                        time_average[4, j] += time;
                        stopWatch.Reset();
                        */
                        time = 0;
                        tableTime = 0;
                        Point_Multiplication_Affine_Coord_10(points[i, 0], points[i, 1], 1, a, k1, (int)w, p, out x2, out y2, out z2, out time, out tableTime, type);
                        if (checkBox11.Checked) time_average[0, j] += tableTime;
                        else time_average[2, j] += time;
                        stopWatch.Reset();
                        /*
                        time = 0;
                        Point_Multiplication_Affine_Coord_11_1(points[i, 0], points[i, 1], 1, a, k1, (int)w, p, out x2, out y2, out z2, out time, type);
                        time_average[6, j] += time;
                        stopWatch.Reset();

                        time = 0;
                        Point_Multiplication_Affine_Coord_11_2(points[i, 0], points[i, 1], 1, a, k1, (int)w, p, out x2, out y2, out z2, out time, type);
                        time_average[7, j] += time;
                        stopWatch.Reset();
                        */
                        time = 0;
                        tableTime = 0;
                        Point_Multiplication_Affine_Coord_12(points[i, 0], points[i, 1], 1, a, k1, (int)w, p, out x2, out y2, out z2, out time, out tableTime, type);
                        if (checkBox11.Checked) time_average[0, j] += tableTime;
                        else time_average[3, j] += time;
                        stopWatch.Reset();
                        /*
                        time = 0;
                        Point_Multiplication_Affine_Coord_13(points[i, 0], points[i, 1], 1, a, k1, (int)w, p, out x2, out y2, out z2, out time, type);
                        time_average[9, j] += time;
                        stopWatch.Reset();

                        time = 0;
                        Point_Multiplication_Affine_Coord_14(points[i, 0], points[i, 1], 1, a, k1, (int)w, p, out x2, out y2, out z2, out time, type);
                        time_average[10, j] += time;
                        stopWatch.Reset();*/
                    }
                    j++;
                }
                numberOfK++;
            }

            for (int i = 0; i < num; i++)
            {
                for (j = 0; j < wNum; j++)
                {
                    time_average[i, j] /= mass_k.Count;
                }
            }
            string st = " ;";
            for (BigInteger i = Start; i < End; i += Step)
            {
                st += i + ";";
            }
            sw.WriteLine(st);
            for (int i = 0; i < num; i++)
            {
                sw.Write(name[i] + ";");
                for (j = 0; j < wNum; j++)
                {
                    sw.Write(time_average[i, j] / quantity + ";");
                }
                sw.WriteLine();
            }
            sw.Close();
            sw = new StreamWriter("Best" + openFileDialog1.FileName);
            for (int i = 0; i < num; i++)
            {
                BigInteger bestW = Start;
                double bestAvg = time_average[i, 0] / quantity;
                sw.Write(name[i] + ";");
                for (j = 1; j < wNum; j++)
                {
                    if (bestAvg > time_average[i, j] / quantity)
                    {
                        bestAvg = time_average[i, j] / quantity;
                        bestW = Start + j * Step;
                    }
                }
                sw.WriteLine(bestW + ";" + bestAvg + ";");
            }
            MessageBox.Show("Записано успішно!", "УСПІШНО!", MessageBoxButtons.OK, MessageBoxIcon.None);
            sw.Close();
        }

        private void TBBEnd_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            int type = 0;
            if (radioButton30.Checked)
                type = 0;
            else if (radioButton31.Checked)
                type = 1;
            else if (radioButton32.Checked)
                type = 2;
            BigInteger p, x2, y2, z2, a;
            int quantity = int.Parse(textBox14.Text), num;
            BigInteger step = 0;
            string[] name;
            string fname;
            num = 4;
            name = new string[4];
            name[0] = "19_1";
            name[1] = "19_2";
            name[2] = "20_1";
            name[3] = "20_2";
            fname = "a_max_b_max";
            BigInteger BStart = BigInteger.Parse(TBBStart.Text);
            BigInteger BEnd = BigInteger.Parse(TBBEnd.Text);
            BigInteger BStep = BigInteger.Parse(TBBStep.Text);
            BigInteger AStart = BigInteger.Parse(TBAStart.Text);
            Int32 W = Int32.Parse(textBox5.Text);
            BigInteger AEnd = BigInteger.Parse(TBAEnd.Text);
            BigInteger AStep = BigInteger.Parse(TBAStep.Text);
            int aNum = (int)Math.Ceiling(((double)(AEnd - AStart)) / (double)AStep);
            int bNum = (int)Math.Ceiling(((double)(BEnd - BStart)) / (double)BStep);
            BigInteger[,] points = new BigInteger[quantity, 2];
            double[, ,] time_average = new double[num, aNum, bNum];

            double time = 0;
            points = EllipticCC.ReadFromFile(quantity, out a, out p);

            int p_bits = Functions.ToBin(p).Length;

            openFileDialog1.Filter = "txt файли(*.txt)|*.csv";
            openFileDialog1.FileName = fname + p_bits + "_" + AStart.ToString() + "_" + AEnd.ToString() + "_" + AStep.ToString() +"_"+ BStart.ToString() + "_" + BEnd.ToString() + "_" + BStep.ToString() +
                "_" + DateTime.Now.ToString().Replace(':', '-').Replace(' ', '-').Replace('.', '-') + ".csv";

            BigInteger stepK = 0, max_k = 0, k;
            List<BigInteger> mass_k = new List<BigInteger>();


            if (radioButton17.Checked)
            {
                step = BigInteger.Parse(textBox13.Text);
                max_k = BigInteger.Parse(textBox12.Text);
                k = BigInteger.Parse(textBox11.Text);

                while (k <= max_k)
                {
                    mass_k.Add(k);
                    k += step;

                }
            }

            if (radioButton18.Checked)
            {
                max_k = BigInteger.Parse(textBox19.Text);
                step = 1;
                int left = int.Parse(textBox21.Text);
                int right = int.Parse(textBox20.Text);
                for (int i = 0; i < max_k; i++)
                {
                    mass_k.Add(Functions.rand(left, right));
                }
            }


            if (radioButton19.Checked)
            {
                max_k = BigInteger.Parse(textBox22.Text);
                step = 1;
                int left = int.Parse(textBox24.Text);
                int right = int.Parse(textBox23.Text);
                for (int i = 0; i < max_k; i++)
                {
                    int rand = Functions.rand(left, right);
                    mass_k.Add(Functions.random_max(rand));
                }
            }
            Stopwatch stopWatch = new Stopwatch();
            int a1 = 0, b1 = 0;
            foreach (var k1 in mass_k)
            {
                a1 = 0;
                for (BigInteger a_max = AStart; a_max < AEnd; a_max += AStep)
                {
                    b1 = 0;
                    for (BigInteger b_max = BStart; b_max < BEnd; b_max += BStep)
                    {
                        for (int i = 0; i < quantity; i++)
                        {
                            time = 0;
                            PointMultiplication.Point_Multiplication_Affine_Coord_19_1(points[i, 0], points[i, 1], 1, a, k1, p,
                            out x2, out y2, out z2, out time, type, a_max, b_max);
                            time_average[0, a1, b1] += time;
                            time = 0;
                            PointMultiplication.Point_Multiplication_Affine_Coord_19_2(points[i, 0], points[i, 1], 1, a, k1, p,
                            out x2, out y2, out z2, out time, type, a_max, b_max);
                            time_average[1, a1, b1] += time;
                            time = 0;
                            PointMultiplication.Point_Multiplication_Affine_Coord_20_1(points[i, 0], points[i, 1], 1, a, k1, p,
                            out x2, out y2, out z2, out time, type, a_max, b_max);
                            time_average[2, a1, b1] += time;
                            time = 0;
                            PointMultiplication.Point_Multiplication_Affine_Coord_20_2(points[i, 0], points[i, 1], 1, a, k1, p,
                            out x2, out y2, out z2, out time, type, a_max, b_max);
                            time_average[3, a1, b1] += time;
                        }
                        b1++;
                    }
                    a1++;
                }
            }

            for (int i = 0; i < num; i++)
            {
                for (a1 = 0; a1 < aNum; a1++)
                {
                    for (b1 = 0; b1 < bNum; b1++)
                    {
                        time_average[i, a1, b1] /= mass_k.Count;
                        time_average[i, a1, b1] /= quantity;
                    }
                }
            }
            int[,] minPair = new int[num, 2];
            double min = 0;
            for (int i = 0; i < num; i++)
            {
                minPair[i, 0] = 0;
                minPair[i, 1] = 0;
                min = time_average[i, 0, 0];
                string filename = name[i] + openFileDialog1.FileName;
                FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                string st = " ;";
                for (BigInteger j = BStart; j < BEnd; j += BStep)
                {
                    st += j + ";";
                }
                sw.WriteLine(st);
                for (a1 = 0; a1 < aNum; a1++)
                {
                    sw.Write(AStart + a1 * AStep + ";");
                    for (b1 = 0; b1 < bNum; b1++)
                    {
                        sw.Write(time_average[i, a1, b1] + ";");
                        if (time_average[i, a1, b1] < min)
                        {
                            min = time_average[i, a1, b1];
                            minPair[i, 0] = a1;
                            minPair[i, 1] = b1;
                        }
                    }
                    sw.WriteLine();
                }
                sw.Close();
            }
            StreamWriter swBest = new StreamWriter("Best" + openFileDialog1.FileName);
            swBest.WriteLine("Name;a_max;b_max;Time (ms)");
            for (int i = 0; i < num; i++)
            {
                swBest.Write(name[i] + ";");
                swBest.Write(AStart + AStep * minPair[i, 0] + ";");
                swBest.Write(BStart + BStep * minPair[i, 1] + ";");
                swBest.WriteLine(time_average[i, minPair[i, 0], minPair[i, 1]] + ";");
            }
            swBest.Close();
            MessageBox.Show("Записано успішно!", "УСПІШНО!", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //CP
            int type = 0;
            if (radioButton30.Checked)
                type = 0;
            else if (radioButton31.Checked)
                type = 1;
            else if (radioButton32.Checked)
                type = 2;
            BigInteger p, x2, y2, z2, a;
            int quantity = int.Parse(textBox14.Text), num;
            BigInteger step = 0;
            string fname;
            string[] name;
        
            //end CP
            int startB = (int)Int64.Parse(textBox31.Text);
            int endB = (int)Int64.Parse(textBox32.Text);
            int lenghtB = (int)Int64.Parse(textBox33.Text);
            int startS = (int)Int64.Parse(textBox34.Text);
            int endS = (int)Int64.Parse(textBox35.Text);
            int lenghtS = (int)Int64.Parse(textBox36.Text);
            int startM = (int)Int64.Parse(textBox37.Text);
            int endM = (int)Int64.Parse(textBox38.Text);
            int lenghtM = (int)Int64.Parse(textBox39.Text);

            BigInteger B;
            BigInteger[,] arrayS = Functions.ArrayOfArray(startS, endS, lenghtS);
            BigInteger[,] arrayM = Functions.ArrayOfArray1(startM, endM, lenghtM);
            if (checkBox10.Checked == false)
            {
                arrayM = Functions.ArrayOfArray(startM, endM, lenghtM);
            }

            int numS = Functions.Rows(startS,endS,lenghtS);
            int numM = Functions.Rows(startM,endM,lenghtM);

            num = 2;
            name = new string[2];
            name[0] = "27";
            name[1] = "28";
            fname = "B_S_M";
            int numB = endB - startB + 1;


            //copy-paste

            BigInteger[,] points = new BigInteger[quantity, 2];
            double[, , ,] time_average = new double[num, numB, numS, numM];

            double time = 0;
            points = EllipticCC.ReadFromFile(quantity, out a, out p);

            int p_bits = Functions.ToBin(p).Length;

            openFileDialog1.Filter = "txt файли(*.txt)|*.csv";
            openFileDialog1.FileName = fname + p_bits + "_" + startB.ToString() + "_" + endB.ToString() + "_" + lenghtB.ToString() + startS.ToString() + "_" + endS.ToString() + "_" + lenghtS.ToString() +
                    "_" + DateTime.Now.ToString().Replace(':', '-').Replace(' ', '-').Replace('.', '-') + ".csv";


            BigInteger stepK = 0, max_k = 0, k;
            List<BigInteger> mass_k = new List<BigInteger>();

            if (radioButton17.Checked)
            {
                step = BigInteger.Parse(textBox13.Text);
                max_k = BigInteger.Parse(textBox12.Text);
                k = BigInteger.Parse(textBox11.Text);

                while (k <= max_k)
                {
                    mass_k.Add(k);
                    k += step;

                }
            }

            if (radioButton18.Checked)
            {
                max_k = BigInteger.Parse(textBox19.Text);
                step = 1;
                int left = int.Parse(textBox21.Text);
                int right = int.Parse(textBox20.Text);
                for (int i = 0; i < max_k; i++)
                {
                    mass_k.Add(Functions.rand(left, right));
                }
            }


            if (radioButton19.Checked)
            {
                max_k = BigInteger.Parse(textBox22.Text);
                step = 1;
                int left = int.Parse(textBox24.Text);
                int right = int.Parse(textBox23.Text);
                for (int i = 0; i < max_k; i++)
                {
                    int rand = Functions.rand(left, right);
                    mass_k.Add(Functions.random_max(rand));
                }
            }

            //end of copy-paste



            Stopwatch stopWatch = new Stopwatch();
            int bN, sN, mN;
            BigInteger[] S = new BigInteger[lenghtS];
            BigInteger[] M = new BigInteger[lenghtM];

            int numberK = 0;
            

            foreach (var k1 in mass_k)
            {
                int numberM = 0;
                textBox41.Text = null;
                numberK++;
                textBox41.AppendText(numberK.ToString());
                
                /*Nothing checked*/
                if (checkBox4.Checked == false && checkBox5.Checked == false && checkBox9.Checked == false)
                {
                    B = BigInteger.Parse(textBox26.Text);
                    S = writeToArray(textBox29);
                    M = writeToArray(textBox30);
                    arrayS = new BigInteger[1, S.Length];
                    arrayM = new BigInteger[1, M.Length];

                    bN = 0; mN = 0; sN = 0;
                    Flag = true;
                    for (int i = 0; i < quantity; i++)
                    {
                        time = 0;
                        PointMultiplication.Point_Multiplication_Affine_Coord_27(points[i, 0], points[i, 1], 1, a, k1, p,
                        out x2, out y2, out z2, B, S, M, type, out time);
                        time_average[0, bN, sN, mN] += time;
                        time = 0;
                        PointMultiplication.Point_Multiplication_Affine_Coord_28(points[i, 0], points[i, 1], 1, a, k1, p,
                        out x2, out y2, out z2, B, S, M, type, out time);
                        time_average[1, bN, sN, mN] += time;
                        time = 0;
                    }

                    numB = 1;
                    numS = 1;
                    numM = 1;
                    startB = Int32.Parse(textBox26.Text);
                    endB = startB;
                    lenghtB = 1;
                    for (int f = 0; f < S.Length; f++)
                    {
                        arrayS[0, f] = S[f];
                    }
                    for (int t = 0; t < M.Length; t++)
                    {
                        arrayM[0, t] = M[t];
                    }

                    lenghtM = M.Length;
                    lenghtS = S.Length;

                }

                /*only B checked*/
                if (checkBox4.Checked && !checkBox5.Checked && !checkBox9.Checked)
                {     
         
                    S = writeToArray(textBox29);
                    M = writeToArray(textBox30);
                    arrayS = new BigInteger[1,S.Length];
                    arrayM = new BigInteger[1,M.Length];

                    bN = 0; mN = 0; sN = 0;
                    for (int ii = startB; ii <= endB; ii++)
                    {
                        Flag = true;
                        for (int i = 0; i < quantity; i++)
                        {
                            time = 0;
                            PointMultiplication.Point_Multiplication_Affine_Coord_27(points[i, 0], points[i, 1], 1, a, k1, p,
                            out x2, out y2, out z2, ii, S, M, type, out time);
                            time_average[0, bN, sN, mN] += time;
                            time = 0;
                            PointMultiplication.Point_Multiplication_Affine_Coord_28(points[i, 0], points[i, 1], 1, a, k1, p,
                            out x2, out y2, out z2, ii, S, M, type, out time);
                            time_average[1, bN, sN, mN] += time;
                            time = 0;
                            Flag = false;
                        }
                        bN++;
                    }

                    numS = 1;
                    numM = 1;
                    for (int f = 0; f < S.Length; f++)
                    {
                        arrayS[0, f] = S[f];
                    }
                    for (int t = 0; t < M.Length; t++)
                    {
                        arrayM[0, t] = M[t];
                    }

                }
                else
                {
                    /*only S checked*/
                    if (!checkBox4.Checked && checkBox5.Checked && !checkBox9.Checked)
                    {
                          
                        B = BigInteger.Parse(textBox26.Text);
                        M = writeToArray(textBox30);
                        bN = 0; mN = 0;

                        sN = 0;
                        for (int j = 0; j < numS; j++)
                        {
                            for (int jj = 0; jj < lenghtS; jj++)
                            {
                                S[jj] = arrayS[j, jj];
                            }
                            Flag = true;
                            for (int i = 0; i < quantity; i++)
                            {
                                time = 0;
                                PointMultiplication.Point_Multiplication_Affine_Coord_27(points[i, 0], points[i, 1], 1, a, k1, p,
                                out x2, out y2, out z2, B, S, M, type, out time);
                                time_average[0, bN, sN, mN] += time;
                                time = 0;
                                PointMultiplication.Point_Multiplication_Affine_Coord_28(points[i, 0], points[i, 1], 1, a, k1, p,
                                out x2, out y2, out z2, B, S, M, type, out time);
                                time_average[1, bN, sN, mN] += time;
                                time = 0;
                                Flag = false;
                            }
                            sN++;
                        }

                        numB = 1;
                        numM = 1;
                        for (int t = 0; t < M.Length; t++)
                        {
                            arrayM[0, t] = M[t];
                        }

                    }                

                else{
                /*only M checked*/
                if (!checkBox4.Checked && !checkBox5.Checked && checkBox9.Checked)
                {

                    bN = 0; sN = 0;
                    B = BigInteger.Parse(textBox26.Text);
                    S = writeToArray(textBox29);
                   

                        mN = 0;
                        for (int l = 0; l < numM; l++)
                        {
                            textBox40.Text = null;
                            numberM++;
                            textBox40.AppendText(numberM.ToString());

                            for (int ll = 0; ll < lenghtM; ll++)
                            {
                                M[ll] = arrayM[l, ll];
                            }
                            Flag = true;
                            for (int i = 0; i < quantity; i++)
                            {
                                time = 0;
                                PointMultiplication.Point_Multiplication_Affine_Coord_27(points[i, 0], points[i, 1], 1, a, k1, p,
                                out x2, out y2, out z2, B, S, M, type, out time);
                                time_average[0, bN, sN, mN] += time;
                                time = 0;
                                PointMultiplication.Point_Multiplication_Affine_Coord_28(points[i, 0], points[i, 1], 1, a, k1, p,
                                out x2, out y2, out z2, B, S, M, type, out time);
                                time_average[1, bN, sN, mN] += time;
                                time = 0;
                                Flag = false;
                            }
                            mN++;
                        }
                        numB = 1;
                        numS = 1;
                        for (int f = 0; f < S.Length; f++)
                        {
                            arrayS[0, f] = S[f];
                        }

                }
                
                    else{
                /*B and S checked*/
                        if (checkBox4.Checked && checkBox5.Checked && !checkBox9.Checked)
                        {                          
                            M = writeToArray(textBox30);

                            bN = 0; mN = 0;
                            for (int ii = startB; ii <= endB; ii++)
                            {
                                sN = 0;
                                for (int j = 0; j < numS; j++)
                                {
                                    for (int jj = 0; jj < lenghtS; jj++)
                                    {
                                        S[jj] = arrayS[j, jj];
                                    }
                                    Flag = true;
                                    for (int i = 0; i < quantity; i++)
                                    {
                                        time = 0;
                                        PointMultiplication.Point_Multiplication_Affine_Coord_27(points[i, 0], points[i, 1], 1, a, k1, p,
                                        out x2, out y2, out z2, ii, S, M, type, out time);
                                        time_average[0, bN, sN, mN] += time;
                                        time = 0;
                                        PointMultiplication.Point_Multiplication_Affine_Coord_28(points[i, 0], points[i, 1], 1, a, k1, p,
                                        out x2, out y2, out z2, ii, S, M, type, out time);
                                        time_average[1, bN, sN, mN] += time;
                                        time = 0;
                                        Flag = false;
                                    }
                                    sN++;
                                }
                                bN++;
                            }

                            numM = 1;
                            for (int t = 0; t < M.Length; t++)
                            {
                                arrayM[0, t] = M[t];
                            }
                        }

                        else
                        {
                            /*B and M checked*/
                            if (checkBox4.Checked && checkBox5.Checked == false && checkBox9.Checked)
                            {
                                S = writeToArray(textBox29);

                                bN = 0; sN = 0;
                                for (int ii = startB; ii <= endB; ii++)
                                {
                                    mN = 0;
                                    for (int l = 0; l < numM; l++)
                                    {
                                        for (int ll = 0; ll < lenghtM; ll++)
                                        {
                                            M[ll] = arrayM[l, ll];
                                        }
                                        Flag = true;
                                        for (int i = 0; i < quantity; i++)
                                        {
                                            time = 0;
                                            PointMultiplication.Point_Multiplication_Affine_Coord_27(points[i, 0], points[i, 1], 1, a, k1, p,
                                            out x2, out y2, out z2, ii, S, M, type, out time);
                                            time_average[0, bN, sN, mN] += time;
                                            time = 0;
                                            PointMultiplication.Point_Multiplication_Affine_Coord_28(points[i, 0], points[i, 1], 1, a, k1, p,
                                            out x2, out y2, out z2, ii, S, M, type, out time);
                                            time_average[1, bN, sN, mN] += time;
                                            time = 0;
                                            Flag = false;
                                        }
                                        mN++;
                                    }
                                    bN++;
                                }
                                numS = 1;
                                for (int f = 0; f < S.Length; f++)
                                {
                                    arrayS[0, f] = S[f];
                                }
                            }
                            else
                            {
                                /*S and M checked*/
                                if (checkBox4.Checked == false && checkBox5.Checked && checkBox9.Checked)
                                {
                                    bN = 0;
                                    B = BigInteger.Parse(textBox26.Text);                                   

                                    sN = 0;
                                    for (int j = 0; j < numS; j++)
                                    {
                                        for (int jj = 0; jj < lenghtS; jj++)
                                        {
                                            S[jj] = arrayS[j, jj];
                                        }
                                        mN = 0;
                                        for (int l = 0; l < numM; l++)
                                        {
                                            for (int ll = 0; ll < lenghtM; ll++)
                                            {
                                                M[ll] = arrayM[l, ll];
                                            }
                                            Flag = true;
                                            for (int i = 0; i < quantity; i++)
                                            {
                                                time = 0;
                                                PointMultiplication.Point_Multiplication_Affine_Coord_27(points[i, 0], points[i, 1], 1, a, k1, p,
                                                out x2, out y2, out z2, B, S, M, type, out time);
                                                time_average[0, bN, sN, mN] += time;
                                                time = 0;
                                                PointMultiplication.Point_Multiplication_Affine_Coord_28(points[i, 0], points[i, 1], 1, a, k1, p,
                                                out x2, out y2, out z2, B, S, M, type, out time);
                                                time_average[1, bN, sN, mN] += time;
                                                time = 0;
                                                Flag = false;
                                            }
                                            mN++;
                                        }
                                        sN++;
                                    }

                                    numB = 1;
                                }

                                else
                                {
                                    /*All checked*/
                                    if (checkBox4.Checked && checkBox5.Checked && checkBox9.Checked)
                                    {
                                        bN = 0;
                                        for (int ii = startB; ii <= endB; ii++)
                                        {
                                            sN = 0;
                                            for (int j = 0; j < numS; j++)
                                            {
                                                for (int jj = 0; jj < lenghtS; jj++)
                                                {
                                                    S[jj] = arrayS[j, jj];
                                                }
                                                mN = 0;
                                                for (int l = 0; l < numM; l++)
                                                {
                                                    for (int ll = 0; ll < lenghtM; ll++)
                                                    {
                                                        M[ll] = arrayM[l, ll];
                                                    }
                                                    Flag = true;
                                                    for (int i = 0; i < quantity; i++)
                                                    {
                                                        time = 0;
                                                        PointMultiplication.Point_Multiplication_Affine_Coord_27(points[i, 0], points[i, 1], 1, a, k1, p,
                                                        out x2, out y2, out z2, ii, S, M, type, out time);
                                                        time_average[0, bN, sN, mN] += time;
                                                        time = 0;
                                                        PointMultiplication.Point_Multiplication_Affine_Coord_28(points[i, 0], points[i, 1], 1, a, k1, p,
                                                        out x2, out y2, out z2, ii, S, M, type, out time);
                                                        time_average[1, bN, sN, mN] += time;
                                                        time = 0;
                                                        Flag = false;
                                                    }
                                                    mN++;
                                                }
                                                sN++;
                                            }
                                            bN++;
                                        }
                                    }
                                }
                            }
                        }
                        }
                    }
                }
            }
      

            for (int i = 0; i < num; i++)
            {
                for (bN = 0; bN < numB; bN++)
                {
                    for (sN = 0; sN < numS; sN++)
                    {
                        for (mN = 0; mN < numM; mN++)
                        {
                            time_average[i, bN, sN, mN] /= mass_k.Count;
                            time_average[i, bN, sN, mN] /= quantity;
                        }
                    }
                }
            }
            int[,] minPair = new int[num, 3];
            double min = 0;
            for (int i = 0; i < num; i++)
            {
                minPair[i, 0] = 0;
                minPair[i, 1] = 0;
                minPair[i, 2] = 0;
                min = time_average[i, 0, 0,0];
                string filename = name[i] + openFileDialog1.FileName;
                FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                


                for (bN = 0; bN < numB; bN++)
                {
                    sw.WriteLine();
                    sw.Write("B = " + (startB+bN));
                    sw.WriteLine();
                    string st = " ;";
                    for (int j = 0; j < numM; j++)
                    {
                        st += "M={";
                        for (int jj = 0; jj < lenghtM; jj++)
                        {
                            st += arrayM[j, jj] + ",";
                        }
                        st += "}" + ";";
                    }
                    sw.WriteLine(st);

                    for (sN = 0; sN < numS; sN++)
                    {
                        sw.Write("S={");
                        for (int l = 0; l < lenghtS; l++)
                        {
                            sw.Write(arrayS[sN, l] + ",");

                        }
                        sw.Write("}" + ";");
                        for (mN = 0; mN < numM; mN++)
                        {
                            sw.Write(time_average[i, bN, sN, mN] + ";");
                            if (time_average[i, bN, sN, mN] < min)
                            {
                                min = time_average[i, bN, sN, mN];
                                minPair[i, 0] = bN;
                                minPair[i, 1] = sN;
                                minPair[i, 2] = mN;
                            }
                        }
                        sw.WriteLine();
                    }
                }
                sw.Close();
            }

            StreamWriter swBest = new StreamWriter("Best" + openFileDialog1.FileName);
            swBest.WriteLine("Name;B_max;S_max;M_max;Time (ms)");
      
            for (int i = 0; i < num; i++)
            {
                swBest.Write(name[i] + ";");
                swBest.Write(startB + 1*minPair[i,0] + ";");
                for (int j = 0; j < S.Length; j++)
                {
                    swBest.Write( arrayS[minPair[i,1],j] + ",");              
                }
                swBest.Write(";");
                for (int j = 0; j < M.Length; j++)
                {
                    swBest.Write(arrayM[minPair[i,2],j] + ",");                 
                }
                swBest.Write(";");
                swBest.WriteLine(time_average[i, minPair[i, 0], minPair[i, 1], minPair[i,2]] + ";");
            }
            swBest.Close();
            MessageBox.Show("Записано успішно!", "УСПІШНО!", MessageBoxButtons.OK, MessageBoxIcon.None);
        }
        private void testingButton_Click(object sender, EventArgs e)
        {
            BigInteger a = BigInteger.Parse(textBox2.Text);
            BigInteger b = BigInteger.Parse(textBox3.Text);
            BigInteger p = BigInteger.Parse(textBox48.Text);
            int quantity = int.Parse(textBox46.Text);
            List<BigInteger[]> pointsList = new List<BigInteger[]>();
            EllipticCC.Generate_Point_EC_(a,b,p, quantity, out pointsList);
            BigInteger[] coord = new BigInteger[3];
            BigInteger kFrom = BigInteger.Parse(textBox51.Text);
            BigInteger kTo = BigInteger.Parse(textBox50.Text);
            BigInteger kStep = BigInteger.Parse(textBox49.Text);
            BigInteger x1, y1, z1, x2 = 0, y2 = 1, z2 = 0, x3 = 0, y3 = 1, z3 = 0;
            bool flag = false;
            int count = 0;
            double time;
            for (BigInteger k = kFrom; k < kTo; k += kStep)
            {            
                    foreach (var l in pointsList)
                    {
                        coord = l;
                        Point_Multiplication_Affine_Coord_32(coord[0], coord[1], coord[2], a, k, p, out x1, out y1, out z1, out time, 0);
                        if (radioButton47.Checked)
                        {
                            Point_Multiplication_Affine_Coord_32(coord[0], coord[1], coord[2], a, k, p, out x2, out y2, out z2, out time, 1);
                            x3 = x2; y3 = y2; z3 = z2;
                            ProjectiveToAffine(x2, y2, z2, p, out x2, out y2, out z2);
                        }
                        if (radioButton46.Checked)
                        {
                            Point_Multiplication_Affine_Coord_32(coord[0], coord[1], coord[2], a, k, p, out x2, out y2, out z2, out time, 2);
                            x3 = x2; y3 = y2; z3 = z2;
                            JacobyToAffine(x2,y2,z2,p, out x2, out y2, out z2);
                        }
                        if (radioButton51.Checked)
                        {
                            Point_Multiplication_Affine_Coord_32(coord[0], coord[1], coord[2], a, k, p, out x2, out y2, out z2, out time, 4);
                            x3 = x2; y3 = y2; z3 = z2;
                            JacobyToAffine(x2, y2, z2, p, out x2, out y2, out z2);
                        }
                        if (radioButton52.Checked)
                        {
                            Point_Multiplication_Affine_Coord_32(coord[0], coord[1], coord[2], a, k, p, out x2, out y2, out z2, out time, 3);
                            x3 = x2; y3 = y2; z3 = z2;
                            JacobyToAffine(x2, y2, z2, p, out x2, out y2, out z2);
                        }
                        if ((x1 != x2) || (y1 != y2) || (z1 != z2))
                        {
                            flag = true;
                            dataGridView8.Rows.Add();
                            dataGridView8.Rows[count].Cells[0].Value = coord[0];
                            dataGridView8.Rows[count].Cells[1].Value = coord[1];
                            dataGridView8.Rows[count].Cells[2].Value = coord[2];
                            count++;
                        }
                    }              
            }
            if (flag) MessageBox.Show("Множення виконується не правильно", "ПОМИЛКА");
            else MessageBox.Show("Множення виконується правильно", "ПРАВИЛЬНО");
        }
        private void radioButton33_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void radioButton34_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void textBox46_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView4.Rows.Clear();
            BigInteger a, b, p, x1, y1, z1, x2 = 0, y2 = 0, z2 = 0, k, a_max, b_max;
            int w;
            a = BigInteger.Parse(richTextBox4.Text);
            b = -3;
            p = BigInteger.Parse(richTextBox5.Text);
            a_max = BigInteger.Parse(textBox27.Text);
            b_max = BigInteger.Parse(textBox28.Text);
            k = BigInteger.Parse(textBox4.Text);
            w = int.Parse(textBox5.Text);
            BigInteger[] S;
            BigInteger[] M;
            BigInteger B;
            B = BigInteger.Parse(textBox26.Text);
            S = writeToArray(textBox29);
            M = writeToArray(textBox30);

            string[] numOfAlg = new string[] { "1", "1", "2", "2", "3", "3", "4", "4", "5", "5", "6", "6","7_1","7_1","7_2","7_2","8","8","9","9","10","10",
                "11_1","11_1","11_2","11_2","12","12","17","17","18","18","19_1","19_1","19_2","19_2","20_1","20_1","20_2","20_2","21","21","22","22", "27", "27", "28", "28", "29", "29", "30", "30", "31", "31", "32", "32" };

            double time = 0;
            double tableTime = 0;
            x1 = BigInteger.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
            y1 = BigInteger.Parse(dataGridView1.CurrentRow.Cells[1].Value.ToString());
            z1 = BigInteger.Parse(dataGridView1.CurrentRow.Cells[2].Value.ToString());

            BigInteger x3, y3, z3;
            int type = 0;
            if (radioButton49.Checked)
            {
                type = 4;
            }
            else
            {
                if (radioButton50.Checked)
                    type = 3;
                else
                    MessageBox.Show("Choose coordinate system", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (x1 == 0 || y1 == 0)
                MessageBox.Show("Выбирете точку!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                int i = 0;
                dataGridView4.RowCount = 1;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_2(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_3(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_4(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, out tableTime, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;               
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_5(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_6(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, out tableTime, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_7_1(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_7_2(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_8(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_9(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_10(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, out tableTime, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_11_1(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_11_2(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_12(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, out tableTime, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();                
                i++;
                /*
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_13(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_14(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_15(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_16(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;*/

                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_17(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;

                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_18(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_19(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_19_2(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_20_1(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_20(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_21(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_22(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_27(x1, y1, z1, a, k, p, out x2, out y2, out z2, B, S, M, type, out time);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_28(x1, y1, z1, a, k, p, out x2, out y2, out z2, B, S, M, type, out time);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_29(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_30(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_31(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_32(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                JacobyToAffine(x2, y2, z2, p, out x3, out y3, out z3);
                dataGridView4.Rows[i].Cells[1].Value = x3.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y3.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z3.ToString();
                for (int s = 0; s < numOfAlg.Length; s++)
                {
                    dataGridView4.Rows[s].Cells[0].Value = numOfAlg[s];
                }
            }
        }

        private void textBox52_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // Test button
        private void button7_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            String selectedCurve = ((ComboBox)sender).Text;
            Console.WriteLine("Selected curve" + selectedCurve);
            Console.WriteLine("This is params" + curveValues[selectedCurve]);
            BigInteger p = curveValues[selectedCurve]["p"];
            BigInteger a = curveValues[selectedCurve]["a"];
            BigInteger b = curveValues[selectedCurve]["b"];
            BigInteger N = curveValues[selectedCurve]["N"];

            textBox1.Text = p.ToString();
            textBox2.Text = a.ToString();
            textBox3.Text = b.ToString();
            textBox25.Text = N.ToString();
        }
    }
}

     