namespace Workflows.Tests
{
    public class FailingStep : Step<ITestContext>
    {
        protected override void Execute(ITestContext context)
        {
            Fail();
        }
    }
}