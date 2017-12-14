using System;
using System.Collections.Generic;
using System.Linq;

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
            return type
                .GetCustomAttributes(typeof(RequiresAttribute), true)
                .Cast<RequiresAttribute>();
        }

        private static IEnumerable<RequiresAnyAttribute> GetRequiresAnyAttributes(Type type)
        {
            return type
                .GetCustomAttributes(typeof(RequiresAnyAttribute), true)
                .Cast<RequiresAnyAttribute>();
        }
    }
}