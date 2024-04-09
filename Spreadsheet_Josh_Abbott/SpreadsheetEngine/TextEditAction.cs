// <copyright file="TextEditAction.cs" company="Josh Abbott">
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
    using System.Threading.Tasks.Sources;
    using static SpreadsheetEngine.Spreadsheet;

    /// <summary>
    /// A concrete class to hold the text editing action.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="TextEditAction"/> class.
    /// </remarks>
    /// <param name="cell">The cell being edited.</param>
    public class TextEditAction(Cell cell) : IEditAction
    {
        private readonly Cell cell = cell;
        private string oldText = string.Empty;
        private string newText = string.Empty;

        /// <summary>
        /// Modifies the oldText and newText values.
        /// </summary>
        /// <param name="newText">The updated new text.</param>
        public void AddChange(string newText)
        {
            this.oldText = this.cell.Text;
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

        /// <summary>
        /// Returns the action description for this edit option.
        /// </summary>
        /// <returns>A string containing the action description.</returns>
        public string GetActDesc()
        {
            return "Text change";
        }
    }
}
