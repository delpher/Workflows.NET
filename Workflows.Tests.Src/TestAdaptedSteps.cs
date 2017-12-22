using System;
using FluentAssertions;
using NSubstitute;
using Workflows.StepAdapter;
using Xunit;

namespace Workflows.Tests
{
    public class TestAdaptedSteps
    {
        private readonly TestWorkflow _workflow;
        private readonly ITestContext _context;
        private readonly ITestOutput _out;

        public TestAdaptedSteps()
        {
            WorkflowServices.Instance.StepFactory = new AdaptedStepsFactory();
            _workflow = new TestWorkflow();
            _context = Substitute.For<ITestContext>();
            _out = Substitute.For<ITestOutput>();
            _context.Out.Returns(_out);
        }

        [Fact]
        public void Should_Execute_Step_Of_Any_Type()
        {
            _workflow.Add<TestAdaptedStep>();
            _workflow.Execute(_context);
            _out.Received().Executed(Arg.Any<TestAdaptedStep>());
        }

        [Fact]
        public void Should_Handle_Adapted_Step_Failure()
        {
            _workflow.Add<TestAdaptedFailingStep>();
            _workflow.Execute(_context);
            _workflow.FailedSteps.Should().HaveCount(1);
            _workflow.FailedSteps[0].Should().BeOfType<StepAdapter<ITestContext>>();
        }

        [Fact]
        public void Should_Handle_Addapted_Step_Skip()
        {
            _workflow.Add<TestAdaptedSkippedStep>();
            _workflow.Execute(_context);
            _workflow.SkippedSteps.Should().HaveCount(1);
            _workflow.SkippedSteps[0].Should().BeOfType<StepAdapter<ITestContext>>();
        }

        [Fact]
        public void Should_Handle_Adapted_Steps_Requirements()
        {
            var requiredAdapter = new StepAdapter<ITestContext>(new RequiredAdaptedStep());
            var dependentAdapter = new StepAdapter<ITestContext>(new DependentAdaptedStep());

            dependentAdapter.Requires(requiredAdapter)
                .Should().BeTrue("adapted steps has dependency defined");

            dependentAdapter.Requires(new RequiredNotAdaptedStep())
                .Should().BeTrue();

            new DependentNotAdaptedStep().Requires(requiredAdapter)
                .Should().BeTrue();

            dependentAdapter.HasDependencies().Should().BeTrue();
        }
    }

    public class TestAdaptedStep
    {
        public void Execute(ITestContext ctx)
        {
            ctx.Out.Executed(this);
        }
    }

    public class TestAdaptedFailingStep
    {
        public event EventHandler Failed;

        public void Execute(ITestContext ctx)
        {
            Fail();
        }

        private void Fail()
        {
            Failed?.Invoke(this, EventArgs.Empty);
        }
    }

    public class TestAdaptedSkippedStep
    {
        public event EventHandler Skipped;

        public void Execute(ITestContext ctx)
        {
            Skip();
        }

        private void Skip()
        {
            Skipped?.Invoke(this, EventArgs.Empty);
        }
    }

    public class RequiredAdaptedStep
    {

    }

    public class RequiredNotAdaptedStep : Step<ITestContext>
    {
        protected override void Execute(ITestContext context)
        {
        }
    }

    [Requires(typeof(RequiredNotAdaptedStep))]
    [Requires(typeof(RequiredAdaptedStep))]
    public class DependentAdaptedStep
    {

    }

    [Requires(typeof(RequiredAdaptedStep))]
    public class DependentNotAdaptedStep : Step<ITestContext>
    {
        protected override void Execute(ITestContext context)
        {
        }
    }
}
