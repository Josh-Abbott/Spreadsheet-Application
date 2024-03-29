// <copyright file="Cell.cs" company="Josh Abbott">
// Copyright (c) Josh Abbott. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System.ComponentModel;

    /// <summary>
    /// An abstract class for the cell.
    /// </summary>
    public abstract class Cell : INotifyPropertyChanged
    {
        /// <summary>
        /// An index to keep track of the rows.
        /// </summary>
        protected int rowIndex;

        /// <summary>
        /// An index to keep track of the columns.
        /// </summary>
        protected int columnIndex;

        /// <summary>
        /// A text string to keep the text of the cell.
        /// </summary>
        protected string text = string.Empty;

        /// <summary>
        /// A value string to keep the value of the cell.
        /// </summary>
        protected string value = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// Constructor for the Cell class.
        /// </summary>
        /// <param name="row">The number of rows.</param>
        /// <param name="column">The number of columns.</param>
        public Cell(int row, int column)
        {
            this.rowIndex = row;
            this.columnIndex = column;
        }

        /// <summary>
        /// An event to keep track of when the property is changed.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets the row index.
        /// </summary>
        /// <returns>The rowIndex integer from the getter.</returns>
        public int RowIndex
        {
            get { return this.rowIndex; }
        }

        /// <summary>
        /// Gets the column index.
        /// </summary>
        /// <returns>The columnIndex integer from the getter.</returns>
        public int ColumnIndex
        {
            get { return this.columnIndex; }
        }

        /// <summary>
        /// Gets or sets for the text string variable.
        /// </summary>
        /// <returns>The text variable from the getter.</returns>
        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                if (value != this.text)
                {
                    this.text = value;
                    this.OnPropertyChanged(nameof(this.Text));
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Gets or sets the value string variable.
        /// </summary>
        /// <returns>The value variable from the getter.</returns>
        public string Value
        {
            get
            {
                return this.value;
            }

            protected internal set
            {
                if (value != this.value)
                {
                    this.value = value;
                    this.OnPropertyChanged(nameof(this.Value));

                    if (this.PropertyChanged != null)
                    {
                        this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(this.Value)));
                    }
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Verify that PropertyChanged is not null and then fire the event.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
