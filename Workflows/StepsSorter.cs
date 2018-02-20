using System.Collections.Generic;
using System.Linq;
using Workflows.Sorting;

namespace Workflows
{
    internal static class StepsSorter<TCtx>
    {
        public static IEnumerable<Step<TCtx>> OrderRequiredFirst(DependencyGraphBuilder<Step<TCtx>> _graphBuilder, IEnumerable<Step<TCtx>> steps)
        {
            var sort = new TopologicalSort<Step<TCtx>>();
            var unsorted = steps.ToList();
            var edges = _graphBuilder.BuildDependencyGraph(unsorted).ToArray();
            return sort.Sort(unsorted.ToList(), edges);
        }
    }
}
