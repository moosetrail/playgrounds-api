using NUnit.Framework;

namespace Moosetrail.Playgrounds.Unittesting.API
{
    [TestFixture]
    public class TestCaseSpecs
    {
        private string SUT;

        [SetUp]
        public void Setup()
        {
          
        }

        [TearDown]
        public void Teardown()
        {
            SUT = null;
        }

        [Test]
        public void this_is_my_test()
        {
            // Given

            // When

            // Then

        }
    }
}