// <copyright file="ExpTests.cs" company="Josh Abbott">
// Copyright (c) Josh Abbott. All rights reserved.
// </copyright>

namespace SpreadsheetEngine.Tests
{
    using System.Data;
    using System.Globalization;
    using System.Linq.Expressions;
    using NUnit.Framework;

    /// <summary>
    /// A class to hold tests for the expression tree functionality.
    /// </summary>
    [TestFixture]
    public class ExpTests
    {
        /// <summary>
        /// The function that runs all of the above normal test cases.
        /// </summary>
        /// <param name="expression">The expression string.</param>
        /// <returns>The result of the evaluation.</returns>
        [Test]
        [TestCase("3+5", ExpectedResult = 8.0)] // expression with a single operator
        [TestCase("100/10*10", ExpectedResult = 100.0)] // mixing operators (/ and *) with same precedence
        [TestCase("100/(10*10)", ExpectedResult = 1.0)] // mixing operators (/ and *) with same precedence and parenthesis
        [TestCase("7-4+2", ExpectedResult = 5.0)] // mixing operators (+ and -) with same precedence
        [TestCase("10/(7-2)", ExpectedResult = 2.0)] // operators with different precedence with parentheses - higher precedence first
        [TestCase("(12-2)/2", ExpectedResult = 5.0)] // operators with different precedence with parentheses - lower precedence first
        [TestCase("(((((2+3)-(4+5)))))", ExpectedResult = -4.0)] // extra parentheses and negative result
        [TestCase("2*3+5", ExpectedResult = 11.0)] // operators with different precedence - higher precedence first
        [TestCase("2+3*5", ExpectedResult = 17.0)] // operators with different precedence - lower precedence first
        [TestCase("2 + 3 * 5", ExpectedResult = 17.0)] // spaces and mixing operators (+ and *) with different precedence
        [TestCase("5/0", ExpectedResult = double.PositiveInfinity)] // Dividing a floating-point value by zero doesn't throw an exception; it results in positive infinity, negative infinity, or not a number (NaN), according to the rules of IEEE 754 arithmetic.
        public double TestEvaluateNormalCases(string expression)
        {
            TestExpTree exp = new TestExpTree(expression);
            return exp.Evaluate();
        }

        /// <summary>
        /// A function to test an invalid expression.
        /// </summary>
        /// <param name="expression">The expression string.</param>
        [TestCase("((2+5))-2(2+3))")] // extra parenthesis at the end
        public void TestConstructInvalidExpression(string expression)
        {
            Assert.Throws<InvalidExpressionException>(() => new TestExpTree(expression), "Invalid expression!");
        }

        /// <summary>
        /// A function to test an unsupported operator in the expression tree.
        /// </summary>
        /// <param name="expression">The expression string.</param>
        [TestCase("4%2")]
        public void TestEvaluateUnsupportedOperator(string expression)
        {
            Assert.Throws<NotSupportedException>(() => new TestExpTree(expression), "Invalid character in expression!");
        }

        /// <summary>
        /// A function to test attempting to get infinity from the expression tree.
        /// </summary>
        [Test]
        public void TestInfinity()
        {
            string maxValue = double.MaxValue.ToString("F", CultureInfo.InvariantCulture);
            double result = new TestExpTree($"{maxValue}+{maxValue}").Evaluate();
            Assert.That(double.IsInfinity(result), Is.True);
        }

        /// <summary>
        /// A function to test an expression that uses variable values.
        /// </summary>
        [Test]
        public void TestExpressionsWithVariableValues()
        {
            TestExpTree exp = new TestExpTree("A3+5");
            exp.SetVariable("A3", 23);
            Assert.That(exp.Evaluate(), Is.EqualTo(28));

            exp = new TestExpTree("B2+A3");
            exp.SetVariable("A3", 3);
            exp.SetVariable("B2", 2);
            Assert.That(exp.Evaluate(), Is.EqualTo(5));
        }
    }
}
