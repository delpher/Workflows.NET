namespace Workflows.Tests
{
    [Requires(typeof(A))]
    [RequiresAny(typeof(B), typeof(C))]
    public class D { }
}