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
        /// Вывод пути в консоль.
        /// </summary>
        /// <param name="list">Список-путь со значениями узлов</param>
        public static void Output(List<int> list)
        {
            foreach (int element in list)
            {
                Console.Write(element + " ");
            }

            Console.WriteLine();
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
                        if (tree.Depth == 0)
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
                            tree.GetMinMaxWay(out List<int>[] minMaxWay);

                            if (minMaxWay[0] != null)
                            {
                                Console.Write("Мин. путь  (длина " + (minMaxWay[0].Count - 1) + "): ");

                                Output(minMaxWay[0]);

                                Console.Write("Макс. путь (длина " + (minMaxWay[1].Count - 1) + "): ");

                                Output(minMaxWay[1]);
                            }

                            else
                            {
                                Console.WriteLine("Только 1 лист - нет ни одного пути.");
                            }

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
                            tree.ToArray(out int?[][] treeArray);

                            tree.AddToTree();

                            //Если дерево было неполным, то оно должно было дополниться единицами. Сообщаем пользователю об успехе дополнения.
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
