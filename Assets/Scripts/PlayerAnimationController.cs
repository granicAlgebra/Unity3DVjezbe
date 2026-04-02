using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private float _walkingBlendValue;
    [SerializeField] private float _runBlendValue;
    [SerializeField] private float _sprintBlendValue;

    [SerializeField] private float _attackBlendDuration;
    [SerializeField] private Animator _animator;
    [SerializeField] private CharacterControllerMovement _movement;

    private Dictionary<string, int> _paramHashes = new Dictionary<string, int>();
    private bool _isAttacking = false;

    private void Start()
    {
        _movement.JumpEvent += OnJump;
    }

    private void Update()
    {
        IsGrounded();
        SetVelocity(_movement.VelocityMagnitude / _movement.MaximumVelocity);
        if (Input.GetMouseButton(0)) 
        {
            StartCoroutine(Attack());
        }
    }

    public IEnumerator Attack()
    {
        _isAttacking = true;
        _animator.SetTrigger(GetHash("Attack"));
        yield return StartCoroutine(BlendLayerWeight(1, 1f, _attackBlendDuration));
        AnimatorStateInfo stateInfo;
        do
        {
            yield return null;
            stateInfo = _animator.GetCurrentAnimatorStateInfo(1);

        } while (stateInfo.normalizedTime < 0.9f);

        yield return StartCoroutine(BlendLayerWeight(1, 0f, _attackBlendDuration));
    }

    public void SetVelocity(float velocityNormalized)
    {
        _animator.SetFloat(GetHash("Velocity"), velocityNormalized);
    }

    public void OnJump()
    {
        _animator.SetTrigger(GetHash("Jump"));
    }

    public void IsGrounded()
    {
        _animator.SetBool(GetHash("IsGrounded"), _movement.IsGrounded);
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

    private int GetHash(string paramName)
    {
        if (!_paramHashes.TryGetValue(paramName, out var hash))
        {
            hash = Animator.StringToHash(paramName);
            _paramHashes[paramName] = hash;
        }
        return hash;
    }
}