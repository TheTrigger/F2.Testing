using Oibi.Demo;
using Oibi.TestHelper;
using Xunit;

namespace Oibi.Tests
{
    /// <summary>
    /// Basic configuration
    /// </summary>
    public class DemoTests : IClassFixture<ServerFixture<Startup>>
    {
        private readonly ServerFixture<Startup> _testFixure;

        public DemoTests(ServerFixture<Startup> testFixture)
        {
            _testFixure = testFixture;
        }


        [Fact]
        public void Test1()
        {

        }
    }
}
