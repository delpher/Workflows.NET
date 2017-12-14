using System;
using System.Collections.Generic;
using FluentAssertions;
using Workflows.Sorting;
using Xunit;

namespace Workflows.Tests
{
    public class TopologicalSortTests
    {
        private readonly TopologicalSort<string> _topologicalSort = new TopologicalSort<string>();
        private string[] _nodes;
        private List<Edge<string>> _edges;
        private void Nodes(params string[] nodes)
        {
            _nodes = nodes;
        }

        private void Edge(string from, params string[] targets)
        {
            foreach (var to in targets)
                Edge(from, to);
        }

        private void Edge(string from, string to)
        {
            if (_edges == null) _edges = new List<Edge<string>>();
            _edges.Add(new Edge<string>(from, to));
        }


        private IEnumerable<string> Sorted()
        {
            return _topologicalSort.Sort(_nodes, _edges?.ToArray());
        }

        private void AssertCircularDependencyDetected()
        {
            Action sort = () => Sorted();
            sort.ShouldThrow<CircularDependencyException>();
        }

        [Fact]
        public void Given_Null_Returns_Empty()
        {
            Sorted().Should().BeEmpty();
        }

        [Fact]
        public void Given_Empty_Returns_Empty()
        {
            _nodes = new string[0];
            Sorted().Should().BeEmpty();
        }

        [Fact]
        public void Given_One_Node_Returns_One()
        {
            Nodes("A");
            Sorted().Should()
                .HaveCount(1)
                .And.Contain("A");
        }

        [Fact]
        public void Given_Two_Related_Should_Sort()
        {
            Nodes("A", "B");
            Edge("A", "B");

            Sorted().Should().HaveCount(2)
                .And.ContainInOrder("B", "A");
        }

        [Fact]
        public void Given_Three_Nodes_In_A_Fork_Should_Sort()
        {
            Nodes("A", "B", "C");
            Edge("A", "B");
            Edge("A", "C");

            Sorted().Should().HaveCount(3)
                .And.ContainInOrder("B", "A")
                .And.ContainInOrder("C", "A");
        }

        [Fact]
        public void Given_Three_Dependent_Should_Sort()
        {
            Nodes("A", "B", "C");
            Edge("A", "B");
            Edge("B", "C");

            Sorted().Should().HaveCount(3)
                .And.ContainInOrder("C", "B", "A");
        }

        [Fact]
        public void Given_Four_Nodes_Should_Sort()
        {
            Nodes("A", "B", "C", "D");
            Edge("A", "B");
            Edge("B", "C");
            Edge("B", "D");

            Sorted().Should().HaveCount(4)
                .And.ContainInOrder("B", "A")
                .And.ContainInOrder("C", "B")
                .And.ContainInOrder("D", "B");
        }

        [Fact]
        public void Given_Four_Nodes_Should_Sort_Case2()
        {
            Nodes("A", "B", "C", "D");
            Edge("A", "B");
            Edge("B", "C");
            Edge("B", "D");
            Edge("C", "D");

            Sorted().Should().HaveCount(4)
                .And.ContainInOrder("B", "A")
                .And.ContainInOrder("C", "B")
                .And.ContainInOrder("D", "B");
        }

        [Fact]
        public void Given_Partitioned_Should_Sort()
        {
            Nodes("A", "B", "C", "D");
            Edge("A", "B");
            Edge("C", "D");

            Sorted().Should().HaveCount(4)
                .And.ContainInOrder("B", "A")
                .And.ContainInOrder("D", "C");
        }

        [Fact]
        public void Given_Circular_Dependency_Throws_Exception()
        {
            Nodes("A", "B");
            Edge("A", "B");
            Edge("B", "A");

            AssertCircularDependencyDetected();
        }

        [Fact]
        public void Given_Circular_Dependency_Throws_Exception_Case2()
        {
            Nodes("A", "B", "C", "D");
            Edge("A", "B");
            Edge("B", "C");
            Edge("C", "D");
            Edge("D", "A");

            AssertCircularDependencyDetected();
        }

        [Fact]
        public void Given_Four_Nodes_With_Circular_Dependency_Should_Throw()
        {
            Nodes("A", "B", "C", "D");
            Edge("A", "B");
            Edge("B", "C");
            Edge("B", "D");
            Edge("C", "D");
            Edge("D", "A");

            AssertCircularDependencyDetected();
        }

        [Fact]
        public void Given_Complex_Graph_Should_Sort()
        {
            Nodes("m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z");
            Edge("m", "x", "q", "r");
            Edge("n", "q", "u", "o");
            Edge("o", "r", "v", "s");
            Edge("p", "o", "s", "z");
            Edge("q", "t");
            Edge("r", "u", "y");
            Edge("s", "r");
            Edge("u", "t");
            Edge("v", "x", "w");
            Edge("w", "z");
            Edge("y", "v");

            Sorted().Should().HaveCount(14)
                .And.ContainInOrder("x", "m")
                .And.ContainInOrder("q", "m")
                .And.ContainInOrder("r", "m")
                .And.ContainInOrder("q", "n")
                .And.ContainInOrder("u", "n")
                .And.ContainInOrder("o", "n")
                .And.ContainInOrder("r", "o")
                .And.ContainInOrder("v", "o")
                .And.ContainInOrder("s", "o")
                .And.ContainInOrder("o", "p")
                .And.ContainInOrder("s", "p")
                .And.ContainInOrder("z", "p")
                .And.ContainInOrder("t", "q")
                .And.ContainInOrder("u", "r")
                .And.ContainInOrder("y", "r")
                .And.ContainInOrder("r", "s")
                .And.ContainInOrder("t", "u")
                .And.ContainInOrder("x", "v")
                .And.ContainInOrder("w", "v")
                .And.ContainInOrder("z", "w")
                .And.ContainInOrder("v", "y");
        }
    }
}
