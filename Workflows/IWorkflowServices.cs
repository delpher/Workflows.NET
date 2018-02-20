using System.Collections.Generic;

namespace Workflows
{
    public interface IWorkflowServices
    {
        IStepActivator StepActivator { get; set; }
        IStepFactory StepFactory { get; set; }
        List<IDependencyExplorer> DependencyExlorers { get; }
    }
}