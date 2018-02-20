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
        private readonly IDependencyExplorer _dependencyExplorer;
        private readonly DependencyGraphBuilder<Step<TCtx>> _dependencyGraphBuilder;

        private Action<Step<TCtx>, TCtx, Exception> _crashHandler;
        private Action<TCtx> _successHandler;

        public Workflow()
        {
            _steps = new List<Step<TCtx>>();
            _failedSteps = new List<Step<TCtx>>();
            _skippedSteps = new List<Step<TCtx>>();
            _passedSteps = new List<Step<TCtx>>();
            _dependencyExplorer = new DepenencyExplorer();
            _dependencyGraphBuilder = new DependencyGraphBuilder<Step<TCtx>>(_dependencyExplorer);
        }

        public void Execute(TCtx context)
        {
            EnsureAllRequiredStepsPresent();

            var orderedSteps = StepsSorter<TCtx>.OrderRequiredFirst(_dependencyGraphBuilder, _steps);

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
            if (HasRequiredDependencies(step) && 
                !_steps.Any(s => RequiresDependency(step, s)))
                    throw new MissingRequiredStepException(step.GetType());
        }

        private bool HasRequiredDependencies(Step<TCtx> step)
        {
            return _dependencyExplorer.HasRequired(step.GetStepType());
        }

        private bool RequiresDependency(Step<TCtx> step, Step<TCtx> s)
        {
            return _dependencyExplorer.Requires(step.GetStepType(), s.GetStepType());
        }

        private void ExecuteStep(TCtx context, Step<TCtx> step)
        {
            if (_failedSteps.Any(fs => RequiresDependency(step, fs)))
                MarkFailed(step);
            else if (_skippedSteps.Any(fs => RequiresDependency(step, fs)))
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
        {
            var stepType = typeof(TStep);

            if (_steps.Any(s=>s.GetType() == stepType))
                throw new StepAlreadyIncludedException();

            _steps.Add(CreateStepInstance<TStep>());
        }

        private static Step<TCtx> CreateStepInstance<TStep>()
        {
            var instance = WorkflowServices.Instance.StepActivator.Create<TStep>();
            return WorkflowServices.Instance.StepFactory.CreateFrom<TCtx>(instance);
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