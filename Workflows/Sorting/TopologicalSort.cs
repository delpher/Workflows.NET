using System.Collections.Generic;
using System.Linq;

namespace Workflows.Sorting
{
    internal class TopologicalSort<T>
    {
        private List<T> _sorted;
        private Edge<T>[] _edges;
        private List<T> _visited;

        public List<T> Sort(IList<T> nodes, Edge<T>[] edges)
        {
            _sorted = new List<T>();

            if (nodes == null)
                return _sorted;

            if (edges == null || edges.Length == 0)
                return nodes.ToList();

            _edges = edges;
            _visited = new List<T>();

            foreach (var node in nodes)
                Visit(node);

            return _sorted;
        }

        private void Visit(T node)
        {
            if (_sorted.Contains(node)) return;

            if (_visited.Contains(node))
                throw new CircularDependencyException(node);

            _visited.Add(node);

            foreach (var sibling in GetSiblings(node))
                Visit(sibling);

            _sorted.Add(node);
        }

        private IEnumerable<T> GetSiblings(T current)
        {
            return _edges
                .Where(e => e.From.Equals(current))
                .Select(e => e.To)
                .ToList();
        }
    }
}