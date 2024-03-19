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
    }
}