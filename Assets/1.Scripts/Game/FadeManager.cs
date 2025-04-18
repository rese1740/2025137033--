using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환해도 유지되게
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FadeIn(System.Action onComplete = null)
    {
        StartCoroutine(FadeRoutine(1, 0, onComplete));
    }

    public void FadeOut(System.Action onComplete = null)
    {
        StartCoroutine(FadeRoutine(0, 1, onComplete));
    }

    private IEnumerator FadeRoutine(float startAlpha, float endAlpha, System.Action onComplete)
    {
        float time = 0f;
        Color color = fadeImage.color;
        color.a = startAlpha;
        fadeImage.color = color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);
            color.a = alpha;
            fadeImage.color = color;
            yield return null;
        }

        color.a = endAlpha;
        fadeImage.color = color;
        onComplete?.Invoke();
    }
}
