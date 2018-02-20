using System.Collections.Generic;
using System.Linq;
using Workflows.Sorting;

namespace Workflows
{
    internal class DependencyGraphBuilder<T>
    {
        private readonly IDependencyExplorer _dependencyExplorer;

        public DependencyGraphBuilder(IDependencyExplorer dependencyExplorer)
        {
            _dependencyExplorer = dependencyExplorer;
        }

        public IEnumerable<Edge<T>> BuildDependencyGraph(IList<T> nodes)
        {
            return nodes.SelectMany(node => GetRequirements(node, nodes)).ToArray();
        }

        private IEnumerable<Edge<T>> GetRequirements(T node, IEnumerable<T> nodes)
        {
            var requirements = nodes
                .Where(n=> HasDependency(node, n))
                .Select(n => new Edge<T>(node, n))
                .ToArray();
            return requirements;
        }

        private  bool HasDependency(T from, T to)
        {
            return _dependencyExplorer.Requires(from.GetType(), to.GetType());
        }
    }
}