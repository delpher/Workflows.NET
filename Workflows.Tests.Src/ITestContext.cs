using System.Collections.Generic;

namespace Workflows.Tests
{
    public interface ITestContext
    {
        IDictionary<string, bool> TestFlags { get; }
        ITestOutput Out { get; }
    }
}