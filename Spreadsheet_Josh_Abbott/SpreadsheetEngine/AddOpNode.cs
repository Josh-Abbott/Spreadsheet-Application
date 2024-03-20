// <copyright file="AddOpNode.cs" company="Josh Abbott">
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
    /// A class for the addition functionality of the operator node.
    /// </summary>
    public class AddOpNode : OperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddOpNode"/> class.
        /// </summary>
        /// <param name="op">The operator character.</param>
        public AddOpNode(char op)
            : base(op)
        {
            this.Precedence = 1;
        }

        /// <summary>
        /// A function to perform the addition operation on the numbers.
        /// </summary>
        /// <returns>The result of the operation.</returns>
        public override double Evaluate()
        {
            if (this.Left != null && this.Right != null)
            {
                return this.Left.Evaluate() + this.Right.Evaluate();
            }
            else
            {
                throw new Exception("Not a valid expression!");
            }
        }
    }
}
