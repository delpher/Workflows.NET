using System.Collections.Generic;
using System.Linq;
using Workflows.Sorting;

namespace Workflows
{
    internal static class DependencyGraphBuilder<T>
    {
        public static IEnumerable<Edge<T>> BuildDependencyGraph(IList<T> nodes)
        {
            return nodes.SelectMany(node => GetRequirements(node, nodes)).ToArray();
        }

        private static IEnumerable<Edge<T>> GetRequirements(T node, IEnumerable<T> nodes)
        {
            var requirements = nodes
                .Where(n=>HasDependency(node, n))
                .Select(n => new Edge<T>(node, n))
                .ToArray();
            return requirements;
        }

        private static bool HasDependency(T from, T to)
        {
            return DependencyDiscovery.Requires(from.GetType(), to.GetType());
        }
    }
}