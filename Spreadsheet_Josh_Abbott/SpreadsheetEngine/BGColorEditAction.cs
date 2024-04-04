// <copyright file="BGColorEditAction.cs" company="Josh Abbott">
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
    /// A concrete class to hold the background color editing action.
    /// </summary>
    public class BGColorEditAction : IEditAction
    {
        private Cell cell;
        private uint oldColor;
        private uint newColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="BGColorEditAction"/> class.
        /// </summary>
        /// <param name="cell">The cell being edited.</param>
        /// <param name="oldColor">The previous color.</param>
        /// <param name="newColor">The new color.</param>
        public BGColorEditAction(Cell cell, uint oldColor, uint newColor)
        {
            this.cell = cell;
            this.oldColor = oldColor;
            this.newColor = newColor;
        }

        /// <summary>
        /// Sets the cell's background color to that of it's previous color.
        /// </summary>
        public void Undo()
        {
            this.cell.BGColor = this.oldColor;
        }

        /// <summary>
        /// Sets the cell's background color to that of it's most recent color.
        /// </summary>
        public void Redo()
        {
            this.cell.BGColor = this.newColor;
        }
    }
}
