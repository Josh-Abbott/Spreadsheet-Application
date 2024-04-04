// <copyright file="SpreadTests.cs" company="Josh Abbott">
// Copyright (c) Josh Abbott. All rights reserved.
// </copyright>

namespace SpreadsheetEngine.Tests
{
    using System.ComponentModel;
    using NUnit.Framework;

    /// <summary>
    /// A class containing the test cases created for property changed functionality.
    /// </summary>
    public class SpreadTests
    {
        /// <summary>
        /// A normal test case for the OnCellPropertyChanged method.
        /// </summary>
        [Test]
        public void PropertyChangedNormal()
        {
            var cell = new TestCell();
            var target = new Spreadsheet(0, 0);
            target.PropertyChanged += (s, e) => Assert.That(e.PropertyName, Is.EqualTo("Value"));

            cell.Text = "NewTextTest";
            target.OnCellPropertyChanged(cell, new System.ComponentModel.PropertyChangedEventArgs("Text"));
        }

        /// <summary>
        /// An edge test case for the OnCellPropertyChanged method.
        /// </summary>
        [Test]
        public void PropertyChangedEdge()
        {
            var cell = new TestCell();
            var target = new Spreadsheet(0, 0);
            cell.Text = "=";

            Assert.Throws<ArgumentException>(() =>
                target.OnCellPropertyChanged(cell, new PropertyChangedEventArgs("Text")));
        }

        /// <summary>
        /// An exception test case for the OnCellPropertyChanged method.
        /// </summary>
        [Test]
        public void PropertyChangedExcep()
        {
            var cell = new TestCell();
            var target = new Spreadsheet(0, 0);
            target.PropertyChanged += null;

            Assert.Throws<NullReferenceException>(() =>
                target.OnCellPropertyChanged(cell, new PropertyChangedEventArgs("Text")));
        }

        /// <summary>
        /// A normal case to verify that using a formula in a cell is correctly evaluated.
        /// </summary>
        [Test]
        public void CellFormulaUpdatingNorm()
        {
            var spreadsheet = new Spreadsheet(5, 5);
            var cell = spreadsheet.GetCell(0, 0);
            if (cell != null)
            {
                spreadsheet.PropertyChanged += (s, e) => Assert.That(e.PropertyName, Is.EqualTo("Value"));

                // Simulate updating cell
                cell.Text = "=2+5";
                spreadsheet.OnCellPropertyChanged(cell, new System.ComponentModel.PropertyChangedEventArgs("Text"));

                Assert.That(cell.Value, Is.EqualTo("7"));
            }
        }

        /// <summary>
        /// An edge case to verify that using a formula in a cell is correctly evaluated.
        /// </summary>
        [Test]
        public void CellFormulaUpdatingEdge()
        {
            var spreadsheet = new Spreadsheet(5, 5);
            var cell = spreadsheet.GetCell(0, 0);
            if (cell != null)
            {
                cell.Text = string.Empty;

                Assert.That(cell.Value, Is.EqualTo(string.Empty));
            }
        }

        /// <summary>
        /// An exception case to verify that using a formula in a cell is correctly evaluated.
        /// </summary>
        [Test]
        public void CellFormulaUpdatingExcep()
        {
            var spreadsheet = new Spreadsheet(5, 5);
            var cell = spreadsheet.GetCell(0, 0);
            if (cell != null)
            {
                spreadsheet.PropertyChanged += (s, e) => Assert.That(e.PropertyName, Is.EqualTo("Value"));

                // Simulate updating cell
                cell.Text = "=2%5";
                spreadsheet.OnCellPropertyChanged(cell, new System.ComponentModel.PropertyChangedEventArgs("Text"));

                Assert.That(cell.Value, Is.EqualTo("#ERROR"));
            }
        }

        /// <summary>
        /// A normal case for testing both the undo and redo functionality for a cell.
        /// </summary>
        [Test]
        public void CellUndoRedoNormal()
        {
            var spreadsheet = new Spreadsheet(5, 5);
            var cell = spreadsheet.GetCell(0, 0);

            if (cell != null)
            {
                Assert.That(cell.Text, Is.EqualTo(string.Empty)); // Assuming empty cell
                cell.Text = "Hello";

                spreadsheet.undo();
                Assert.That(cell.Text, Is.EqualTo(string.Empty));

                spreadsheet.redo();
                Assert.That(cell.Text, Is.EqualTo("Hello"));
            }
        }

        /// <summary>
        /// An edge case for testing both the undo and redo functionality for a cell.
        /// </summary>
        [Test]
        public void CellUndoRedoEdge()
        {
            var spreadsheet = new Spreadsheet(5, 5);
            var cell1 = spreadsheet.GetCell(0, 0);
            var cell2 = spreadsheet.GetCell(1, 0);

            if (cell1 != null && cell2 != null)
            {
                cell1.Text = "Hello";
                cell2.Text = "Hi";

                spreadsheet.undo();
                spreadsheet.undo();
                Assert.Multiple(() =>
                {
                    Assert.That(cell1.Text, Is.EqualTo(string.Empty));
                    Assert.That(cell2.Text, Is.EqualTo("Hi"));
                });
                spreadsheet.redo();
                Assert.That(cell1.Text, Is.EqualTo("Hello"));

                spreadsheet.redo();
                Assert.That(cell2.Text, Is.EqualTo(string.Empty));
            }
        }

        /// <summary>
        /// An exception case for testing both the undo and redo functionality for a cell.
        /// </summary>
        [Test]
        public void CellUndoRedoExcep()
        {
            var spreadsheet = new Spreadsheet(5, 5);

            Assert.Throws<InvalidOperationException>(() => spreadsheet.undo());
        }
    }
}