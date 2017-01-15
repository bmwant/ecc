using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace EllipticCurveCryptography
{
    class Quintuple
    {
        public static void Quintuple_Affine_Coord(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger p, out BigInteger x3, out BigInteger y3, out BigInteger z3)
        {
            BigInteger x2, y2, z2;
            PointMultiplication.Double_Affine_Coord(x1, y1, z1, a, p, out x2, out y2, out z2);
            PointMultiplication.Double_Affine_Coord(x2, y2, z2, a, p, out x2, out y2, out z2);
            PointMultiplication.Add_Affine_Coord(x1, y1, z1, x2, y2, z2, a, p, out x3, out y3, out z3);
        }
        public static void Quintuple_Projective_Coord(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger p, out BigInteger x3, out BigInteger y3, out BigInteger z3)
        {
            BigInteger x2, y2, z2;
            PointMultiplication.Double_Projective_Coord(x1, y1, z1, a, p, out x2, out y2, out z2);
            PointMultiplication.Double_Projective_Coord(x2, y2, z2, a, p, out x2, out y2, out z2);
            PointMultiplication.Add_Projective_Coord(x1, y1, z1, x2, y2, z2, a, p, out x3, out y3, out z3);
        }
        public static void Quintuple_Jacobi_Coord(BigInteger x1, BigInteger y1, BigInteger z1, BigInteger a, BigInteger p, out BigInteger x3, out BigInteger y3, out BigInteger z3)
        {
            BigInteger x2, y2, z2;
            PointMultiplication.Double_Jacoby_Coord(x1, y1, z1, a, p, out x2, out y2, out z2);
            PointMultiplication.Double_Jacoby_Coord(x2, y2, z2, a, p, out x2, out y2, out z2);
            PointMultiplication.Add_Jacoby_Coord(x1, y1, z1, x2, y2, z2, a, p, out x3, out y3, out z3);
        }
    }
}
