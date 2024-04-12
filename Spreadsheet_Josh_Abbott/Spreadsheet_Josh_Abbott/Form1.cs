// <copyright file="Form1.cs" company="Josh Abbott">
// Copyright (c) Josh Abbott. All rights reserved.
// </copyright>

#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).

namespace Spreadsheet_Josh_Abbott
{
    using System.ComponentModel;
    using System.Data.Common;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using SpreadsheetEngine;
    using static System.Net.Mime.MediaTypeNames;
    using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

    /// <summary>
    /// A class for functions for the Form1 WinForm.
    /// </summary>
    public partial class Form1 : Form
    {
        private readonly Spreadsheet spreadsheet;

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// </summary>
        public Form1()
        {
            this.InitializeComponent();
            this.InitializeDataGrid();

            // Create spreadsheet for demo and subscribe to it.
            this.spreadsheet = new Spreadsheet(50, 25);
            this.spreadsheet.PropertyChanged += this.OnCellPropertyChanged;

            this.dataGridView1.CellBeginEdit += this.Spreadsheet_CellBeginEdit;
            this.dataGridView1.CellEndEdit += this.Spreadsheet_CellEndEdit;

            this.undoToolStripMenuItem.Enabled = false;
            this.redoToolStripMenuItem.Enabled = false;
        }

        /// <summary>
        /// Create the data grid UI using the WinForm functions.
        /// </summary>
        private void InitializeDataGrid()
        {
            this.dataGridView1.Columns.Clear();

            // Add rows to the spreadsheet form labeled from A to Z.
            char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            foreach (char letter in letters)
            {
                DataGridViewTextBoxColumn col = new()
                {
                    Name = letter.ToString(),
                };
                this.dataGridView1.Columns.Add(col);
            }

            // Adjust size of headers and rows to properly fit contents
            this.dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;

            // Create 50 rows and label the Header Cell with the number for each.
            for (int i = 1; i < 51; i++)
            {
                DataGridViewRow row = new();
                row.HeaderCell.Value = i.ToString();
                this.dataGridView1.Rows.Add(row);
            }
        }

        /// <summary>
        /// An event for OnCellPropertyChanged to know when any property for any cell in the worksheet has changed.
        /// </summary>
        /// <param name="sender">The object sent.</param>
        /// <param name="e">The property changed.</param>
        private void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Cell cell = (Cell)sender;
            if (e.PropertyName == "Value")
            {
                this.dataGridView1.Rows[cell.RowIndex].Cells[cell.ColumnIndex].Value = cell.Value;
            }
            else if (e.PropertyName == "BGColor")
            {
                this.dataGridView1.Rows[cell.RowIndex].Cells[cell.ColumnIndex].Style.BackColor = Color.FromArgb((int)cell.BGColor);
            }
        }

        /// <summary>
        /// Run the demo of the program required in HW 4.
        /// </summary>
        private void RunDemo()
        {
            Random rand = new();

            // Set the text in about 50 random cells to "Hello World!".
            for (int i = 0; i < 50; i++)
            {
                int colRand = rand.Next(0, 25);
                int rowRand = rand.Next(0, 49);
                Cell? cell = this.spreadsheet.GetCell(rowRand, colRand);
                if (cell != null)
                {
                    cell.Text = "Hello World!";
                }
            }

            // Loop to set the text in every cell of column B.
            for (int i = 0; i < 50; i++)
            {
                Cell? cell = this.spreadsheet.GetCell(i, 1);
                if (cell != null)
                {
                    cell.Text = "This is cell B" + (i + 1).ToString();
                }
            }

            // Loop to set the text in every cell of column A.
            for (int i = 0; i < 50; i++)
            {
                Cell? cell = this.spreadsheet.GetCell(i, 0);
                if (cell != null)
                {
                    cell.Text = "=B" + (i + 1).ToString();
                }
            }
        }

        /// <summary>
        /// The function used to determine when the user begins editing a cell.
        /// </summary>
        /// <param name="sender">The object sent.</param>
        /// <param name="e">The event related to the cell editing.</param>
        private void Spreadsheet_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            Cell? cell = this.spreadsheet.GetCell(e.RowIndex, e.ColumnIndex);
            if (cell != null)
            {
                this.dataGridView1[e.ColumnIndex, e.RowIndex].Value = cell.Text;
            }
        }

        /// <summary>
        /// The function used to determine when the user stops editing a cell.
        /// </summary>
        /// <param name="sender">The object sent.</param>
        /// <param name="e">The event related to the cell editing.</param>
        private void Spreadsheet_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Cell? cell = this.spreadsheet.GetCell(e.RowIndex, e.ColumnIndex);
            if (cell != null)
            {
                // Setup functionality for text undoing and redoing
                string text = this.dataGridView1[e.ColumnIndex, e.RowIndex].Value?.ToString() ?? string.Empty;
                var textEditAction = new TextEditAction(cell);
                textEditAction.AddChange(text);
                this.spreadsheet.AddUndo(textEditAction);
                cell.Text = text;

                this.UpdateUndoRedoButtons();
            }
        }

        /// <summary>
        /// Runs the demo when selected.
        /// </summary>
        /// <param name="sender">The object being sent.</param>
        /// <param name="e">The event.</param>
        private void RunDemoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.RunDemo();
        }

        /// <summary>
        /// Prompts the user with options to change the background color of the cell.
        /// </summary>
        /// <param name="sender">The object being sent.</param>
        /// <param name="e">The event.</param>
        private void ChangeBackgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            Dictionary<Cell, uint> oldColors = new Dictionary<Cell, uint>();
            Dictionary<Cell, uint> newColors = new Dictionary<Cell, uint>();

            // Opens dialog for user.
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                // Loops through selected cells to set them.
                foreach (DataGridViewCell gCell in this.dataGridView1.SelectedCells)
                {
                    Cell? dCell = this.spreadsheet.GetCell(gCell.RowIndex, gCell.ColumnIndex);
                    if (dCell != null)
                    {
                        oldColors[dCell] = dCell.BGColor;
                        newColors[dCell] = (uint)colorDialog.Color.ToArgb();
                        dCell.BGColor = (uint)colorDialog.Color.ToArgb();
                    }
                }

                this.spreadsheet.AddUndo(new BGColorEditAction(oldColors, newColors));
                this.UpdateUndoRedoButtons();
            }
        }

        /// <summary>
        /// Undoes the most recent action.
        /// </summary>
        /// <param name="sender">The object.</param>
        /// <param name="e">The event.</param>
        private void UndoToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.spreadsheet.Undo();
            this.UpdateUndoRedoButtons();
        }

        /// <summary>
        /// Redoes the most recent action.
        /// </summary>
        /// <param name="sender">The object.</param>
        /// <param name="e">The event.</param>
        private void RedoToolStripMenuItem_Click_(object sender, EventArgs e)
        {
            this.spreadsheet.Redo();
            this.UpdateUndoRedoButtons();
        }

        /// <summary>
        /// Update the text and status of both buttons to reflect current actions.
        /// </summary>
        private void UpdateUndoRedoButtons()
        {
            // Update Undo button
            if (this.spreadsheet.CanUndo())
            {
                this.undoToolStripMenuItem.Enabled = true;
                this.undoToolStripMenuItem.Text = "Undo " + this.spreadsheet.GetUndoActionDescription();
            }
            else
            {
                this.undoToolStripMenuItem.Enabled = false;
                this.undoToolStripMenuItem.Text = "Undo";
            }

            // Update Redo button
            if (this.spreadsheet.CanRedo())
            {
                this.redoToolStripMenuItem.Enabled = true;
                this.redoToolStripMenuItem.Text = "Redo " + this.spreadsheet.GetRedoActionDescription();
            }
            else
            {
                this.redoToolStripMenuItem.Enabled = false;
                this.redoToolStripMenuItem.Text = "Redo";
            }
        }

        /// <summary>
        /// Saves the current spreadsheet file using XML.
        /// </summary>
        /// <param name="sender">The object.</param>
        /// <param name="e">The event.</param>
        private void SaveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                        {
                            this.spreadsheet.Save(fileStream);
                        }

                        MessageBox.Show("File saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Loads a selected spreadsheet file using XML.
        /// </summary>
        /// <param name="sender">The object.</param>
        /// <param name="e">The event.</param>
        private void LoadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open))
                        {
                            this.spreadsheet.Load(fileStream);
                        }

                        MessageBox.Show("File loaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
