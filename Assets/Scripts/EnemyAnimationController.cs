using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navmeshAgent;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _maxSpeed;

    [SerializeField] private float _attackBlendDuration;

    private int _movementHash;
    private int _attackHash;
    private bool _isAttacking;

    void Start()
    {
        _movementHash = Animator.StringToHash("Velocity");
        _attackHash = Animator.StringToHash("Attack");
    }

    void Update()
    {
        if (_navmeshAgent != null)
        {
            _animator.SetFloat(_movementHash, _navmeshAgent.velocity.magnitude / _maxSpeed);
        } 
    }

    internal void Attack()
    {
        if (_isAttacking)
            return;
        StartCoroutine(Attack("Attack")); 
    }

    public IEnumerator Attack(string triggerName)
    {
        _isAttacking = true;

        _animator.SetTrigger(_attackHash);

        yield return StartCoroutine(BlendLayerWeight(1, 1f, _attackBlendDuration));

        float timeout = 3f;
        float elapsed = 0f;
        AnimatorStateInfo stateInfo;
        do
        {
            yield return null;
            elapsed += Time.deltaTime;
            stateInfo = _animator.GetCurrentAnimatorStateInfo(1);
        }
        while (stateInfo.normalizedTime < 0.8f && elapsed < timeout);

        yield return StartCoroutine(BlendLayerWeight(1, 0f, _attackBlendDuration));

        _isAttacking = false;
    }

    private IEnumerator BlendLayerWeight(int layerIndex, float targetWeight, float duration)
    {
        float startWeight = _animator.GetLayerWeight(layerIndex);
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            float newWeight = Mathf.Lerp(startWeight, targetWeight, t);
            _animator.SetLayerWeight(layerIndex, newWeight);

            yield return null;
        }

        _animator.SetLayerWeight(layerIndex, targetWeight);
    }
}
