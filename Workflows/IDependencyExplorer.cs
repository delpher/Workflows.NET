using System;
using System.Collections.Generic;
using System.Text;

namespace Workflows
{
    public interface IDependencyExplorer
    {
        bool HasRequired(Type dependent);
        bool Requires(Type dependent, Type required);
    }
}
