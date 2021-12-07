using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModelRotation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private PlayerProfileSO profile;
    [SerializeField] private ProfileEventChannelSO profileEventSO;

    private bool canRotate = true;

    public void OnPointerClick(PointerEventData eventData)
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        profileEventSO.RaiseEvent(profile);
        canRotate = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        canRotate = true;
    }
    void Start() { transform.parent.gameObject.SetActive(true); }
    void FixedUpdate()
    {
        if (canRotate && gameObject.activeSelf)
        {
            transform.rotation = Quaternion.AngleAxis(Time.realtimeSinceStartup * 100 % 360, Vector3.up);
        }
    }

}
