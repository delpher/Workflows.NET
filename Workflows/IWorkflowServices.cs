namespace Workflows
{
    public interface IWorkflowServices
    {
        IStepActivator StepActivator { get; set; }
    }
}