namespace Workflows.Tests
{
    public class TestStep : Step<ITestContext>
    {
        protected override void Execute(ITestContext context)
        {
            context.Out.Executed(this);
        }

        public override void Rollback(ITestContext context)
        {
            context.Out.Rollback(this);
        }
    }
}