// <copyright file="VariableNode.cs" company="Josh Abbott">
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
    /// A class for the variable node type.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="VariableNode"/> class.
    /// </remarks>
    /// <param name="name">The variable name.</param>
    /// <param name="variables">The list of variables.</param>
    public class VariableNode(string name, Dictionary<string, double> variables) : Node
    {
        private string varName = name;
        private Dictionary<string, double> vars = variables;

        /// <summary>
        /// Gets or sets for the variable name.
        /// </summary>
        public string VarName
        {
            get { return this.varName; }
            set { this.varName = value; }
        }

        /// <summary>
        /// A function to get the evaluation of the variable by finding it in the list.
        /// </summary>
        /// <returns>The result of the evaluation.</returns>
        public override double Evaluate()
        {
            double val = this.vars[this.varName];
            return val;
        }
    }
}
