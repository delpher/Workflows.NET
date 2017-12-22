namespace Workflows.Tests
{
    [Requires(typeof(SkipStep))]
    public class DependsOnSkippedStep : Step<ITestContext>
    {
        protected override void Execute(ITestContext context)
        {
            context.Out.Executed(this);
        }
    }
}