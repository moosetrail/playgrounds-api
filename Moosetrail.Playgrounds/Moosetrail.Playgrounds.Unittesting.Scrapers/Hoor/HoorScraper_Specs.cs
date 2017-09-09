using System.Threading.Tasks;
using Moosetrail.Playgrounds.Scrapers.Hoor;
using NUnit.Framework;

namespace Moosetrail.Playgrounds.Unittesting.Scrapers.Hoor
{
    [TestFixture]
    public class HoorScraper_Specs
    {
        private HoorScraper SUT;

        [SetUp]
        public void Setup()
        {
            SUT = new HoorScraper();
        }

        [TearDown]
        public void Teardown()
        {
            SUT = null;
        }

        [Test]
        public async Task debug_should_run_task()
        {
            // Given

            // When
            await SUT.Run();

            // Then

        }
    }
}