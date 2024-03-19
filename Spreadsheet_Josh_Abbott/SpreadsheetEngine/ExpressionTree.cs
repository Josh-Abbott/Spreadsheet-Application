// <copyright file="ExpressionTree.cs" company="Josh Abbott">
// Copyright (c) Josh Abbott. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography.X509Certificates;

    /// <summary>
    /// An abstract class to contain the expression tree and all relevant variables and functions.
    /// </summary>
    public abstract class ExpressionTree
    {
        private Node? rootNode;
        private string expression;
        private Dictionary<string, double> vars = new Dictionary<string, double>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
        /// </summary>
        /// <param name="expression">A string representing the expression.</param>
        public ExpressionTree(string expression)
        {
            this.expression = expression;
            this.rootNode = this.ConstructTree(expression);
        }

        /// <summary>
        /// Gets or sets for the expression string.
        /// </summary>
        public string Expression
        {
            get
            {
                return this.expression;
            }

            set
            {
                this.expression = value;
            }
        }

        /// <summary>
        /// A function to change the variable values based on the name.
        /// </summary>
        /// <param name="variableName">The name of the variable.</param>
        /// <param name="variableValue">The value of the variable.</param>
        public void SetVariable(string variableName, double variableValue)
        {
            this.vars[variableName] = variableValue;
        }

        /// <summary>
        /// A public Evaluate() function with a default of 0 that can redirect.
        /// </summary>
        /// <returns>The result of the evaluation.</returns>
        public double Evaluate()
        {
            return this.rootNode?.Evaluate() ?? 0;
        }

        private static bool IsOperator(char ch)
        {
            return ch == '+' || ch == '-' || ch == '*' || ch == '/';
        }

        private static bool HasHigherPrecedence(char op1, char op2)
        {
            int precedence1 = (op1 == '*' || op1 == '/') ? 2 : 1;
            int precedence2 = (op2 == '*' || op2 == '/') ? 2 : 1;
            return precedence1 >= precedence2;
        }

        private static Node PopAndEvaluate(Stack<char> operators, Stack<Node> operands)
        {
            char op = operators.Pop();
            Node right = operands.Pop();
            Node left = operands.Pop();
            switch (op)
            {
                case '+':
                    return new AddOpNode(op) { Left = left, Right = right };
                case '-':
                    return new SubtractOpNode(op) { Left = left, Right = right };
                case '*':
                    return new MultiplyOpNode(op) { Left = left, Right = right };
                case '/':
                    return new DivideOpNode(op) { Left = left, Right = right };
                default:
                    throw new Exception("Invalid operator");
            }
        }

        private Node? ConstructTree(string expression)
        {
            Stack<Node> operands = new Stack<Node>();
            Stack<char> operators = new Stack<char>();

            // Loop through the length of the expression to identify each component and construct the tree.
            for (int i = 0; i < expression.Length; i++)
            {
                char ch = expression[i];

                // If it's a white space character, ignore it
                if (char.IsWhiteSpace(ch))
                {
                    continue;
                }

                // Check if it's an unsupported operator
                if (!char.IsNumber(ch) && !char.IsLetter(ch) && !IsOperator(ch) && !char.IsWhiteSpace(ch))
                {
                    throw new NotSupportedException("Unsupported operator!");
                }

                // Check to see if the character is an operator
                if (IsOperator(ch))
                {
                    // If the character is an operator, check precedence via a loop
                    while (operators.Count > 0 && IsOperator(operators.Peek()) && HasHigherPrecedence(ch, operators.Peek()))
                    {
                        operands.Push(PopAndEvaluate(operators, operands));
                    }

                    operators.Push(ch);
                }

                // Check to see if the character is a number
                else if (char.IsNumber(ch))
                {
                    int start = i;

                    // Loop until the end of the number
                    while (i < expression.Length && char.IsNumber(expression[i]))
                    {
                        i++;
                    }

                    double value = double.Parse(expression.Substring(start, i - start));

                    // Create a new ConstantNode to store the number
                    operands.Push(new ConstantNode(value));
                    i--;
                }

                // Check to see if the character is a letter
                else if (char.IsLetter(ch))
                {
                    // Get the variable name from the letter
                    string varName = string.Empty;
                    while (i < expression.Length && char.IsLetterOrDigit(expression[i]))
                    {
                        varName += expression[i];
                        i++;
                    }

                    i--;

                    // Create a new VariableNode to store the letter
                    operands.Push(new VariableNode(varName, this.vars));
                }
            }

            // Process any remaining operators
            while (operators.Count > 0)
            {
                operands.Push(PopAndEvaluate(operators, operands));
            }

            return operands.Pop();
        }
    }
}
