using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Workflows
{
    internal static class DependencyDiscovery
    {
        public static bool Requires(Type dependent, Type required)
        {
            return
                GetRequiredAttributes(dependent).Any(a => a.DependsOn == required) ||
                GetRequiresAnyAttributes(dependent).Any(a => a.DependsOn.Any(t => t == required));
        }

        public static bool HasRequired(Type dependent)
        {
            return GetRequiredAttributes(dependent).Any() ||
                GetRequiresAnyAttributes(dependent).Any();
        }

        private static IEnumerable<RequiresAttribute> GetRequiredAttributes(Type type)
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

        private static IEnumerable<RequiresAnyAttribute> GetRequiresAnyAttributes(Type type)
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