using UnityEngine;
using UnityEngine.UI;
using System.Collections;
/// <summary>
/// Attach this script to an Image gameobject
/// You can use a fade effect on start through the inspector and or activate it by code using the public Fade function
/// </summary>
public class FadeToBlackScript : MonoBehaviour
{
    [SerializeField]
    private bool _activateOnStart;
    [SerializeField]
    private float _fadeInTime;
    [SerializeField]
    private float _fadeOutTime;
    [SerializeField]
    private FadeType _fadeType;

    public enum FadeType { IN, OUT, INOUT, OUTIN }

    private Image _image;
    private Color _color;
    private float _fadeTime;
    private float _step;

    private float _timeStamp;

    /// <summary>
    /// A little helper class for the fade coroutines, since you can only pass in one object in StartCoroutine()
    /// </summary>
    private class FadeTime
    {
        public float IN { get { return _IN; } }
        public float OUT { get { return _OUT; } }

        private float _IN;
        private float _OUT;

        public FadeTime(float pIN, float pOUT)
        {
            _IN = pIN;
            _OUT = pOUT;
        }
    }

    private void Start()
    {
        _image = GetComponent<Image>();
        _color = _image.color;

        if (_activateOnStart) { Fade(_fadeType, _fadeInTime, _fadeOutTime); }
    }

    private void Update()
    {
        _step = Time.deltaTime / _fadeTime;
    }

    /// <summary>
    /// Makes a fade effect with the Image component attached to this gameobject
    /// </summary>
    /// <param name="pFadeType"></param>
    /// <param name="pFadeINTimeInMS"></param>
    /// <param name="pFadeOUTTimeInMS"></param>
    public void Fade(FadeType pFadeType, float pFadeINTimeInMS = 1.0f, float pFadeOUTTimeInMS = 1.0f)
    {
        _timeStamp = Time.time;
        FadeTime fadeTime = new FadeTime(pFadeINTimeInMS, pFadeOUTTimeInMS);
        IEnumerator _function = null;
        switch (pFadeType)
        {
            case FadeType.IN:
                _function = fadeIN(fadeTime);
                break;
            case FadeType.OUT:
                _function = fadeOUT(fadeTime);
                break;
            case FadeType.INOUT:
                _function = fadeINOUT(fadeTime);
                break;
            case FadeType.OUTIN:
                _function = fadeOUTIN(fadeTime);
                break;
        }
        StartCoroutine(_function);
    }

    private IEnumerator fadeIN(FadeTime pFadeTime)
    {
        _fadeTime = pFadeTime.IN;
        for (float f = 0.0f; f <= 1.0f; f += _step)
        {
            _color.a = f;
            _image.color = _color;
            yield return null;
        }
        _color.a = 1.0f;
        _image.color = _color;
    }

    private IEnumerator fadeOUT(FadeTime pFadeTime)
    {
        _fadeTime = pFadeTime.OUT;
        for (float f = 1.0f; f >= 0.0f; f -= _step)
        {
            _color.a = f;
            _image.color = _color;
            yield return null;
        }
        _color.a = 0.0f;
        _image.color = _color;
        print(Time.time - _timeStamp);
    }

    private IEnumerator fadeINOUT(FadeTime pFadeTime)
    {
        _fadeTime = pFadeTime.IN;
        for (float f = 0.0f; f <= 1.0; f += _step)
        {
            _color.a = f;
            _image.color = _color;
            yield return null;
        }
        _color.a = 1.0f;
        _image.color = _color;
        _fadeTime = pFadeTime.OUT;
        for (float f = 1.0f; f >= 0.0f; f -= _step)
        {
            _color.a = f;
            _image.color = _color;
            yield return null;
        }
        _color.a = 0.0f;
        _image.color = _color;
    }

    private IEnumerator fadeOUTIN(FadeTime pFadeTime)
    {
        _fadeTime = pFadeTime.OUT;
        for (float f = 1.0f; f >= 0.0; f -= _step)
        {
            _color.a = f;
            _image.color = _color;
            yield return null;
        }
        _color.a = 1.0f;
        _image.color = _color;
        _fadeTime = pFadeTime.IN;
        for (float f = 0.0f; f <= 1.0f; f += _step)
        {
            _color.a = f;
            _image.color = _color;
            yield return null;
        }
    }
}
