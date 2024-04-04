// <copyright file="TextEditAction.cs" company="Josh Abbott">
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
    /// A concrete class to hold the text editing action.
    /// </summary>
    public class TextEditAction : IEditAction
    {
        private Cell cell;
        private string oldText;
        private string newText;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextEditAction"/> class.
        /// </summary>
        /// <param name="cell">The cell being edited.</param>
        /// <param name="oldText">The old text in the cell.</param>
        /// <param name="newText">The new text in the cell.</param>
        public TextEditAction(Cell cell, string oldText, string newText)
        {
            this.cell = cell;
            this.oldText = oldText;
            this.newText = newText;
        }

        /// <summary>
        /// Sets the cell's text to that of it's previous text.
        /// </summary>
        public void Undo()
        {
            this.cell.Text = this.oldText;
        }

        /// <summary>
        /// Sets the cell's text to that of it's most recent text.
        /// </summary>
        public void Redo()
        {
            this.cell.Text = this.newText;
        }
    }
}
