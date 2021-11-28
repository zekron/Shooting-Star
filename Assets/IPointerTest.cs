using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IPointerTest : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector2 moveDirection;
    private Vector3 targetDirection;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Down");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Up");
    }

    // Start is called before the first frame update
    void Start()
    {
        Check();
    }
    [ContextMenu("Check")]
    private void Check()
    {
        if (moveDirection != Vector2.right)
            transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector2.right, moveDirection);
    }

    [ContextMenu("TranslateWorld")]
    private void TranslateWorld()
    {
        transform.Translate(moveDirection * Time.deltaTime, Space.World);
    }

    [ContextMenu("TranslateSelf")]
    private void TranslateSelf()
    {
        transform.Translate(moveDirection * Time.deltaTime, Space.Self);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(target.transform.position, transform.position, Color.red);
        targetDirection = target.transform.position - transform.position;
        transform.rotation = Quaternion.AngleAxis((Mathf.Atan2(targetDirection.y, targetDirection.x) - Mathf.PI / 2) * Mathf.Rad2Deg, Vector3.forward)
       * Quaternion.Euler(0f, 0f, 0);
    }
}
