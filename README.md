# Workflows.NET
A simple library for executing sequential workfows defined in code. Supports dependencies management, rollback, crash and failure handling.

## Quick start
It is very easy to get started. You should define a class that will represent an execution context. Then define classes that will represent workflow steps, accepting this context. Next you need to create an instance of the Workflow, configure it with the step classes and execute it, providing an instance of context. Then you can use context to get results of workflow execution. 
```csharp
public class SampleContext
{
    public string Data { get; set; }
}

public class SampleStep : Step<SampleContext>
{
    protected override void Execute(SampleContext context)
    {
        if (string.IsNullOrEmpty(context.Data))
            Fail();

        context.Data += " additional data";
    }
}

public class HomeController : Controller
{
    public IActionResult Action()
    {
        var context = new SampleContext();
        var workflow = new Workflow<SampleContext>();
        workflow.Add<SampleStep>();
        workflow.Execute(context);
        return View(context);
    }
}
```

More documentation on a [Workflows.NET project page](https://alex-onashyuk.azurewebsites.net/Projects/Workflows).
