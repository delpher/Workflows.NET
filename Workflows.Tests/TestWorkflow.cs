using System.Collections.Generic;

namespace Workflows.Tests
{
    public class TestWorkflow : Workflow<ITestContext>
    {
        public readonly List<Step<ITestContext>> PassedSteps;
        public readonly List<Step<ITestContext>> FailedSteps;
        public readonly List<Step<ITestContext>> SkippedSteps;

        public TestWorkflow()
        {
            FailedSteps = new List<Step<ITestContext>>();
            PassedSteps = new List<Step<ITestContext>>();
            SkippedSteps = new List<Step<ITestContext>>();
        }

        protected override void MarkFailed(Step<ITestContext> step)
        {
            base.MarkFailed(step);
            FailedSteps.Add(step);
        }

        protected override void MarkPassed(Step<ITestContext> step)
        {
            base.MarkPassed(step);
            PassedSteps.Add(step);
        }

        protected override void MarkSkipped(Step<ITestContext> step)
        {
            base.MarkSkipped(step);
            SkippedSteps.Add(step);
        }
    }
}