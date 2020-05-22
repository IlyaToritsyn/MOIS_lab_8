using System;
using System.Xml;

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

            if (Root != null)
            {
                GoAround(Root, ref str);
            }

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
            currentDepth++;

            if (node.Left != null)
            {
                GoAround(node.Left, currentDepth, ref maxDepth); //Идём в левое поддерево.
            }

            if (currentDepth > maxDepth)
            {
                maxDepth = currentDepth;
            }

            if (node.Right != null)
            {
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
            if (node.Left != null)
            {
                GoAround(node.Left, ref str);
            }

            str += node.Value + " ";

            if (node.Right != null)
            {
                GoAround(node.Right, ref str);
            }
        }

        /// <summary>
        /// Поиск узла перед узлом с заданным значением.
        /// </summary>
        /// <param name="nodeValue">Заданное значение узла</param>
        /// <returns>Узел перед узлом с заданныи значением</returns>
        private Node FindPrevious(int nodeValue)
        {
            Node current = Root;
            Node Next = current;

            while (Next != null)
            {
                current = Next;

                Next = Next.Value > nodeValue ? Next.Left : Next.Right; //Сдвиг по дереву ближе к заданному числу.
            }

            return current;
        }

        /// <summary>
        /// Поиск узла по заданному значению.
        /// </summary>
        /// <param name="nodeValue">Значение искомого узла</param>
        /// <returns>Узел с искомым значением; null - нет искомого узла</returns>
        private Node Find(int nodeValue)
        {
            if (Root == null)
            {
                return null;
            }

            Node current = Root;

            while (current != null)
            {
                if (nodeValue > current.Value)
                {
                    current = current.Right;
                }

                else if (nodeValue < current.Value)
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

        /// <summary>
        /// Добавление узла с заданным значением.
        /// </summary>
        /// <param name="nodeValue">Значение добавляемого узла</param>
        public void Add(int nodeValue)
        {
            Node addedNode = new Node(nodeValue);

            if (Root == null)
            {
                Root = addedNode;
            }

            else
            {
                Node current = FindPrevious(nodeValue);

                if (addedNode.Value < current.Value)
                {
                    current.Left = addedNode;
                    addedNode.Parent = current;
                }

                else
                {
                    current.Right = addedNode;
                    addedNode.Parent = current;
                }
            }
        }

        /// <summary>
        /// Удаление узла с заданным значением.
        /// </summary>
        /// <param name="nodeValue">Значение удаляемого узла</param>
        /// <returns>true - узел удалён; false - ничего не удалено</returns>
        public bool DeleteNode(int nodeValue)
        {
            if (Root == null)
            {
                throw new Exception("Дерево пустое - удалять нечего.");
            }

            Node deletedNode = Find(nodeValue);

            //Если узла с заданным значением нет, то ничего не удаляем.
            if (deletedNode == null)
            {
                return false;
            }

            Node current;

            //Если удаляем корень.
            if (deletedNode == Root)
            {
                //Если у корня есть правый наследник, то последний становится корнем.
                if (deletedNode.Right != null)
                {
                    current = Root.Right;
                    Root = current;
                }

                //Если у корня только левый наследник, то последний становится корнем.
                else if (deletedNode.Left != null)
                {
                    current = Root.Left;
                    Root = current;
                    current.Parent = null;

                    return true;
                }

                //Если у корня нет подузлов, то просто удаляем его.
                else
                {
                    Root = null;

                    return true;
                }

                while (current.Left != null)
                {
                    current = current.Left;
                }

                current.Left = Root.Parent.Left; //Самый левый узел (самое маленькое значение) правого поддерева становится родителем левого поддерева.

                //Если левое поддерево непустое, то "подвязываем" его к самому левому узлу правого поддерева (см. выше).
                if (Root.Parent.Left != null)
                {
                    Root.Parent.Left.Parent = current;
                }

                Root.Parent = null;

                return true;
            }

            //Если удаляем лист.
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

            //Если удаляем узел, имеющий только левое поддерево.
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

            //Если удаляем узел, имеющий только правое поддерево.
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

            //Если удаляем узел, имеющий оба поддерева.
            if ((deletedNode.Left != null) && (deletedNode.Right != null))
            {
                current = deletedNode.Right;

                while (current.Left != null)
                {
                    current = current.Left;
                }

                //Если у правого наследника удаляемого узла нет левых наследников, то он просто замещает собой удаляемый узел.
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

                //Если у правого наследника удаляемого узла есть левые наследники, то самый левый узел правого поддерева становится на место удаляемого.
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

        /// <summary>
        /// Получение ступенчатого массива, отражающего структуру дерева.
        /// </summary>
        /// <param name="treeArray">Массив для записи дерева</param>
        public void GetArray(out Node[][] treeArray)
        {
            if (Root == null)
            {
                treeArray = null;

                return;
            }

            treeArray = new Node[Depth][];

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
                        //Если нет родителя, то нет и наследников.
                        if (treeArray[i - 1][j] == null)
                        {
                            treeArray[i][((j + 1) * 2) - 2] = null;
                            treeArray[i][((j + 1) * 2) - 1] = null;
                        }

                        //Если родитель есть, то получаем наследников.
                        else
                        {
                            treeArray[i][((j + 1) * 2) - 2] = treeArray[i - 1][j].Left;
                            treeArray[i][((j + 1) * 2) - 1] = treeArray[i - 1][j].Right;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Вывод дерева в консоль.
        /// </summary>
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
                GetArray(out Node[][] treeArray);


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
