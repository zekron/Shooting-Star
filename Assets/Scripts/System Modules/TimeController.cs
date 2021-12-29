using System.Collections;
using UnityEngine;

public class TimeController : Singleton<TimeController>
{
    [SerializeField, Range(0f, 1f)] private float bulletTimeScale = 0.1f;

    private float defaultFixedDeltaTime;
    private float timeScaleBeforePause;
    private float defaultTimeScale;
    private float timer;

    protected override void Awake()
    {
        base.Awake();
        defaultFixedDeltaTime = Time.fixedDeltaTime;
        defaultTimeScale = Time.timeScale;
    }

    public void Pause()
    {
        timeScaleBeforePause = Time.timeScale;
        Time.timeScale = 0f;
    }

    public void Unpause()
    {
        Time.timeScale = timeScaleBeforePause;
    }

    public void BulletTime(float duration)
    {
        Time.timeScale = bulletTimeScale;
        StartCoroutine(SlowOutCoroutine(duration));
    }

    public void BulletTime(float inDuration, float outDuration)
    {
        StartCoroutine(SlowInAndOutCoroutine(inDuration, outDuration));
    }

    public void BulletTime(float inDuration, float keepingDuration, float outDuration)
    {
        StartCoroutine(SlowInKeepAndOutDuration(inDuration, keepingDuration, outDuration));
    }

    public void SlowIn(float duration)
    {
        StartCoroutine(SlowInCoroutine(duration));
    }

    public void SlowOut(float duration)
    {
        StartCoroutine(SlowOutCoroutine(duration));
    }

    IEnumerator SlowInKeepAndOutDuration(float inDuration, float keepingDuration, float outDuration)
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));
        yield return new WaitForSecondsRealtime(keepingDuration);

        StartCoroutine(SlowOutCoroutine(outDuration));
    }

    IEnumerator SlowInAndOutCoroutine(float inDuration, float outDuration)
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));

        StartCoroutine(SlowOutCoroutine(outDuration));
    }

    IEnumerator SlowInCoroutine(float duration)
    {
        timer = 0f;

        while (timer < duration)
        {
            if (GameManager.Instance.CurrentGameState != GameState.Paused)
            {
                timer += Time.unscaledDeltaTime;
                Time.timeScale = Mathf.Lerp(defaultTimeScale, bulletTimeScale, timer / duration);
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
            }

            yield return null;
        }
    }

    IEnumerator SlowOutCoroutine(float duration)
    {
        timer = 0f;

        while (timer < duration)
        {
            if (GameManager.Instance.CurrentGameState != GameState.Paused)
            {
                timer += Time.unscaledDeltaTime;
                Time.timeScale = Mathf.Lerp(bulletTimeScale, defaultTimeScale, timer / duration);
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
            }

            yield return null;
        }
    }
}