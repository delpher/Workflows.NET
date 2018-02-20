using Xunit;

namespace Workflows.Tests.Src
{
    public class TestDependencyAttributeExplorer : TestDependencyExplorer
    {
        public TestDependencyAttributeExplorer() : base(new DependencyAttributeExplorer())
        {
            
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
