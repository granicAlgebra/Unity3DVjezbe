using UnityEngine;

public class ExplodingBarrel : MonoBehaviour, IDamageable
{
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Collider _collider;
    [SerializeField] private ParticleSystem _particlePrefab;
    [SerializeField] private AudioSource _sfxPrefab;
    [SerializeField] private AudioClip _explosionSFX;
    private bool _hasExploded = false;

    public void Damage()
    {
        Explode();
    }

    public void Explode()
    {
        if (_hasExploded)
        {
            return;
        }
        _hasExploded = true;
        VFXManager.Instance.PlayVFX(_particlePrefab, transform.position);
        SFXManager.Instance.PlaySFX(_sfxPrefab, transform.position, _explosionSFX, 2);
        var colliders = Physics.OverlapSphere(transform.position, _explosionRadius, _layerMask, QueryTriggerInteraction.Collide);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.TryGetComponent<GenericRagdoll>(out var ragdoll))
            {
                ragdoll.Die(transform.position, _explosionForce, _explosionRadius);
            }
            if (colliders[i].gameObject.TryGetComponent<IDamageable>(out var damageable))
            {
                if (!damageable.Equals(this))
                {
                    damageable.Damage();
                }
            }
        }

        gameObject.SetActive(false);
        _collider.enabled = false;
    }
}
