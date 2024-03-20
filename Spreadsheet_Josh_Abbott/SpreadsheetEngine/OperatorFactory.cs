// <copyright file="OperatorFactory.cs" company="Josh Abbott">
// Copyright (c) Josh Abbott. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// A Factory pattern class designed to create the appropriate type of OperatorNode for the expression tree.
    /// </summary>
    public class OperatorFactory
    {
        /// <summary>
        /// Determines the correct type of node based on the operator.
        /// </summary>
        /// <returns>The appropriate type of OperatorNode..</returns>
        /// <param name="op">The operator character.</param>
        public static OperatorNode CreateOperatorNode(char op)
        {
            switch (op)
            {
                case '+': return new AddOpNode(op);
                case '-': return new SubtractOpNode(op);
                case '*': return new MultiplyOpNode(op);
                case '/': return new DivideOpNode(op);
                default: throw new NotSupportedException("This operator is not supported!");
            }
        }
    }
}
