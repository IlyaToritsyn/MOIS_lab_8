using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;

namespace MOIS_lab_8
{
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

        static void Main(string[] args)
        {
            Tree tree = new Tree();

            //string treeString = "Дерево: ";
            //string empty = "[Пусто.]";

            int commandNumber;
            int value;

            do
            {
                Console.Clear();

                Console.WriteLine("1. Добавить узел дерева.");
                Console.WriteLine("2. Вывести дерево на экран.");
                Console.WriteLine("3. Удалить узел дерева.\n");
                //Console.WriteLine("4. Задание по вариантам 1.");
                //Console.WriteLine("5. Задание по вариантам 2.\n");

                tree.OutputTreeConsole();

                commandNumber = InputInteger("Введите номер команды (1 - 3):");

                switch (commandNumber)
                {
                    case 1:
                        value = InputInteger("Введите значение нового узла:");

                        tree.Add(value);

                        Console.WriteLine("Узел со значением " + value + " добавлен.");

                        break;
                    case 2:
                        Console.Clear();

                        tree.OutputTreeConsole();

                        break;
                    case 3:
                        if (tree.Root == null)
                        {
                            Console.WriteLine("Нет дерева - удалять нечего.");

                            break;
                        }

                        value = InputInteger("Введите значение удаляемого узла:");

                        if (!tree.DeleteNode(value))
                        {
                            Console.WriteLine("Ничего не удалено - нет узла со значением " + value + ".");
                        }

                        else
                        {
                            Console.WriteLine("Узел со значением " + value + " удалён.");
                        }

                        break;
                    //case 4:
                    //    break;
                    //case 5:
                    //    break;
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
