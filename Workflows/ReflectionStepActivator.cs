using System;

namespace Workflows
{
    internal class ReflectionStepActivator : IStepActivator
    {
        public object Create<TStep>()
        {
            return (TStep) Activator.CreateInstance(typeof(TStep));
        }
    }
}