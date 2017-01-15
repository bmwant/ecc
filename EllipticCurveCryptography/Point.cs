using System.Numerics;

namespace EllipticCurveCryptography
{
    public class Point
    {
        public BigInteger X { get; set; }
        public BigInteger Y { get; set; }

        public Point(BigInteger x, BigInteger y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}