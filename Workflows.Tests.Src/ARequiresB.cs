namespace Workflows.Tests
{
    [Requires(typeof(BRequiresC))]
    public class ARequiresB : Step<ITestContext>
    {
        protected override void Execute(ITestContext context)
        {
            context.Out.Executed(this);
        }
    }
}