using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIController : MonoBehaviour
{
    [Header("==== PLAYER INPUT ====")]
    [SerializeField] private PlayerInputSO playerInput;

    [Header("==== AUDIO DATA ====")]
    [SerializeField] private AudioDataSO pauseSFX;
    [SerializeField] private AudioDataSO unpauseSFX;

    [Header("==== CANVAS ====")]
    [SerializeField] private Canvas hUDCanvas;
    [SerializeField] private Canvas menusCanvas;
    [SerializeField] private WaveUI waveUI;
    [SerializeField] private ScoreDisplay scoreDisplay;
    /// <summary>
    /// 当前总得分
    /// </summary>
    private int totalScore;
    /// <summary>
    /// 当前显示分数
    /// </summary>
    private int currentScore = -1;

    [Header("==== BUTTONS ====")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button mainMenuButton;

    [Header("==== EVENTS ====")]
    [SerializeField] private IntEventChannelSO updateWaveEventSO;
    [SerializeField] private IntEventChannelSO updateTotalScoreEventSO;

    [Header("Options")]
    [SerializeField] private Canvas optionCanvas;
    [SerializeField] private VoidEventChannelSO optionQuitEventSO;

    private Vector3 scoreTextScale = new Vector3(1.2f, 1.2f, 1.2f);

    private int buttonPressedParameterID = Animator.StringToHash("Pressed");

    private void OnEnable()
    {
        playerInput.onPause += Pause;
        playerInput.onUnpause += Unpause;

        updateWaveEventSO.OnEventRaised += UpdateWave;
        updateTotalScoreEventSO.OnEventRaised += UpdateScoreText;
        optionQuitEventSO.OnEventRaised += CloseOptionCanvas;

        ButtonPressedBehavior.buttonFunctionTable.Add(resumeButton.gameObject.GetInstanceID(), OnResumeButtonClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(optionsButton.gameObject.GetInstanceID(), OnOptionsButtonClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(mainMenuButton.gameObject.GetInstanceID(), OnMainMenuButtonClick);
    }

    private void OnDisable()
    {
        playerInput.onPause -= Pause;
        playerInput.onUnpause -= Unpause;

        updateWaveEventSO.OnEventRaised -= UpdateWave;
        updateTotalScoreEventSO.OnEventRaised -= UpdateScoreText;
        optionQuitEventSO.OnEventRaised -= CloseOptionCanvas;

        ButtonPressedBehavior.buttonFunctionTable.Clear();
    }

    private void Start()
    {
        GameManager.Instance.CurrentGameState = GameState.Playing;
    }

    private void Pause()
    {
        hUDCanvas.enabled = false;
        menusCanvas.enabled = true;
        GameManager.Instance.CurrentGameState = GameState.Paused;
        TimeController.Instance.Pause();
        playerInput.EnablePauseMenuInput();
        playerInput.SwitchToDynamicUpdateMode();
        UIInput.Instance.SelectUI(resumeButton);
        AudioManager.Instance.PlaySFX(pauseSFX);
    }

    private void Unpause()
    {
        resumeButton.Select();
        resumeButton.animator.SetTrigger(buttonPressedParameterID);
        AudioManager.Instance.PlaySFX(unpauseSFX);
    }

    private void OnResumeButtonClick()
    {
        hUDCanvas.enabled = true;
        menusCanvas.enabled = false;
        GameManager.Instance.CurrentGameState = GameState.Playing;
        TimeController.Instance.Unpause();
        playerInput.EnableGameplayInput();
        playerInput.SwitchToFixedUpdateMode();
    }

    private void OnOptionsButtonClick()
    {
        // TODO
        optionCanvas.enabled = true;
        menusCanvas.enabled = false;
        UIInput.Instance.SelectUI(optionsButton);
        playerInput.EnablePauseMenuInput();
    }

    private void OnMainMenuButtonClick()
    {
        menusCanvas.enabled = false;
        SceneLoader.Instance.LoadMainMenuScene();
    }

    private void UpdateWave(int value)
    {
        waveUI.gameObject.SetActive(true);
    }

    private void UpdateScoreText(int value)
    {
        totalScore += value;
        StartCoroutine(nameof(AddScoreCoroutine));
    }

    private void CloseOptionCanvas()
    {
        optionCanvas.enabled = false;
        menusCanvas.enabled = true;
        UIInput.Instance.SelectUI(resumeButton);
    }

    private IEnumerator AddScoreCoroutine()
    {
        scoreDisplay.ScaleText(scoreTextScale);

        while (totalScore > currentScore)
        {
            currentScore += 1;
            scoreDisplay.UpdateText(currentScore);

            yield return null;
        }

        scoreDisplay.ScaleText(Vector3.one);
    }
}