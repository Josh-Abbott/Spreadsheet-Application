// <copyright file="TestCell.cs" company="Josh Abbott">
// Copyright (c) Josh Abbott. All rights reserved.
// </copyright>

namespace SpreadsheetEngine.Tests
{
    /// <summary>
    /// A mock Cell that allows for the test cases to occur with the abstract class.
    /// </summary>
    public class TestCell : Cell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestCell"/> class.
        /// </summary>
        public TestCell()
            : base(0, 0)
        {
        }
    }
}