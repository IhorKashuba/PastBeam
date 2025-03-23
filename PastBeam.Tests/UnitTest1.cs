using Xunit;

namespace PastBeam.Tests
{
    public class Tests
    {
        public Tests()
        {
            // Використовуй конструктор для ініціалізації (аналог [SetUp] в NUnit)
        }

        [Fact] // В Xunit замість [Test] використовується [Fact]
        public void Test1()
        {
            Assert.True(true); // В Xunit немає Assert.Pass(), використовуй Assert.True()
        }
    }
}
