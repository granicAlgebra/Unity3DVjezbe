using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TheKiwiCoder;
using System;


#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(GenericRagdoll))]
public class RagdollEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GenericRagdoll ragdoll = (GenericRagdoll)target;

        if (GUILayout.Button("Cache Ragdoll Rigidbodies"))
        {
            ragdoll.ChacheRagdollRigidbodies();
        }

        if (GUILayout.Button("Activate Ragdoll"))
        {
            ragdoll.Die();
        }
    }
}

#endif

public class GenericRagdoll : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private PlayerAnimationController _playerAnimationController;
    [SerializeField] private Animator _playerAnimatior;
    [SerializeField] private IkController _kController;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private EnemyAnimationController _enemyAnimationController;
    [SerializeField] private CharacterControllerMovement _characterControllerMovement;
    [SerializeField] private BehaviourTreeRunner _behaviourTreeRunner;
    [SerializeField] private Entity _entity;

    [SerializeField] private List<Rigidbody> _ragdollRigidbodies = new List<Rigidbody>();
    [SerializeField] private List<Collider> _ragdollColliders = new List<Collider>();
    [SerializeField] private Transform _ragdollRootObject;

    private void Awake()
    {
        SetRagdollKinematic(true);
        if (_entity != null)
            _entity.ParameterValueChange += OnParameterValueChange;
    }

    private void OnParameterValueChange(Parameter parameter)
    {
        if (parameter.Type.Equals(ParameterType.Health))
        {
            if (parameter.Value == parameter.Min)
                Die();
        }
    }

    public void SetRagdollKinematic(bool value)
    {
        for (int i = 0; i < _ragdollRigidbodies.Count; i++)
        {
            _ragdollRigidbodies[i].isKinematic = value;
        }
    }

    public void ChacheRagdollRigidbodies()
    {
        _ragdollRootObject.GetComponentsInChildren(true, _ragdollRigidbodies);
    }

    public void Die()
    {
        if (_characterController != null)
            _characterController.enabled = false;
        if (_playerAnimationController != null)
            _playerAnimationController.enabled = false; 
        if (_characterControllerMovement != null)
            _characterControllerMovement.enabled = false;
        if (_kController != null)
            _kController.enabled = false;
        if (_navMeshAgent != null)
            _navMeshAgent.enabled = false;
        if (_enemyAnimationController != null)
            _enemyAnimationController.enabled=false;
        if (_behaviourTreeRunner != null)
            _behaviourTreeRunner.enabled = false;   

        _playerAnimatior.enabled = false;

        SetRagdollKinematic(false);
        _ragdollRootObject.gameObject.SetActive(true);

        if (_characterController != null)
            for (int i = 0; i < _ragdollRigidbodies.Count; i++)
            {
                _ragdollRigidbodies[i].linearVelocity = _characterController.velocity;
            }

    }

    public void Die(Vector3 forcePosition, float force, float forceRadius)
    {
        Die();
        for (int i = 0; i < _ragdollRigidbodies.Count; i++)
        {
            Vector3 distance = _ragdollRigidbodies[i].position - forcePosition;
            float magnitude = distance.magnitude;
            if (magnitude < forceRadius)
            {
                _ragdollRigidbodies[i].AddForce((1 - (magnitude / forceRadius)) * force *  distance, ForceMode.Impulse);
            }
            
        }
    }
}
