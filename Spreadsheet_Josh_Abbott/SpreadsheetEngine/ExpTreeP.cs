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
    /// <remarks>
    /// Initializes a new instance of the <see cref="ExpTreeP"/> class.
    /// </remarks>
    /// <param name="expression">The expression string.</param>
    public class ExpTreeP(string expression) : ExpressionTree(expression)
    {
    }
}
