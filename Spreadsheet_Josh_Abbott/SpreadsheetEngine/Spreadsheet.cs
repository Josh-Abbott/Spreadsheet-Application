// <copyright file="Spreadsheet.cs" company="Josh Abbott">
// Copyright (c) Josh Abbott. All rights reserved.
// </copyright>

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).

namespace SpreadsheetEngine
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;
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
        private Cell[,]? spreadsheet;

        private Dictionary<Cell, HashSet<Cell>> dependencies;
        private UndoRedoCollection undoRedo = new UndoRedoCollection();

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
            this.dependencies = new Dictionary<Cell, HashSet<Cell>>();

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

            if (cell != null)
            {
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

                        // Check for dependencies that need to be updated
                        if (this.dependencies.TryGetValue(cell, out HashSet<Cell>? value))
                        {
                            foreach (var dependentCell in value)
                            {
                                if (cell.Value != "!(circular reference)")
                                {
                                    this.CalculateCell(dependentCell);
                                }
                            }
                        }
                    }
                    else if (cell.Text == null)
                    {
                        throw new NullReferenceException("The cell text cannot be null.");
                    }
                    else
                    {
                        cell.Value = cell.Text;

                        // Update dependencies
                        if (this.dependencies.TryGetValue(cell, out HashSet<Cell>? value))
                        {
                            foreach (var dependentCell in value)
                            {
                                this.CalculateCell(dependentCell);
                            }
                        }
                    }

                    this.PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs("Value"));
                }
                else if (e.PropertyName == "BGColor")
                {
                    uint oldColor = cell.BGColor;
                    this.PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs("BGColor"));
                }
                else
                {
                    this.PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs("Value"));
                }

                if (this.PropertyChanged == null)
                {
                    throw new NullReferenceException("The cell text cannot be null.");
                }
            }
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

                    // Check for circular reference before updating dependencies
                    if (this.HasCircularReference(cell, expTree))
                    {
                        cell.Value = "!(circular reference)";
                        return;
                    }

                    foreach (string varName in expTree.GetVariableNames())
                    {
                        int col = varName[0] - 'A';
                        int row = int.Parse(varName.Substring(1)) - 1;

                        Cell? referencedCell = this.GetCell(row, col);
                        if (referencedCell != null)
                        {
                            if (cell.Equals(referencedCell))
                            {
                                cell.Value = "!(self reference)";
                                return;
                            }

                            if (double.TryParse(referencedCell.Value, out double value))
                            {
                                expTree.SetVariable(varName, value);
                            }
                            else
                            {
                                cell.Value = "0";
                            }

                            if (!this.dependencies.TryGetValue(referencedCell, out HashSet<Cell>? val))
                            {
                                val = new HashSet<Cell>();
                                this.dependencies[referencedCell] = val;
                            }

                            val.Add(cell);
                        }
                        else
                        {
                            cell.Value = "!(bad reference)";
                            return;
                        }
                    }

                    // Evaluate and set cell's Value
                    cell.Value = expTree.Evaluate().ToString();
                }
                catch (Exception)
                {
                    cell.Value = "!(bad reference)";
                }
            }
            else
            {
                cell.Value = cell.Text;
            }
        }

        /// <summary>
        /// Adds the action to the UndoRedoCollection.
        /// </summary>
        /// <param name="act">The action being added.</param>
        public void AddUndo(IEditAction act)
        {
            this.undoRedo.AddAction(act);
        }

        /// <summary>
        /// Calls the undo function to undo the intended action.
        /// </summary>
        public void Undo()
        {
            if (this.CanUndo())
            {
                this.undoRedo.Undo();
            }
            else
            {
                throw new InvalidOperationException("There are no actions to undo!");
            }
        }

        /// <summary>
        /// Calls the redo function to redo the intended action.
        /// </summary>
        public void Redo()
        {
            if (this.CanRedo())
            {
                this.undoRedo.Redo();
            }
            else
            {
                throw new InvalidOperationException("There are no actions to redo!");
            }
        }

        /// <summary>
        /// Calls the CanUndo function to determine if the action can be performed.
        /// </summary>
        /// <returns>A boolean value of if it can be used or not.</returns>
        public bool CanUndo()
        {
            return this.undoRedo.CanUndo();
        }

        /// <summary>
        /// Calls the CanRedo function to determine if the action can be performed.
        /// </summary>
        /// <returns>A boolean value of if it can be used or not.</returns>
        public bool CanRedo()
        {
            return this.undoRedo.CanRedo();
        }

        /// <summary>
        /// Returns the description of the undo action currently.
        /// </summary>
        /// <returns>The action description.</returns>
        public string GetUndoActionDescription()
        {
            if (this.undoRedo.CanUndo())
            {
                return this.undoRedo.GetUndoAction().GetActDesc();
            }

            return "Undo";
        }

        /// <summary>
        /// Returns the description of the redo action currently.
        /// </summary>
        /// <returns>The action description.</returns>
        public string GetRedoActionDescription()
        {
            if (this.undoRedo.CanRedo())
            {
                return this.undoRedo.GetRedoAction().GetActDesc();
            }

            return "Redo";
        }

        /// <summary>
        /// Clears the spreadsheet GUI and undo/redo stacks by setting back to defaults.
        /// </summary>
        public void Clear()
        {
            if (this.spreadsheet != null)
            {
                for (int row = 0; row < this.rowCount; row++)
                {
                    for (int column = 0; column < this.columnCount; column++)
                    {
                        Cell cell = this.spreadsheet[row, column];
                        if (cell != null)
                        {
                            cell.Text = string.Empty;
                            cell.BGColor = 0xFFFFFFFF;
                            cell.Value = string.Empty;
                        }
                    }
                }
            }

            this.undoRedo.Clear();
        }

        /// <summary>
        /// Save the spreadsheet information to an XML file.
        /// </summary>
        /// <param name="outfile">The file being saved to.</param>
        public void Save(FileStream outfile)
        {
            using XmlWriter writer = XmlWriter.Create(outfile);
            writer.WriteStartElement("spreadsheet");

            // Loop through the entire spreadsheet
            for (int row = 0; row < this.rowCount; row++)
            {
                for (int column = 0; column < this.columnCount; column++)
                {
                    if (this.spreadsheet != null)
                    {
                        // Get the cell and determine if it's worth saving or not
                        Cell cell = this.spreadsheet[row, column];
                        if (!IsDefaultCell(cell))
                        {
                            writer.WriteStartElement("cell");
                            writer.WriteAttributeString("name", $"{((char)('A' + column)).ToString()}{row + 1}");

                            writer.WriteElementString("bgcolor", cell.BGColor.ToString());
                            writer.WriteElementString("text", cell.Text);

                            writer.WriteEndElement();
                        }
                    }
                }
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Load the spreadsheet information from an XML file.
        /// </summary>
        /// <param name="infile">The file being loaded from.</param>
        public void Load(FileStream infile)
        {
            this.undoRedo.Clear();

            if (this.spreadsheet != null)
            {
                bool readingBGColor = false;
                uint currentBGColor = 0xFFFFFFFF;
                string currentText = string.Empty;

                using XmlReader reader = XmlReader.Create(infile);
                while (reader.Read())
                {
                    if (reader.IsStartElement() && reader.Name == "cell")
                    {
                        string? cellName = reader.GetAttribute("name");
                        if (cellName != null)
                        {
                            int col = cellName[0] - 'A';
                            int row = int.Parse(cellName.Substring(1)) - 1;

                            currentBGColor = 0xFFFFFFFF;
                            currentText = string.Empty;

                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Element)
                                {
                                    switch (reader.Name)
                                    {
                                        case "bgcolor":
                                            readingBGColor = true;
                                            break;
                                        case "text":
                                            readingBGColor = false;
                                            break;
                                    }
                                }
                                else if (reader.NodeType == XmlNodeType.Text)
                                {
                                    if (readingBGColor)
                                    {
                                        currentBGColor = uint.Parse(reader.Value);
                                    }
                                    else
                                    {
                                        currentText = reader.Value;
                                    }
                                }
                                else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "cell")
                                {
                                    break;
                                }
                            }

                            this.spreadsheet[row, col].BGColor = currentBGColor;
                            this.spreadsheet[row, col].Text = currentText;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Determines if a specific cell is the default values or not.
        /// </summary>
        /// <param name="cell">The cell being checked.</param>
        /// <returns>Whether or not it's default.</returns>
        private static bool IsDefaultCell(Cell cell)
        {
            return cell.BGColor == 0xFFFFFFFF && string.IsNullOrEmpty(cell.Text);
        }

        /// <summary>
        /// A helper function that creates a visited HashSet and then calls a function to determine if there is a circular reference.
        /// </summary>
        /// <param name="cell">The cell being checked.</param>
        /// <param name="expTree">The expression tree.</param>
        /// <returns>Returns if there exists a circular reference or not.</returns>
        private bool HasCircularReference(Cell cell, ExpTreeP expTree)
        {
            var visited = new HashSet<Cell>();
            return this.CheckCircularReference(cell, expTree, visited);
        }

        /// <summary>
        /// Determines if there exists a circular reference or not.
        /// </summary>
        /// <param name="cell">The cell being looked at.</param>
        /// <param name="expTree">The expression tree.</param>
        /// <param name="visited">The hash of cells visited.</param>
        /// <returns>Whether there exists a circular reference or not.</returns>
        private bool CheckCircularReference(Cell cell, ExpTreeP expTree, HashSet<Cell> visited)
        {
            // Check for initial circular reference
            if (visited.Contains(cell))
            {
                return true;
            }

            visited.Add(cell);

            // Loop through spreadsheet to search for circular reference
            foreach (string varName in expTree.GetVariableNames())
            {
                int col = varName[0] - 'A';
                int row = int.Parse(varName.Substring(1)) - 1;

                if (col >= 0 && col < this.columnCount && row >= 0 && row < this.rowCount)
                {
                    Cell? referencedCell = this.GetCell(row, col);
                    if (referencedCell != null && referencedCell != cell)
                    {
                        // Verify that the cell is empty
                        if (!string.IsNullOrEmpty(referencedCell.Text))
                        {
                            ExpTreeP referencedExpTree = new ExpTreeP(referencedCell.Text.Substring(1));

                            // Verify if there is a circular reference
                            if (this.CheckCircularReference(referencedCell, referencedExpTree, visited))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            visited.Remove(cell);

            return false;
        }
    }
}
