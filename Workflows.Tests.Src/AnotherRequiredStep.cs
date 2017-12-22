namespace Workflows.Tests
{
    public class AnotherRequiredStep : RequiredStep
    {
        protected override void Execute(ITestContext context)
        {
            if (context.TestFlags.ContainsKey("FailStep2"))
                Fail();

            context.Out.Executed(this);
        }
    }
}