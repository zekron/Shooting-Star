using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [Header("---- Boss ----")]
    [SerializeField] float continuousFireDuration = 1.5f;

    [Header("---- Player Detection ----")]
    [SerializeField] Transform playerDetectionTransform;
    [SerializeField] Vector3 playerDetectionSize;
    [SerializeField] LayerMask playerLayer;

    [Header("---- Beam Weapon ----")]
    [SerializeField] private float beamCooldownInterval = 12f;
    [SerializeField] private AudioDataSO beamChargingSFX;
    [SerializeField] private AudioDataSO beamLaunchSFX;
    private bool isBeamReady;

    private BossHealthBar healthBar;
    private Canvas healthBarCanvas;
    private Animator beamAnimator;
    private int launchBeamID = Animator.StringToHash("launchBeam");

    private WaitForSeconds waitForContinuousFireInterval;
    private WaitForSeconds waitForBeamCooldown;

    private List<GameObject> magazine;

    protected override void Awake()
    {
        base.Awake();

        healthBar = FindObjectOfType<BossHealthBar>();
        healthBarCanvas = healthBar.GetComponentInChildren<Canvas>();
        beamAnimator = GetComponent<Animator>();

        waitForContinuousFireInterval = new WaitForSeconds(minFireInterval);
        waitForFireInterval = new WaitForSeconds(maxFireInterval);
        waitForBeamCooldown = new WaitForSeconds(beamCooldownInterval);

        magazine = new List<GameObject>(projectiles.Length);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        healthBar.Initialize(Health, maxHealth);
        healthBarCanvas.enabled = true;
        muzzleVFX.Stop();

        isBeamReady = false;
        StartCoroutine(nameof(BeamCooldownCoroutine));
    }
    protected override void SetProfile()
    {
        base.SetProfile();
        continuousFireDuration = ((BossProfileSO)enemyProfile).ContinuousFireDuration;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(playerDetectionTransform.position, playerDetectionSize);
    }

    protected override void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.GetDie();
        }
    }

    public override void GetDie()
    {
        healthBarCanvas.enabled = false;
        base.GetDie();
    }

    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
        healthBar.UpdateStats(Health, maxHealth);
    }

    protected override IEnumerator FireCoroutine()
    {
        while (isActiveAndEnabled)
        {
            if (GameManager.CurrentGameState == GameState.GameOver) yield break;

            if (isBeamReady)
            {
                ActivateBeamWeapon();
                enemyController.StartChasingPlayer();
                yield break;
            }
            yield return waitForFireInterval;
            yield return StartCoroutine(nameof(ContinuousFireCoroutine));
        }
    }

    private void LoadProjectiles()
    {
        magazine.Clear();

        if (Physics2D.OverlapBox(playerDetectionTransform.position, playerDetectionSize, 0f, playerLayer))
        {
            magazine.Add(projectiles[0]);
        }
        else
        {
            if (Random.value < 0.5f)
            {
                magazine.Add(projectiles[1]);
            }
            else
            {
                for (int i = 2; i < projectiles.Length; i++)
                {
                    magazine.Add(projectiles[i]);
                }
            }
        }
    }

    private void ActivateBeamWeapon()
    {
        beamAnimator.SetTrigger(launchBeamID);
        isBeamReady = false;
        AudioManager.Instance.PlaySFX(beamChargingSFX);
    }

    protected override void EnemyLevelUp(int value)
    {
        maxHealth += value * levelUpFactor;
    }

    private void AnimationEventLaunchBeam()
    {
        AudioManager.Instance.PlaySFX(beamLaunchSFX);
    }

    private void AnimationEventStopBeam()
    {
        enemyController.StopChasingPlayer();
        StartCoroutine(nameof(BeamCooldownCoroutine));
        StartCoroutine(nameof(FireCoroutine));
    }

    private IEnumerator ContinuousFireCoroutine()
    {
        LoadProjectiles();
        muzzleVFX.Play();

        float continuousFireTimer = 0f;

        while (continuousFireTimer < continuousFireDuration)
        {
            foreach (var projectile in magazine)
            {
                ObjectPoolManager.Release(projectile, muzzles[0].position);
            }

            continuousFireTimer += minFireInterval;
            AudioManager.Instance.PlaySFX(projectileLaunchSFX);

            yield return waitForContinuousFireInterval;
        }
        muzzleVFX.Stop();
    }

    private IEnumerator BeamCooldownCoroutine()
    {
        yield return waitForBeamCooldown;
        isBeamReady = true;
    }
}
