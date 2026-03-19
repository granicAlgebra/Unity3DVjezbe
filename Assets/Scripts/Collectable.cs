using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private ParameterType _type;
    [SerializeField] private int _addValue;

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.TryGetComponent<Entity>(out Entity entity))
        {
            Debug.Log($"Name: {other.gameObject.name} Parameter: {_type} Value: {_addValue}");
            if (entity.AddToParameter(_type, _addValue))
            {
                gameObject.SetActive(false);
            }
        }
    }
}
