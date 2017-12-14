using System;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Workflows.Tests
{
    public class WorkflowExecutionTests : WorkflowTestsBase
    {
        [Fact]
        public void Empty_Workflow_Executes_Successfully()
        {
            Workflow.Execute(Context);
            Output.DidNotReceive();
        }

        [Fact]
        public void Should_Execute_Step()
        {
            Workflow.Add<TestStep>();

            Workflow.Execute(Context);

            Workflow.PassedSteps.Should().HaveCount(1);
        }

        [Fact]
        public void Given_Step_Thrown_Exception_Should_Fail_Step()
        {
            Workflow.Add<CrashingStep>();

            Workflow.Execute(Context);

            Workflow.PassedSteps.Should().HaveCount(0);
            Workflow.FailedSteps.Should().HaveCount(1);
        }

        [Fact]
        public void Given_Step_Fails_Should_Mark_It_As_Failed()
        {
            Workflow.Add<FailingStep>();

            Workflow.Execute(Context);

            Workflow.PassedSteps.Should().HaveCount(0);
            Workflow.FailedSteps.Should().HaveCount(1);
        }

        [Fact]
        public void Given_Failure_Handler_Should_Invoke_When_Step_Fails()
        {
            Step<ITestContext> crashedStep = null;

            Workflow.Add<CrashingStep>();
            Workflow.OnCrash((step, ctx, ex) =>
            {
                ctx.Out.Failed(ex);
                crashedStep = step;
            });

            Workflow.Execute(Context);

            Context.Out.Received().Failed(Arg.Any<InvalidOperationException>());
            crashedStep.Should().NotBe(null);
            crashedStep.Should().BeOfType<CrashingStep>();
        }

        [Fact]
        public void Failure_Handler_Should_Not_Invoke_On_Success()
        {
            Workflow.Add<TestStep>();
            Workflow.OnCrash((step, ctx, ex) => ctx.Out.Failed(ex));

            Workflow.Execute(Context);

            Context.Out.DidNotReceive().Failed(Arg.Any<Exception>());
        }

        [Fact]
        public void Should_Execute_Success_Handler_On_Success()
        {
            Workflow.Add<TestStep>();
            Workflow.OnSuccess(ctx => ctx.Out.Success());

            Workflow.Execute(Context);

            Context.Out.Received().Success();
        }

        [Fact]
        public void Should_Not_Execute_Success_Handler_When_Steps_Failed()
        {
            Workflow.Add<TestStep>();
            Workflow.Add<FailingStep>();
            Workflow.OnSuccess(ctx => ctx.Out.Success());

            Workflow.Execute(Context);

            Context.Out.DidNotReceive().Success();
        }

        [Fact]
        public void Should_Not_Execute_Success_Handler_When_Steps_Crashed()
        {
            Workflow.Add<TestStep>();
            Workflow.Add<CrashingStep>();

            Workflow.Execute(Context);

            Context.Out.DidNotReceive().Success();
        }

        [Fact]
        public void Should_Add_Steps_To_Failed_If_Step_Invoked_Fail_Method()
        {
            Workflow.Add<FailingStep>();

            Workflow.Execute(Context);

            Workflow.FailedSteps.Should().HaveCount(1);
        }

        [Fact]
        public void Given_Step_Calls_Fail_Twice_Should_Be_Added_To_Failed_Only_Once()
        {
            Workflow.Add<FailingTwiceStep>();

            Workflow.Execute(Context);

            Workflow.FailedSteps.Should().HaveCount(1);
            Context.Out.Received(1).Executed(Arg.Any<FailingTwiceStep>());
        }

        [Fact]
        public void Given_Test_Calls_Skip_Should_Be_Skiped()
        {
            Workflow.Add<SkipStep>();

            Workflow.Execute(Context);

            Workflow.FailedSteps.Should().HaveCount(0);
            Workflow.PassedSteps.Should().HaveCount(0);
            Workflow.SkippedSteps.Should().HaveCount(1);
        }

        [Fact]
        public void Given_Workflow_Failed_Should_Rollback_Passed_Steps()
        {
            Workflow.Add<TestStep>();
            Workflow.Add<FailingStep>();

            Workflow.Execute(Context);

            Context.Out.Received(1).Rollback(Arg.Any<TestStep>());
        }

        [Fact]
        public void Given_Step_Crashed_Should_Rollback_Passed_Steps()
        {
            Workflow.Add<TestStep>();
            Workflow.Add<CrashingStep>();

            Workflow.Execute(Context);

            Context.Out.Received(1).Rollback(Arg.Any<TestStep>());
        }

        [Fact]
        public void Given_Rollback_Crashed_Should_Invoke_Crash_Handler()
        {
            Workflow.Add<RollbackCrashingStep>();
            Workflow.Add<TestStep>();
            Workflow.Add<FailingStep>();

            Exception caughtException = null;
            Step<ITestContext> crashedStep = null;
            Workflow.OnCrash((step, ctx, exception) =>
            {
                caughtException = exception;
                crashedStep = step;
            });

            Workflow.Execute(Context);

            Context.Out.Received(1).Rollback(Arg.Any<RollbackCrashingStep>());
            Context.Out.Received(1).Rollback(Arg.Any<TestStep>());
            caughtException.Should().BeOfType<InvalidOperationException>();
            crashedStep.Should().BeOfType<RollbackCrashingStep>();
        }

        [Fact]
        public void Should_Not_Rollback_Steps_If_Succeeded()
        {
            Workflow.Add<RollbackStep>();
            Workflow.Execute(Context);

            Context.Out.DidNotReceive().Rollback(Arg.Any<RollbackStep>());
        }
    }

    public class RollbackStep : Step<ITestContext> 
    {
        protected override void Execute(ITestContext context)
        {
        }

        public override void Rollback(ITestContext context)
        {
            context.Out.Rollback(this);
        }
    }

    public class RollbackCrashingStep : Step<ITestContext>
    {
        protected override void Execute(ITestContext context)
        {
            context.Out.Executed(this);
        }

        public override void Rollback(ITestContext context)
        {
            context.Out.Rollback(this);
            throw new InvalidOperationException();
        }
    }

    public class SkipStep : Step<ITestContext>
    {
        protected override void Execute(ITestContext context)
        {
            Skip();
        }
    }

    public class FailingTwiceStep : Step<ITestContext>
    {
        protected override void Execute(ITestContext context)
        {
            context.Out.Executed(this);
            Fail();

            context.Out.Executed(this);
            Fail();
        }
    }

    public class FailingStep : Step<ITestContext>
    {
        protected override void Execute(ITestContext context)
        {
            Fail();
        }
    }

    public class CrashingStep : Step<ITestContext>
    {
        protected override void Execute(ITestContext context)
        {
            throw new InvalidOperationException();
        }
    }


    public class TestStep : Step<ITestContext>
    {
        protected override void Execute(ITestContext context)
        {
            context.Out.Executed(this);
        }

        public override void Rollback(ITestContext context)
        {
            context.Out.Rollback(this);
        }
    }
}
