using System;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Workflows.Tests
{
    public class WorkflowDependencyTests : WorkflowTestsBase
    {
        [Fact]
        public void Should_Execute_Dependent_Steps_In_Correct_Order()
        {
            Workflow.Add<DependentStep>();
            Workflow.Add<RequiredStep>();

            Workflow.Execute(Context);

            Received.InOrder(() =>
                {
                    Output.Executed(Arg.Any<RequiredStep>());
                    Output.Executed(Arg.Any<DependentStep>());
                });
        }

        [Fact]
        public void Should_Not_Execute_If_Required_Step_Failed()
        {
            Workflow.Add<DependentStep>();
            Workflow.Add<RequiredStep>();
            TestFlags.Add("FailStep1", true);

            Workflow.Execute(Context);

            Workflow.FailedSteps.Should().HaveCount(2);
            Context.Out.DidNotReceive().Executed(Arg.Any<DependentStep>());
        }

        [Fact]
        public void Should_Execute_If_Non_Required_Step_Failed()
        {
            Workflow.Add<DependentStep>();
            Workflow.Add<CrashingStep>();
            Workflow.Add<RequiredStep>();

            Workflow.Execute(Context);

            Output.Received().Executed(Arg.Any<RequiredStep>());
            Output.Received().Executed(Arg.Any<DependentStep>());
        }

        [Fact]
        public void Should_Throw_If_Required_Step_Not_Included()
        {
            Workflow.Add<DependentStep>();

            Action exec = () => Workflow.Execute(Context);
            exec.ShouldThrow<MissingRequiredStepException>();
        }

        [Fact]
        public void Should_Execute_Step_When_Any_Of_Required_Executed()
        {
            Workflow.Add<MultiDependentStep>();
            Workflow.Add<RequiredStep>();

            Workflow.Execute(Context);

            Output.Received().Executed(Arg.Any<RequiredStep>());
            Output.Received().Executed(Arg.Any<MultiDependentStep>());
        }

        [Fact]
        public void Should_Execute_Step_When_Any_Of_Required_Executed_Case2()
        {
            Workflow.Add<MultiDependentStep>();
            Workflow.Add<AnotherRequiredStep>();

            Workflow.Execute(Context);

            Output.Received().Executed(Arg.Any<AnotherRequiredStep>());
            Output.Received().Executed(Arg.Any<MultiDependentStep>());
        }

        [Fact]
        public void Should_Not_Execute_When_All_Required_Steps_Failed()
        {
            Workflow.Add<MultiDependentStep>();
            Workflow.Add<RequiredStep>();
            Workflow.Add<AnotherRequiredStep>();
            TestFlags.Add("FailStep1", true);
            TestFlags.Add("FailStep2", true);

            Workflow.Execute(Context);

            Output.DidNotReceive().Executed(Arg.Any<MultiDependentStep>());
        }

        [Fact]
        public void Should_Not_Execute_If_Reqired_Step_Was_Not_Executed()
        {
            Workflow.Add<ARequiresB>();            
            Workflow.Add<BRequiresC>();            
            Workflow.Add<CFails>();            

            Workflow.Execute(Context);

            Output.Received().Executed(Arg.Any<CFails>());
            Output.DidNotReceive().Executed(Arg.Any<BRequiresC>());
            Output.DidNotReceive().Executed(Arg.Any<ARequiresB>());
        }

        [Fact]
        public void Should_Not_Execute_Step_If_Any_Of_Required_Skipped()
        {
            Workflow.Add<DependsOnSkippedStep>();
            Workflow.Add<SkipStep>();

            Workflow.Execute(Context);

            Output.DidNotReceive().Executed(Arg.Any<DependsOnSkippedStep>());
        }

        [Fact]
        public void If_Required_Step_Was_Skipped_Dependent_Should_Also_Be_Skipped()
        {
            Workflow.Add<DependsOnSkippedStep>();
            Workflow.Add<SkipStep>();

            Workflow.Execute(Context);

            Workflow.SkippedSteps.Should().HaveCount(2);
            Workflow.PassedSteps.Should().HaveCount(0);
            Workflow.FailedSteps.Should().HaveCount(0);
        }

        [Fact]
        public void Should_Stop_Workflow_Execution_When_Step_Crashes()
        {
            Workflow.Add<CrashingStep>();
            Workflow.Add<TestStep>();

            Workflow.Execute(Context);

            Context.Out.DidNotReceive().Executed(Arg.Any<TestStep>());
        }
    }

    [Requires(typeof(SkipStep))]
    public class DependsOnSkippedStep : Step<ITestContext>
    {
        protected override void Execute(ITestContext context)
        {
            context.Out.Executed(this);
        }
    }

    [Requires(typeof(BRequiresC))]
    public class ARequiresB : Step<ITestContext>
    {
        protected override void Execute(ITestContext context)
        {
            context.Out.Executed(this);
        }
    }

    [Requires(typeof(CFails))]
    public class BRequiresC : Step<ITestContext>
    {
        protected override void Execute(ITestContext context)
        {
            context.Out.Executed(this);
        }
    }

    public class CFails : Step<ITestContext>
    {
        protected override void Execute(ITestContext context)
        {
            context.Out.Executed(this);
            Fail();
        }
    }

    [Requires(typeof(RequiredStep))]
    public class DependentStep : Step<ITestContext>
    {
        protected override void Execute(ITestContext context)
        {
            context.Out.Executed(this);
        }
    }

    [RequiresAny(typeof(RequiredStep), typeof(AnotherRequiredStep))]
    public class MultiDependentStep : Step<ITestContext>
    {
        protected override void Execute(ITestContext context)
        {
            context.Out.Executed(this);
        }
    }

    public class RequiredStep : Step<ITestContext>
    {
        protected override void Execute(ITestContext context)
        {
            if (context.TestFlags.ContainsKey("FailStep1"))
                Fail();

            context.Out.Executed(this);
        }
    }

    public class AnotherRequiredStep : RequiredStep
    {
        protected override void Execute(ITestContext context)
        {
            if (context.TestFlags.ContainsKey("FailStep2"))
                Fail();

            context.Out.Executed(this);
        }
    }
}
