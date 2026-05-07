using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FollowTargetAgent : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navmeshAgent;
    [SerializeField] private Transform _target;
    [SerializeField] private float _positionUpdateTime = 0.2f;

    IEnumerator Start()
    {
        var waiting = new WaitForSeconds(_positionUpdateTime);
        if (_target != null)
        {
            while (true)
            {
                _navmeshAgent.SetDestination(_target.position);
                yield return waiting;
            }
        }
    }
}
