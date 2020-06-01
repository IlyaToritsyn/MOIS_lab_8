using System;
using System.Collections.Generic;

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
        private class Node
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
        private Node Root { get; set; } = null;

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
        /// <param name="nodeArray">Массив для записи дерева</param>
        private void ToArray(out Node[][] nodeArray)
        {
            //if (Root == null)
            //{
            //    treeArray = null;

            //    return;
            //}

            nodeArray = new Node[Depth][];

            for (int i = 0; i < nodeArray.GetLength(0); i++)
            {
                nodeArray[i] = new Node[(int)Math.Pow(2, i)];

                if (i == 0)
                {
                    nodeArray[0][0] = Root;
                }

                else
                {
                    for (int j = 0; j < nodeArray[i - 1].Length; j++)
                    {
                        //Если нет родителя, то нет и наследников.
                        if (nodeArray[i - 1][j] == null)
                        {
                            nodeArray[i][((j + 1) * 2) - 2] = null;
                            nodeArray[i][((j + 1) * 2) - 1] = null;
                        }

                        //Если родитель есть, то получаем наследников.
                        else
                        {
                            nodeArray[i][((j + 1) * 2) - 2] = nodeArray[i - 1][j].Left;
                            nodeArray[i][((j + 1) * 2) - 1] = nodeArray[i - 1][j].Right;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Перевод дерева в массив значений.
        /// </summary>
        /// <param name="valueArray">Массив значений для записи</param>
        public void ToArray(out int?[][] valueArray)
        {
            if (Root == null)
            {
                valueArray = null;

                return;
            }

            ToArray(out Node[][] nodeArray);

            valueArray = new int?[nodeArray.GetLength(0)][];

            for (int i = 0; i < valueArray.GetLength(0); i++)
            {
                valueArray[i] = new int?[nodeArray[i].Length];

                for (int j = 0; j < nodeArray[i].Length; j++)
                {
                    if (nodeArray[i][j] == null)
                    {
                        valueArray[i][j] = null;
                    }

                    else
                    {
                        valueArray[i][j] = nodeArray[i][j].Value;
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
                ToArray(out int?[][] treeArray);


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
                            Console.Write(treeArray[i][j] + margin + margin);
                        }
                    }

                    Console.WriteLine();
                }
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Получение левого листа дерева.
        /// </summary>
        /// <returns>Левый узел-лист</returns>
        private Node GetLeftLeaf()
        {
            if (Root == null)
            {
                throw new Exception("Дерево пусто - нет листов.");
            }

            Node current = Root;

            //Пока не дошли до листа, спускаемся вниз по дереву.
            while (current.Left != null || current.Right != null)
            {
                if (current.Left != null)
                {
                    current = current.Left;

                    continue;
                }

                current = current.Right;
            }

            return current;
        }

        /// <summary>
        /// Получение левого листа дерева с записью пути.
        /// </summary>
        /// <param name="currentWay">Текущий путь</param>
        /// <param name="current">Текущий узел</param>
        /// <returns>Левый узел-лист</returns>
        private Node GetLeftLeaf(List<Node> currentWay, Node current)
        {
            //Пока не дошли до листа, спускаемся вниз по дереву.
            while (current.Left != null || current.Right != null)
            {
                if (current.Left != null)
                {
                    current = current.Left;

                    currentWay.Add(current);

                    continue;
                }

                current = current.Right;

                currentWay.Add(current);
            }

            return current;
        }

        /// <summary>
        /// Перебор всех путей от текущего листа до др. листьев правее текущего.
        /// </summary>
        /// <param name="minMaxWays">Мин. и макс. пути</param>
        /// <param name="current">Текущий узел</param>
        private void FindWaysToLeaves(List<int>[] minMaxWays, Node current)
        {
            List<Node> currentWay = new List<Node>();

            bool isNextLeafActivated = false;

            currentWay.Add(current);

            //Пока не перебраны все пути от текущего листа до др. листьев правее, продолжаем перебирать оставшиеся пути.
            while ((current.Parent.Parent != null) || ((current.Parent.Left == current) && (current.Parent.Right != null)))
            {
                //Если мы поднимаемся к родителю слева и есть возможность перейти к правому брату, то переходим к нему.
                if ((current.Parent.Left == current) && (current.Parent.Right != null))
                {
                    if (currentWay.Contains(current.Parent))
                    {
                        currentWay.RemoveAt(currentWay.Count - 1);
                    }

                    else
                    {
                        currentWay.Add(current.Parent);
                    }

                    current = current.Parent.Right;

                    currentWay.Add(current);

                    current = GetLeftLeaf(currentWay, current);

                    if (!isNextLeafActivated)
                    {
                        FindWaysToLeaves(minMaxWays, current);

                        isNextLeafActivated = true;
                    }

                    if (minMaxWays[0] == null)
                    {
                        minMaxWays[0] = new List<int>(currentWay.Count);
                        minMaxWays[1] = new List<int>(currentWay.Count);

                        foreach (Node node in currentWay)
                        {
                            minMaxWays[0].Add(node.Value);
                            minMaxWays[1].Add(node.Value);
                        }
                    }

                    else
                    {
                        if (currentWay.Count < minMaxWays[0].Count)
                        {
                            minMaxWays[0].Clear();

                            foreach (Node node in currentWay)
                            {
                                minMaxWays[0].Add(node.Value);
                            }
                        }

                        if (currentWay.Count > minMaxWays[1].Count)
                        {
                            minMaxWays[1].Clear();

                            foreach (Node node in currentWay)
                            {
                                minMaxWays[1].Add(node.Value);
                            }
                        }
                    }
                }

                //Если мы поднимаемся к родителю справа или мы поднимаемся слева и вместе с тем нет возможности перейти к правому брату, то переходим к родителю.
                else
                {
                    if (currentWay.Contains(current.Parent))
                    {
                        currentWay.RemoveAt(currentWay.Count - 1);
                    }

                    else
                    {
                        currentWay.Add(current.Parent);
                    }

                    current = current.Parent;
                }
            }
        }

        /// <summary>
        /// Нахождение мин. и макс. путей (по длине) между листьями.
        /// </summary>
        /// <param name="minMaxWay">Мин. и макс. пути между листьями</param>
        public void GetMinMaxWay(out List<int>[] minMaxWay)
        {
            Node currentLeaf = GetLeftLeaf();

            minMaxWay = new List<int>[2];

            if (currentLeaf == Root)
            {
                throw new Exception("Лист только 1 - нет путей.");
            }

            FindWaysToLeaves(minMaxWay, currentLeaf);
        }

        /// <summary>
        /// Рекурсивное дополнение поддерева единицами.
        /// </summary>
        /// <param name="current">Текущий узел</param>
        /// <param name="currentDepth">Текущая глубина дерева</param>
        /// <param name="maxDepth">Макс. глубина дерева</param>
        private void GoAroundAdding(Node current, int currentDepth, int maxDepth)
        {
            currentDepth++;

            //Если нет левого сына и глубина дерева не макс., то добавляем левого сына со значением 1.
            if (current.Left == null && currentDepth < maxDepth)
            {
                current.Left = new Node(1)
                {
                    Parent = current
                };
            }

            if (current.Left != null)
            {
                GoAroundAdding(current.Left, currentDepth, maxDepth); //Идём в левое поддерево.
            }

            //Если нет правого сына и глубина дерева не макс., то добавляем правого сына со значением 1.
            if (current.Right == null && currentDepth < maxDepth)
            {
                current.Right = new Node(1)
                {
                    Parent = current
                };
            }

            if (current.Right != null)
            {
                GoAroundAdding(current.Right, currentDepth, maxDepth); //Идём в правое поддерево.
            }
        }

        /// <summary>
        /// Дополнение дерева.
        /// </summary>
        public void AddToTree()
        {
            if (Root == null)
            {
                throw new Exception("Дерево пусто - дополнять нечего.");
            }

            GoAroundAdding(Root, 0, Depth);
        }
    }
}
