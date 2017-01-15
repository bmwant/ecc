using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace EllipticCurveCryptography
{
    class Node
    {
        public BigInteger Origin;
        public Node Parent;
        public BigInteger Data;
        public int[] DataDivasor;
        public List<ChildNode> Childrens = new List<ChildNode>();

        public class ChildNode : IComparable
        {
            public Node Parent;
            public BigInteger Offset;
            public BigInteger Data;
            public Node Node;

            public int CompareTo(object obj)
            {
                var other = obj as ChildNode;
                if (this.Data < other.Data) return -1;
                else if (this.Data > other.Data) return 1;
                else return 0;
            }
        }

        public Node(BigInteger origin, Node parent, BigInteger[] S, BigInteger[] M)
        {
            Origin = origin;
            Parent = parent;
            Data = Origin;
            DataDivasor = new int[M.Length];

            for (int i = 0; i < M.Length; )
            {
                if (Data % M[i] == 0)
                {
                    Data = Data / M[i];
                    ++DataDivasor[i];
                }
                else
                {
                    i++;
                }
            }

            if (Data == 1)
                return;

            for (Int16 i = 0; i < S.Length; i++)
            {
                ChildNode tmp = new ChildNode();
                if (Data > S[i])
                {
                    tmp.Parent = this;
                    tmp.Offset = -i - 1;
                    tmp.Data = Data - S[i];
                    Childrens.Add(tmp);
                }

                tmp = new ChildNode();
                tmp.Parent = this;
                tmp.Offset = i + 1;
                tmp.Data = Data + S[i];
                Childrens.Add(tmp);
            }

            /* foreach (BigInteger i in S)
             {
                 ChildNode tmp = new ChildNode();
                 tmp.Parent = this;
                 tmp.Offset = -i;
                 tmp.Data = Data - i;
                 Childrens.Add(tmp);

                 tmp = new ChildNode();
                 tmp.Parent = this;
                 tmp.Offset = i;
                 tmp.Data = Data + i;
                 Childrens.Add(tmp);
             }*/
        }

        public void Print(int indent)
        {
            string b = "";
            for (int i = 0; i < indent; ++i)
                b += " ";

            Console.Write(b + Origin + " ");
            foreach (var j in DataDivasor)
                Console.Write(j);
            Console.Write(" " + Data + "\n");

            foreach (var c in Childrens)
            {
                Console.Write(b + "  " + c.Offset + " " + c.Data + "\n");
                if (c.Node != null)
                    c.Node.Print(indent + 4);
            }
        }
    }
}
