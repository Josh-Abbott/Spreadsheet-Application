// <copyright file="Spreadsheet.cs" company="Josh Abbott">
// Copyright (c) Josh Abbott. All rights reserved.
// </copyright>

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).

namespace SpreadsheetEngine
{
    using System.ComponentModel;
    using System.Xml.Xsl;

    /// <summary>
    /// The class containing all relevant spreadsheet variables and functions.
    /// </summary>
    public partial class Spreadsheet
    {
        private readonly int rowCount;
        private readonly int columnCount;

        /// <summary>
        /// A table of cells to represent the spreadsheet.
        /// </summary>
        private Cell[,] ? spreadsheet;

        private Dictionary<Cell, List<Cell>> dependencies = new Dictionary<Cell, List<Cell>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
        /// The constructor for the spreadsheet class that initializes the array of cells.
        /// </summary>
        /// <param name="rowCount">The count of rows.</param>
        /// <param name="columnCount">The count of columns.</param>
        public Spreadsheet(int rowCount, int columnCount)
        {
            this.spreadsheet = new Cell[rowCount, columnCount];
            this.rowCount = rowCount;
            this.columnCount = columnCount;

            // Create the spreadsheet object
            for (int r = 0; r < rowCount; r++)
            {
                for (int c = 0; c < columnCount; c++)
                {
                    this.spreadsheet[r, c] = new CellP(r, c, string.Empty);
                    this.spreadsheet[r, c].PropertyChanged += new PropertyChangedEventHandler(this.OnCellPropertyChanged);
                }
            }
        }

        /// <summary>
        /// An event for when the property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// An event for OnCellPropertyChanged to know when any property for any cell in the worksheet has changed.
        /// </summary>
        /// <param name="sender">The object sent.</param>
        /// <param name="e">The property changed.</param>
        public void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Cell? cell = (Cell)sender;

            if (e.PropertyName == "Text")
            {
                // Determine if the cell text is a formula or not.
                if (cell.Text.StartsWith('='))
                {
                    // Verify that the cell text actually contains a formula.
                    if (cell.Text.Length == 1)
                    {
                        throw new ArgumentException("The cell text cannot be only '='.");
                    }

                    this.CalculateCell(cell);
                }
                else if (cell.Text == null)
                {
                    throw new NullReferenceException("The cell text cannot be null.");
                }
                else
                {
                    cell.Value = cell.Text;
                }
            }

            if (this.PropertyChanged == null)
            {
                throw new NullReferenceException("PropertyChanged cannot be set to null.");
            }

            this.PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs("Value"));
        }

        /// <summary>
        /// Looks for the cell at the location given in the passed row and column variables.
        /// </summary>
        /// <param name="row">The specific row number.</param>
        /// <param name="column">The specific column number.</param>
        /// <returns>The abstract cell at the given position.</returns>
        public Cell? GetCell(int row, int column)
        {
            if (this.spreadsheet != null)
            {
                if (row < this.spreadsheet.GetLength(0) || column < this.spreadsheet.GetLength(1))
                {
                    return this.spreadsheet[row, column];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private void RecalculateDependentCells(Cell changedCell)
        {
            if (this.dependencies.TryGetValue(changedCell, out List<Cell>? value))
            {
                foreach (CellP dependentCell in value)
                {
                    // Assuming formulas are not nested more than once.
                    this.CalculateCell(dependentCell);
                }
            }
        }

        /// <summary>
        /// A function to calculate the contents in a cell when a formula is used.
        /// </summary>
        /// <param name="cell">The cell that the formula has been entered in.</param>
        public void CalculateCell(Cell cell)
        {
            if (cell.Text.StartsWith('='))
            {
                try
                {
                    // Create ExpressionTree and set variables
                    ExpTreeP expTree = new ExpTreeP(cell.Text.Substring(1));
                    this.dependencies[cell] = new List<Cell>();  // Clear existing
                    foreach (string varName in expTree.GetVariableNames())
                    {
                        int col = varName[0] - 'A';
                        int row = int.Parse(varName.Substring(1)) - 1;

                        Cell? referencedCell = this.GetCell(row, col);
                        if (referencedCell != null)
                        {
                            if (double.TryParse(referencedCell.Value, out double value))
                            {
                                expTree.SetVariable(varName, value);
                            }
                            else
                            {
                                cell.Value = "#ERROR";
                                return;
                            }
                        }
                        else
                        {
                            cell.Value = "#ERROR";
                            return;
                        }

                        // Update dependencies dictionary
                        this.dependencies[cell].Add(referencedCell);
                    }

                    // Evaluate and set cell's Value
                    cell.Value = expTree.Evaluate().ToString();
                }
                catch (Exception)
                {
                    // Catch exceptions from expression evaluation
                    cell.Value = "#ERROR";
                }
            }
            else
            {
                cell.Value = cell.Text;
            }

            // Dependency recalculation
            this.RecalculateDependentCells(cell);
        }
    }
}
