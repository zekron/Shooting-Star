using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer), typeof(RectTransform))]
[AddComponentMenu("UI/Selectable Graphic", 1000)]
public class CustomSelectableGraphic : Graphic, IPointerEnterHandler, IPointerExitHandler
{
    [Space(), Header("Custom")]
    [SerializeField] private IntEventChannelSO profileIndexEventSO;
    [SerializeField] private BooleanEventChannelSO[] booleanEventSO;
    [SerializeField] private int profileIndex;

    public void OnPointerEnter(PointerEventData eventData)
    {
        profileIndexEventSO.RaiseEvent(profileIndex);
        for (int i = 0; i < booleanEventSO.Length; i++)
        {
            booleanEventSO[i].RaiseEvent(false);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        for (int i = 0; i < booleanEventSO.Length; i++)
        {
            booleanEventSO[i].RaiseEvent(true);
        }
    }
}
