// <copyright file="Menu.cs" company="Josh Abbott">
// Copyright (c) Josh Abbott. All rights reserved.
// </copyright>

namespace SpreadsheetEngine.Tests
{
    /// <summary>
    /// A class for the test menu functionality of the expression tree.
    /// </summary>
    internal class Menu
    {
        private static void Main(string[] args)
        {
            RunMenu();
        }

        private static void PrintInfo(string curExp)
        {
            Console.WriteLine("---------- | MENU | ----------");
            Console.WriteLine("Current Expression: {0}", curExp);
            Console.WriteLine("1. Enter a new expression");
            Console.WriteLine("2. Set a variable value");
            Console.WriteLine("3. Evaluate tree");
            Console.WriteLine("4. Quit");
        }

        private static void RunMenu()
        {
            int selection = 0;
            string expression = "A1+B2+C3";
            TestExpTree tree = new TestExpTree(expression);

            do
            {
                PrintInfo(expression);
                if (int.TryParse(Console.ReadLine(), out selection))
                {
                    switch (selection)
                    {
                        case 1:
                            Console.WriteLine("Enter an expression: ");
                            string? inputExp = Console.ReadLine();
                            if (inputExp != null)
                            {
                                tree = new TestExpTree(inputExp);
                                expression = inputExp;
                            }

                            break;
                        case 2:
                            string? varName = string.Empty;
                            Console.WriteLine("Enter variable name: ");
                            varName = Console.ReadLine();
                            Console.WriteLine("Enter variable value: ");
                            int varVal;
                            if (int.TryParse(Console.ReadLine(), out varVal) && varName != null)
                            {
                                tree.SetVariable(varName, varVal);
                            }

                            break;
                        case 3:
                            Console.WriteLine("{0}", tree.Evaluate());
                            break;
                        case 4:
                            selection = 4;
                            break;
                        default:
                            break;
                    }
                }
            }
            while (selection != 4);
        }
    }
}