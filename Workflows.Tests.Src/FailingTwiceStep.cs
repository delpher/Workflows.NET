namespace Workflows.Tests
{
    public class FailingTwiceStep : Step<ITestContext>
    {
        protected override void Execute(ITestContext context)
        {
            context.Out.Executed(this);
            Fail();

            context.Out.Executed(this);
            Fail();
        }
    }
}