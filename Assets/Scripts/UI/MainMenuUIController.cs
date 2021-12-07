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
    [SerializeField] private Text textProfile;
    [SerializeField] private Button buttonSubmit;
    [SerializeField] private Button buttonCancel;

    void OnEnable()
    {
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonStart.gameObject.name, OnButtonStartClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonOptions.gameObject.name, OnButtonOptionsClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonQuit.gameObject.name, OnButtonQuitClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonSubmit.gameObject.name, OnButtonSubmitClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonCancel.gameObject.name, OnButtonCancelClicked);

        profileEventSO.OnEventRaised += RefreshSelectionPanel;
    }

    void OnDisable()
    {
        ButtonPressedBehavior.buttonFunctionTable.Clear();

        profileEventSO.OnEventRaised -= RefreshSelectionPanel;
    }

    void Start()
    {
        Time.timeScale = 1f;
        GameManager.CurrentGameState = GameState.Playing;
        UIInput.Instance.SelectUI(buttonStart);
    }

    void OnButtonStartClicked()
    {
        mainMenuCanvas.enabled = false;
        playerModel.SetActive(true);
        UIInput.Instance.SelectUI(buttonSubmit);
        tipsCanvas.enabled = true;
        //SceneLoader.Instance.LoadGameplayScene();
    }

    void OnButtonOptionsClicked()
    {
        UIInput.Instance.SelectUI(buttonOptions);
    }

    void OnButtonQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void RefreshSelectionPanel(PlayerProfileSO profile)
    {
        playerSelectionCanvas.enabled = true;
        textProfile.text = string.Format($"Max Health: {profile.MaxHealth}\nMove Speed: {profile.MoveSpeed}\nFire Interval: {profile.FireInterval}\nWeapon Type: {profile.defaultWeaponType}");

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
        playerSelectionCanvas.enabled = false;
        UIInput.Instance.SelectUI(buttonSubmit);
    }
}