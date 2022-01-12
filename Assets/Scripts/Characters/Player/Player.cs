using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(MoveController))]
public class Player : Character
{
    [Header("Player")]
    [SerializeField] private PlayerProfileSO playerProfile;
    [SerializeField] private FloatEventChannelSO shieldInitEventSO;
    [SerializeField] private FloatEventChannelSO shieldUpdateEventSO;
    [SerializeField] private FloatEventChannelSO restoreShieldEventSO;
    [SerializeField] private IntEventChannelSO updatePlayerEnergyEventSO;
    [SerializeField] private IntEventChannelSO updateBombEventSO;
    [SerializeField] private PlayerInputSO input;

    [Header("Regeneration")]
    [SerializeField] private bool regenerateHealth = true;
    [SerializeField] private float healthRegenerateTime;
    [SerializeField, Range(0f, 1f)] private float healthRegeneratePercent;

    [Header("MainWeapon")]
    [SerializeField] private IntEventChannelSO upgradeMainWeaponPowerEventSO;
    [SerializeField] private IntEventChannelSO setMainWeaponTypeEventSO;
    [SerializeField] private GameObject projectileOverdrive;
    [SerializeField, Range(0, (int)MainWeaponPower.MAX - 1)] private int mainWeaponPower = 0;
    private MainWeaponType mainWeaponType;
    protected float fireInterval = 0.2f;

    [Header("SubWeapon")]
    [SerializeField] private IntEventChannelSO upgradeSubWeaponPowerEventSO;
    [SerializeField] private IntEventChannelSO setSubWeaponTypeEventSO;
    [SerializeField] private SubWeaponType subWeaponType;
    [SerializeField, Range(0, (int)SubWeaponPower.MAX - 1)] private int subWeaponPower;
    private SubWeaponSystem subWeaponSystem;

    [Header("Dodge")]
    [SerializeField] private AudioDataSO dodgeSFX;
    [SerializeField] private float dodgeDuration = 2;
    [SerializeField] private float rollSpeed = 360f;
    [SerializeField] private Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);
    private int dodgeEnergyCost = 25;
    private bool isDodging = false;
    private float currentRoll;
    private float maxRoll;

    [Header("OverDrive")]
    [SerializeField] private FloatEventChannelSO overdriveOnEvent;
    [SerializeField] private VoidEventChannelSO overdriveOffEvent;
    [SerializeField] private Material[] laserMaterials;
    private float overdriveDuration = 10;
    private int overdriveDodgeFactor = 2;
    private float overdriveSpeedFactor = 1.2f;
    private float overdriveFireFactor = 1.2f;
    private bool isOverdriving = false;

    private Rigidbody2D playerRigidbody;
    private Collider2D playerCollider;
    private MoveController moveController;
    private PlayerEnergy playerEnergy;
    private MissileSystem missile;

    private readonly float slowMotionDuration = 1f;
    private readonly float invincibleTime = 1f;

    private float coroutineTimer;
    private Coroutine moveCoroutine;
    private Coroutine healthRegenerateCoroutine;

    private WaitForSeconds waitForOverdriveFireInterval;
    private WaitForSeconds waitHealthRegenerateTime;
    private WaitForSeconds waitInvincibleTime;
    private WaitForSeconds waitForOverdriveInterval;
    private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    public bool IsInfiniteEnergy = false;
    public bool IsInfiniteBomb = false;
    public bool IsInvincible = false;

    protected override void OnEnable()
    {
        base.OnEnable();

        input.eventOnFire += Fire;
        input.eventOnStopFire += StopFire;
        input.eventOnDodge += Dodge;
        input.eventOnOverdrive += OverDrive;
        input.eventOnLaunchMissile += LaunchMissile;

        overdriveOnEvent.OnEventRaised += OpenOverdrive;
        overdriveOffEvent.OnEventRaised += StopOverdrive;

        setMainWeaponTypeEventSO.OnEventRaised += SetMainWeaponType;
        setSubWeaponTypeEventSO.OnEventRaised += SetSubWeaponType;
        upgradeMainWeaponPowerEventSO.OnEventRaised += UpgradeMainWeaponPower;
        upgradeSubWeaponPowerEventSO.OnEventRaised += UpgradeSubWeaponPower;
        restoreShieldEventSO.OnEventRaised += GetHealing;
        updatePlayerEnergyEventSO.OnEventRaised += GetEnergy;
        updateBombEventSO.OnEventRaised += GetMissile;

        SetProfile();
    }

    protected override void OnDisable()
    {
        input.eventOnFire -= Fire;
        input.eventOnStopFire -= StopFire;
        input.eventOnDodge -= Dodge;
        input.eventOnOverdrive -= OverDrive;
        input.eventOnLaunchMissile -= LaunchMissile;

        overdriveOnEvent.OnEventRaised -= OpenOverdrive;
        overdriveOffEvent.OnEventRaised -= StopOverdrive;

        setMainWeaponTypeEventSO.OnEventRaised -= SetMainWeaponType;
        setSubWeaponTypeEventSO.OnEventRaised -= SetSubWeaponType;
        upgradeMainWeaponPowerEventSO.OnEventRaised -= UpgradeMainWeaponPower;
        upgradeSubWeaponPowerEventSO.OnEventRaised -= UpgradeSubWeaponPower;
        restoreShieldEventSO.OnEventRaised -= GetHealing;
        updatePlayerEnergyEventSO.OnEventRaised -= GetEnergy;
        updateBombEventSO.OnEventRaised -= GetMissile;

        base.OnDisable();
    }

    private void OnValidate()
    {
        SetProfile();
    }

    protected override void Awake()
    {
        base.Awake();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        moveController = GetComponent<MoveController>();
        subWeaponSystem = GetComponent<SubWeaponSystem>();
        missile = GetComponent<MissileSystem>();
        playerEnergy = GetComponent<PlayerEnergy>();

        maxRoll = dodgeDuration * rollSpeed;
        playerRigidbody.gravityScale = 0;

        waitForFireInterval = new WaitForSeconds(fireInterval);
        waitForOverdriveFireInterval = new WaitForSeconds(fireInterval / overdriveFireFactor);
        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);
        waitInvincibleTime = new WaitForSeconds(invincibleTime);
    }

    private void Start()
    {

        input.EnableGameplayInput();
        shieldInitEventSO.RaiseEvent(maxHealth);

#if UNITY_ANDROID
        StartCoroutine(nameof(FireCoroutine));
#endif
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            interactable.Activate(this);
        }
    }

    protected override void SetProfile()
    {
        moveController = GetComponent<MoveController>();

        maxHealth = playerProfile.MaxHealth;

        MoveSpeed = playerProfile.MoveSpeed;
        MoveRotationAngle = playerProfile.MoveRotationAngle;
        moveController.SetMoveProfile(MoveSpeed, MoveRotationAngle);

        fireInterval = playerProfile.FireInterval;
        mainWeaponType = playerProfile.defaultWeaponType;
        dodgeEnergyCost = playerProfile.DodgeCost;

        overdriveDodgeFactor = playerProfile.OverdriveDodgeFactor;
        overdriveSpeedFactor = playerProfile.OverdriveSpeedFactor;
        overdriveFireFactor = playerProfile.OverdriveFireFactor;
    }

    private void GetEnergy(int value)
    {
        playerEnergy.GainEnergy(value);
    }

    #region HEALTH
    public override void GetDamage(float damage)
    {
        if (IsInvincible) return;

        base.GetDamage(damage);
        shieldUpdateEventSO.RaiseEvent(Health);
        TimeController.Instance.BulletTime(slowMotionDuration);

        if (gameObject.activeSelf)
        {
            StartCoroutine(nameof(InvincibleCoroutine));
            if (regenerateHealth)
            {
                if (healthRegenerateCoroutine != null)
                {
                    StopCoroutine(healthRegenerateCoroutine);
                }

                healthRegenerateCoroutine = StartCoroutine(HealthRegenerateCoroutine(waitHealthRegenerateTime, healthRegeneratePercent));
            }
        }
    }

    public override void GetHealing(float value)
    {
        base.GetHealing(value);
        shieldUpdateEventSO.RaiseEvent(Health);
    }

    public override void GetDie()
    {
        if (IsInvincible) return;

        GameManager.Instance.CurrentGameState = GameState.GameOver;
        shieldUpdateEventSO.RaiseEvent(Health = 0);
        base.GetDie();
    }

    private IEnumerator InvincibleCoroutine()
    {
        playerCollider.isTrigger = true;
        yield return waitInvincibleTime;
        playerCollider.isTrigger = false;
    }
    #endregion

    public override void Move(Vector2 deltaMovement)
    {
        base.Move(deltaMovement);
        transform.position = Viewport.PlayerMoveablePosition(transform.position, paddingX, paddingY);
    }

    #region Fire
    protected override IEnumerator FireCoroutine()
    {
        while (true)
        {
            if (mainWeaponType == MainWeaponType.ShotGun)
            {
                CloseLaser();

                ReloadShotGun((MainWeaponPower)mainWeaponPower);
            }
            else
                ReloadLaser();
            subWeaponSystem.Launch(multiMuzzles, subWeaponType, (SubWeaponPower)subWeaponPower);

            AudioManager.Instance.PlaySFX(projectileLaunchSFX);
            yield return isOverdriving ? waitForOverdriveFireInterval : waitForFireInterval;
        }
    }
    public override void StopFire()
    {
        base.StopFire();
        CloseLaser();
    }

    private void ReloadShotGun(MainWeaponPower weaponPower)
    {
        int muzzleAmount;
        if (weaponPower >= MainWeaponPower.Level6 || weaponPower == MainWeaponPower.DEBUG)
        {
            muzzleAmount = multiMuzzles.Length;
        }
        else if (weaponPower >= MainWeaponPower.Level3)
        {
            muzzleAmount = multiMuzzles.Length - 2;
        }
        else if (weaponPower >= MainWeaponPower.Level1)
        {
            muzzleAmount = multiMuzzles.Length - 4;
        }
        else
        {
            muzzleAmount = 1;
        }

        for (int i = 0; i < muzzleAmount; i++)
        {
            ObjectPoolManager.Release(isOverdriving ? projectileOverdrive : projectiles[i],
                  multiMuzzles[i].GetMuzzle(StaticData.GetShotGunPower(weaponPower)));
        }
    }

    private PlayerLaser laser;

    private void ReloadLaser()
    {
        if (!laser)
        {
            laser = ObjectPoolManager.Release(projectiles[projectiles.Length - 1],
                 multiMuzzles[0].muzzle.position).GetComponent<PlayerLaser>();
        }

        SetLaser();
    }

    private void SetLaser()
    {
        if (!laser) return;

        laser.SetPlayer(multiMuzzles[0].transform.parent);
        laser.SetLaserWidth(StaticData.SetLaserWidth((MainWeaponPower)mainWeaponPower));
        laser.SetLaserDamage(isOverdriving ? (mainWeaponPower + 1) * 2 : mainWeaponPower + 1);
        laser.SetLineRenderer(laserMaterials[isOverdriving ? 1 : 0]);
    }

    private void CloseLaser()
    {
        if (laser)
        {
            laser.gameObject.SetActive(false);
            laser = null;
        }
    }

    private void UpgradeMainWeaponPower(int levelToUp)
    {
        if (!CanUpgradeMainWeaponPower(levelToUp)) return;

        mainWeaponPower += levelToUp;

        if (mainWeaponType == MainWeaponType.Laser) SetLaser();
    }

    public bool CanUpgradeMainWeaponPower(int levelToUp)
    {
        return mainWeaponPower + levelToUp < (int)MainWeaponPower.MAX;
    }

    private void UpgradeSubWeaponPower(int levelToUp)
    {
        if (!CanUpgradeSubWeaponPower(levelToUp)) return;

        subWeaponPower += levelToUp;
    }

    public bool CanUpgradeSubWeaponPower(int levelToUp)
    {
        return subWeaponPower + levelToUp < (int)SubWeaponPower.MAX;
    }

    private void SetMainWeaponType(int type)
    {
        mainWeaponType = (MainWeaponType)type;
    }

    private void SetSubWeaponType(int type)
    {
        subWeaponType = (SubWeaponType)type;
    }
    #endregion

    #region Dodge
    private void Dodge()
    {
        if (isDodging || !playerEnergy.IsEnough(dodgeEnergyCost)) return;
        StartCoroutine(nameof(DodgeCoroutine));
    }
    private IEnumerator DodgeCoroutine()
    {
        isDodging = true;
        AudioManager.Instance.PlaySFX(dodgeSFX);
        if (!IsInfiniteEnergy) playerEnergy.DrainEnergy(dodgeEnergyCost);
        playerCollider.isTrigger = true;
        currentRoll = 0f;
        TimeController.Instance.BulletTime(slowMotionDuration, slowMotionDuration);

        while (currentRoll < maxRoll)
        {
            currentRoll += rollSpeed * Time.fixedDeltaTime;
            transform.rotation = Quaternion.AngleAxis(currentRoll, Vector3.up);
            transform.localScale = BezierCurve.QuadraticPoint(Vector3.one, Vector3.one, dodgeScale, currentRoll / maxRoll);

            yield return waitForFixedUpdate;
        }

        playerCollider.isTrigger = false;
        isDodging = false;
    }
    #endregion

    #region OverDrive
    private void OverDrive()
    {
        if (playerEnergy.IsEnough(PlayerEnergy.PERCENT))
        {
            if (!playerEnergy.IsEnough(PlayerEnergy.MAX))
                if (isOverdriving)
                {
                    overdriveOffEvent.RaiseEvent();
                }
                else
                {
                    overdriveOnEvent.RaiseEvent(overdriveDuration / 2);
                }
            else
                overdriveOnEvent.RaiseEvent(overdriveDuration);
        }
    }

    private void OpenOverdrive(float unused)
    {
        isOverdriving = true;
        dodgeEnergyCost *= overdriveDodgeFactor;
        moveController.SetMoveSpeedByFactor(overdriveSpeedFactor);
        TimeController.Instance.BulletTime(slowMotionDuration, slowMotionDuration);
    }

    private void StopOverdrive()
    {
        isOverdriving = false;
        dodgeEnergyCost /= overdriveDodgeFactor;
        moveController.SetMoveSpeedByFactor(1 / overdriveSpeedFactor);

        if (IsInfiniteEnergy) playerEnergy.GainEnergy(PlayerEnergy.MAX);
    }
    #endregion

    #region Launch Missile
    public bool CanGainMissile() => missile.CanGainMissile();

    private void GetMissile(int value)
    {
        missile.UpdateMissile(value);
    }

    private void LaunchMissile()
    {
        missile.Launch(multiMuzzles[0].muzzle, isDebugMode: IsInfiniteBomb);
    }
    #endregion
}