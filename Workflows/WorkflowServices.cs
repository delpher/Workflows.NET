namespace Workflows
{
    public class WorkflowServices : IWorkflowServices
    {
        private static IWorkflowServices _instance;

        public static IWorkflowServices Instance => _instance ?? (_instance = CreateInstance());

        private static IWorkflowServices CreateInstance()
        {
            return new WorkflowServices
            {
                StepActivator = new ReflectionStepActivator(),
                StepFactory = new DefaultStepFactory()
            };
        }

        private WorkflowServices() { }

        public IStepActivator StepActivator { get; set; }

        public IStepFactory StepFactory { get; set; }
    }
}