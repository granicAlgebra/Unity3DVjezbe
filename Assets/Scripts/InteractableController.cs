using UnityEngine;

public class InteractableController : MonoBehaviour
{
    [SerializeField] private IkController _ikController;
    private Interactable _interactable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Interactable>(out var interactable))
        {
            _interactable = interactable;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_interactable != null && other.gameObject.Equals(_interactable.gameObject))
        {
            _interactable = null;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_interactable != null)
            {
                _interactable.Interact(_ikController);
            }
        }
    }
}
