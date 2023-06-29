using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class TextFade : MonoBehaviour
{
    public float animationDuration;
    public AnimationCurve valueCurve;
    [SerializeField]
    private TMP_Text _text;
    private TMP_Text text
    {
        get
        {
            if (_text == null) GetComponent<TMP_Text>();
            return _text;
        }
    }
    private Coroutine fadeCoroutineInstance = null;
    private float animationSpeedSign = 1f;
    private float animationSpeed => (1f * animationSpeedSign) / animationDuration;
    private float currentAnimationPercentage = 0;

    private void OnDisable()
    {
        StopAllCoroutines();
        fadeCoroutineInstance = null;
    }
    [Button]
    public void FadeIn()
    {
        animationSpeedSign = 1f;
        if (fadeCoroutineInstance == null)
        {
            currentAnimationPercentage = 0f;
            fadeCoroutineInstance = StartCoroutine(FadeCoroutine());
        }
    }

    [Button]
    public void FadeOut()
    {
        animationSpeedSign = -1f;
        if (fadeCoroutineInstance == null)
        {
            currentAnimationPercentage = 1f;
            fadeCoroutineInstance = StartCoroutine(FadeCoroutine());
        }
    }

    private IEnumerator FadeCoroutine()
    {
        while (currentAnimationPercentage >= 0f && currentAnimationPercentage <= 1f)
        {
            text.alpha = valueCurve.Evaluate(currentAnimationPercentage);
            yield return null;
            currentAnimationPercentage += animationSpeed * Time.deltaTime;
        }
        currentAnimationPercentage = Mathf.Clamp01(currentAnimationPercentage);
        fadeCoroutineInstance = null;
    }
}
