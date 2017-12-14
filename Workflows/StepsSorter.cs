using System.Collections.Generic;
using System.Linq;
using Workflows.Sorting;

namespace Workflows
{
    internal static class StepsSorter<TCtx>
    {
        public static IEnumerable<Step<TCtx>> OrderRequiredFirst(IEnumerable<Step<TCtx>> steps)
        {
            var sort = new TopologicalSort<Step<TCtx>>();
            var unsorted = steps.ToList();
            var edges = DependencyGraphBuilder<Step<TCtx>>.BuildDependencyGraph(unsorted).ToArray();
            return sort.Sort(unsorted.ToList(), edges);
        }
    }
}
