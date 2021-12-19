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
    [SerializeField] private PlayerProfileSO profile;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private ProfileEventChannelSO profileEventSO;
    [SerializeField] private BooleanEventChannelSO[] booleanEventSO;

    public void OnPointerEnter(PointerEventData eventData)
    {
        profileEventSO.RaiseEvent(profile, playerPrefab);
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
