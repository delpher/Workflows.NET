using System;

namespace Workflows
{
    public interface IStepActivator
    {
        object Create<TStep>();        
        object Create(Type stepType);
    }
}