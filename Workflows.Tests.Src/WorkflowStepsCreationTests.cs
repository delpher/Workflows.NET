using System;
using FluentAssertions;
using Xunit;

namespace Workflows.Tests
{
    public class WorkflowStepsCreationTests : WorkflowTestsBase
    {
        [Fact]
        public void Should_Throw_If_Step_Included_More_Than_Once()
        {
            Workflow.Add<TestStep>();
            Action addStep = () => Workflow.Add<TestStep>();
            addStep.ShouldThrow<StepAlreadyIncludedException>();
        }
    }

}