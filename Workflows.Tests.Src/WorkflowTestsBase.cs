using System;
using System.Collections.Generic;
using NSubstitute;

namespace Workflows.Tests
{
    public class WorkflowTestsBase
    {
        protected readonly ITestContext Context;
        protected readonly ITestOutput Output;
        protected readonly TestWorkflow Workflow;
        protected readonly Dictionary<string, bool> TestFlags;

        protected WorkflowTestsBase()
        {
            Context = Substitute.For<ITestContext>();
            Output = Substitute.For<ITestOutput>();
            TestFlags = new Dictionary<string, bool>();
            Context.TestFlags.Returns(TestFlags);
            Context.Out.Returns(Output);
            Workflow = new TestWorkflow();
        }

        
    }

    public interface ITestOutput
    {
        void Executed(object step);
        void Failed(Exception exception);
        void Success();
        void Rollback(object step);
    }

    public interface ITestContext
    {
        IDictionary<string, bool> TestFlags { get; }
        ITestOutput Out { get; }
    }

}