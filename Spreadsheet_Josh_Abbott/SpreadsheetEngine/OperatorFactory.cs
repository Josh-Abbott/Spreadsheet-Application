// <copyright file="OperatorFactory.cs" company="Josh Abbott">
// Copyright (c) Josh Abbott. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Design;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// A Factory pattern class designed to create the appropriate type of OperatorNode for the expression tree.
    /// </summary>
    public class OperatorFactory
    {
        private static readonly Dictionary<char, Func<char, OperatorNode>> RegisteredOperators = new Dictionary<char, Func<char, OperatorNode>>();

        /// <summary>
        /// Initializes static members of the <see cref="OperatorFactory"/> class. It also registers all existing operators into the system.
        /// </summary>
        static OperatorFactory()
        {
            RegisterOperator('+', op => new AddOpNode(op));
            RegisterOperator('-', op => new SubtractOpNode(op));
            RegisterOperator('*', op => new MultiplyOpNode(op));
            RegisterOperator('/', op => new DivideOpNode(op));
        }

        /// <summary>
        /// Determines the correct type of node based on the operator.
        /// </summary>
        /// <returns>The appropriate type of OperatorNode..</returns>
        /// <param name="op">The operator character.</param>
        public static OperatorNode CreateOperatorNode(char op)
        {
            if (RegisteredOperators.TryGetValue(op, out var creator))
            {
                return creator(op);
            }
            else
            {
                throw new NotSupportedException("This operator is not supported!");
            }
        }

        /// <summary>
        /// Registers operators supported in the program to be used later.
        /// </summary>
        /// <param name="op">The operator character.</param>
        /// <param name="creator">.</param>

        private static void RegisterOperator(char op, Func<char, OperatorNode> creator)
        {
            RegisteredOperators.Add(op, creator);
        }
    }
}
