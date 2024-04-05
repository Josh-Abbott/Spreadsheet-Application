// <copyright file="ConstantNode.cs" company="Josh Abbott">
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
    /// A class for constants found in the expression.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ConstantNode"/> class.
    /// </remarks>
    /// <param name="value">The value of the constant.</param>
    public class ConstantNode(double value) : Node
    {
        private double value = value;

        /// <summary>
        /// Gets or sets for the value of the constant.
        /// </summary>
        public double Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        /// <summary>
        /// Evaluates the constant by simply returning the value of it.
        /// </summary>
        /// <returns>The value of the constant.</returns>
        public override double Evaluate()
        {
            return this.value;
        }
    }
}
