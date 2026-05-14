using System.Diagnostics;
using TheKiwiCoder;

public class HealthCheck : ActionNode
{
    public float HealthPercent = 0.3f;
    public Parameter Health;

    protected override void OnStart()
    {
        Health = context.Entity.GetParameter(ParameterType.Health);
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (Health == null)
        {
            return State.Failure;
        }

        float healthPercent = (float)Health.Value / Health.Max;

        return healthPercent < HealthPercent ? State.Success : State.Failure;        
    }
}
