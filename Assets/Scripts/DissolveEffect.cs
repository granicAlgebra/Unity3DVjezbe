using System.Collections;
using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
    private const string AMOUNT_NAME = "_Amount";
    [SerializeField] private SkinnedMeshRenderer _skinedMeshRenderer;
    [SerializeField] private float _dissolveTime;
    [SerializeField] private float _startAmount = 1;

    private Material _material;

    void Start()
    {
        _material = _skinedMeshRenderer.material;
        _material.SetFloat(AMOUNT_NAME, _startAmount);
    }

    public void DissolveIn()
    {
        StartCoroutine(DissolveCoroutine(true));
    }

    public void DissolveOut()
    {
        StartCoroutine(DissolveCoroutine(false));
    }

    private IEnumerator DissolveCoroutine(bool dissolveIn)
    {
        float time = 0;

        while (time < _dissolveTime)
        {
            if (dissolveIn)
            {
                _material.SetFloat(AMOUNT_NAME, time / _dissolveTime);
            }
            else
            {
                _material.SetFloat(AMOUNT_NAME, 1 - time / _dissolveTime);
            }

            time += Time.deltaTime;
            yield return null;  
        }

    }
}
