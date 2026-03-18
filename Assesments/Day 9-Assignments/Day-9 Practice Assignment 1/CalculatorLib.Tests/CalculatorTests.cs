using NUnit.Framework;
using CalculatorLib;

namespace CalculatorLib.Tests
{
    [TestFixture]
    public class CalculatorTests
    {
        private Calculator calculator;

        [SetUp]
        public void Setup()
        {
            calculator = new Calculator();
        }

        // Tests for Add method
        [Test]
        public void Add_TwoPositiveNumbers_ReturnsCorrectSum()
        {
            double result = calculator.Add(5, 3);
            Assert.That(result, Is.EqualTo(8));
        }

        [Test]
        public void Add_WithZero_ReturnsSameNumber()
        {
            double result = calculator.Add(7, 0);
            Assert.That(result, Is.EqualTo(7));
        }

        [Test]
        public void Add_TwoNegativeNumbers_ReturnsCorrectSum()
        {
            double result = calculator.Add(-4, -6);
            Assert.That(result, Is.EqualTo(-10));
        }

        // Tests for Subtract method
        [Test]
        public void Subtract_TwoPositiveNumbers_ReturnsCorrectDifference()
        {
            double result = calculator.Subtract(10, 4);
            Assert.That(result, Is.EqualTo(6));
        }

        [Test]
        public void Subtract_WithZero_ReturnsSameNumber()
        {
            double result = calculator.Subtract(5, 0);
            Assert.That(result, Is.EqualTo(5));
        }

        [Test]
        public void Subtract_ResultIsNegative()
        {
            double result = calculator.Subtract(3, 8);
            Assert.That(result, Is.EqualTo(-5));
        }

        // Tests for Multiply method
        [Test]
        public void Multiply_TwoPositiveNumbers_ReturnsCorrectProduct()
        {
            double result = calculator.Multiply(4, 5);
            Assert.That(result, Is.EqualTo(20));
        }

        [Test]
        public void Multiply_WithZero_ReturnsZero()
        {
            double result = calculator.Multiply(9, 0);
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void Multiply_WithNegativeNumber_ReturnsNegativeResult()
        {
            double result = calculator.Multiply(3, -2);
            Assert.That(result, Is.EqualTo(-6));
        }

        // Tests for Divide method
        [Test]
        public void Divide_TwoPositiveNumbers_ReturnsCorrectQuotient()
        {
            double result = calculator.Divide(10, 2);
            Assert.That(result, Is.EqualTo(5));
        }

        [Test]
        public void Divide_ByZero_ThrowsDivideByZeroException()
        {
            Assert.Throws<DivideByZeroException>(() => calculator.Divide(10, 0));
        }

        [Test]
        public void Divide_ZeroByNumber_ReturnsZero()
        {
            double result = calculator.Divide(0, 5);
            Assert.That(result, Is.EqualTo(0));
        }
    }
}
