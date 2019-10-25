# TestHelper

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/643b3b1a38644b0dab7f6d160bd08ec8)](https://www.codacy.com/manual/TheTrigger/TestHelper?utm_source=github.com&utm_medium=referral&utm_content=TheTrigger/TestHelper&utm_campaign=Badge_Grade)

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
