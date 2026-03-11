using F2.Testing.Ordering;
using Xunit;

namespace F2.Testing.Tests;

[Collection(nameof(ByPriorityOrder))]
[TestCaseOrderer(typeof(PriorityOrderer))]
public class ByPriorityOrder
{
    private static int _executionCount;

    [Fact, TestPriority(-5)]
    public void Test1()
    {
        Assert.Equal(0, _executionCount);
        _executionCount++;
    }

    [Fact]
    public void Test2A()
    {
        Assert.Equal(1, _executionCount);
        _executionCount++;
    }

    [Fact, TestPriority(0)]
    public void Test2B()
    {
        Assert.Equal(2, _executionCount);
        _executionCount++;
    }

    [Fact, TestPriority(5)]
    public void Test3()
    {
        Assert.Equal(3, _executionCount);
    }
}
