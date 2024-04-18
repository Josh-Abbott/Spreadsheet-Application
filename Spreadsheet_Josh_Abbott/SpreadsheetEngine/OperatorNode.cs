// <copyright file="OperatorNode.cs" company="Josh Abbott">
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
    /// A class for the operators found in the expression.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="OperatorNode"/> class.
    /// </remarks>
    /// <param name="op">The operator character.</param>
    public abstract class OperatorNode(char op) : Node
    {
        private char? operation = op;
        private Node? left = null;
        private Node? right = null;

        /// <summary>
        /// Gets or sets the assigned precedence value for each operation.
        /// </summary>
        public int Precedence { get; set; }

        /// <summary>
        /// Gets or sets for the operation character.
        /// </summary>
        public char? Operation
        {
            get { return this.operation.HasValue ? (char)this.operation : default(char?); }
            set { this.operation = value; }
        }

        /// <summary>
        /// Gets or sets for the left node of the operator.
        /// </summary>
        public Node? Left
        {
            get { return this.left; }
            set { this.left = value; }
        }

        /// <summary>
        /// Gets or sets for the right node of the operator.
        /// </summary>
        public Node? Right
        {
            get { return this.right; }
            set { this.right = value; }
        }

        /// <summary>
        /// An abstract class for the evaluation of each type of operation that can be performed.
        /// </summary>
        /// <returns>The result of the operation.</returns>
        public abstract override double Evaluate();
    }
}
