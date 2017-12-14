namespace Workflows.Sorting
{
    internal class Edge<T>
    {
        public T From { get; }

        public T To { get; }

        public Edge(T from, T to)
        {
            From = from;
            To = to;
        }
    }
}