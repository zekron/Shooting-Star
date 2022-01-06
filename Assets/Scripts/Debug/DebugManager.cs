using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    [SerializeField] private PlayerInputSO inputEvent;
    [SerializeField] private Canvas debugCanvas;

    private bool isOpenDebugCanvas = false;

    private void Start()
    {
        inputEvent.onSetDebugMode += InputEvent_onSetDebugMode;
    }

    private void OnDestroy()
    {
        inputEvent.onSetDebugMode -= InputEvent_onSetDebugMode;
    }

    private void InputEvent_onSetDebugMode()
    {
        if (!debugCanvas)
        {
            debugCanvas = Instantiate(Resources.Load<Canvas>("Prefabs/Canvas_Debug"), GameObject.FindGameObjectWithTag("MainCanvas").transform);
        }

        debugCanvas.gameObject.SetActive(isOpenDebugCanvas = !isOpenDebugCanvas);
    }
}
