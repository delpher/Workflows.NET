using System.Linq;
using FluentAssertions;
using Xunit;

namespace Workflows.Tests
{
    public class TestDependencyDiscovery
    {
        [Fact]
        public void TestDiscoveringRelationshipsBetweenTypes()
        {
            DependencyDiscovery.Requires(typeof(A), typeof(B)).Should().BeTrue();
            DependencyDiscovery.Requires(typeof(B), typeof(A)).Should().BeFalse();

            DependencyDiscovery.Requires(typeof(C), typeof(A)).Should().BeTrue();
            DependencyDiscovery.Requires(typeof(C), typeof(B)).Should().BeTrue();
            DependencyDiscovery.Requires(typeof(B), typeof(C)).Should().BeFalse();
            DependencyDiscovery.Requires(typeof(A), typeof(C)).Should().BeFalse();

            DependencyDiscovery.Requires(typeof(D), typeof(A)).Should().BeTrue();
            DependencyDiscovery.Requires(typeof(D), typeof(B)).Should().BeTrue();
            DependencyDiscovery.Requires(typeof(D), typeof(C)).Should().BeTrue();
            DependencyDiscovery.Requires(typeof(D), typeof(E)).Should().BeFalse();
        }

        [Fact]
        public void TestBuildingDependencyGraph()
        {
            var a = new A();
            var b = new B();
            var c = new C();
            var d = new D();

            var nodes = new object[] {a, b, c, d};

            var edges = DependencyGraphBuilder<object>.BuildDependencyGraph(nodes).ToArray();

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
