using System;

namespace Workflows
{
    public interface IDependencyExplorer
    {
        bool HasRequired(Type dependent);
        bool Requires(Type dependent, Type required);
    }
}
