using Xunit;

namespace Workflows.Tests.Src
{
    public class TestDependencyDictionaryExplorer : TestDependencyExplorer
    {
        public TestDependencyDictionaryExplorer() : base(new DependencyDictionaryExplorer())
        {
            var explorer = (DependencyDictionaryExplorer)_dependencyExplorer;

            explorer.Step(typeof(A)).Requires(typeof(B));
            explorer.Step(typeof(C)).RequiresAny(typeof(A), typeof(B));
            explorer.Step(typeof(D)).Requires(typeof(A)).RequiresAny(typeof(B), typeof(C));
            explorer.Step(typeof(E));
        }

        [Fact]
        public override void TestDiscoveringRelationshipsBetweenTypes()
        {
            base.TestDiscoveringRelationshipsBetweenTypes();
        }

        [Fact]
        public override void TestBuildingDependencyGraph()
        {
            base.TestBuildingDependencyGraph();
        }
    }
}
