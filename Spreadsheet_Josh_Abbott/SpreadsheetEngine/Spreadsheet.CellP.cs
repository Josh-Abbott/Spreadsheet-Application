// <copyright file="Spreadsheet.CellP.cs" company="Josh Abbott">
// Copyright (c) Josh Abbott. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    /// <summary>
    /// A partial class from the spreadsheet.
    /// </summary>
    public partial class Spreadsheet
    {
        /// <summary>
        /// A concrete class for the cell.
        /// </summary>
        public class CellP : Cell
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CellP"/> class.
            /// </summary>
            /// <param name="row">The specific row number.</param>
            /// <param name="column">The specific column number.</param>
            /// <param name="textVar">The variables of the text.</param>
            public CellP(int row, int column, string textVar)
                : base(row, column)
            {
                this.rowIndex = row;
                this.columnIndex = column;
                this.text = textVar;
                this.value = textVar;
            }
        }
    }
}
