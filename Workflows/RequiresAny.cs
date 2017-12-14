using System;

namespace Workflows
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class RequiresAnyAttribute : Attribute
    {
        public Type[] DependsOn { get; }

        public RequiresAnyAttribute(params Type[] dependsOn)
        {
            DependsOn = dependsOn;
        }
    }
}
