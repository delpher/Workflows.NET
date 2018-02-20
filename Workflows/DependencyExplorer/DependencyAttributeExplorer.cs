using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Workflows.DependencyExplorer;

namespace Workflows
{
    internal class DependencyAttributeExplorer : IDependencyExplorer
    {
        public virtual bool Requires(Type dependent, Type required)
        {
            return
                GetRequiredAttributes(dependent).Any(a => a.DependsOn == required) ||
                GetRequiresAnyAttributes(dependent).Any(a => a.DependsOn.Any(t => t == required));
        }

        public virtual bool HasRequired(Type dependent)
        {
            return GetRequiredAttributes(dependent).Any() ||
                   GetRequiresAnyAttributes(dependent).Any();
        }

        private IEnumerable<RequiresAttribute> GetRequiredAttributes(Type type)
        {
#if NETSTANDARD1_1
            return type.GetTypeInfo()
                .GetCustomAttributes(typeof(RequiresAttribute), true)
                .Cast<RequiresAttribute>();
#else
            return type
                .GetCustomAttributes(typeof(RequiresAttribute), true)
                .Cast<RequiresAttribute>();
#endif
        }

        private IEnumerable<RequiresAnyAttribute> GetRequiresAnyAttributes(Type type)
        {
#if NETSTANDARD1_1
            return type.GetTypeInfo()
                .GetCustomAttributes(typeof(RequiresAnyAttribute), true)
                .Cast<RequiresAnyAttribute>();
#else
            return type
                .GetCustomAttributes(typeof(RequiresAnyAttribute), true)
                .Cast<RequiresAnyAttribute>();
#endif

        }
    }
}