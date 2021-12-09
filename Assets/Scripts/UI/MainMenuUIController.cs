using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [Header("==== CANVAS ====")]
    [SerializeField] private Canvas mainMenuCanvas;
    [SerializeField] private Canvas playerSelectionCanvas;
    [SerializeField] private Canvas tipsCanvas;
    [SerializeField] private GameObject playerModel;

    [Header("==== BUTTONS ====")]
    [SerializeField] private Button buttonStart;
    [SerializeField] private Button buttonOptions;
    [SerializeField] private Button buttonQuit;

    [Header("==== PLAYERSELECTION ====")]
    [SerializeField] private ProfileEventChannelSO profileEventSO;
    [SerializeField] private PlayerProfileGraphic profileGraphic;
    [SerializeField] private Button buttonSubmit;
    [SerializeField] private Button buttonCancel;

    [Header("Options")]
    [SerializeField] private Canvas optionCanvas;
    [SerializeField] private VoidEventChannelSO optionQuitEventSO;

    void OnEnable()
    {
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonStart.gameObject.GetInstanceID(), OnButtonStartClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonOptions.gameObject.GetInstanceID(), OnButtonOptionsClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonQuit.gameObject.GetInstanceID(), OnButtonQuitClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonSubmit.gameObject.GetInstanceID(), OnButtonSubmitClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonCancel.gameObject.GetInstanceID(), OnButtonCancelClicked);

        profileEventSO.OnEventRaised += RefreshSelectionPanel;
        optionQuitEventSO.OnEventRaised += CloseOptionCanvas;
    }

    void OnDisable()
    {
        ButtonPressedBehavior.buttonFunctionTable.Clear();

        profileEventSO.OnEventRaised -= RefreshSelectionPanel;
        optionQuitEventSO.OnEventRaised -= CloseOptionCanvas;
    }

    void Start()
    {
        Time.timeScale = 1f;
        GameManager.CurrentGameState = GameState.Playing;
        UIInput.Instance.SelectUI(buttonStart);
    }

    private void OnButtonStartClicked()
    {
        mainMenuCanvas.enabled = false;
        playerModel.SetActive(true);
        UIInput.Instance.SelectUI(buttonSubmit);
        tipsCanvas.enabled = true;
    }

    private void OnButtonOptionsClicked()
    {
        optionCanvas.enabled = true;
        mainMenuCanvas.enabled = false;
        UIInput.Instance.SelectUI(buttonOptions);
    }

    private void OnButtonQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void CloseOptionCanvas()
    {
        optionCanvas.enabled = false;
        mainMenuCanvas.enabled = true;
        UIInput.Instance.SelectUI(buttonStart);
    }

    private void RefreshSelectionPanel(PlayerProfileSO profile, GameObject gameObject)
    {
        playerSelectionCanvas.enabled = true;
        //TODO
        profileGraphic.SetValues(profile.DataNormalization(profile.MaxHealth, profile.MoveSpeed, profile.FireInterval, profile.DodgeCost));
        GameManager.Instance.CurrentPlayerModel = gameObject;

        UIInput.Instance.SelectUI(buttonSubmit);
    }
    private void OnButtonSubmitClicked()
    {
        playerSelectionCanvas.enabled = false;
        tipsCanvas.enabled = false;
        playerModel.SetActive(false);

        SceneLoader.Instance.LoadGameplayScene();
    }
    private void OnButtonCancelClicked()
    {
        playerModel.SetActive(false);
        playerSelectionCanvas.enabled = false;
        tipsCanvas.enabled = false;
        mainMenuCanvas.enabled = true;
        UIInput.Instance.SelectUI(buttonStart);
    }
}