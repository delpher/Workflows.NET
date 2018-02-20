using System;

namespace Workflows
{
    public interface IDependencyBuilder
    {
        IDependencyBuilder Requires(params Type[] types);
        IDependencyBuilder RequiresAny(params Type[] types);
    }
}
