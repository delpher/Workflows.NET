using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Workflows.DependencyExplorer;

namespace Workflows
{
    public class WorkflowFactory<TCtx>
    {
        private readonly Workflow<TCtx> _workflow;
        private readonly DependencyDictionaryExplorer _dependencyDictionaryExplorer;
        private readonly IWorkflowServices _workflowServices;

        public WorkflowFactory()
        {
            _dependencyDictionaryExplorer = new DependencyDictionaryExplorer();
            _workflowServices = WorkflowServices.CreateInstance();
            _workflowServices.DependencyExlorers.Add(_dependencyDictionaryExplorer);

            _workflow = new Workflow<TCtx>(_workflowServices);            
        }

        public WorkflowFactory(IEnumerable<Type> steps) : this()
        {
            foreach (var step in steps)
            {
                _workflow.Add(step);
            }            
        }

        public WorkflowFactory(IEnumerable<Step<TCtx>> steps) : this()
        {
            foreach (var step in steps)
            {
                _workflow.Add(step);
            }
        }

        public IDependencyBuilder AddStep(Type step)
        {
            _workflow.Add(step);
            return _dependencyDictionaryExplorer.Step(step);
        }

        public IDependencyBuilder AddStep<TStep>()
        {
            _workflow.Add<TStep>();
            return _dependencyDictionaryExplorer.Step(typeof(TStep));
        }

        public IDependencyBuilder AddStep(Step<TCtx> step)
        {
            _workflow.Add(step);
            return _dependencyDictionaryExplorer.Step(step.GetType());
        }

        public virtual Workflow<TCtx> Create()
        {
            return _workflow;
        }

        public void Use()
        {
            var factory = new WorkflowFactory<String>();

            IEnumerable<Type> addItionalSteps = Enumerable.Empty<Type>();

            if (true)
            {
                addItionalSteps = new List<Type>()
                {
                    typeof(int),
                    typeof(double)
                };
            }

            factory.AddStep<String>().Requires(true ? new[] { typeof(int) } : new Type[0]);
        }
    }
}
