using System;

namespace Workflows
{
    public class DefaultStepFactory : IStepFactory
    {
        public Step<TCtx> CreateFrom<TCtx>(object stepInstance)
        {
            if (!(stepInstance is Step<TCtx>))
            throw new NotSupportedException("Creating instances of steps not inheriting " +
                                            "from Step<T> base class is not supported by DefaultStepFactory." +
                                            "Use Workflows.NET.StepAdapter library for this. If you already using it," +
                                            "don't forget to register AdaptedStepFactory in WorkflowServices. See documentation" +
                                            "at http://alex-onashyuk.azurewebsites.net/Projects/Workflows");
            return (Step<TCtx>)stepInstance;
        }
    }
}