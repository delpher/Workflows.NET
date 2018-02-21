using System;
using System.Linq;

namespace Workflows
{
    public class DepenencyExplorer : IDependencyExplorer
    {
        private readonly IWorkflowServices _workflowServices;

        public DepenencyExplorer(IWorkflowServices services)
        {
            _workflowServices = services;
        }

        public bool HasRequired(Type dependent)
        {
            return _workflowServices.DependencyExlorers.Any(x => x.HasRequired(dependent));
        }

        public bool Requires(Type dependent, Type required)
        {
            return _workflowServices.DependencyExlorers.Any(x => x.Requires(dependent, required));
        }
    }
}
