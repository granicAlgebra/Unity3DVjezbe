using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navmeshAgent;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _maxSpeed;

    private int _movementHash;
   

    void Start()
    {
        _movementHash = Animator.StringToHash("Velocity");
    }

    void Update()
    {
        if (_navmeshAgent != null)
        {
            _animator.SetFloat(_movementHash, _navmeshAgent.velocity.magnitude / _maxSpeed);
        } 
    }
}
