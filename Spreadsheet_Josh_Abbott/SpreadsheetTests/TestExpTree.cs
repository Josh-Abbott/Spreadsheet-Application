// <copyright file="TestExpTree.cs" company="Josh Abbott">
// Copyright (c) Josh Abbott. All rights reserved.
// </copyright>

namespace SpreadsheetEngine.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SpreadsheetEngine;

    /// <summary>
    /// A concrete class to be able to test the expression tree.
    /// </summary>
    public class TestExpTree : ExpressionTree
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestExpTree"/> class.
        /// </summary>
        /// <param name="expression">The expression string.</param>
        public TestExpTree(string expression)
            : base(expression)
        {
        }
    }
}
