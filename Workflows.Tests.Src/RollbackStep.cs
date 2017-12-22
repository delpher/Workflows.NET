namespace Workflows.Tests
{
    public class RollbackStep : Step<ITestContext> 
    {
        protected override void Execute(ITestContext context)
        {
        }

        public override void Rollback(ITestContext context)
        {
            context.Out.Rollback(this);
        }
    }
}