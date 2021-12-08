using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUIController : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Canvas optionCanvas;
    [SerializeField] private Button buttonSubmit;
    [SerializeField] private Button buttonCancel;
    [SerializeField] private Slider sliderSFXVolume;
    [SerializeField] private Slider sliderBGMVolume;
    [SerializeField] private Toggle toggleShowHealthBar;

    [Header("Events")]
    [SerializeField] private VoidEventChannelSO optionQuitEventSO;
    [SerializeField] private FloatEventChannelSO changeSFXVolumeEventSO;
    [SerializeField] private FloatEventChannelSO changeBGMVolumeEventSO;
    [SerializeField] private BooleanEventChannelSO setHealthBarEventSO;

    private PlayerOptionData data;
    private float previousSFXVolume;
    private bool previousNeedShow;
    private float previousBGMVolume;

    private void OnEnable()
    {
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonSubmit.gameObject.GetInstanceID(), OnButtonSubmitClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonCancel.gameObject.GetInstanceID(), OnButtonCancelClicked);

        sliderSFXVolume.onValueChanged.AddListener(ChangeSFXVolume);
        sliderBGMVolume.onValueChanged.AddListener(ChangeBGMVolume);
        toggleShowHealthBar.onValueChanged.AddListener(SetShowHealthBar);
    }
    private void Start()
    {
        data = OptionManager.LoadPlayerOptionData();
        sliderBGMVolume.value = previousBGMVolume = data.BGMVolume;
        sliderSFXVolume.value = previousSFXVolume = data.SFXVolume;
        toggleShowHealthBar.isOn = previousNeedShow = data.NeedShowHealthBar;

        changeBGMVolumeEventSO.RaiseEvent(previousBGMVolume);
        changeSFXVolumeEventSO.RaiseEvent(previousSFXVolume);
        setHealthBarEventSO.RaiseEvent(previousNeedShow);
    }

    private void OnDisable()
    {
        ButtonPressedBehavior.buttonFunctionTable.Clear();

        sliderSFXVolume.onValueChanged.RemoveListener(ChangeSFXVolume);
        sliderBGMVolume.onValueChanged.RemoveListener(ChangeBGMVolume);
        toggleShowHealthBar.onValueChanged.RemoveListener(SetShowHealthBar);
    }

    private void OnButtonCancelClicked()
    {
        changeSFXVolumeEventSO.RaiseEvent(previousSFXVolume);
        changeBGMVolumeEventSO.RaiseEvent(previousBGMVolume);

        optionQuitEventSO.RaiseEvent();
    }

    private void OnButtonSubmitClicked()
    {
        OptionManager.SavePlayerOptionData(data);

        optionQuitEventSO.RaiseEvent();
    }

    private void ChangeSFXVolume(float value)
    {
        changeSFXVolumeEventSO.RaiseEvent(data.SFXVolume = value);
    }
    private void ChangeBGMVolume(float value)
    {
        changeBGMVolumeEventSO.RaiseEvent(data.BGMVolume = value);
    }

    private void SetShowHealthBar(bool value)
    {
        setHealthBarEventSO.RaiseEvent(data.NeedShowHealthBar = value);
    }
}
