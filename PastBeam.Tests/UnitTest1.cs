using Xunit;

namespace PastBeam.Tests
{
    public class Tests
    {
        public Tests()
        {
            // ������������ ����������� ��� ����������� (������ [SetUp] � NUnit)
        }

        [Fact] // � Xunit ������ [Test] ��������������� [Fact]
        public void Test1()
        {
            Assert.True(true); // � Xunit ���� Assert.Pass(), ������������ Assert.True()
        }
    }
}
