using UnityEngine;

public class StareController : MonoBehaviour
{
    [SerializeField] private IkController _ikController;
    [SerializeField] private Transform _target;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Stareable>(out var stareable))
        {
            _target = stareable.transform;
            _ikController.StareAt(other.transform); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.Equals(_target))
        {
            _ikController.StopStaring();    
        }
    }
}
