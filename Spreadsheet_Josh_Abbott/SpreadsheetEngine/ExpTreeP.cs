// <copyright file="ExpTreeP.cs" company="Josh Abbott">
// Copyright (c) Josh Abbott. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SpreadsheetEngine;

    /// <summary>
    /// A concrete class to be able to make use of the abstract expression tree.
    /// </summary>
    public class ExpTreeP : ExpressionTree
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpTreeP"/> class.
        /// </summary>
        /// <param name="expression">The expression string.</param>
        public ExpTreeP(string expression)
            : base(expression)
        {
        }
    }
}
