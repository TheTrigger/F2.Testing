using System.Reflection;
using Xunit.Sdk;
using Xunit.v3;

namespace F2.Testing.Ordering;

/// <summary>
/// <see href="https://learn.microsoft.com/it-it/dotnet/core/testing/order-unit-tests?pivots=xunit"></see>
/// </summary>
public class PriorityOrderer : ITestCaseOrderer
{
    public IReadOnlyCollection<TTestCase> OrderTestCases<TTestCase>(IReadOnlyCollection<TTestCase> testCases)
        where TTestCase : notnull, ITestCase
    {
        var sortedMethods = new SortedDictionary<int, List<TTestCase>>();

        foreach (var testCase in testCases)
        {
            int priority = 0;

            if (testCase is IXunitTestCase xunitTestCase)
            {
                var attr = xunitTestCase.TestMethod.Method.GetCustomAttribute<TestPriorityAttribute>();
                if (attr != null)
                    priority = attr.Priority;
            }

            GetOrCreate(sortedMethods, priority).Add(testCase);
        }

        var result = new List<TTestCase>();
        foreach (var list in sortedMethods.Keys.Select(priority => sortedMethods[priority]))
        {
            list.Sort((x, y) => StringComparer.OrdinalIgnoreCase.Compare(x.TestMethodName, y.TestMethodName));
            result.AddRange(list);
        }
        return result;
    }

    static TValue GetOrCreate<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key) where TValue : new()
    {
        ArgumentNullException.ThrowIfNull(dictionary, nameof(dictionary));

        if (dictionary.TryGetValue(key, out TValue result)) return result;

        result = new TValue();
        dictionary[key] = result;

        return result;
    }
}
