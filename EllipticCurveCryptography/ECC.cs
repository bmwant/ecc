using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Numerics;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;
using Excel = Microsoft.Office.Interop.Excel;
using ECC.EllipticCurveCryptography;


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
            },
            {
                "secp192k1", new Dictionary<String, BigInteger> {
                    {"p", BigInteger.Parse("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFEE37", NumberStyles.AllowHexSpecifier)},
                    {"a", BigInteger.Parse("0", NumberStyles.AllowHexSpecifier)},
                    {"b", BigInteger.Parse("3", NumberStyles.AllowHexSpecifier)},
                    {"N", BigInteger.Parse("FFFFFFFFFFFFFFFFFFFFFFFE26F2FC170F69466A74DEFD8D", NumberStyles.AllowHexSpecifier)},
                }
            },
            {
                "secp192r1", new Dictionary<String, BigInteger> {
                    {"p", BigInteger.Parse("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFFFFFFFFFFFF", NumberStyles.AllowHexSpecifier)},
                    {"a", BigInteger.Parse("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFFFFFFFFFFFC", NumberStyles.AllowHexSpecifier)},
                    {"b", BigInteger.Parse("64210519E59C80E70FA7E9AB72243049FEB8DEECC146B9B1", NumberStyles.AllowHexSpecifier)},
                    {"N", BigInteger.Parse("FFFFFFFFFFFFFFFFFFFFFFFF99DEF836146BC9B1B4D22831", NumberStyles.AllowHexSpecifier)},
                }
            },
            {
                "secp224k1", new Dictionary<String, BigInteger> {
                    {"p", BigInteger.Parse("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFE56D", NumberStyles.AllowHexSpecifier)},
                    {"a", BigInteger.Parse("0", NumberStyles.AllowHexSpecifier)},
                    {"b", BigInteger.Parse("5", NumberStyles.AllowHexSpecifier)},
                    {"N", BigInteger.Parse("010000000000000000000000000001DCE8D2EC6184CAF0A971769FB1F7", NumberStyles.AllowHexSpecifier)},
                }
            },
            {
                "secp224r1", new Dictionary<String, BigInteger> {
                    {"p", BigInteger.Parse("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF000000000000000000000001", NumberStyles.AllowHexSpecifier)},
                    {"a", BigInteger.Parse("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFE", NumberStyles.AllowHexSpecifier)},
                    {"b", BigInteger.Parse("B4050A850C04B3ABF54132565044B0B7D7BFD8BA270B39432355FFB4", NumberStyles.AllowHexSpecifier)},
                    {"N", BigInteger.Parse("FFFFFFFFFFFFFFFFFFFFFFFFFFFF16A2E0B8F03E13DD29455C5C2A3D", NumberStyles.AllowHexSpecifier)},
                }
            },
            {
                "secp256k1", new Dictionary<String, BigInteger> {
                    {"p", BigInteger.Parse("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFC2F", NumberStyles.AllowHexSpecifier)},
                    {"a", BigInteger.Parse("0", NumberStyles.AllowHexSpecifier)},
                    {"b", BigInteger.Parse("7", NumberStyles.AllowHexSpecifier)},
                    {"N", BigInteger.Parse("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141", NumberStyles.AllowHexSpecifier)},
                }
            },
            {
                "secp256r1", new Dictionary<String, BigInteger> {
                    {"p", BigInteger.Parse("FFFFFFFF00000001000000000000000000000000FFFFFFFFFFFFFFFFFFFFFFFF", NumberStyles.AllowHexSpecifier)},
                    {"a", BigInteger.Parse("FFFFFFFF00000001000000000000000000000000FFFFFFFFFFFFFFFFFFFFFFFC", NumberStyles.AllowHexSpecifier)},
                    {"b", BigInteger.Parse("5AC635D8AA3A93E7B3EBBD55769886BC651D06B0CC53B0F63BCE3C3E27D2604B", NumberStyles.AllowHexSpecifier)},
                    {"N", BigInteger.Parse("FFFFFFFF00000000FFFFFFFFFFFFFFFFBCE6FAADA7179E84F3B9CAC2FC632551", NumberStyles.AllowHexSpecifier)},
                }
            },
            {
                "secp384r1", new Dictionary<String, BigInteger> {
                    {"p", BigInteger.Parse("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFFFF0000000000000000FFFFFFFF", NumberStyles.AllowHexSpecifier)},
                    {"a", BigInteger.Parse("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFFFF0000000000000000FFFFFFFC", NumberStyles.AllowHexSpecifier)},
                    {"b", BigInteger.Parse("B3312FA7E23EE7E4988E056BE3F82D19181D9C6EFE8141120314088F5013875AC656398D8A2ED19D2A85C8EDD3EC2AEF", NumberStyles.AllowHexSpecifier)},
                    {"N", BigInteger.Parse("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC7634D81F4372DDF581A0DB248B0A77AECEC196ACCC52973", NumberStyles.AllowHexSpecifier)},
                }
            },
            {
                "secp521r1", new Dictionary<String, BigInteger> {
                    {"p", BigInteger.Parse("01FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", NumberStyles.AllowHexSpecifier)},
                    {"a", BigInteger.Parse("01FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC", NumberStyles.AllowHexSpecifier)},
                    {"b", BigInteger.Parse("0051953EB9618E1C9A1F929A21A0B68540EEA2DA725B99B315F3B8B489918EF109E156193951EC7E937B1652C0BD3BB1BF073573DF883D2C34F1EF451FD46B503F00", NumberStyles.AllowHexSpecifier)},
                    {"N", BigInteger.Parse("01FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFA51868783BF2F966B7FCC0148F709A5D03BB5C9B8899C47AEBB6FB71E91386409", NumberStyles.AllowHexSpecifier)},
                }
            },
        };

        Dictionary<string, string> curveURI = new Dictionary<string, string>()
        {
            { "Curve P-192", "http://nvlpubs.nist.gov/nistpubs/FIPS/NIST.FIPS.186-4.pdf" },
            { "Curve P-224", "http://nvlpubs.nist.gov/nistpubs/FIPS/NIST.FIPS.186-4.pdf" },
            { "Curve P-256", "http://nvlpubs.nist.gov/nistpubs/FIPS/NIST.FIPS.186-4.pdf" },
            { "Curve P-384", "http://nvlpubs.nist.gov/nistpubs/FIPS/NIST.FIPS.186-4.pdf" },
            { "Curve P-521", "http://nvlpubs.nist.gov/nistpubs/FIPS/NIST.FIPS.186-4.pdf" },

            { "secp192k1", "http://www.secg.org/sec2-v2.pdf" },
            { "secp192r1", "http://www.secg.org/sec2-v2.pdf" },
            { "secp224k1", "http://www.secg.org/sec2-v2.pdf" },
            { "secp224r1", "http://www.secg.org/sec2-v2.pdf" },
            { "secp256k1", "http://www.secg.org/sec2-v2.pdf" },
            { "secp256r1", "http://www.secg.org/sec2-v2.pdf" },
            { "secp384r1", "http://www.secg.org/sec2-v2.pdf" },
            { "secp521r1", "http://www.secg.org/sec2-v2.pdf" },
        };

        public bool Flag = true;
        IProgress<int> progress;

        List<string> cryptAlgorithms = new List<string>() {
            "ECDSA",
            "GOST_R34_10_2001",
            "KCDSA",
            "Shor",
        };

        List<string> multAlgorithms = new List<string>() {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7.1",
            "7.2",
            "8",
            "9",
            "10",
            "11.1",
            "11.2",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "19.1",
            "19.2",
            "20.1",
            "20.2",
            "21",
            "22",
            "21m",
            "22m",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32",
            "33"
        };

        public ECC()
        {
            InitializeComponent();
            progress = new Progress<int>(percent =>
            {
                toolStripProgressBar1.Value = percent;
            });
        }

        byte[] Get_Data_Bytes()
        {
            //BigInteger a, b, p, x3, y3, z3, x2 = 0, y2 = 0, z2 = 0, k, l, a_max, b_max;
            //int w;
            //BigInteger[] S;
            //BigInteger[] M;
            //BigInteger B;
            //B = BigInteger.Parse(textBox26.Text);
            //S = writeToArray(textBox29);
            //M = writeToArray(textBox30);
            //a = BigInteger.Parse(richTextBox4.Text);
            //b = -3;
            //p = BigInteger.Parse(richTextBox5.Text);
            //k = BigInteger.Parse(textBox4.Text);
            //l = BigInteger.Parse(textBox47.Text);
            //w = int.Parse(textBox5.Text);
            //a_max = BigInteger.Parse(textBox27.Text);
            //b_max = BigInteger.Parse(textBox28.Text);
            //double time = 0;
            // todo: you can implement here other types of getting data
            return Encoding.UTF8.GetBytes(textBoxCryptData.Text);
        }

        private void Run_ECDSA(MultiplyPoint multiplier, int coorType, out double time, OperationsCounter ops)
        {
            int w = int.Parse(textBox5.Text);
            var ecdsa = new ECDSA(2, 6, 17, 2, 1, 11, multiplier: multiplier, ops: ops);
            byte[] data = Get_Data_Bytes();
            var Ks = new List<BigInteger>() { 3, 4 };
            var Ds = new List<BigInteger>() { 8, 9 };
            BigInteger r, s;
            ecdsa.Sign(data, 22, out r, out s);
            time = ecdsa.time;
            Console.WriteLine("Result of ECDSA: r = " + r + ", s = " + s);
        }

        private void Run_GOST_R34_10_2001(MultiplyPoint multiplier, int coorType, out double time, OperationsCounter ops)
        {
            int w = int.Parse(textBox5.Text);
            var gost = new GOST_R34_10_2001(2, 6, 17, 2, 1, 11, 22, multiplier: multiplier, ops: ops);
            var data = Get_Data_Bytes();
            var Ks = new List<BigInteger>() { 3, 4 };
            var Ds = new List<BigInteger>() { 8, 9 };
            BigInteger r, s;
            gost.GroupSign(data, Ks, Ds, out r, out s);
            time = gost.time;
            Console.WriteLine("Result of GOST_R34_10_2001: r = " + r + ", s = " + s);
        }

        private void Run_KCDSA(MultiplyPoint multiplier, int coorType, out double time, OperationsCounter ops)
        {
            int w = int.Parse(textBox5.Text);
            var kcdsa = new KCDSA(2, 6, 17, 2, 1, 11, multiplier: multiplier, ops: ops);
            var data = Get_Data_Bytes();
            var cert = Encoding.UTF8.GetBytes("certificate");
            var Ks = new List<BigInteger>() { 3, 4 };
            BigInteger r, s;
            BigInteger d = new BigInteger(5);
            kcdsa.Sign(data, cert, d, out r, out s);
            time = kcdsa.time;
            Console.WriteLine("Result of KCDSA: r = " + r + ", s = " + s);
        }

        private void Run_Shor(MultiplyPoint multiplier, int coorType, out double time, OperationsCounter ops)
        {
            int w = int.Parse(textBox5.Text);
            var shor = new Shor(2, 6, 17, 2, 1, 11, w: w, multiplier: multiplier, ops: ops);
            var Ds = new List<BigInteger>() { 8, 5 };
            var data = Get_Data_Bytes();
            var Ks = new List<BigInteger>() { 3, 4 };
            BigInteger r, s;
            shor.GroupSign(data, Ks, Ds, out r, out s);
            time = shor.time;
            Console.WriteLine("Result of Shor: r = " + r + ", s = " + s);
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
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #12"));
            PointMultiplication.Point_Multiplication_Affine_Coord_12(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, type, out time, w, ops);
        }

        private void Point_Multiplication_Affine_Coord_15(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #15"));
            PointMultiplication.Point_Multiplication_Affine_Coord_15(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, type, out time, ops: ops);
        }

        private void Point_Multiplication_Affine_Coord_16(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #16"));
            PointMultiplication.Point_Multiplication_Affine_Coord_16(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, type, out time, ops: ops);
        }

        private void Point_Multiplication_Affine_Coord_17(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #17"));
            PointMultiplication.Point_Multiplication_Affine_Coord_17(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, type, out time, ops: ops);
        }

        private void Point_Multiplication_Affine_Coord_18(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #18"));
            PointMultiplication.Point_Multiplication_Affine_Coord_18(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, type, out time, ops: ops);
        }

        private void Point_Multiplication_Affine_Coord_19(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, BigInteger a_max, BigInteger b_max, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #19"));
            PointMultiplication.Point_Multiplication_Affine_Coord_19_1(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, out time, type, a_max, b_max, ops);
        }

        private void Point_Multiplication_Affine_Coord_20(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, BigInteger a_max, BigInteger b_max, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #20"));
            PointMultiplication.Point_Multiplication_Affine_Coord_20_2(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, out time, type, a_max, b_max, ops);
        }

        private void Point_Multiplication_Affine_Coord_19_2(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, BigInteger a_max, BigInteger b_max, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #19.2"));
            PointMultiplication.Point_Multiplication_Affine_Coord_19_2(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, out time, type, a_max, b_max, ops);
        }

        private void Point_Multiplication_Affine_Coord_20_1(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, BigInteger a_max, BigInteger b_max, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #20.1"));
            PointMultiplication.Point_Multiplication_Affine_Coord_20_1(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, out time, type, a_max, b_max, ops);
        }

        private void Point_Multiplication_Affine_Coord_21(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #21"));
            PointMultiplication.Point_Multiplication_Affine_Coord_21(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, type, out time, ops: ops);
        }

        private void Point_Multiplication_Affine_Coord_21m(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #21m"));
            PointMultiplication.Point_Multiplication_Affine_Coord_21m(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, type, out time, ops: ops);
        }
        private void Point_Multiplication_Affine_Coord_22(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #22"));
            PointMultiplication.Point_Multiplication_Affine_Coord_22(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, type, out time, type, ops: ops);
        }
        private void Point_Multiplication_Affine_Coord_22m(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p,
          out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #22m"));
            PointMultiplication.Point_Multiplication_Affine_Coord_22m(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, type, out time, ops: ops);
        }
        private void Point_Multiplication_Affine_Coord_13(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, int w, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #13"));
            PointMultiplication.Point_Multiplication_Affine_Coord_13(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, type, out time, w, ops);
        }

        private void Point_Multiplication_Affine_Coord_14(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, int w, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #14"));
            PointMultiplication.Point_Multiplication_Affine_Coord_14(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, type, out time, w, ops);
        }

        private void Point_Multiplication_Affine_Coord_11_2(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, int w, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #11.2"));
            PointMultiplication.Point_Multiplication_Affine_Coord_11_2(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, type, out time, w, ops);
        }

        private void Point_Multiplication_Affine_Coord_11_1(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, int w, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #11.1"));
            PointMultiplication.Point_Multiplication_Affine_Coord_11_1(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, type, out time, w, ops);
        }


        private void Point_Multiplication_Affine_Coord_10(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, int w, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, int type, out double time, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #10"));
            PointMultiplication.Point_Multiplication_Affine_Coord_10(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, type, out time, w, ops);
        }

        private void Point_Multiplication_Affine_Coord_9(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, int w, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #9"));
            PointMultiplication.Point_Multiplication_Affine_Coord_9(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, type, out time, w, ops);
        }

        private void Point_Multiplication_Affine_Coord_8(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, int w, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #8"));
            PointMultiplication.Point_Multiplication_Affine_Coord_8(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, type, out time, w, ops);
        }

        private void Point_Multiplication_Affine_Coord_7_2(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, int w, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #7.2"));
            PointMultiplication.Point_Multiplication_Affine_Coord_7_2(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, type, out time, w, ops);
        }

        private void Point_Multiplication_Affine_Coord_7_1(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, int w, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #7.1"));
            PointMultiplication.Point_Multiplication_Affine_Coord_7_1(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, type, out time, w, ops);
        }

        private void Point_Multiplication_Affine_Coord_6(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, int w, BigInteger p, out BigInteger x2,
            out BigInteger y2, out BigInteger z2, out double time, int type, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #6"));
            PointMultiplication.Point_Multiplication_Affine_Coord_6(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, type, out time, w, ops);
        }

        private void Point_Multiplication_Affine_Coord_5(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, int w, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #5"));
            PointMultiplication.Point_Multiplication_Affine_Coord_5(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, type, out time, w, ops);
        }

        private void Point_Multiplication_Affine_Coord_4(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, int w, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #4"));
            PointMultiplication.Point_Multiplication_Affine_Coord_4(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, type, out time, w, ops);
        }

        private void Point_Multiplication_Affine_Coord_3(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, int w, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #3"));
            PointMultiplication.Point_Multiplication_Affine_Coord_3(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, type, out time, w, ops: ops);
        }

        private void Point_Multiplication_Affine_Coord_2(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #2"));
            PointMultiplication.Point_Multiplication_Affine_Coord_2(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, type, out time, ops: ops);
        }

        private void Point_Multiplication_Affine_Coord_1(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, out double time, int type, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #1"));
            PointMultiplication.Point_Multiplication_Affine_Coord_1(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, type, out time, ops: ops);
        }

        //With trees
        private void Point_Multiplication_Affine_Coord_27(BigInteger bigIntegerX, BigInteger bigIntegerY, BigInteger bigIntegerZ, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, BigInteger B, BigInteger[] S, BigInteger[] M, int type, out double time, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #27"));
            PointMultiplication.Point_Multiplication_Affine_Coord_27(bigIntegerX, bigIntegerY, bigIntegerZ, a, k, p, out x2, out y2, out z2,
                 B, S, M, type, out time, ops);
        }

        private void Point_Multiplication_Affine_Coord_28(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger k, BigInteger p,
            out BigInteger x2, out BigInteger y2, out BigInteger z2, BigInteger B, BigInteger[] S, BigInteger[] M, int type, out double time, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #28"));
            PointMultiplication.Point_Multiplication_Affine_Coord_28(x1, y1, z1, a, k, p, out x2, out y2, out z2,
               B, S, M, type, out time, ops);
        }

        private void Point_Multiplication_Affine_Coord_29(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p, out BigInteger x2,
            out BigInteger y2, out BigInteger z2, out double time, int type, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #29"));
            PointMultiplication.Point_Multiplication_Affine_Coord_29(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, type, out time, ops: ops);
        }

        private void Point_Multiplication_Affine_Coord_30(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p, out BigInteger x2,
            out BigInteger y2, out BigInteger z2, out double time, int type, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #30"));
            PointMultiplication.Point_Multiplication_Affine_Coord_30(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, type, out time, ops: ops);
        }

        private void Point_Multiplication_Affine_Coord_31(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p, out BigInteger x2,
            out BigInteger y2, out BigInteger z2, out double time, int type, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #31"));
            PointMultiplication.Point_Multiplication_Affine_Coord_31(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, type, out time, ops: ops);
        }

        private void Point_Multiplication_Affine_Coord_32(BigInteger bigInteger, BigInteger bigInteger_2, BigInteger bigInteger_3, BigInteger a, BigInteger k, BigInteger p, out BigInteger x2,
            out BigInteger y2, out BigInteger z2, out double time, int type, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #32"));
            PointMultiplication.Point_Multiplication_Affine_Coord_32(bigInteger, bigInteger_2, bigInteger_3, a, k, p,
                out x2, out y2, out z2, type, out time, ops: ops);
        }

        private void PointMultiplication33(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger x2, BigInteger y2, BigInteger z2, BigInteger a, BigInteger k, BigInteger l, int w,
            BigInteger p, out BigInteger x3, out BigInteger y3, out BigInteger z3, out double time, int type, OperationsCounter ops)
        {
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Алгоритм #33"));
            PointMultiplication.Point_Multiplication_33(x1, y1, z1, x2, y2, z2, a, k, l, p, 
                out x3, out y3, out z3, type, out time, w, ops);
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
            if(checkedListBox3.CheckedItems.Count != 1)
            {
                MessageBox.Show("Виберіть одну систему координат!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new ArgumentException("Exactly one checkbox selected required for coord type");
            }

            return checkedListBox3.CheckedIndices[0];
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
            BigInteger x1, y1, z1;

            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Згенеруйте чи завантажте точку", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            x1 = BigInteger.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
            y1 = BigInteger.Parse(dataGridView1.CurrentRow.Cells[1].Value.ToString());
            z1 = BigInteger.Parse(dataGridView1.CurrentRow.Cells[2].Value.ToString());

            if (x1 == 0 || y1 == 0 || z1 == 0)
            {
                MessageBox.Show("Виберіть точку!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            BigInteger a, b, p, x3, y3, z3, x2 = 0, y2 = 0, z2 = 0, k, l, a_max, b_max;
            int w = int.Parse(textBox5.Text);
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
            a_max = BigInteger.Parse(textBox27.Text);
            b_max = BigInteger.Parse(textBox28.Text);
            double time = 0;

            OperationsCounter ops = new OperationsCounter(totalOperations: 100, progress: progress);
            int type = 0;
            try
            {
                type = Get_Type();
            }
            catch (ArgumentException)
            {
                return;
            }
            // Determine if there are any items checked.
            if (checkedListBox1.CheckedItems.Count == 1)
            {
                for (int x = 0; x <= checkedListBox1.CheckedItems.Count - 1; x++)
                {
                    String Coord_Type = checkedListBox1.CheckedItems[x].ToString();
                    if (Coord_Type == "1")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops));
                    else if (Coord_Type == "2")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_2(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                    else if (Coord_Type == "3")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_3(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                    else if (Coord_Type == "4")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_4(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                    else if (Coord_Type == "5")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_5(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                    else if (Coord_Type == "6")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_6(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                    else if (Coord_Type == "7.1")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_7_1(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                    else if (Coord_Type == "7.2")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_7_2(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                    else if (Coord_Type == "8")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_8(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                    else if (Coord_Type == "9")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_9(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                    else if (Coord_Type == "10")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_10(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, type, out time, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                    else if (Coord_Type == "11.1")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_11_1(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                    else if (Coord_Type == "15")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_15(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                    else if (Coord_Type == "11.2")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_11_2(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                    else if (Coord_Type == "13")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_13(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                    else if (Coord_Type == "14")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_14(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                    else if (Coord_Type == "16")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_16(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                    else if (Coord_Type == "17")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_17(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                    else if (Coord_Type == "18")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_18(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                    else if (Coord_Type == "19")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_19(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                    else if (Coord_Type == "20")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_20(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                    else if (Coord_Type == "19.2")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_19_2(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                    else if (Coord_Type == "20.1")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_20_1(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                    else if (Coord_Type == "21")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_21(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                    else if (Coord_Type == "22")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_22(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                    else if (Coord_Type == "12")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_12(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                    else if (Coord_Type == "27")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_27(x1, y1, z1, a, k, p, out x2, out y2, out z2, B, S, M, type, out time, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                    else if (Coord_Type == "28")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_28(x1, y1, z1, a, k, p, out x2, out y2, out z2, B, S, M, type, out time, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                    else if (Coord_Type == "29")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_29(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                    else if (Coord_Type == "30")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_30(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                    else if (Coord_Type == "31")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_31(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                    else if (Coord_Type == "32")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_32(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                    else if (Coord_Type == "21m")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_21m(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                    else if (Coord_Type == "22m")
                        Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_22m(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, ops))
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
                        Task.Factory.StartNew(() => PointMultiplication33(x1, y1, z1, x3, y3, z3, a, k, l, w, p, out x2, out y2, out z2, out time, type, ops))
                            .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                    }
                }
            }
            else
            {
                MessageBox.Show("Виберіть один алгоритм", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Run multiple algorithms in different threads at the same time
        private void button8_Click(object sender, EventArgs e)
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

            x1 = BigInteger.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
            y1 = BigInteger.Parse(dataGridView1.CurrentRow.Cells[1].Value.ToString());
            z1 = BigInteger.Parse(dataGridView1.CurrentRow.Cells[2].Value.ToString());

            OperationsCounter ops = new OperationsCounter(totalOperations: 100, progress: progress);
            int type = 0;
            try {
                type = Get_Type();
            }
            catch (ArgumentException)
            {
                return;
            }

            if (x1 == 0 || y1 == 0 || z1 == 0)
                MessageBox.Show("Виберіть точку!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                // todo: make this as dynamic mapping
                OperationsCounter ops1 = new OperationsCounter(name: "alg #1");
                OperationsCounter ops2 = new OperationsCounter(name: "alg #2");
                OperationsCounter ops3 = new OperationsCounter(name: "alg #3");
                OperationsCounter ops4 = new OperationsCounter(name: "alg #4");
                OperationsCounter ops5 = new OperationsCounter(name: "alg #5");
                OperationsCounter ops6 = new OperationsCounter(name: "alg #6");
                OperationsCounter ops7_1 = new OperationsCounter(name: "alg #7.1");
                OperationsCounter ops7_2 = new OperationsCounter(name: "alg #7.2");
                OperationsCounter ops8 = new OperationsCounter(name: "alg #8");
                OperationsCounter ops9 = new OperationsCounter(name: "alg #9");
                OperationsCounter ops10 = new OperationsCounter(name: "alg #10");
                OperationsCounter ops11_1 = new OperationsCounter(name: "alg #11.1");
                OperationsCounter ops11_2 = new OperationsCounter(name: "alg #11.2");
                OperationsCounter ops12 = new OperationsCounter(name: "alg #12");
                OperationsCounter ops13 = new OperationsCounter(name: "alg #13");
                OperationsCounter ops14 = new OperationsCounter(name: "alg #14");
                OperationsCounter ops15 = new OperationsCounter(name: "alg #15");
                OperationsCounter ops16 = new OperationsCounter(name: "alg #16");
                OperationsCounter ops17 = new OperationsCounter(name: "alg #17");
                OperationsCounter ops18 = new OperationsCounter(name: "alg #18");
                OperationsCounter ops19 = new OperationsCounter(name: "alg #19");
                OperationsCounter ops19_2 = new OperationsCounter(name: "alg #19.2");
                OperationsCounter ops20 = new OperationsCounter(name: "alg #20");
                OperationsCounter ops20_1 = new OperationsCounter(name: "alg #20.1");
                OperationsCounter ops21 = new OperationsCounter(name: "alg #21");
                OperationsCounter ops21m = new OperationsCounter(name: "alg #21m");
                OperationsCounter ops22 = new OperationsCounter(name: "alg #22");
                OperationsCounter ops22m = new OperationsCounter(name: "alg #22m");
                OperationsCounter ops23 = new OperationsCounter(name: "alg #23");
                OperationsCounter ops24 = new OperationsCounter(name: "alg #24");
                OperationsCounter ops25 = new OperationsCounter(name: "alg #25");
                OperationsCounter ops26 = new OperationsCounter(name: "alg #26");
                OperationsCounter ops27 = new OperationsCounter(name: "alg #27");
                OperationsCounter ops28 = new OperationsCounter(name: "alg #28");
                OperationsCounter ops29 = new OperationsCounter(name: "alg #29");
                OperationsCounter ops30 = new OperationsCounter(name: "alg #30");
                OperationsCounter ops31 = new OperationsCounter(name: "alg #31");
                OperationsCounter ops32 = new OperationsCounter(name: "alg #32");
                OperationsCounter ops33 = new OperationsCounter(name: "alg #33");

                // Determine if there are any items checked.
                if (checkedListBox1.CheckedItems.Count != 0)
                {
                    for (int x = 0; x < checkedListBox1.CheckedItems.Count; x++)
                    {
                        String Coord_Type = checkedListBox1.CheckedItems[x].ToString();
                        if (Coord_Type == "1")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, ops1))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops1));
                        }
                        else if (Coord_Type == "2")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_2(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, ops2))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops2));
                        }
                        else if (Coord_Type == "3")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_3(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, ops3))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops3));
                        }
                        else if (Coord_Type == "4")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_4(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, ops4))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops4));
                        }
                        else if (Coord_Type == "5")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_5(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, ops5))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops5));
                        }
                        else if (Coord_Type == "6")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_6(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, ops6))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops6));
                        }
                        else if (Coord_Type == "7.1")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_7_1(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, ops7_1))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops7_1));
                        }
                        else if (Coord_Type == "7.2")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_7_2(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, ops7_2))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops7_2));
                        }
                        else if (Coord_Type == "8")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_8(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, ops8))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops8));
                        }
                        else if (Coord_Type == "9")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_9(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, ops9))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops9));
                        }
                        else if (Coord_Type == "10")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_10(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, type, out time, ops10))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops10));
                        }
                        else if (Coord_Type == "11.1")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_11_1(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, ops11_1))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops11_1));
                        }
                        else if (Coord_Type == "15")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_15(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, ops15))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops15));
                        }
                        else if (Coord_Type == "11.2")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_11_2(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, ops11_2))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops11_2));
                        }
                        else if (Coord_Type == "13")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_13(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, ops13))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops13));
                        }
                        else if (Coord_Type == "14")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_14(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, ops14))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops14));
                        }
                        else if (Coord_Type == "16")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_16(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, ops16))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops16));
                        }
                        else if (Coord_Type == "17")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_17(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, ops17))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops17));
                        }
                        else if (Coord_Type == "18")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_18(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, ops18))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops18));
                        }
                        else if (Coord_Type == "19")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_19(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max, ops19))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops19));
                        }
                        else if (Coord_Type == "20")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_20(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max, ops20))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops20));
                        }
                        else if (Coord_Type == "19.2")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_19_2(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max, ops19_2))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops19_2));
                        }
                        else if (Coord_Type == "20.1")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_20_1(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max, ops20_1))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops20_1));
                        }
                        else if (Coord_Type == "21")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_21(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, ops21))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops21));
                        }
                        else if (Coord_Type == "22")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_22(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, ops22))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops22));
                        }
                        else if (Coord_Type == "12")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_12(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, ops12))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops12));
                        }
                        else if (Coord_Type == "27")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_27(x1, y1, z1, a, k, p, out x2, out y2, out z2, B, S, M, type, out time, ops27))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops27));
                        }
                        else if (Coord_Type == "28")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_28(x1, y1, z1, a, k, p, out x2, out y2, out z2, B, S, M, type, out time, ops28))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops28));
                        }
                        else if (Coord_Type == "29")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_29(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, ops29))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops29));
                        }
                        else if (Coord_Type == "30")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_30(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, ops30))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time));
                        }
                        else if (Coord_Type == "31")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_31(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, ops31))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops31));
                        }
                        else if (Coord_Type == "32")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_32(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, ops32))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops32));
                        }
                        else if (Coord_Type == "21m")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_21m(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, ops21m))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops21m));
                        }
                        else if (Coord_Type == "22m")
                        {
                            Task.Factory.StartNew(() => Point_Multiplication_Affine_Coord_22m(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, ops22m))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops22m));
                        }
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
                            Task.Factory.StartNew(() => PointMultiplication33(x1, y1, z1, x3, y3, z3, a, k, l, w, p, out x2, out y2, out z2, out time, type, ops33))
                                .ContinueWith(r => updateDataGridValues(x2, y2, z2, time, ops33));
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Виберіть хоча б один алгоритм", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void updateDataGridValues(BigInteger x2, BigInteger y2, BigInteger z2, double time, OperationsCounter ops = null)
        {
            if (x2 != 0 || y2 != 0 || z2 != 0)
            {
                String name = "-";
                if (ops != null)
                    name = ops.Name;
                dataGridView4.Invoke((MethodInvoker)(() => dataGridView4.Rows.Add(name, x2.ToString(), y2.ToString(), z2.ToString())));
            }
            toolStripStatusLabel2.Text = "Виконано за " + time/1000 + "c";
            statusStrip1.Invoke((MethodInvoker)(() => toolStripProgressBar1.Value = 0));
            if (ops != null)
                MessageBox.Show(ops.ToString(), "Info " + ops.Name, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        //ne pravilno rabotaet
        private void writeTimeInFile_Click(object sender, EventArgs e)
        {
            OperationsCounter dummyOps = new OperationsCounter();
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
            try
            {
                type = Get_Type();
            }
            catch (ArgumentException)
            {
                return;
            }

            if (type == 0)
            {
                sw.WriteLine("Афінні координати");
            }
            else if (type == 1)
            {
                sw.WriteLine("Проективні координати");
            }
            else if (type == 2)
            {
                sw.WriteLine("Координати Якобі");
            }
            else if (type == 3)
            {
                sw.WriteLine("Jacoby Chudnovskii");
            }
            else if (type == 4)
            {
                sw.WriteLine("Modified Jacoby");
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
                    // Determine if there are any items checked.
                    if (checkedListBox1.CheckedItems.Count != 0)
                    {
                        for (int x = 0; x <= checkedListBox1.CheckedItems.Count - 1; x++)
                        {
                            String Coord_Type = checkedListBox1.CheckedItems[x].ToString();
                            if (Coord_Type == "1")
                                Point_Multiplication_Affine_Coord_1(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type, dummyOps);
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
                    }
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

            points = EllipticCC.ReadFromFile(quantity, out a, out p);
            int p_bits = Functions.ToBin(p).Length;
            int type = 0;
            try
            {
                type = Get_Type();
            }
            catch (ArgumentException)
            {
                return;
            }
            string sysCoord = "";
            if (type == 0)
            {
                sysCoord = "AffineCoordinate";
            }
            else if (type == 1)
            {
                sysCoord = "ProjectiveCoordinate";
            }
            else if (type == 2)
            {
                sysCoord = "JacobiCoordinate";
            }
            else if (type == 3)
            {
                sysCoord = "JacobiChudnovskyiCoordinate";
            }
            else if (type == 4)
            {
                sysCoord = "ModifiedJacobiCoordinate";
            }

            openFileDialog1.Filter = "txt файли(*.txt)|*.txt";
            openFileDialog1.FileName = "All_Algorithms_Time_" + p_bits + "_" + sysCoord + ".txt"; //All_Algorithms_Time_
            string filename = openFileDialog1.FileName;
            FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(sysCoord);
            OperationsCounter dummyOps = new OperationsCounter();  // No need to count operations here
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
                        Point_Multiplication_Affine_Coord_1(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, out time, type, dummyOps);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 0] += ts.TotalMilliseconds;
                        stopWatch.Reset();

                        stopWatch.Start();
                        Point_Multiplication_Affine_Coord_2(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2,
                            out time, type, dummyOps);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 1] += ts.TotalMilliseconds;
                        stopWatch.Reset();

                        time = 0;
                        Point_Multiplication_Affine_Coord_3(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2,
                            out time, type, dummyOps);
                        time_average[j, 2] += time;
                        stopWatch.Reset();

                        time = 0;
                        Point_Multiplication_Affine_Coord_4(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2,
                            out time, type, dummyOps);
                        time_average[j, 3] += time;
                        stopWatch.Reset();

                        time = 0;
                        Point_Multiplication_Affine_Coord_5(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2,
                            out time, type, dummyOps);
                        time_average[j, 4] += time;
                        stopWatch.Reset();

                        time = 0;
                        Point_Multiplication_Affine_Coord_6(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2,
                            out time, type, dummyOps);
                        time_average[j, 5] += time;
                        stopWatch.Reset();

                        stopWatch.Start();
                        Point_Multiplication_Affine_Coord_7_1(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2,
                            out time, type, dummyOps);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 6] += ts.TotalMilliseconds;
                        stopWatch.Reset();

                        stopWatch.Start();
                        Point_Multiplication_Affine_Coord_7_2(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2,
                            out time, type, dummyOps);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 7] += ts.TotalMilliseconds;
                        stopWatch.Reset();

                        stopWatch.Start();
                        Point_Multiplication_Affine_Coord_8(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2,
                            out time, type, dummyOps);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 8] += ts.TotalMilliseconds;
                        stopWatch.Reset();

                        time = 0;
                        Point_Multiplication_Affine_Coord_9(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2,
                            out time, type, dummyOps);
                        time_average[j, 9] += time;
                        stopWatch.Reset();

                        time = 0;
                        Point_Multiplication_Affine_Coord_10(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2,
                            type, out time, dummyOps);
                        time_average[j, 10] += time;
                        stopWatch.Reset();

                        time = 0;
                        Point_Multiplication_Affine_Coord_11_1(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2,
                            out time, type, dummyOps);
                        time_average[j, 11] += time;
                        stopWatch.Reset();

                        time = 0;
                        Point_Multiplication_Affine_Coord_11_2(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2,
                            out time, type, dummyOps);
                        time_average[j, 12] += time;
                        stopWatch.Reset();

                        time = 0;
                        Point_Multiplication_Affine_Coord_12(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], w, p, out x2, out y2, out z2,
                            out time, type, dummyOps);
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
                        Point_Multiplication_Affine_Coord_17(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2,
                            out time, type, dummyOps);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 14] += ts.TotalMilliseconds;
                        stopWatch.Reset();

                        stopWatch.Start();
                        Point_Multiplication_Affine_Coord_18(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2,
                            out time, type, dummyOps);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 15] += ts.TotalMilliseconds;
                        stopWatch.Reset();

                        stopWatch.Start();
                        Point_Multiplication_Affine_Coord_19(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2,
                            out time, type, a_max, b_max, dummyOps);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 16] += ts.TotalMilliseconds;
                        stopWatch.Reset();

                        stopWatch.Start();
                        Point_Multiplication_Affine_Coord_19_2(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2,
                            out time, type, a_max, b_max, dummyOps);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 17] += ts.TotalMilliseconds;
                        stopWatch.Reset();

                        stopWatch.Start();
                        Point_Multiplication_Affine_Coord_20_1(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2,
                            out time, type, a_max, b_max, dummyOps);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 18] += ts.TotalMilliseconds;
                        stopWatch.Reset();

                        stopWatch.Start();
                        Point_Multiplication_Affine_Coord_20(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2,
                            out time, type, a_max, b_max, dummyOps);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 19] += ts.TotalMilliseconds;
                        stopWatch.Reset();

                        stopWatch.Start();
                        Point_Multiplication_Affine_Coord_21(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2,
                            out time, type, dummyOps);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 20] += ts.TotalMilliseconds;
                        stopWatch.Reset();

                        stopWatch.Start();
                        Point_Multiplication_Affine_Coord_22(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2,
                            out time, type, dummyOps);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 21] += ts.TotalMilliseconds;
                        stopWatch.Reset();

                        stopWatch.Start();
                        Point_Multiplication_Affine_Coord_27(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, B, S, M,
                            type, out time, dummyOps);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 22] += ts.TotalMilliseconds;
                        stopWatch.Reset();

                        stopWatch.Start();
                        Point_Multiplication_Affine_Coord_28(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2, B, S, M,
                            type, out time, dummyOps);
                        stopWatch.Stop();
                        ts = stopWatch.Elapsed;
                        time_average[j, 23] += ts.TotalMilliseconds;
                        stopWatch.Reset();

                        time = 0;
                        Point_Multiplication_Affine_Coord_29(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2,
                            out time, type, dummyOps);
                        time_average[j, 24] += time;
                        stopWatch.Reset();

                        time = 0;
                        Point_Multiplication_Affine_Coord_30(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2,
                            out time, type, dummyOps);
                        time_average[j, 25] += time;
                        stopWatch.Reset();

                        time = 0;
                        Point_Multiplication_Affine_Coord_31(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2,
                            out time, type, dummyOps);
                        time_average[j, 26] += time;
                        stopWatch.Reset();

                        time = 0;
                        Point_Multiplication_Affine_Coord_32(points[i, 0], points[i, 1], points[i, 2], a, mass_k[l], p, out x2, out y2, out z2,
                            out time, type, dummyOps);
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
            OperationsCounter dummyOps = new OperationsCounter(); // Не потрібно рахувати кількість операцій в режимі "Калькулятора"
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

            x1 = BigInteger.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
            y1 = BigInteger.Parse(dataGridView1.CurrentRow.Cells[1].Value.ToString());
            z1 = BigInteger.Parse(dataGridView1.CurrentRow.Cells[2].Value.ToString());
            a_max = BigInteger.Parse(textBox27.Text);
            b_max = BigInteger.Parse(textBox28.Text);

            int type = 0;
            try
            {
                type = Get_Type();
            }
            catch (ArgumentException)
            {
                return;
            }


            if (x1 == 0 || y1 == 0)
                MessageBox.Show("Виберіть точку!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                int i = 0;
                dataGridView4.RowCount = 1;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, dummyOps);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_2(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, dummyOps);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_3(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, dummyOps);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_4(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, dummyOps);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_5(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, dummyOps);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_6(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, dummyOps);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_7_1(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, dummyOps);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_7_2(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, dummyOps);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_8(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, dummyOps);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_9(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, dummyOps);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_10(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, type, out time, dummyOps);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_11_1(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, dummyOps);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_11_2(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, dummyOps);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_12(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, dummyOps);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_13(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, dummyOps);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_14(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, dummyOps);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();
                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_15(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, dummyOps);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_16(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, dummyOps);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_17(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, dummyOps);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_18(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, dummyOps);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_19(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max, dummyOps);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_19_2(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max, dummyOps);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_20_1(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max, dummyOps);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_20(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max, dummyOps);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_21(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, dummyOps);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_22(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, dummyOps);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_27(x1, y1, z1, a, k, p, out x2, out y2, out z2, B, S, M, type, out time, dummyOps);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_28(x1, y1, z1, a, k, p, out x2, out y2, out z2, B, S, M, type, out time, dummyOps);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_29(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, dummyOps);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_30(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, dummyOps);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_31(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, dummyOps);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_32(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, dummyOps);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_21m(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, dummyOps);
                dataGridView4.Rows[i].Cells[1].Value = x2.ToString();
                dataGridView4.Rows[i].Cells[2].Value = y2.ToString();
                dataGridView4.Rows[i].Cells[3].Value = z2.ToString();

                i++;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_22m(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, dummyOps);
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
            BigInteger po;

            a = BigInteger.Parse(textBox2.Text);
            b = BigInteger.Parse(textBox3.Text);
            p = BigInteger.Parse(textBox1.Text);
            if (textBox25.Text != "")
            {
                po = BigInteger.Parse(textBox25.Text);
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
            // Fill multiplications algorithms list box
            for (int i = 0; i < multAlgorithms.Count; i++)
            {
                checkedListBox1.Items.Add(multAlgorithms.ElementAt(i));
            }

            // Fill cryptography algorithms list box
            for(int i = 0; i < cryptAlgorithms.Count; i++)
            {
                checkedListBox2.Items.Add(cryptAlgorithms.ElementAt(i));
            }
            
            // Fill all available curves
            for(int i = 0; i < curveURI.Keys.Count; i++)
            {
                comboBox2.Items.Add(curveURI.Keys.ElementAt(i));
            }

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
            OperationsCounter dummyOps = new OperationsCounter();
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

            x1 = BigInteger.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
            y1 = BigInteger.Parse(dataGridView1.CurrentRow.Cells[1].Value.ToString());
            z1 = BigInteger.Parse(dataGridView1.CurrentRow.Cells[2].Value.ToString());

            BigInteger x3, y3, z3;

            int type = 1;

            if (x1 == 0 || y1 == 0)
                MessageBox.Show("Виберіть точку!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                int i = 0;
                dataGridView4.RowCount = 1;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_2(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_3(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_4(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_5(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_6(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_7_1(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_7_2(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_8(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_9(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_10(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, type, out time, dummyOps);
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
                Point_Multiplication_Affine_Coord_11_1(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_11_2(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_12(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_13(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_14(x1, y1, z1, a, k, w, p, out x2, out y2, out z2, out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_15(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_16(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_17(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_18(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_19(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max, dummyOps);
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
                Point_Multiplication_Affine_Coord_19_2(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max, dummyOps);
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
                Point_Multiplication_Affine_Coord_20_1(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max, dummyOps);
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
                Point_Multiplication_Affine_Coord_20(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, a_max, b_max, dummyOps);
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
                Point_Multiplication_Affine_Coord_21(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_22(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_27(x1, y1, z1, a, k, p, out x2, out y2, out z2, B, S, M,
                    type, out time, dummyOps);
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
                Point_Multiplication_Affine_Coord_28(x1, y1, z1, a, k, p, out x2, out y2, out z2, B, S, M,
                    type, out time, dummyOps);
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
                Point_Multiplication_Affine_Coord_29(x1, y1, z1, a, k, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_30(x1, y1, z1, a, k, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_31(x1, y1, z1, a, k, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_32(x1, y1, z1, a, k, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
            OperationsCounter dummyOps = new OperationsCounter();
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

            x1 = BigInteger.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
            y1 = BigInteger.Parse(dataGridView1.CurrentRow.Cells[1].Value.ToString());
            z1 = BigInteger.Parse(dataGridView1.CurrentRow.Cells[2].Value.ToString());

            BigInteger x3, y3, z3;
            int type = 2;
            if (x1 == 0 || y1 == 0)
                MessageBox.Show("Виберіть точку!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                int i = 0;
                dataGridView4.RowCount = 1;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_2(x1, y1, z1, a, k, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_3(x1, y1, z1, a, k, w, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_4(x1, y1, z1, a, k, w, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_5(x1, y1, z1, a, k, w, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_6(x1, y1, z1, a, k, w, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_7_1(x1, y1, z1, a, k, w, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_7_2(x1, y1, z1, a, k, w, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_8(x1, y1, z1, a, k, w, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_9(x1, y1, z1, a, k, w, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_10(x1, y1, z1, a, k, w, p, out x2, out y2, out z2,
                    type, out time, dummyOps);
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
                Point_Multiplication_Affine_Coord_11_1(x1, y1, z1, a, k, w, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_11_2(x1, y1, z1, a, k, w, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_12(x1, y1, z1, a, k, w, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_13(x1, y1, z1, a, k, w, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_14(x1, y1, z1, a, k, w, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_15(x1, y1, z1, a, k, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_16(x1, y1, z1, a, k, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_17(x1, y1, z1, a, k, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_18(x1, y1, z1, a, k, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_19(x1, y1, z1, a, k, p, out x2, out y2, out z2,
                    out time, type, a_max, b_max, dummyOps);
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
                Point_Multiplication_Affine_Coord_19_2(x1, y1, z1, a, k, p, out x2, out y2, out z2,
                    out time, type, a_max, b_max, dummyOps);
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
                Point_Multiplication_Affine_Coord_20_1(x1, y1, z1, a, k, p, out x2, out y2, out z2,
                    out time, type, a_max, b_max, dummyOps);
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
                Point_Multiplication_Affine_Coord_20(x1, y1, z1, a, k, p, out x2, out y2, out z2,
                    out time, type, a_max, b_max, dummyOps);
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
                Point_Multiplication_Affine_Coord_21(x1, y1, z1, a, k, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_22(x1, y1, z1, a, k, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_27(x1, y1, z1, a, k, p, out x2, out y2, out z2, B, S, M,
                    type, out time, dummyOps);
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
                Point_Multiplication_Affine_Coord_28(x1, y1, z1, a, k, p, out x2, out y2, out z2, B, S, M,
                    type, out time, dummyOps);
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
                Point_Multiplication_Affine_Coord_29(x1, y1, z1, a, k, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_30(x1, y1, z1, a, k, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_31(x1, y1, z1, a, k, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_32(x1, y1, z1, a, k, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
            OperationsCounter dummyOps = new OperationsCounter();
            int type = 0;
            try
            {
                type = Get_Type();
            }
            catch (ArgumentException)
            {
                return;
            }

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

                        Point_Multiplication_Affine_Coord_4(points[i, 0], points[i, 1], 1, a, k1, (int)w, p, out x2, out y2, out z2,
                            out time, type, dummyOps);
                        if (checkBox11.Checked) time_average[0, j] += 0;
                        else time_average[0, j] += time;
                        stopWatch.Reset();
                        /*
                        time = 0;
                        Point_Multiplication_Affine_Coord_5(points[i, 0], points[i, 1], 1, a, k1, (int)w, p, out x2, out y2, out z2, out time, type);
                        time_average[2, j] += time;
                        stopWatch.Reset();
                        */
                        time = 0;

                        Point_Multiplication_Affine_Coord_6(points[i, 0], points[i, 1], 1, a, k1, (int)w, p, out x2, out y2, out z2,
                            out time, type, dummyOps);
                        if (checkBox11.Checked) time_average[0, j] += 0;
                        else time_average[1, j] += time;
                        stopWatch.Reset();
                        /*
                        time = 0;
                        Point_Multiplication_Affine_Coord_9(points[i, 0], points[i, 1], 1, a, k1, (int)w, p, out x2, out y2, out z2, out time, type);
                        time_average[4, j] += time;
                        stopWatch.Reset();
                        */
                        time = 0;

                        Point_Multiplication_Affine_Coord_10(points[i, 0], points[i, 1], 1, a, k1, (int)w, p, out x2, out y2, out z2,
                            type, out time, dummyOps);
                        if (checkBox11.Checked) time_average[0, j] += 0;
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

                        Point_Multiplication_Affine_Coord_12(points[i, 0], points[i, 1], 1, a, k1, (int)w, p, out x2, out y2, out z2,
                            out time, type, dummyOps);
                        if (checkBox11.Checked) time_average[0, j] += 0;
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
            OperationsCounter dummyOps = new OperationsCounter();
            int type = 0;
            try
            {
                type = Get_Type();
            }
            catch (ArgumentException)
            {
                return;
            }
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
                            PointMultiplication.Point_Multiplication_Affine_Coord_19_1(points[i, 0], points[i, 1], 1, a, k1, p, out x2, out y2, out z2,
                                out time, type, a_max, b_max, dummyOps);
                            time_average[0, a1, b1] += time;
                            time = 0;
                            PointMultiplication.Point_Multiplication_Affine_Coord_19_2(points[i, 0], points[i, 1], 1, a, k1, p, out x2, out y2, out z2,
                                out time, type, a_max, b_max, dummyOps);
                            time_average[1, a1, b1] += time;
                            time = 0;
                            PointMultiplication.Point_Multiplication_Affine_Coord_20_1(points[i, 0], points[i, 1], 1, a, k1, p, out x2, out y2, out z2,
                                out time, type, a_max, b_max, dummyOps);
                            time_average[2, a1, b1] += time;
                            time = 0;
                            PointMultiplication.Point_Multiplication_Affine_Coord_20_2(points[i, 0], points[i, 1], 1, a, k1, p, out x2, out y2, out z2,
                                out time, type, a_max, b_max, dummyOps);
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
            OperationsCounter dummyOps = new OperationsCounter();
            //CP
            int type = 0;
            try
            {
                type = Get_Type();
            }
            catch (ArgumentException)
            {
                return;
            }
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
                        PointMultiplication.Point_Multiplication_Affine_Coord_27(points[i, 0], points[i, 1], 1, a, k1, p, out x2, out y2, out z2, B, S, M,
                            type, out time, dummyOps);
                        time_average[0, bN, sN, mN] += time;
                        time = 0;
                        PointMultiplication.Point_Multiplication_Affine_Coord_28(points[i, 0], points[i, 1], 1, a, k1, p, out x2, out y2, out z2, B, S, M,
                            type, out time, dummyOps);
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
                            PointMultiplication.Point_Multiplication_Affine_Coord_27(points[i, 0], points[i, 1], 1, a, k1, p, out x2, out y2, out z2, ii, S, M,
                                type, out time, dummyOps);
                            time_average[0, bN, sN, mN] += time;
                            time = 0;
                            PointMultiplication.Point_Multiplication_Affine_Coord_28(points[i, 0], points[i, 1], 1, a, k1, p, out x2, out y2, out z2, ii, S, M,
                                type, out time, dummyOps);
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
                                PointMultiplication.Point_Multiplication_Affine_Coord_27(points[i, 0], points[i, 1], 1, a, k1, p, out x2, out y2, out z2, B, S, M,
                                    type, out time, dummyOps);
                                time_average[0, bN, sN, mN] += time;
                                time = 0;
                                PointMultiplication.Point_Multiplication_Affine_Coord_28(points[i, 0], points[i, 1], 1, a, k1, p, out x2, out y2, out z2, B, S, M,
                                    type, out time, dummyOps);
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
                                PointMultiplication.Point_Multiplication_Affine_Coord_27(points[i, 0], points[i, 1], 1, a, k1, p, out x2, out y2, out z2, B, S, M,
                                    type, out time, dummyOps);
                                time_average[0, bN, sN, mN] += time;
                                time = 0;
                                PointMultiplication.Point_Multiplication_Affine_Coord_28(points[i, 0], points[i, 1], 1, a, k1, p, out x2, out y2, out z2, B, S, M,
                                    type, out time, dummyOps);
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
                                        PointMultiplication.Point_Multiplication_Affine_Coord_27(points[i, 0], points[i, 1], 1, a, k1, p, out x2, out y2, out z2, ii, S, M,
                                            type, out time, dummyOps);
                                        time_average[0, bN, sN, mN] += time;
                                        time = 0;
                                        PointMultiplication.Point_Multiplication_Affine_Coord_28(points[i, 0], points[i, 1], 1, a, k1, p, out x2, out y2, out z2, ii, S, M,
                                            type, out time, dummyOps);
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
                                            PointMultiplication.Point_Multiplication_Affine_Coord_27(points[i, 0], points[i, 1], 1, a, k1, p, out x2, out y2, out z2, ii, S, M,
                                                type, out time, dummyOps);
                                            time_average[0, bN, sN, mN] += time;
                                            time = 0;
                                            PointMultiplication.Point_Multiplication_Affine_Coord_28(points[i, 0], points[i, 1], 1, a, k1, p, out x2, out y2, out z2, ii, S, M,
                                                type, out time, dummyOps);
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
                                                PointMultiplication.Point_Multiplication_Affine_Coord_27(points[i, 0], points[i, 1], 1, a, k1, p, out x2, out y2, out z2, B, S, M,
                                                    type, out time, dummyOps);
                                                time_average[0, bN, sN, mN] += time;
                                                time = 0;
                                                PointMultiplication.Point_Multiplication_Affine_Coord_28(points[i, 0], points[i, 1], 1, a, k1, p, out x2, out y2, out z2, B, S, M, type, out time, dummyOps);
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
                                                        PointMultiplication.Point_Multiplication_Affine_Coord_27(points[i, 0], points[i, 1], 1, a, k1, p, out x2, out y2, out z2, ii, S, M,
                                                            type, out time, dummyOps);
                                                        time_average[0, bN, sN, mN] += time;
                                                        time = 0;
                                                        PointMultiplication.Point_Multiplication_Affine_Coord_28(points[i, 0], points[i, 1], 1, a, k1, p, out x2, out y2, out z2, ii, S, M, type, out time, dummyOps);
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
            OperationsCounter dummyOps = new OperationsCounter();
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
                        Point_Multiplication_Affine_Coord_32(coord[0], coord[1], coord[2], a, k, p, out x1, out y1, out z1, out time, 0, dummyOps);
                        if (radioButton47.Checked)
                        {
                            Point_Multiplication_Affine_Coord_32(coord[0], coord[1], coord[2], a, k, p, out x2, out y2, out z2, out time, 1, dummyOps);
                            x3 = x2; y3 = y2; z3 = z2;
                            ProjectiveToAffine(x2, y2, z2, p, out x2, out y2, out z2);
                        }
                        if (radioButton46.Checked)
                        {
                            Point_Multiplication_Affine_Coord_32(coord[0], coord[1], coord[2], a, k, p, out x2, out y2, out z2, out time, 2, dummyOps);
                            x3 = x2; y3 = y2; z3 = z2;
                            JacobyToAffine(x2,y2,z2,p, out x2, out y2, out z2);
                        }
                        if (radioButton51.Checked)
                        {
                            Point_Multiplication_Affine_Coord_32(coord[0], coord[1], coord[2], a, k, p, out x2, out y2, out z2, out time, 4, dummyOps);
                            x3 = x2; y3 = y2; z3 = z2;
                            JacobyToAffine(x2, y2, z2, p, out x2, out y2, out z2);
                        }
                        if (radioButton52.Checked)
                        {
                            Point_Multiplication_Affine_Coord_32(coord[0], coord[1], coord[2], a, k, p, out x2, out y2, out z2, out time, 3, dummyOps);
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
            OperationsCounter dummyOps = new OperationsCounter();
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

            x1 = BigInteger.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
            y1 = BigInteger.Parse(dataGridView1.CurrentRow.Cells[1].Value.ToString());
            z1 = BigInteger.Parse(dataGridView1.CurrentRow.Cells[2].Value.ToString());

            BigInteger x3, y3, z3;
            int type = 0;
            try
            {
                type = Get_Type();
            }
            catch (ArgumentException)
            {
                return;
            }

            if (x1 == 0 || y1 == 0)
                MessageBox.Show("Выбирете точку!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                int i = 0;
                dataGridView4.RowCount = 1;
                dataGridView4.Rows.Add();
                Point_Multiplication_Affine_Coord_1(x1, y1, z1, a, k, p, out x2, out y2, out z2, out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_2(x1, y1, z1, a, k, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_3(x1, y1, z1, a, k, w, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_4(x1, y1, z1, a, k, w, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_5(x1, y1, z1, a, k, w, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_6(x1, y1, z1, a, k, w, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_7_1(x1, y1, z1, a, k, w, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_7_2(x1, y1, z1, a, k, w, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_8(x1, y1, z1, a, k, w, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_9(x1, y1, z1, a, k, w, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_10(x1, y1, z1, a, k, w, p, out x2, out y2, out z2,
                    type, out time, dummyOps);
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
                Point_Multiplication_Affine_Coord_11_1(x1, y1, z1, a, k, w, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_11_2(x1, y1, z1, a, k, w, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_12(x1, y1, z1, a, k, w, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_17(x1, y1, z1, a, k, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_18(x1, y1, z1, a, k, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_19(x1, y1, z1, a, k, p, out x2, out y2, out z2,
                    out time, type, a_max, b_max, dummyOps);
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
                Point_Multiplication_Affine_Coord_19_2(x1, y1, z1, a, k, p, out x2, out y2, out z2,
                    out time, type, a_max, b_max, dummyOps);
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
                Point_Multiplication_Affine_Coord_20_1(x1, y1, z1, a, k, p, out x2, out y2, out z2,
                    out time, type, a_max, b_max, dummyOps);
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
                Point_Multiplication_Affine_Coord_20(x1, y1, z1, a, k, p, out x2, out y2, out z2,
                    out time, type, a_max, b_max, dummyOps);
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
                Point_Multiplication_Affine_Coord_21(x1, y1, z1, a, k, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_22(x1, y1, z1, a, k, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_27(x1, y1, z1, a, k, p, out x2, out y2, out z2, B, S, M,
                    type, out time, dummyOps);
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
                Point_Multiplication_Affine_Coord_28(x1, y1, z1, a, k, p, out x2, out y2, out z2, B, S, M,
                    type, out time, dummyOps);
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
                Point_Multiplication_Affine_Coord_29(x1, y1, z1, a, k, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_30(x1, y1, z1, a, k, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_31(x1, y1, z1, a, k, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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
                Point_Multiplication_Affine_Coord_32(x1, y1, z1, a, k, p, out x2, out y2, out z2,
                    out time, type, dummyOps);
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

        private void button9_Click(object sender, EventArgs e)
        {
            dataGridView4.Rows.Clear();
            dataGridView4.Refresh();
        }

        private void groupBox17_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton30_CheckedChanged(object sender, EventArgs e)
        {

        }

        Dictionary<string, MultiplyPoint> multiplyAlgorithms = new Dictionary<string, MultiplyPoint>() {
            {"1", PointMultiplication.Point_Multiplication_Affine_Coord_1},
            {"2", PointMultiplication.Point_Multiplication_Affine_Coord_2},
            {"3", PointMultiplication.Point_Multiplication_Affine_Coord_3},
            {"4", PointMultiplication.Point_Multiplication_Affine_Coord_4},
            {"5", PointMultiplication.Point_Multiplication_Affine_Coord_5},
            {"6", PointMultiplication.Point_Multiplication_Affine_Coord_6},
            {"7.1", PointMultiplication.Point_Multiplication_Affine_Coord_7_1},
            {"7.2", PointMultiplication.Point_Multiplication_Affine_Coord_7_2},
            {"8", PointMultiplication.Point_Multiplication_Affine_Coord_8},
            {"9", PointMultiplication.Point_Multiplication_Affine_Coord_9},
            {"10", PointMultiplication.Point_Multiplication_Affine_Coord_10},
            {"11.1", PointMultiplication.Point_Multiplication_Affine_Coord_11_1},
            {"11.2", PointMultiplication.Point_Multiplication_Affine_Coord_11_2},
            {"12", PointMultiplication.Point_Multiplication_Affine_Coord_12},
            {"13", PointMultiplication.Point_Multiplication_Affine_Coord_13},
            {"14", PointMultiplication.Point_Multiplication_Affine_Coord_14},
            {"15", PointMultiplication.Point_Multiplication_Affine_Coord_15},
            {"16", PointMultiplication.Point_Multiplication_Affine_Coord_16},
            {"17", PointMultiplication.Point_Multiplication_Affine_Coord_17},
            // Unsupported signatures. Are this methods not designed to be used here?
            /*
            {"19.1", PointMultiplication.Point_Multiplication_Affine_Coord_19_1},
            {"19.2", PointMultiplication.Point_Multiplication_Affine_Coord_19_2},
            {"20.1", PointMultiplication.Point_Multiplication_Affine_Coord_20_1},
            {"20.2", PointMultiplication.Point_Multiplication_Affine_Coord_20_2},
            */
            {"21", PointMultiplication.Point_Multiplication_Affine_Coord_21},
            {"22", PointMultiplication.Point_Multiplication_Affine_Coord_22},
            {"21m", PointMultiplication.Point_Multiplication_Affine_Coord_21m},
            {"22m", PointMultiplication.Point_Multiplication_Affine_Coord_22m},
            /*
            {"27", PointMultiplication.Point_Multiplication_Affine_Coord_27},
            {"28", PointMultiplication.Point_Multiplication_Affine_Coord_28},
            */
            {"29", PointMultiplication.Point_Multiplication_Affine_Coord_29},
            {"30", PointMultiplication.Point_Multiplication_Affine_Coord_30},
            {"31", PointMultiplication.Point_Multiplication_Affine_Coord_31},
            {"32", PointMultiplication.Point_Multiplication_Affine_Coord_32},
            //{"33", PointMultiplication.Point_Multiplication_Affine_Coord_33},
        };

        // Система координат -> Алгоритм шифрування -> Алгоритм скалярного множення -> Об'єкт кількості операцій
        Dictionary<string, Dictionary<string, Dictionary<string, OperationsCounter>>> tables1 = new Dictionary<string, Dictionary<string, Dictionary<string, OperationsCounter>>>();

        private void exportTables1(Dictionary<string, Dictionary<string, Dictionary<string, OperationsCounter>>> table)
        {
            var excelApp = new Excel.Application();
            excelApp.Visible = true;
            excelApp.DisplayAlerts = false;
            Excel.Workbook workbook = excelApp.Workbooks.Add();
            for (int i = 0; i < table.Keys.Count; i++)
            {
                string currentKey = table.Keys.ElementAt(i);

                Excel.Worksheet newWorkSheet = (Excel.Worksheet)workbook.Worksheets.Add();
                newWorkSheet.Name = currentKey;
                newWorkSheet.Cells[1, "A"] = "Назва алгоритму шифрування";
                newWorkSheet.Cells[1, "B"] = "Назва алгоритму скалярного множення";
                newWorkSheet.Cells[1, "C"] = "Кількість операцій";

                newWorkSheet.Cells[2, "C"] = "Додавання точок еліптичної кривої";
                newWorkSheet.Cells[2, "D"] = "Подвоєння точок еліптичної кривої";
                newWorkSheet.Cells[2, "E"] = "Загалом";
                newWorkSheet.Columns["A:B"].AutoFit();


                Dictionary<string, Dictionary<string, OperationsCounter>> cryptsIterator = table[currentKey];
                for (int j = 0; j < cryptsIterator.Keys.Count; j++)
                {
                    string currentCryptAlg = cryptsIterator.Keys.ElementAt(j);
                    Dictionary<string, OperationsCounter> multIterator = cryptsIterator[currentCryptAlg];
                    int multCount = multIterator.Keys.Count;
                    newWorkSheet.Cells[3 + j * multCount, "A"] = currentCryptAlg;

                    for (int k = 0; k < multCount; k++)
                    {
                        string currentMultAlg = multIterator.Keys.ElementAt(k);
                        int rowPosOffset = 3 + j * multCount + k;
                        OperationsCounter currentOps = multIterator[currentMultAlg];
                        newWorkSheet.Cells[rowPosOffset, "B"] = currentMultAlg;
                        newWorkSheet.Cells[rowPosOffset, "C"] = currentOps.AddPointsOperations;
                        newWorkSheet.Cells[rowPosOffset, "D"] = currentOps.DoublingPointsOperations;
                        newWorkSheet.Cells[rowPosOffset, "E"] = currentOps.Operations;
                    }
                }
            }

            try
            {
                workbook.SaveAs("Еліптичні криві. Кількість операцій.xlsx",
                    Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                    false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                MessageBox.Show("Помилка збереження в файл", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        // Система координат -> Алгоритм шифрування -> Алгоритм скалярного множення -> Час виконання
        Dictionary<string, Dictionary<string, Dictionary<string, double>>> tables2 = new Dictionary<string, Dictionary<string, Dictionary<string, double>>>();

        private void exportTables2(Dictionary<string, Dictionary<string, Dictionary<string, double>>> table)
        {
            var excelApp = new Excel.Application();
            excelApp.Visible = true;
            excelApp.DisplayAlerts = false;
            Excel.Workbook workbook = excelApp.Workbooks.Add();
            for (int i = 0; i < table.Keys.Count; i++)
            {
                string currentKey = table.Keys.ElementAt(i);

                Excel.Worksheet newWorkSheet = (Excel.Worksheet)workbook.Worksheets.Add();
                newWorkSheet.Name = currentKey;
                newWorkSheet.Cells[1, "A"] = "Назва алгоритму шифрування";
                newWorkSheet.Cells[1, "B"] = "Назва алгоритму скалярного множення";
                newWorkSheet.Cells[1, "C"] = "Час виконання";
                newWorkSheet.Columns["A:C"].AutoFit();

                Dictionary<string, Dictionary<string, double>> cryptsIterator = table[currentKey];
                for (int j = 0; j < cryptsIterator.Keys.Count; j++)
                {
                    string currentCryptAlg = cryptsIterator.Keys.ElementAt(j);
                    Dictionary<string, double> multIterator = cryptsIterator[currentCryptAlg];
                    int multCount = multIterator.Keys.Count;
                    newWorkSheet.Cells[2 + j * multCount, "A"] = currentCryptAlg;

                    for (int k = 0; k < multCount; k++)
                    {
                        string currentMultAlg = multIterator.Keys.ElementAt(k);
                        int rowPosOffset = 2 + j * multCount + k;
                        double time = multIterator[currentMultAlg];
                        newWorkSheet.Cells[rowPosOffset, "B"] = currentMultAlg;
                        newWorkSheet.Cells[rowPosOffset, "C"] = time;
                    }
                }
            }

            string filename = "Еліптичні криві. Часові показники.xlsx";
            try
            {
                workbook.SaveAs(filename,
                    Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                    false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                MessageBox.Show("Помилка збереження в файл " + filename, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        List<Task> TaskList = new List<Task>();
        int tasksFinished = 0;

        private void button10_Click(object sender, EventArgs e)
        {
            if(checkedListBox3.CheckedItems.Count == 0)
            {
                MessageBox.Show("Оберіть хоча б одну систему координат", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (checkedListBox1.CheckedItems.Count == 0)
            {
                MessageBox.Show("Оберіть хоча б один алгоритм множення", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (checkedListBox2.CheckedItems.Count == 0)
            {
                MessageBox.Show("Оберіть хоча б один алгоритм шифрування", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBoxCryptData.Text.Length == 0)
            {
                MessageBox.Show("Заповніть поле для вхідних даних", "Невірні дані", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            tables1.Clear(); // Renew information from previous launch
            tables2.Clear();
            TaskList.Clear();
            tasksFinished = 0;

            foreach (string coorSysItem in checkedListBox3.CheckedItems)
            {
                tables1.Add(coorSysItem, new Dictionary<string, Dictionary<string, OperationsCounter>>());
                tables2.Add(coorSysItem, new Dictionary<string, Dictionary<string, double>>());
                foreach (string cryptAlgItem in checkedListBox2.CheckedItems)
                {
                    tables1[coorSysItem].Add(cryptAlgItem, new  Dictionary<string, OperationsCounter>());
                    tables2[coorSysItem].Add(cryptAlgItem, new  Dictionary<string, double>());
                    foreach (string multAlgItem in checkedListBox1.CheckedItems)
                    {
                        int coorType = checkedListBox2.Items.IndexOf(coorSysItem);
                        double executionTime = 0;
                        MultiplyPoint multiplier = multiplyAlgorithms[multAlgItem];
                        OperationsCounter ops = new OperationsCounter();
                        Console.WriteLine("Running " + cryptAlgItem);
                        Console.WriteLine("\tPoint multiplication algorithm: " + multAlgItem);
                        Console.WriteLine("\tCoordinate system " + coorSysItem + "\n");
                        switch (cryptAlgItem)
                        {
                            case "ECDSA":
                                Task Launch_ECDSA = Task.Factory.StartNew(() => Run_ECDSA(multiplier: multiplier, coorType: coorType, time: out executionTime, ops: ops))
                                    .ContinueWith(r => taskFinished(coorSysItem, cryptAlgItem, multAlgItem, executionTime, ops));
                                TaskList.Add(Launch_ECDSA);
                                break;

                            case "GOST_R34_10_2001":
                                Task Launch_GOST = Task.Factory.StartNew(() => Run_GOST_R34_10_2001(multiplier: multiplier, coorType: coorType, time: out executionTime, ops: ops))
                                    .ContinueWith(r => taskFinished(coorSysItem, cryptAlgItem, multAlgItem, executionTime, ops));
                                TaskList.Add(Launch_GOST);
                                break;

                            case "KCDSA":
                                Task Launch_KCDSA = Task.Factory.StartNew(() => Run_KCDSA(multiplier: multiplier, coorType: coorType, time: out executionTime, ops: ops))
                                    .ContinueWith(r => taskFinished(coorSysItem, cryptAlgItem, multAlgItem, executionTime, ops));
                                TaskList.Add(Launch_KCDSA);
                                break;

                            case "Shor":
                                Task Launch_Shor = Task.Factory.StartNew(() => Run_Shor(multiplier: multiplier, coorType: coorType, time: out executionTime, ops: ops))
                                    .ContinueWith(r => taskFinished(coorSysItem, cryptAlgItem, multAlgItem, executionTime, ops));
                                TaskList.Add(Launch_Shor);
                                break;
                        }
                    }
                }
            }
            toolStripStatusLabel2.Text = "Виконуються...";
        }

        private void taskFinished(string coorSysItem, string cryptAlgItem, string multAlgItem, double time, OperationsCounter ops)
        {
            // Add info to dictionary
            tables1[coorSysItem][cryptAlgItem].Add(multAlgItem, ops);
            tables2[coorSysItem][cryptAlgItem].Add(multAlgItem, time);

            tasksFinished++;
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Виконано " + tasksFinished + " з " + TaskList.Count));
            statusStrip1.Invoke((MethodInvoker)(() => toolStripProgressBar1.Value = (int)((double)tasksFinished / TaskList.Count * 100)));
            if (tasksFinished == TaskList.Count)
            {
                statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = "Виконано " + tasksFinished + " з " + TaskList.Count));
                button11.Invoke((MethodInvoker)(() => button7.Enabled = true));
                button11.Invoke((MethodInvoker)(() => button11.Enabled = true));
                statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel2.Text = "Завершено"));
                MessageBox.Show("Всі обчислення завершено. Тепер ви можете експортувати дані для перегляду результатів",
                    "Інфо", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            exportTables1(tables1);
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            exportTables2(tables2);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string selectedCurve = comboBox2.Text;
            if (selectedCurve == "")
            {
                MessageBox.Show("Не обрано жодної кривої", "Інфо", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string docURI = curveURI[selectedCurve];
            if (docURI == "")
            {
                MessageBox.Show("Для даної кривої немає ресурсів", "Інфо", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Process.Start(docURI);
        }
    }
}
