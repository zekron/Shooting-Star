using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ModelRotation : MonoBehaviour
{
    [SerializeField] private BooleanEventChannelSO setCanRotateEventSO;
    
    private bool canRotate = true;

    private void OnEnable()
    {
        setCanRotateEventSO.OnEventRaised += SetCanRotation;
    }

    private void OnDisable()
    {
        setCanRotateEventSO.OnEventRaised -= SetCanRotation;
    }

    void FixedUpdate()
    {
        if (canRotate && gameObject.activeSelf)
        {
            transform.rotation = Quaternion.AngleAxis(Time.realtimeSinceStartup * 100 % 360, Vector3.up);
        }
    }

    private void SetCanRotation(bool value)
    {
        canRotate = value;
    }
}
