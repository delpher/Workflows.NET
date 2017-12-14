namespace Workflows
{
    public interface IStepActivator
    {
        object Create<TStep>();
    }
}