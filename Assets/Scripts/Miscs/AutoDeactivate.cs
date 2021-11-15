using System.Collections;
using UnityEngine;

public class AutoDeactivate : MonoBehaviour
{
    [SerializeField] private bool destroyGameObject;
    [SerializeField] private float lifeLime = 3f;

    WaitForSeconds waitLifeTime;

    void Awake()
    {
        waitLifeTime = new WaitForSeconds(lifeLime);
    }

    void OnEnable()
    {
        StartCoroutine(DeactivateCoroutine());
    }

    IEnumerator DeactivateCoroutine()
    {
        yield return waitLifeTime;

        if (destroyGameObject)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}