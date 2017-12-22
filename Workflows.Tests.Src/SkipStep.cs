namespace Workflows.Tests
{
    public class SkipStep : Step<ITestContext>
    {
        protected override void Execute(ITestContext context)
        {
            Skip();
        }
    }
}