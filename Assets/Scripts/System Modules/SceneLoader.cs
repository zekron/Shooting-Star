using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : PersistentSingleton<SceneLoader>
{
    [SerializeField] private float fadeTime = 3.5f;
    private UnityEngine.UI.Image transitionImage;
    private Canvas canvas;

    private Color color;

    private const string GAMEPLAY = "Gameplay";
    private const string MAIN_MENU = "MainMenu";
    private const string SCORING = "Scoring";

    protected override void Awake()
    {
        base.Awake();
        canvas = GetComponent<Canvas>();
        transitionImage = GetComponent<UnityEngine.UI.Image>();
    }

    IEnumerator LoadingCoroutine(string sceneName)
    {
        // Load new scene in background and
        var loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        // Set this scene inactive
        loadingOperation.allowSceneActivation = false;

        // Fade out
        canvas.enabled = true;
        //transitionImage.gameObject.SetActive(true);

        while (color.a < 1f)
        {
            color.a = Mathf.Clamp01(color.a + Time.unscaledDeltaTime / fadeTime);
            transitionImage.color = color;

            yield return null;
        }

        yield return new WaitUntil(() => loadingOperation.progress >= 0.9f);

        // Activate the new scene
        loadingOperation.allowSceneActivation = true;

        // Fade in
        while (color.a > 0f)
        {
            color.a = Mathf.Clamp01(color.a - Time.unscaledDeltaTime / fadeTime);
            transitionImage.color = color;

            yield return null;
        }

        canvas.enabled = false;
        //transitionImage.gameObject.SetActive(false);
    }

    public void LoadGameplayScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(GAMEPLAY));
    }

    public void LoadMainMenuScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(MAIN_MENU));
    }

    public void LoadScoringScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(SCORING));
    }
}