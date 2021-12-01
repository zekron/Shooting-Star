using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour, IMoveable, IRotate
{
    [SerializeField] private GameObject hitVFX;
    [SerializeField] AudioDataSO hitSFX;
    [SerializeField] private float damage;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] protected Vector2 moveDirection;

    protected GameObject target;

    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

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
        transform.Translate(deltaMovement);
        //transform.position = deltaMovement;
    }

    public virtual void Rotate(Quaternion moveRotation)
    {
        transform.rotation = moveRotation;
    }
}