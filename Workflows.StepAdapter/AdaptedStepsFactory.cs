namespace Workflows.StepAdapter
{
    public class AdaptedStepsFactory : IStepFactory
    {
        public Step<TCtx> CreateFrom<TCtx>(object stepInstance)
        {
            return StepAdapterHelper.InheritsStepBase<TCtx>(stepInstance.GetType()) 
                ? (Step<TCtx>)stepInstance
                : new StepAdapter<TCtx>(stepInstance);
        }
    }
}
