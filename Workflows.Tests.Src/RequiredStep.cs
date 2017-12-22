namespace Workflows.Tests
{
    public class RequiredStep : Step<ITestContext>
    {
        protected override void Execute(ITestContext context)
        {
            if (context.TestFlags.ContainsKey("FailStep1"))
                Fail();

            context.Out.Executed(this);
        }
    }
}