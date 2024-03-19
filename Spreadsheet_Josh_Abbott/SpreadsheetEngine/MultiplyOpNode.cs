// <copyright file="MultiplyOpNode.cs" company="Josh Abbott">
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
    /// A class for the multiplication functionality of the operator node.
    /// </summary>
    public class MultiplyOpNode : OperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiplyOpNode"/> class.
        /// </summary>
        /// <param name="op">The operator character.</param>
        public MultiplyOpNode(char op)
            : base(op)
        {
        }

        /// <summary>
        /// A function to perform the multiplication operation on the numbers.
        /// </summary>
        /// <returns>The result of the operation.</returns>
        public override double Evaluate()
        {
            if (this.Left != null && this.Right != null)
            {
                return this.Left.Evaluate() * this.Right.Evaluate();
            }
            else
            {
                throw new Exception("Not a valid expression!");
            }
        }
    }
}
