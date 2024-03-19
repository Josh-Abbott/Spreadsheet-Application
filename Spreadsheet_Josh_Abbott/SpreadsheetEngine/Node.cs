// <copyright file="Node.cs" company="Josh Abbott">
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
    /// An abstract class for the various types of nodes.
    /// </summary>
    public abstract class Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        public Node()
        {
        }

        /// <summary>
        /// An abstract evaluate method for each type of node.
        /// </summary>
        /// <returns>The result of the evaluation.</returns>
        public abstract double Evaluate();
    }
}
