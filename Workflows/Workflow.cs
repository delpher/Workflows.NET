using System;
using System.Collections.Generic;
using System.Linq;

namespace Workflows
{
    public class Workflow<TCtx>
    {
        private readonly List<Step<TCtx>> _steps;
        private readonly List<Step<TCtx>> _failedSteps;
        private readonly List<Step<TCtx>> _skippedSteps;
        private readonly List<Step<TCtx>> _passedSteps;

        private Action<Step<TCtx>, TCtx, Exception> _crashHandler;
        private Action<TCtx> _successHandler;

        public Workflow()
        {
            _steps = new List<Step<TCtx>>();
            _failedSteps = new List<Step<TCtx>>();
            _skippedSteps = new List<Step<TCtx>>();
            _passedSteps = new List<Step<TCtx>>();
        }

        public void Execute(TCtx context)
        {
            EnsureAllRequiredStepsPresent();

            var orderedSteps = StepsSorter<TCtx>.OrderRequiredFirst(_steps);

            foreach (var step in orderedSteps)
                try
                {
                    ExecuteStep(context, step);
                }
                catch (Exception exception)
                {
                    Crashed(step, context, exception);
                    break;
                }

            HandleWorkflowCompleted(context);
        }

        private void HandleWorkflowCompleted(TCtx context)
        {
            if (_failedSteps.Any())
                RollbackPassedSteps(context);
            else
                _successHandler?.Invoke(context);
        }

        private void RollbackPassedSteps(TCtx context)
        {
            foreach (var step in _passedSteps)
                step.Rollback(context, this);
        }

        private void EnsureAllRequiredStepsPresent()
        {
            foreach (var step in _steps)
                EnsureRequiredStepsPresent(step);
        }

        private void EnsureRequiredStepsPresent(Step<TCtx> step)
        {
            if (step.HasDependencies() && !_steps.Any(step.Requires))
                throw new MissingRequiredStepException(step.GetType());
        }

        private void ExecuteStep(TCtx context, Step<TCtx> step)
        {
            if (_failedSteps.Any(step.Requires))
                MarkFailed(step);
            else if (_skippedSteps.Any(step.Requires))
                MarkSkipped(step);
            else
                step.Execute(context, this);
        }

        internal void Passed(Step<TCtx> step)
        {
            MarkPassed(step);
        }

        protected virtual void MarkPassed(Step<TCtx> step)
        {
            _passedSteps.Add(step);
        }

        internal void Crashed(Step<TCtx> step, object context, Exception exception)
        {
            MarkFailed(step);
            _crashHandler?.Invoke(step, (TCtx)context, exception);
        }

        internal void Failed(Step<TCtx> step)
        {
            MarkFailed(step);
        }

        protected virtual void MarkFailed(Step<TCtx> step)
        {
            _failedSteps.Add(step);
        }

        public void Add<TStep>()
            where TStep : Step<TCtx>
        {
            var stepType = typeof(TStep);

            if (_steps.Any(s=>s.GetType() == stepType))
                throw new StepAlreadyIncludedException();

            _steps.Add((Step<TCtx>) WorkflowServices.Instance.StepActivator.Create<TStep>());
        }

        internal void Skipped(Step<TCtx> step)
        {
            MarkSkipped(step);
        }

        protected virtual void MarkSkipped(Step<TCtx> step)
        {
            _skippedSteps.Add(step);
        }

        public void OnCrash(Action<Step<TCtx>, TCtx, Exception> handler)
        {
            _crashHandler = handler;
        }

        public void OnSuccess(Action<TCtx> handler)
        {
            _successHandler = handler;
        }
    }
}