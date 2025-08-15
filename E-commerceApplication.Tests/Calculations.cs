using AutoFixture;
using E_commerceApplication.Business;

namespace E_commerceApplication.Tests
{
    public class Calculations
    {
        [Fact]
        public void Sum_ShouldReturnCorrectValue()
        {
            Fixture fixture = new();
            int a = fixture.Create<int>();
            int b = fixture.Create<int>();

            Calculator calculator = new Calculator();
            int result = calculator.Sum(a, b);

            Assert.Equal(a + b, result);
        }
    }
}
