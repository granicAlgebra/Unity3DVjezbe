using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class IkController : MonoBehaviour
{
    [SerializeField] private MultiAimConstraint _headLook;
    [SerializeField] private MultiAimConstraint _chestLook;
    [SerializeField] private TwoBoneIKConstraint _rightHandIK;
    [SerializeField] private Transform _lookTarget;
    [SerializeField] private Transform _handTarget;
    [SerializeField] private float _lookLimit;

    private Vector3 _lookStart;
    private Vector3 _lookPosition;
    private bool _isLooking;
    private float _lookWeight;
    private Transform _handEndTarget;

    void Start()
    {
        _lookStart = _lookTarget.localPosition;
    }

    void Update()
    {
        if (_isLooking)
        {
            _lookTarget.position = _lookPosition;

            var direction = (_lookTarget.position - transform.position).normalized;
            if (Vector3.Dot(transform.forward, direction) > _lookLimit)
            {
                _lookWeight = Mathf.Lerp(_lookWeight, 1, Time.deltaTime * 3);
            }
            else
            {
                _lookWeight = Mathf.Lerp(_lookWeight, 0, Time.deltaTime * 3);
            }

            _headLook.weight = _lookWeight;
            _chestLook.weight = _lookWeight;
        }

        if (_handEndTarget != null)
        {
            _handTarget.SetPositionAndRotation(_handEndTarget.position, _handEndTarget.rotation);
        }

    }

    public void StareAt(Transform target)
    {
        if (target == null) return;

        _lookPosition = target.position;
        _isLooking = true;
    }

    public void StopStaring()
    {
        _isLooking = false;
        _lookTarget.localPosition = _lookStart;
    }

    public void PlaceRightHand(Transform target, float timeToMove)
    {
        _handEndTarget = target;
        StartCoroutine(HandIkMoveCoroutine(_rightHandIK, true, timeToMove));
    }

    public void ReturnRightHand(float timeToMove)
    {
        StartCoroutine(HandIkMoveCoroutine(_rightHandIK, false, timeToMove));
    }

    private IEnumerator HandIkMoveCoroutine(TwoBoneIKConstraint constraint, bool moveIn, float timeToMove)
    {
        float timePassed = 0;
        while (timeToMove - timePassed > 0)
        {
            if (moveIn)
                constraint.weight = timePassed / timeToMove;
            else
                constraint.weight = 1 - timePassed / timeToMove;
            timePassed += Time.deltaTime;
            yield return null;  
        }

        if (!moveIn)
        {
            _handEndTarget = null;
        }
    }

    internal void Interact(IkController ikController)
    {
        throw new NotImplementedException();
    }
}
