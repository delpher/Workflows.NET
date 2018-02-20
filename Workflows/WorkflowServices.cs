using System.Collections.Generic;
using Workflows.DependencyExplorer;

namespace Workflows
{
    public class WorkflowServices : IWorkflowServices
    {
        private static IWorkflowServices _instance;

        public static IWorkflowServices Instance => _instance ?? (_instance = CreateInstance());

        public List<IDependencyExplorer> DependencyExlorers { get; private set; }

        private static IWorkflowServices CreateInstance()
        {
            return new WorkflowServices
            {
                StepActivator = new ReflectionStepActivator(),
                StepFactory = new DefaultStepFactory(),
                DependencyExlorers = new List<IDependencyExplorer>() {
                    new DependencyAttributeExplorer()
                }
            };
        }

        private WorkflowServices() { }

        public IStepActivator StepActivator { get; set; }

        public IStepFactory StepFactory { get; set; }
        
    }
}