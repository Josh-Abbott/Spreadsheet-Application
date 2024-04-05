// <copyright file="BGColorEditAction.cs" company="Josh Abbott">
// Copyright (c) Josh Abbott. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// A concrete class to hold the background color editing action.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="BGColorEditAction"/> class.
    /// </remarks>
    /// <param name="oldColors">The dictionary of old colors.</param>
    /// <param name="newColors">The dictionary of new colors.</param>
    public class BGColorEditAction(Dictionary<Cell, uint> oldColors, Dictionary<Cell, uint> newColors) : IEditAction
    {
        private Dictionary<Cell, uint> oldColors = oldColors ?? throw new ArgumentNullException(nameof(oldColors));
        private Dictionary<Cell, uint> newColors = newColors;

        /// <summary>
        /// Sets the cell's background color to that of it's previous color.
        /// </summary>
        public void Undo()
        {
            foreach (var cell in this.oldColors.Keys)
            {
                cell.BGColor = this.oldColors[cell];
            }
        }

        /// <summary>
        /// Sets the cell's background color to that of it's most recent color.
        /// </summary>
        public void Redo()
        {
            foreach (var cell in this.newColors.Keys)
            {
                cell.BGColor = this.newColors[cell];
            }
        }

        /// <summary>
        /// Returns the action description for this edit option.
        /// </summary>
        /// <returns>A string containing the action description.</returns>
        public string GetActDesc()
        {
            return "Color change";
        }
    }
}
