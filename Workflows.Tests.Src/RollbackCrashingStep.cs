using System;

namespace Workflows.Tests
{
    public class RollbackCrashingStep : Step<ITestContext>
    {
        protected override void Execute(ITestContext context)
        {
            context.Out.Executed(this);
        }

        public override void Rollback(ITestContext context)
        {
            context.Out.Rollback(this);
            throw new InvalidOperationException();
        }
    }
}