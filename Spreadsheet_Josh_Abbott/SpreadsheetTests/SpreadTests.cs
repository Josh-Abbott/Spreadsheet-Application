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
                cell.Text = "=2+5";

                spreadsheet.CalculateCell(cell);

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

                spreadsheet.CalculateCell(cell);

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
                cell.Text = "=5%2";

                Assert.Throws<NotSupportedException>(() => spreadsheet.CalculateCell(cell));
            }
        }
    }
}