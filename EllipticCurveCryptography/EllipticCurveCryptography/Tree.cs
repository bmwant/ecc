using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace EllipticCurveCryptography
{
    class Tree
    {
        public Tree(BigInteger origin, BigInteger[] s, BigInteger[] m, BigInteger b)
        {
            S = s;
            M = m;
            B = b;
            RootNode = new Node(origin, null, s, m);
            bool stop = false;

            List<Node> currNodes = new List<Node>();
            currNodes.Add(RootNode);

            List<Node.ChildNode> currChilds = new List<Node.ChildNode>();
            foreach (var i in RootNode.Childrens)
            {
                currChilds.Add(i);
            }

            if (RootNode.Data == 1)
            {
                stop = true;
                EndNode = RootNode;
            }

            while (!stop)
            {
                List<Node.ChildNode> newChilds = new List<Node.ChildNode>();
                List<BigInteger> dataList = new List<BigInteger>();
                List<BigInteger> newDataList = new List<BigInteger>();
                BigInteger tmp = 0;

                for (BigInteger i = 0; i < currNodes.Count; ++i)
                {
                    foreach (var c in currChilds)
                    {
                        c.Node = new Node(c.Data, c.Parent, s, m);
                        if (c.Node.Data == 1)
                        {
                            stop = true;
                            EndNode = c.Node;
                            tmp = c.Node.Data;
                            break;
                        }

                        dataList.Add(c.Node.Data);
                        dataList.Sort();
                    }
                }


                if (tmp != 1)
                {
                    for (int j = 0; (j < b) && (j < dataList.Count); ++j)
                    {
                        newDataList.Add(dataList[j]);
                    }

                    foreach (BigInteger d in newDataList)
                    {
                        foreach (var c in currChilds)
                        {
                            if (c.Node.Data == d && c.Node.Data != 1)
                            {
                                foreach (var cc in c.Node.Childrens)
                                {
                                    newChilds.Add(cc);
                                }
                            }
                        }
                    }


                    //newChilds.Sort();
                    currChilds.Clear();

                    for (int i = 0; i < newChilds.Count; ++i)
                    {
                        currChilds.Add(newChilds[i]);
                    }
                }
            }
        }

        public void Print()
        {
            RootNode.Print(0);
        }



        Node RootNode;
        Node EndNode;
        BigInteger[] S;
        BigInteger[] M;
        BigInteger B;



        public List<DecompositonItem> GetDecomposition()
        {
            List<DecompositonItem> res = new List<DecompositonItem>();
            DecomposImpl(res, EndNode);
            return res;
        }

        public List<DecompositonItem> GetDecompositionForRL()
        {
            List<DecompositonItem> res = new List<DecompositonItem>();
            DecomposImpl(res, EndNode);
            DecompositonItem help = new DecompositonItem();
            res.ToArray();

            help.offset = res[res.Count - 1].offset;

            for (int j = res.Count - 1; j > 0; j--)
            {
                res[j].offset = res[j - 1].offset;
            }
            res[0].offset = help.offset;

            for (int i = 0; i < res.Count / 2; i++)
            {
                help = res[i];
                res[i] = res[res.Count - i - 1];
                res[res.Count - i - 1] = help;
            }
            res.ToList<DecompositonItem>();
            return res;
        }

        private void DecomposImpl(List<DecompositonItem> res, Node currNode)
        {
            if (currNode == null) return;

            DecompositonItem item = new DecompositonItem();
            item.pows = new BigInteger[M.Length, 2];

            for (int i = 0; i < M.Length; ++i)
                if (currNode.DataDivasor[i] != 0)
                {
                    item.pows[i, 0] = M[i];
                    item.pows[i, 1] = currNode.DataDivasor[i];
                }

            item.offset = 0;
            if (currNode.Parent != null)
            {
                foreach (var i in currNode.Parent.Childrens)
                    if (i.Node == currNode)
                    {
                        item.offset = -i.Offset;
                        break;
                    }
            }

            res.Add(item);
            DecomposImpl(res, currNode.Parent);
        }

        private void DecomposImplRL(List<DecompositonItem> res, Node currNode)
        {
            if (currNode == null) return;

            DecompositonItem item = new DecompositonItem();
            item.pows = new BigInteger[M.Length, 2];

            for (int i = 0; i < M.Length; ++i)
                if (currNode.DataDivasor[i] != 0)
                {
                    item.pows[i, 0] = M[i];
                    item.pows[i, 1] = currNode.DataDivasor[i];
                }

            item.offset = 0;
            if (currNode.Parent != null)
            {
                foreach (var i in currNode.Parent.Childrens)
                    if (i.Node == currNode)
                    {
                        item.offset = -i.Offset;
                        break;
                    }
            }

            res.Add(item);
            DecomposImpl(res, currNode.Parent);
        }

        public class DecompositonItem
        {
            public BigInteger[,] pows;
            public BigInteger offset;
        }
    }
}
