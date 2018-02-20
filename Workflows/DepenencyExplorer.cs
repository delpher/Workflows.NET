using System;
using System.Linq;

namespace Workflows
{
    public class DepenencyExplorer : IDependencyExplorer
    {
        public bool HasRequired(Type dependent)
        {
            return WorkflowServices.Instance.DependencyExlorers.Any(x => x.HasRequired(dependent));
        }

        public bool Requires(Type dependent, Type required)
        {
            return WorkflowServices.Instance.DependencyExlorers.Any(x => x.Requires(dependent, required));
        }
    }
}
