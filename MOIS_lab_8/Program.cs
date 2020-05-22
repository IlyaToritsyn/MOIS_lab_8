using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibrary;

namespace MOIS_lab_8
{
    //15. В созданном дереве найти минимальный и максимальный пути (по длине) между листьями бинарного дерева.

    //23. В созданном бинарном дереве, не изменяя его глубины, дополнить дерево до полного.
    //Значение всех добавляемых вершин должно быть равно 1.
    class Program
    {
        /// <summary>
        /// Ввод целого числа с клавиатуры.
        /// </summary>
        /// <param name="message">Сообщение для ввода</param>
        /// <returns>Целое число, введённое с клавиатуры</returns>
        public static int InputInteger(string message)
        {
            bool isParsed = false;

            int N = 0;

            while (!isParsed)
            {
                Console.WriteLine(message);

                isParsed = int.TryParse(Console.ReadLine(), out N);

                Console.WriteLine();
            }

            return N;
        }

        /// <summary>
        /// Получение левого листа дерева.
        /// </summary>
        /// <param name="tree">Обрабатываемое дерево</param>
        /// <returns>Левый узел-лист</returns>
        public static Tree.Node GetLeftLeaf(Tree tree)
        {
            if (tree.Root == null)
            {
                throw new Exception("Дерево пусто - нет листов.");
            }

            Tree.Node current = tree.Root;

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
        private static Tree.Node GetLeftLeaf(List<Tree.Node> currentWay, Tree.Node current)
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
        private static void FindWaysToLeaves(List<Tree.Node>[] minMaxWays, Tree.Node current)
        {
            List<Tree.Node> currentWay = new List<Tree.Node>();

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
                        minMaxWays[0] = new List<Tree.Node>(currentWay);
                        minMaxWays[1] = new List<Tree.Node>(currentWay);
                    }

                    else
                    {
                        if (currentWay.Count < minMaxWays[0].Count)
                        {
                            minMaxWays[0] = new List<Tree.Node>(currentWay);
                        }

                        if (currentWay.Count > minMaxWays[1].Count)
                        {
                            minMaxWays[1] = new List<Tree.Node>(currentWay);
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
        /// <param name="tree">Обрабатываемое дерево</param>
        /// <param name="minMaxWay">Мин. и макс. пути между листьями</param>
        public static void GetMinMaxWay(Tree tree, out List<Tree.Node>[] minMaxWay)
        {
            Tree.Node currentLeaf = GetLeftLeaf(tree);

            minMaxWay = new List<Tree.Node>[2];

            if (currentLeaf == tree.Root)
            {
                throw new Exception("Лист только 1 - нет путей.");
            }

            FindWaysToLeaves(minMaxWay, currentLeaf);
        }

        /// <summary>
        /// Вывод пути в консоль.
        /// </summary>
        /// <param name="list">Список-путь с узлами</param>
        public static void Output(List<Tree.Node> list)
        {
            foreach (Tree.Node element in list)
            {
                Console.Write(element.Value + " ");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Рекурсивное дополнение поддерева единицами.
        /// </summary>
        /// <param name="current">Текущий узел</param>
        /// <param name="currentDepth">Текущая глубина дерева</param>
        /// <param name="maxDepth">Макс. глубина дерева</param>
        private static void GoAroundAdding(Tree.Node current, int currentDepth, int maxDepth)
        {
            currentDepth++;

            //Если нет левого сына и глубина дерева не макс., то добавляем левого сына со значением 1.
            if (current.Left == null && currentDepth < maxDepth)
            {
                current.Left = new Tree.Node(1)
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
                current.Right = new Tree.Node(1)
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
        /// <param name="tree">Дополняемое дерево</param>
        public static void AddToTree(Tree tree)
        {
            if (tree.Root == null)
            {
                throw new Exception("Дерево пусто - дополнять нечего.");
            }

            GoAroundAdding(tree.Root, 0, tree.Depth);
        }

        static void Main()
        {
            Tree tree = new Tree();

            int commandNumber;
            int nodeValue;

            do
            {
                Console.Clear();

                Console.WriteLine("1. Добавить узел дерева.");
                Console.WriteLine("2. Вывести дерево на экран.");
                Console.WriteLine("3. Удалить узел дерева.\n");
                Console.WriteLine("4. Найти мин. и макс. пути (по длине) между листьями.");
                Console.WriteLine("5. Дополнить дерево единицами.\n");

                tree.OutputTreeConsole();

                commandNumber = InputInteger("Введите номер команды (1 - 5):");
                
                switch (commandNumber)
                {
                    case 1:
                        nodeValue = InputInteger("Введите значение нового узла:");

                        tree.Add(nodeValue);

                        Console.WriteLine("Узел со значением " + nodeValue + " добавлен.");

                        break;
                    case 2:
                        Console.Clear();

                        tree.OutputTreeConsole();

                        break;
                    case 3:
                        if (tree.Root == null)
                        {
                            Console.WriteLine("Дерево пустое - удалять нечего.");

                            break;
                        }

                        nodeValue = InputInteger("Введите значение удаляемого узла:");

                        if (!tree.DeleteNode(nodeValue))
                        {
                            Console.WriteLine("Нет узла со значением " + nodeValue + " - ничего не удалено.");
                        }

                        else
                        {
                            Console.WriteLine("Узел со значением " + nodeValue + " удалён.");
                        }

                        break;
                    case 4:
                        try
                        {
                            GetMinMaxWay(tree, out List<Tree.Node>[] minMaxWay);

                            Console.Write("Мин. путь  (длина " + (minMaxWay[0].Count - 1) + "): ");

                            Output(minMaxWay[0]);

                            Console.Write("Макс. путь (длина " + (minMaxWay[1].Count - 1) + "): ");

                            Output(minMaxWay[1]);

                            System.Threading.Thread.Sleep(10000);
                        }

                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        break;
                    case 5:
                        try
                        {
                            tree.GetArray(out Tree.Node[][] treeArray);

                            AddToTree(tree);

                            if (treeArray[treeArray.GetLength(0) - 1].Contains(null))
                            {
                                Console.WriteLine("Дерево успешно дополнено единицами.");
                            }

                            else
                            {
                                Console.WriteLine("Дерево уже полное - ничего не добавлено.");
                            }
                        }

                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        break;
                    default:
                        Console.WriteLine("Нет такой команды.");

                        break;
                }

                System.Threading.Thread.Sleep(2000);
            }
            while (true);
        }
    }
}
