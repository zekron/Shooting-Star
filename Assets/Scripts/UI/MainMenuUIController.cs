using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [Header("==== CANVAS ====")]
    [SerializeField] private Canvas mainMenuCanvas;
    [SerializeField] private Canvas playerSelectionCanvas;
    [SerializeField] private Canvas tipsCanvas;

    [Header("==== BUTTONS ====")]
    [SerializeField] private Button buttonStart;
    [SerializeField] private Button buttonOptions;
    [SerializeField] private Button buttonQuit;

    [Header("==== PLAYERSELECTION ====")]
    [SerializeField] private PlayerProfileSO[] playerProfiles;
    [SerializeField] private IntEventChannelSO profileIndexEventSO;
    [SerializeField] private PlayerProfileGraphic profileGraphic;
    [SerializeField] private Button buttonSubmit;
    [SerializeField] private Button buttonCancel;
    private bool hasSelectedModel = false;

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

        profileIndexEventSO.OnEventRaised += RefreshSelectionPanel;
        optionQuitEventSO.OnEventRaised += CloseOptionCanvas;

        hasSelectedModel = false;
    }

    void OnDisable()
    {
        ButtonPressedBehavior.buttonFunctionTable.Clear();

        profileIndexEventSO.OnEventRaised -= RefreshSelectionPanel;
        optionQuitEventSO.OnEventRaised -= CloseOptionCanvas;
    }

    void Start()
    {
        Time.timeScale = 1f;
        GameManager.Instance.CurrentGameState = GameState.MainMenu;
        UIInput.Instance.SelectUI(buttonStart);
    }

    private void OnButtonStartClicked()
    {
        //mainMenuCanvas.gameObject.layer = StaticData.LAYER_OUTUI;
        //playerSelectionCanvas.gameObject.layer = StaticData.LAYER_UI;
        //tipsCanvas.gameObject.layer = StaticData.LAYER_UI;
        mainMenuCanvas.enabled = false;
        playerSelectionCanvas.enabled = true;
        tipsCanvas.enabled = true;

        UIInput.Instance.SelectUI(buttonSubmit);
    }

    private void OnButtonOptionsClicked()
    {
        //optionCanvas.gameObject.layer = StaticData.LAYER_UI;
        //mainMenuCanvas.gameObject.layer = StaticData.LAYER_OUTUI;
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
        //optionCanvas.gameObject.layer = StaticData.LAYER_OUTUI;
        //mainMenuCanvas.gameObject.layer = StaticData.LAYER_UI;
        optionCanvas.enabled = false;
        mainMenuCanvas.enabled = true;

        UIInput.Instance.SelectUI(buttonStart);
    }

    private void RefreshSelectionPanel(int index)
    {
        //playerSelectionCanvas.gameObject.layer = StaticData.LAYER_UI;
        playerSelectionCanvas.enabled = true;

        PlayerProfileSO profile = playerProfiles[index];
        profileGraphic.SetValues(profile.DataNormalization(profile.MaxHealth, profile.MoveSpeed, profile.FireInterval, profile.DodgeCost));

        UIInput.Instance.SelectUI(buttonSubmit);
        hasSelectedModel = true;
    }
    private void OnButtonSubmitClicked()
    {
        //playerSelectionCanvas.gameObject.layer = StaticData.LAYER_OUTUI;
        //tipsCanvas.gameObject.layer = StaticData.LAYER_OUTUI;
        if (!hasSelectedModel) return;

        playerSelectionCanvas.enabled = false;
        tipsCanvas.enabled = false;

        SceneLoader.Instance.LoadGameplayScene();
    }
    private void OnButtonCancelClicked()
    {
        //playerSelectionCanvas.gameObject.layer = StaticData.LAYER_OUTUI;
        //tipsCanvas.gameObject.layer = StaticData.LAYER_OUTUI;
        //mainMenuCanvas.gameObject.layer = StaticData.LAYER_UI;
        playerSelectionCanvas.enabled = false;
        tipsCanvas.enabled = false;
        mainMenuCanvas.enabled = true;

        UIInput.Instance.SelectUI(buttonStart);
    }
}