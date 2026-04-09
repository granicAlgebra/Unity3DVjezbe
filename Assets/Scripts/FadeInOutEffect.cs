using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOutEffect : MonoBehaviour
{
    [SerializeField] private float _fadeInOutTime = 1.0f;
    private Image _image;

    void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void FadeIn()
    {
        StartCoroutine(FadeInCoroutine());
    }

    private IEnumerator FadeInCoroutine()
    {
        float time = 0;
        while (time < _fadeInOutTime) 
        { 
            time += Time.deltaTime;

            _image.color = new Color(0,0,0, time / _fadeInOutTime);
            yield return null;
        }
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        float time = 0;
        while (time < _fadeInOutTime)
        {
            time += Time.deltaTime;

            _image.color = new Color(0, 0, 0, 1 - (time / _fadeInOutTime));
            yield return null;
        }
    }
}
