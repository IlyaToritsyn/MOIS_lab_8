using System;

namespace ClassLibrary
{
    /// <summary>
    /// Класс дерева.
    /// </summary>
    public class Tree
    {
        /// <summary>
        /// Класс узла.
        /// </summary>
        public class Node
        {
            /// <summary>
            /// Значение узла.
            /// </summary>
            public int Value { set; get; }

            /// <summary>
            /// Узел-родитель.
            /// </summary>
            public Node Parent { set; get; }

            /// <summary>
            /// Левый наследник.
            /// </summary>
            public Node Left { set; get; }

            /// <summary>
            /// Правый наследник.
            /// </summary>
            public Node Right { set; get; }

            public Node(int value)
            {
                Value = value;
                Parent = null;
                Left = null;
                Right = null;
            }

            public Node()
            {
                Value = default;
                Parent = null;
                Left = null;
                Right = null;
            }

            /// <summary>
            /// Строковое представление узла.
            /// </summary>
            /// <returns>Строковое представление</returns>
            public override string ToString()
            {
                return "Родитель - корень - левый - правый: " + Parent.Value + " " + Value + " " + Left.Value + " " + Right.Value;
            }
        }

        /// <summary>
        /// Корень дерева.
        /// </summary>
        public Node Root { get; private set; } = null;

        /// <summary>
        /// Глубина дерева.
        /// </summary>
        public int Depth
        {
            get
            {
                if (Root == null)
                {
                    return 0;
                }

                int depth = 0;

                GoAround(Root, 0, ref depth);

                return depth;
            }
        }

        /// <summary>
        /// Строковое представление дерева. Реализуется путём обхода методом: левый - корень - правый.
        /// </summary>
        /// <returns>Строковое представление (обход: ЛКП)</returns>
        public override string ToString()
        {
            string str = "";

            GoAround(Root, ref str);

            return str;
        }

        /// <summary>
        /// Обход для получения глубины дерева.
        /// </summary>
        /// <param name="node">Текущий узел</param>
        /// <param name="currentDepth">Глубина дерева на данном узле</param>
        /// <param name="maxDepth">Макс. глубина дерева</param>
        private static void GoAround(Node node, int currentDepth, ref int maxDepth)
        {
            if (node != null)
            {
                currentDepth++;

                GoAround(node.Left, currentDepth, ref maxDepth); //Идём в левое поддерево.

                if (currentDepth > maxDepth)
                {
                    maxDepth = currentDepth;
                }

                GoAround(node.Right, currentDepth, ref maxDepth); //Идём в правое поддерево.
            }
        }

        /// <summary>
        /// Обход для получения строкового представления вида: левый - корень - правый.
        /// </summary>
        /// <param name="node">Текущий узел</param>
        /// <param name="str">Строковое представление дерева (ЛКП)</param>
        private static void GoAround(Node node, ref string str)
        {
            if (node != null)
            {
                GoAround(node.Left, ref str);

                str += node.Value + " ";

                GoAround(node.Right, ref str);
            }
        }

        private Node FindPrevious(int a)
        {
            Node q = Root;
            Node tmp = q;

            while (tmp != null)
            {
                q = tmp;

                tmp = tmp.Value > a ? tmp.Left : tmp.Right;
            }

            return q;
        }

        private Node Find(int value)
        {
            if (Root == null)
            {
                return null;
            }

            Node current = Root;

            while (current != null)
            {
                if (value > current.Value)
                {
                    current = current.Right;
                }

                else if (value < current.Value)
                {
                    current = current.Left;
                }

                else
                {
                    return current;
                }
            }

            return null;
        }

        public void Add(int a)
        {
            Node tmp = new Node(a);

            if (Root == null)
            {
                Root = tmp;
            }

            else
            {
                Node q = FindPrevious(a);

                if (tmp.Value < q.Value)
                {
                    q.Left = tmp;
                    tmp.Parent = q;
                }

                else
                {
                    q.Right = tmp;
                    tmp.Parent = q;
                }
            }
        }

        public bool DeleteNode(int value)
        {
            Node deletedNode = Find(value);

            if (deletedNode == null)
            {
                return false;
            }

            Node current;

            if (deletedNode == Root)
            {
                if (deletedNode.Right != null)
                {
                    current = Root.Right;
                    Root = current;
                }

                else if (deletedNode.Left != null)
                {
                    current = Root.Left;
                    Root = current;
                    current.Parent = null;

                    return true;
                }

                else
                {
                    Root = null;

                    return true;
                }

                while (current.Left != null)
                {
                    current = current.Left;
                }

                current.Left = Root.Parent.Left;

                if (Root.Parent.Left != null)
                {
                    Root.Parent.Left.Parent = current;
                }

                Root.Parent = null;

                return true;
            }

            if ((deletedNode.Left == null) && (deletedNode.Right == null))
            {
                if (deletedNode == deletedNode.Parent.Left)
                {
                    deletedNode.Parent.Left = null;
                }

                else
                {
                    deletedNode.Parent.Right = null;
                }

                return true;
            }

            if ((deletedNode.Left != null) && (deletedNode.Right == null))
            {
                deletedNode.Left.Parent = deletedNode.Parent;

                if (deletedNode == deletedNode.Parent.Left)
                {
                    deletedNode.Parent.Left = deletedNode.Left;
                }

                else
                {
                    deletedNode.Parent.Right = deletedNode.Left;
                }

                return true;
            }

            if ((deletedNode.Left == null) && (deletedNode.Right != null))
            {
                deletedNode.Right.Parent = deletedNode.Parent;

                if (deletedNode == deletedNode.Parent.Left)
                {
                    deletedNode.Parent.Left = deletedNode.Right;
                }

                else
                {
                    deletedNode.Parent.Right = deletedNode.Right;
                }

                return true;
            }

            if ((deletedNode.Left != null) && (deletedNode.Right != null))
            {
                current = deletedNode.Right;

                while (current.Left != null)
                {
                    current = current.Left;
                }

                if (current.Parent == deletedNode)
                {
                    current.Left = deletedNode.Left;
                    deletedNode.Left.Parent = current;
                    current.Parent = deletedNode.Parent;

                    if (deletedNode == deletedNode.Parent.Left)
                    {
                        deletedNode.Parent.Left = current;
                    }

                    else
                    {
                        deletedNode.Parent.Right = current;
                    }

                    return true;
                }

                else
                {
                    if (current.Right != null)
                    {
                        current.Right.Parent = current.Parent;
                    }

                    current.Parent.Left = current.Right;
                    current.Right = deletedNode.Right;
                    current.Left = deletedNode.Left;
                    deletedNode.Right.Parent = current;
                    deletedNode.Left.Parent = current;
                    current.Parent = deletedNode.Parent;

                    if (deletedNode == deletedNode.Parent.Left)
                    {
                        deletedNode.Parent.Left = current;
                    }

                    else
                    {
                        deletedNode.Parent.Right = current;
                    }

                    return true;
                }
            }

            return false;
        }

        public Node[][] GetArray()
        {
            if (Root == null)
            {
                return null;
            }

            Node[][] treeArray = new Node[Depth][];

            for (int i = 0; i < treeArray.GetLength(0); i++)
            {
                treeArray[i] = new Node[(int)Math.Pow(2, i)];

                if (i == 0)
                {
                    treeArray[0][0] = Root;
                }

                else
                {
                    for (int j = 0; j < treeArray[i - 1].Length; j++)
                    {
                        if (treeArray[i - 1][j] == null)
                        {
                            treeArray[i][((j + 1) * 2) - 2] = null;
                            treeArray[i][((j + 1) * 2) - 1] = null;
                        }

                        else
                        {
                            treeArray[i][((j + 1) * 2) - 2] = treeArray[i - 1][j].Left;
                            treeArray[i][((j + 1) * 2) - 1] = treeArray[i - 1][j].Right;
                        }
                    }
                }
            }

            return treeArray;
        }

        public void OutputTreeConsole()
        {   
            string margin = new string(' ', 30);

            Console.WriteLine(new string(' ', 27) + "Дерево:\n");

            if (Root == null)
            {
                Console.WriteLine(new string(' ', 26) + "[Пусто.]");
            }

            else
            {
                Node[][] treeArray = GetArray();

                
                string marginElement = new string(' ', 10);

                for (int i = 0; i < treeArray.GetLength(0); i++)
                {
                    if (margin.Length != 0)
                    {
                        if (i != 0)
                        {
                            margin = margin.Remove(margin.Length / 2);
                        }

                        Console.Write(margin);
                    }

                    if (marginElement.Length > 0)
                    {
                        marginElement = marginElement.Remove(marginElement.Length - 1);
                    }

                    for (int j = 0; j < treeArray[i].Length; j++)
                    {
                        if (treeArray[i][j] == null)
                        {
                            Console.Write(new string(' ', 2) + margin + margin);
                        }

                        else
                        {
                            Console.Write(treeArray[i][j].Value + margin + margin);
                        }
                    }

                    Console.WriteLine();
                }
            }

            Console.WriteLine();
        }
    }
}
