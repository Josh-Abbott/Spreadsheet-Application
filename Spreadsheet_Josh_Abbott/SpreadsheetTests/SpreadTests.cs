// <copyright file="SpreadTests.cs" company="Josh Abbott">
// Copyright (c) Josh Abbott. All rights reserved.
// </copyright>

namespace SpreadsheetEngine.Tests
{
    using System.ComponentModel;
    using NUnit.Framework;
    using static SpreadsheetEngine.Spreadsheet;

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
                spreadsheet.PropertyChanged += (s, e) => Assert.That(e.PropertyName, Is.EqualTo("Value"));

                Assert.That(cell.Text, Is.EqualTo(string.Empty)); // Assuming empty cell

                // Simulate a text edit change
                var textEditAction = new TextEditAction(cell);
                textEditAction.AddChange("Hello");
                spreadsheet.AddUndo(textEditAction);

                cell.Text = "Hello";
                spreadsheet.OnCellPropertyChanged(cell, new PropertyChangedEventArgs("Text"));

                spreadsheet.Undo();
                Assert.That(cell.Text, Is.EqualTo(string.Empty));

                spreadsheet.Redo();
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
                spreadsheet.PropertyChanged += (s, e) => Assert.That(e.PropertyName, Is.EqualTo("Value"));

                // Simulate text edit changes
                var textEditAction1 = new TextEditAction(cell1);
                textEditAction1.AddChange("Hello");
                spreadsheet.AddUndo(textEditAction1);

                var textEditAction2 = new TextEditAction(cell2);
                textEditAction2.AddChange("Hi");
                spreadsheet.AddUndo(textEditAction2);

                spreadsheet.Undo();
                spreadsheet.Undo();
                Assert.Multiple(() =>
                {
                    Assert.That(cell1.Text, Is.EqualTo(string.Empty));
                    Assert.That(cell2.Text, Is.EqualTo(string.Empty));
                });
                spreadsheet.Redo();
                Assert.That(cell1.Text, Is.EqualTo("Hello"));

                spreadsheet.Redo();
                Assert.That(cell2.Text, Is.EqualTo("Hi"));
            }
        }

        /// <summary>
        /// An exception case for testing both the undo and redo functionality for a cell.
        /// </summary>
        [Test]
        public void CellUndoRedoExcep()
        {
            var spreadsheet = new Spreadsheet(5, 5);

            Assert.Throws<InvalidOperationException>(() => spreadsheet.Undo());
        }

        /// <summary>
        /// An normal case for testing both the save and load functionality for a spreadsheet.
        /// </summary>
        [Test]
        public void SaveLoadNorm()
        {
            var spreadsheet = new Spreadsheet(5, 5);
            var cell1 = spreadsheet.GetCell(0, 0);
            var cell2 = spreadsheet.GetCell(1, 1);

            if (cell1 != null && cell2 != null)
            {
                spreadsheet.PropertyChanged += (s, e) => Assert.That(e.PropertyName, Is.EqualTo("Value"));

                cell1.Text = "Hello";
                spreadsheet.OnCellPropertyChanged(cell1, new PropertyChangedEventArgs("Text"));
                cell2.Text = "=A1+10";
                spreadsheet.OnCellPropertyChanged(cell2, new PropertyChangedEventArgs("Text"));
            }

            string filePath = "test_normal.xml";
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                spreadsheet.Save(fileStream);
            }

            spreadsheet.Clear();

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                spreadsheet.Load(fileStream);
            }

            Assert.Multiple(() =>
            {
                Assert.That(spreadsheet.GetCell(0, 0)?.Text, Is.EqualTo("Hello"));
                Assert.That(spreadsheet.GetCell(1, 1)?.Text, Is.EqualTo("=A1+10"));
            });
        }

        /// <summary>
        /// An edge case for testing both the save and load functionality for a spreadsheet.
        /// </summary>
        [Test]
        public void SaveLoadEdge()
        {
            var spreadsheet = new Spreadsheet(5, 5);

            string filePath = "test_empty.xml";
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                spreadsheet.Save(fileStream);
            }

            spreadsheet.Clear();

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                spreadsheet.Load(fileStream);
            }

            for (int row = 0; row < 5; row++)
            {
                for (int column = 0; column < 5; column++)
                {
                    Assert.That(spreadsheet.GetCell(row, column)?.Text, Is.EqualTo(string.Empty));
                }
            }
        }

        /// <summary>
        /// An exception case for testing both the save and load functionality for a spreadsheet.
        /// </summary>
        [Test]
        public void SaveLoadExcep()
        {
            var spreadsheet = new Spreadsheet(5, 5);

            string filePath = "fake_file.xml";

            Assert.Throws<FileNotFoundException>(() =>
            {
                using FileStream fileStream = new FileStream(filePath, FileMode.Open);
                spreadsheet.Load(fileStream);
            });
        }
    }
}