using System;

namespace Workflows
{
    public abstract class Step<TCtx>
    {
        public void Execute(TCtx context, Workflow<TCtx> workflow)
        {
            try
            {
                Execute(context);
                workflow.Passed(this);
            }
            catch (StepSkippedException)
            {
                workflow.Skipped(this);
            }
            catch (StepFailedException)
            {
                workflow.Failed(this);
            }
        }

        public void Rollback(TCtx context, Workflow<TCtx> workflow)
        {
            try
            {
                Rollback(context);
            }
            catch (Exception exception)
            {
                workflow.Crashed(this, context, exception);
            }
        }

        protected abstract void Execute(TCtx context);

        public virtual void Rollback(TCtx context) { }

        protected void Fail()
        {
            throw new StepFailedException();
        }

        protected void Skip()
        {
            throw new StepSkippedException();
        }

        protected internal virtual Type GetStepType()
        {
            return GetType();
        }
    }
}