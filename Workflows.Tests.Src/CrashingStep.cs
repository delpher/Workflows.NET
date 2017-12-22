using System;

namespace Workflows.Tests
{
    public class CrashingStep : Step<ITestContext>
    {
        protected override void Execute(ITestContext context)
        {
            throw new InvalidOperationException();
        }
    }
}