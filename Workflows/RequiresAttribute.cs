using System;

namespace Workflows
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class RequiresAttribute : Attribute
    {
        public Type DependsOn { get; }

        public RequiresAttribute(Type dependsOn)
        {
            DependsOn = dependsOn;
        }
    }
}