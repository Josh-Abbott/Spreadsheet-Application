// <copyright file="ParenthesisOpNode.cs" company="Josh Abbott">
// Copyright (c) Josh Abbott. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// A class for the parenthesis functionality of the operator node.
    /// </summary>
    public class ParenthesisOpNode : OperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParenthesisOpNode"/> class.
        /// </summary>
        /// <param name="op">The operator character.</param>
        public ParenthesisOpNode(char op)
                   : base(op)
        {
        }

        /// <summary>
        /// Inherit the abstract evaluate function, although it is not applicable here.
        /// </summary>
        /// <returns>The result of the evaluation.</returns>
        /// <exception cref="NotImplementedException">In the event this is ever called, throw that it is not implemented.</exception>
        public override double Evaluate()
        {
            throw new NotImplementedException();
        }
    }
}
