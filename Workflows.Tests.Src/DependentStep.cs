namespace Workflows.Tests
{
    [Requires(typeof(RequiredStep))]
    public class DependentStep : Step<ITestContext>
    {
        protected override void Execute(ITestContext context)
        {
            context.Out.Executed(this);
        }
    }
}