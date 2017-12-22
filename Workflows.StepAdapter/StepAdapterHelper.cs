using System;
using System.Reflection;

namespace Workflows.StepAdapter
{
    internal static class StepAdapterHelper
    {
        private const string ExecuteMethodName = "Execute";
        private const string FailedEventName = "Failed";
        private const string SkippedEventName = "Skipped";

        public static MethodInfo GetExecuteMethod<TCtx>(object stepInstance)
        {
            var parameters = new[] {typeof(TCtx)};
            var stepType = stepInstance.GetType();
#if NETSTANDARD1_1
            return stepType.GetRuntimeMethod(ExecuteMethodName, parameters);
#else
            return stepType.GetMethod(ExecuteMethodName, parameters);
#endif
        }

        public static bool InheritsStepBase<TCtx>(Type stepType)
        {
            var stepBaseType = typeof(Step<TCtx>);
#if NETSTANDARD1_1
            return stepBaseType.GetTypeInfo().IsAssignableFrom(stepType.GetTypeInfo());
#else
            return stepBaseType.IsAssignableFrom(stepType);
#endif

        }

        public static EventInfo GetEvent(object instance, string name)
        {
            var stepType = instance.GetType();
#if NETSTANDARD1_1
            return stepType.GetRuntimeEvent(name);
#else
            return stepType.GetEvent(name);
#endif
        }

        public static void AddFailedHandler(object instance, EventHandler handler)
        {
            var failedEvent = GetEvent(instance, FailedEventName);
            if (failedEvent != null)
                failedEvent.AddEventHandler(instance, handler);
        }

        public static void AddSkippedHandler(object instance, EventHandler handler)
        {
            var skippedEvent = GetEvent(instance, SkippedEventName);
            if (skippedEvent != null)
                skippedEvent.AddEventHandler(instance, handler);
        }
    }
}