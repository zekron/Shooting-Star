using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour, IMoveable
{
    [SerializeField] private GameObject hitVFX;
    [SerializeField] AudioDataSO hitSFX;
    [SerializeField] private float damage;
    [SerializeField] protected float moveSpeed = 10f;
    [SerializeField] protected Vector2 moveDirection;

    protected GameObject target;

    protected virtual void OnEnable()
    {
        StartCoroutine(MoveDirectlyCoroutine());
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Character>(out Character character))
        {
            character.GetDamage(damage);
            ObjectPoolManager.Release(hitVFX, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal));
            AudioManager.Instance.PlaySFX(hitSFX);
            gameObject.SetActive(false);
        }
    }

    IEnumerator MoveDirectlyCoroutine()
    {
        while (gameObject.activeSelf)
        {
            Move(moveDirection * moveSpeed * Time.deltaTime);

            yield return null;
        }
    }

    protected void SetTarget(GameObject target) => this.target = target;

    public virtual void Move(Vector2 deltaMovement)
    {
        transform.Translate(deltaMovement, Space.World);
        //transform.position = deltaMovement;
    }

    public virtual void Rotate(Quaternion moveRotation)
    {
        transform.rotation = moveRotation;
    }
}