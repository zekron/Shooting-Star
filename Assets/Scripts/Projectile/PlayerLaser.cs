using System.Collections;
using UnityEngine;

public class PlayerLaser : PlayerProjectile
{
    [SerializeField] private LineRenderer laserRenderer;
    [SerializeField] private RaycastHit2D hitInfo;
    [SerializeField] private LayerMask layerMask;

    private BoxCollider2D laserCollider;

    private const float DEFAULT_LENGTH = 15;
    private float laserWidth = 0.5f;
    private Vector2 defaultOffset = new Vector2(0, DEFAULT_LENGTH / 2);
    private Vector2 defaultSize = new Vector2(0.2f, DEFAULT_LENGTH);
    private Vector2 edgePosition = new Vector2();
    private Transform movefollow;

    protected override void OnEnable()
    {
        laserCollider = GetComponent<BoxCollider2D>();
        laserCollider.offset = defaultOffset;
        laserCollider.size = defaultSize;

        base.OnEnable();
    }

    public override void Move(Vector2 deltaMovement)
    {
        if (movefollow)
        {
            transform.position = movefollow.position;

            hitInfo = Physics2D.Raycast(transform.position, Vector2.up, DEFAULT_LENGTH, layerMask);
            if (hitInfo)
            {
                laserRenderer.SetPosition(0, transform.position);
                laserRenderer.SetPosition(1, hitInfo.point);

                defaultOffset.y = hitInfo.distance / 2;
                defaultSize.y = hitInfo.distance;
            }
            else
            {
                edgePosition.x = transform.position.x;
                edgePosition.y = Viewport.MaxY + 1;

                laserRenderer.SetPosition(0, transform.position);
                laserRenderer.SetPosition(1, edgePosition);

                defaultOffset.y = (Viewport.MaxY + 1 - transform.position.y) / 2;
                defaultSize.y = Viewport.MaxY + 1 - transform.position.y;
            }
            laserCollider.offset = defaultOffset;
            laserCollider.size = defaultSize;
        }
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Character>(out Character character))
        {
            character.GetDamage(damage);
            ObjectPoolManager.Release(hitVFX, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal));
            AudioManager.Instance.PlaySFX(hitSFX);
        }
    }
    protected void OnCollisionStay2D(Collision2D collision)
    {
        OnCollisionEnter2D(collision);
    }

    //private IEnumerator ChangeRenderer()
    //{
    //    while (gameObject.activeSelf)
    //    {
    //        yield return null;
    //        if (hitInfo && hitInfo.transform.CompareTag("Enemy"))
    //        {
    //            yield break;
    //            laserRenderer.SetPosition(0, transform.position);
    //            laserRenderer.SetPosition(1, hitInfo.point);


    //            defaultOffset.y = (hitInfo.point.y - transform.position.y) / 2;
    //            defaultSize.y = hitInfo.distance;
    //            //laserCollider.offset = new Vector2(0, (hitInfo.point.y - movefollow.transform.position.y) / 2);
    //            //laserCollider.size = new Vector2(laserWidth, hitInfo.distance);
    //            laserRenderer.startWidth = laserWidth;
    //        }
    //        else
    //        {
    //            laserRenderer.SetPosition(0, transform.position);
    //            laserRenderer.SetPosition(1, transform.position + Vector3.up * 50);

    //            defaultOffset.y = DEFAULT_LENGTH / 2;
    //            defaultSize.y = DEFAULT_LENGTH;
    //            laserCollider.offset = defaultOffset;
    //            laserCollider.size = defaultSize;
    //        }
    //    }
    //}

    public void SetLaserWidth(float width)
    {
        laserWidth = width;
        laserRenderer.startWidth = laserWidth;
        defaultSize.x = laserWidth;
    }
    public void SetLaserDamage(float damage)
    {
        this.damage = damage;
    }
    public void SetPlayer(Transform player)
    {
        movefollow = player;
    }
    public void SetLineRenderer(Material material)
    {
        laserRenderer.material = material;
    }
}