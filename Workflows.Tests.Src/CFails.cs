namespace Workflows.Tests
{
    public class CFails : Step<ITestContext>
    {
        protected override void Execute(ITestContext context)
        {
            context.Out.Executed(this);
            Fail();
        }
    }
}