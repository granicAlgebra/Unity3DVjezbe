using UnityEngine;
using UnityEngine.Animations.Rigging;

public class IkController : MonoBehaviour
{
    [SerializeField] private MultiAimConstraint _headLook;
    [SerializeField] private MultiAimConstraint _chestLook;
    [SerializeField] private Transform _lookTarget;
    [SerializeField] private float _lookLimit;

    private Vector3 _lookStart;
    private Vector3 _lookPosition;
    private bool _isLooking;
    private float _lookWeight;

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
}
