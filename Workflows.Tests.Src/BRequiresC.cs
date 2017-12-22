namespace Workflows.Tests
{
    [Requires(typeof(CFails))]
    public class BRequiresC : Step<ITestContext>
    {
        protected override void Execute(ITestContext context)
        {
            context.Out.Executed(this);
        }
    }
}