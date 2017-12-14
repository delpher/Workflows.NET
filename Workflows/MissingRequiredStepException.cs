using System;

namespace Workflows
{
    public class MissingRequiredStepException : Exception
    {
        public MissingRequiredStepException(Type stepType)
            : base($"Workflow doesn't contain required dependencies for {stepType.Name}.")
        {
        }
    }
}