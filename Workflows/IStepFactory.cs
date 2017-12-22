namespace Workflows
{
    public interface IStepFactory
    {
        Step<TCtx> CreateFrom<TCtx>(object stepInstance);
    }
}