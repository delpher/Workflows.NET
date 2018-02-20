using System.Linq;
using FluentAssertions;
using Xunit;

namespace Workflows.Tests
{
    public class TestDependencyExplorer
    {
        protected readonly  IDependencyExplorer _dependencyExplorer;

        public TestDependencyExplorer(IDependencyExplorer dependencyExplorer)
        {
            _dependencyExplorer = dependencyExplorer;
        }

        public virtual void TestDiscoveringRelationshipsBetweenTypes()
        {
            _dependencyExplorer.Requires(typeof(A), typeof(B)).Should().BeTrue();
            _dependencyExplorer.Requires(typeof(B), typeof(A)).Should().BeFalse();

            _dependencyExplorer.Requires(typeof(C), typeof(A)).Should().BeTrue();
            _dependencyExplorer.Requires(typeof(C), typeof(B)).Should().BeTrue();
            _dependencyExplorer.Requires(typeof(B), typeof(C)).Should().BeFalse();
            _dependencyExplorer.Requires(typeof(A), typeof(C)).Should().BeFalse();

            _dependencyExplorer.Requires(typeof(D), typeof(A)).Should().BeTrue();
            _dependencyExplorer.Requires(typeof(D), typeof(B)).Should().BeTrue();
            _dependencyExplorer.Requires(typeof(D), typeof(C)).Should().BeTrue();
            _dependencyExplorer.Requires(typeof(D), typeof(E)).Should().BeFalse();
        }

        public virtual void TestBuildingDependencyGraph()
        {
            var a = new A();
            var b = new B();
            var c = new C();
            var d = new D();

            var nodes = new object[] {a, b, c, d};

            

            var edges = new DependencyGraphBuilder<object>(_dependencyExplorer)
                        .BuildDependencyGraph(nodes).ToArray();

            edges.Should().Contain(e => e.From == a && e.To == b);
            edges.Should().Contain(e => e.From == c && e.To == a);
            edges.Should().Contain(e => e.From == c && e.To == b);
            edges.Should().Contain(e => e.From == d && e.To == a);
            edges.Should().Contain(e => e.From == d && e.To == b);
            edges.Should().Contain(e => e.From == d && e.To == c);
        }
    }

    [Requires(typeof(B))]
    public class A { }

    public class B { }

    [RequiresAny(typeof(A), typeof(B))]
    public class C { }

    [Requires(typeof(A))]
    [RequiresAny(typeof(B), typeof(C))]
    public class D { }

    public class E { }
}
