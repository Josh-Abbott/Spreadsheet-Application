// <copyright file="ExpressionTree.cs" company="Josh Abbott">
// Copyright (c) Josh Abbott. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System.Data;
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
            this.vars = new Dictionary<string, double>();
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

        /// <summary>
        /// Uses the operator to create an appropriate node for it.
        /// </summary>
        /// <returns>The newly created type of OperatorNode.</returns>
        private static OperatorNode CreateOperatorNode(char op)
        {
            OperatorFactory factory = new OperatorFactory();
            return OperatorFactory.CreateOperatorNode(op);
        }

        /// <summary>
        /// Pops the operands from the expression tree and evaluates them.
        /// </summary>
        /// <returns>The evaluated OperatorNode.</returns>
        private static OperatorNode PopAndEvaluate(Stack<OperatorNode> operators, Stack<Node> operands)
        {
            OperatorNode op = operators.Pop();
            Node right = operands.Pop();
            Node left = operands.Pop();
            op.Right = right;
            op.Left = left;

            return op;
        }

        /// <summary>
        /// Construct the expression tree based off of the input expression.
        /// </summary>
        /// <returns>The new root node in the expression tree.</returns>
        private Node? ConstructTree(string expression)
        {
            Stack<Node> operands = new Stack<Node>();
            Stack<OperatorNode> operators = new Stack<OperatorNode>();

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
                if (!char.IsNumber(ch) && !char.IsLetter(ch) && !OperatorFactory.RegisteredOperators.ContainsKey(ch) && !char.IsWhiteSpace(ch))
                {
                    throw new NotSupportedException("Unsupported operator!");
                }

                if (ch == '(') // Push opening parenthesis to indicate start
                {
                    operators.Push(new ParenthesisOpNode('('));
                }
                else if (ch == ')')
                {
                    // Process operators that exist within the parentheses
                    while (operators.Peek().Operation != '(')
                    {
                        operands.Push(PopAndEvaluate(operators, operands));

                        // Determine if there is an issue with the expression.
                        if (operators.Count == 0)
                        {
                            throw new InvalidExpressionException("Invalid expression!");
                        }
                    }

                    operators.Pop(); // Remove the opening parenthesis
                }
                else if (OperatorFactory.RegisteredOperators.ContainsKey(ch)) // Check to see if the character is an operator
                {
                    // Determine precedence
                    OperatorNode opNode = CreateOperatorNode(ch);

                    while (operators.Count > 0 && operators.Peek().Precedence >= opNode.Precedence)
                    {
                        operands.Push(PopAndEvaluate(operators, operands));
                    }

                    operators.Push(opNode);
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

                    if (!this.vars.ContainsKey(varName))
                    {
                        this.vars.Add(varName, 0);
                    }
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
