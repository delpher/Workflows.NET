using FluentAssertions;
using System;
using Workflows.DependencyExplorer;
using Xunit;

namespace Workflows.Tests.Src
{
    public class TestDependencyCollection
    {
        [Fact]
        public void AddRequiredSteps_Should_Throw()
        {
            DependencyCollection dc = new DependencyCollection();
            dc.AddRequiredSteps(new[] {typeof(String)});

            Action a = () => { dc.AddRequiredSteps(new[] {typeof(String)}); };

            a.ShouldThrow<DuplicateDependencyException>();
        }

        [Fact]
        public void AddRequiredSteps_Should_Throw_If_In_One_List()
        {
            DependencyCollection dc = new DependencyCollection();

            Action a = () => { dc.AddRequiredSteps(new[] { typeof(String), typeof(String), }); };

            a.ShouldThrow<DuplicateDependencyException>();
        }

        [Fact]
        public void AddRequiredAnySteps_Should_Throw()
        {
            DependencyCollection dc = new DependencyCollection();
            dc.AddRequiredAnySteps(new[] { typeof(String) });

            Action a = () => { dc.AddRequiredAnySteps(new[] { typeof(String) }); };

            a.ShouldThrow<DuplicateDependencyException>();
        }

        [Fact]
        public void AddRequiredAnySteps_Should_Throw_If_In_One_List()
        {
            DependencyCollection dc = new DependencyCollection();

            Action a = () => { dc.AddRequiredAnySteps(new[] { typeof(String), typeof(String), }); };

            a.ShouldThrow<DuplicateDependencyException>();
        }
    }
}
