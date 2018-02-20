using System;

namespace Workflows
{
    internal class ReflectionStepActivator : IStepActivator
    {
        public object Create<TStep>()
        {
            return (TStep) Create(typeof(TStep));
        }

        public object Create(Type stepType)
        {
            return Activator.CreateInstance(stepType);
        }
    }
}