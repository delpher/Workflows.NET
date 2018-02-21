using System.Collections.Generic;

namespace Workflows
{
    public interface IWorkflowServices
    {
        IStepActivator StepActivator { get; set; }        
        List<IDependencyExplorer> DependencyExlorers { get; }
    }
}