using System;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    public event Action OnAttackTriggered;
    public event Action OnSpecialAttackTriggered;

    public void Attack()
    {
        OnAttackTriggered?.Invoke();
        Debug.Log("Attack");
    }

    public void SpecialAttack()
    {
        OnSpecialAttackTriggered?.Invoke();
        Debug.Log("SP Attack");
    }
}
