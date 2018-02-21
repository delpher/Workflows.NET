using System;
using System.Reflection;

namespace Workflows.StepAdapter
{
    public class StepAdapter<TCtx> : Step<TCtx>
    {
        private readonly object _instance;
        private readonly MethodInfo _executeMethod;

        static StepAdapter()
        {
            WorkflowServices.StepFactory = new AdaptedStepsFactory();
        }

        public StepAdapter(object instance)
        {
            _instance = instance;
            _executeMethod = StepAdapterHelper.GetExecuteMethod<TCtx>(_instance);
            StepAdapterHelper.AddFailedHandler(_instance, Failed);
            StepAdapterHelper.AddSkippedHandler(_instance, Skipped);
        }

        protected override void Execute(TCtx context)
        {
            try
            {
                _executeMethod.Invoke(_instance, new object[] {context});
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null)
                    throw ex.InnerException;
                throw;
            }
        }

        private void Skipped(object sender, EventArgs e)
        {
            throw new StepSkippedException();
        }

        private void Failed(object sender, EventArgs e)
        {
            throw new StepFailedException();
        }

        protected override Type GetStepType()
        {
            return _instance.GetType();
        }
    }
}