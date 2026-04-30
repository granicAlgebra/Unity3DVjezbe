using UnityEngine;

public class LandMine : MonoBehaviour
{
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private LayerMask _layerMask;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        var colliders = Physics.OverlapSphere(transform.position, _explosionRadius, _layerMask, QueryTriggerInteraction.Collide);
        
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.TryGetComponent<GenericRagdoll>(out var ragdoll))
            {
                ragdoll.Die(transform.position, _explosionForce, _explosionRadius);
            }
        }
    }
}
