// <copyright file="Form1.cs" company="Josh Abbott">
// Copyright (c) Josh Abbott. All rights reserved.
// </copyright>

#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).

namespace Spreadsheet_Josh_Abbott
{
    using System.ComponentModel;
    using System.Data.Common;
    using SpreadsheetEngine;
    using static System.Net.Mime.MediaTypeNames;

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
                DataGridViewTextBoxColumn col = new ()
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
                DataGridViewRow row = new ();
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
            if (e.PropertyName == "Value")
            {
                Cell cell = (Cell)sender;
                this.dataGridView1.Rows[cell.RowIndex].Cells[cell.ColumnIndex].Value = cell.Value;
            }
        }

        /// <summary>
        /// Run the demo of the program required in HW 4.
        /// </summary>
        private void RunDemo()
        {
            Random rand = new ();

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
                string text = this.dataGridView1[e.ColumnIndex, e.RowIndex].Value?.ToString() ?? string.Empty;
                cell.Text = text;
            }
        }

        /// <summary>
        /// Triggered when the demo button is clicked to run it.
        /// </summary>
        private void Button1_Click(object sender, EventArgs e)
        {
            this.RunDemo();
        }
    }
}
