using TheKiwiCoder;
using UnityEngine;
using UnityEngine.AI;

public class FleeFromTarget : ActionNode
{
    public float Speed = 5;
    public float Acceleration = 20;
    public float FleeDistance = 10;
    public float SampleRadius = 20;
    public float Tolerance = 1.0f;

    protected override void OnStart()
    {
        Vector3 awayDirection;

        if (context.Target != null)
        {
            awayDirection = (context.transform.position - context.Target.position).normalized;
        }
        else
        {
            awayDirection = Random.insideUnitSphere.normalized;
        }

        Vector3 rawPosition = context.transform.position + (awayDirection * FleeDistance); 

        if (rawPosition.sqrMagnitude < Tolerance)
        {
            context.agent.isStopped = true;
            state = State.Failure;
            return;
        }

        if (NavMesh.SamplePosition(rawPosition, out NavMeshHit hit, SampleRadius, NavMesh.AllAreas))
        {
            context.agent.speed = Speed;
            context.agent.acceleration = Acceleration;
            context.agent.SetDestination(hit.position);
            context.agent.isStopped = false;
        }
        else
        {
            context.agent.isStopped = true;
            state = State.Failure;
        }
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (context.agent.pathPending)
        {
            return State.Running;
        }
        
        if (context.agent.remainingDistance < Tolerance)
        {
            return State.Success;
        }

        if (context.agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            return State.Failure;   
        }

        return State.Running;
    }
}
