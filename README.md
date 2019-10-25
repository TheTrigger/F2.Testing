# TestHelper

Don't be scared by tests 😨

```CSharp
using Oibi.TestHelper;
using Xunit;

namespace Oibi.Tests
{
    /// <summary>
    /// Basic configuration
    /// </summary>
    public class DemoTests : IClassFixture<ServerFixture<Startup>>
    {
        ServerFixture<Startup> _testFixure;

        public DemoTests(ServerFixture<Startup> testFixture)
        {
            _testFixure = testFixture;
        }
	}
}

```
