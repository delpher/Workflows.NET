namespace Workflows.Tests
{
    [RequiresAny(typeof(RequiredStep), typeof(AnotherRequiredStep))]
    public class MultiDependentStep : Step<ITestContext>
    {
        protected override void Execute(ITestContext context)
        {
            context.Out.Executed(this);
        }
    }
}