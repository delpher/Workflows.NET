using System;
using System.Collections.Generic;
using System.Text;

namespace Workflows
{
    public interface IDependencyBuilder
    {
        IDependencyBuilder Requires(params Type[] types);
        IDependencyBuilder RequiresAny(params Type[] types);
    }
}
