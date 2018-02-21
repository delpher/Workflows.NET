using System.Collections.Generic;
using Workflows.DependencyExplorer;

namespace Workflows
{
    public class WorkflowServices : IWorkflowServices
    {
        public static IWorkflowServices CreateInstance()
        {
            return new WorkflowServices
            {
                StepActivator = new ReflectionStepActivator(),                
                DependencyExlorers = new List<IDependencyExplorer>() {
                    new DependencyAttributeExplorer()
                }
            };
        }

        private WorkflowServices() { }        
        
        public IStepActivator StepActivator { get; set; }

        public static IStepFactory StepFactory { get; set; } = new DefaultStepFactory();

        public List<IDependencyExplorer> DependencyExlorers { get; private set; }

    }
}