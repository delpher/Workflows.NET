using System;

namespace Workflows.Tests
{
    public interface ITestOutput
    {
        void Executed(object step);
        void Failed(Exception exception);
        void Success();
        void Rollback(object step);
    }
}