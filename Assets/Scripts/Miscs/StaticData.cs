using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticData
{
    public static MuzzlePower GetShotGunPower(MainWeaponPower weaponPower)
    {
        switch (weaponPower)
        {
            case MainWeaponPower.Level1:
            case MainWeaponPower.Level3:
                return MuzzlePower.Double;
            case MainWeaponPower.Level2:
            case MainWeaponPower.Level4:
            case MainWeaponPower.Level6:
                return MuzzlePower.Triple;
            case MainWeaponPower.DEBUG:
            case MainWeaponPower.Level5:
            case MainWeaponPower.Level7:
                return MuzzlePower.Quadruple;
            default:
                return MuzzlePower.Single;
        }
    }

    public static MainWeaponType GetNextMainWeaponType(MainWeaponType mainWeaponType)
    {
        return mainWeaponType + 1 >= MainWeaponType.MAX ? 0 : mainWeaponType + 1;
    }
    public static SubWeaponType GetNextSubWeaponType(SubWeaponType subWeaponType)
    {
        return subWeaponType + 1 >= SubWeaponType.MAX ? 0 : subWeaponType + 1;
    }
}
public enum MainWeaponPower
{
    DEBUG = -1,

    Level1,
    Level2,
    Level3,
    Level4,
    Level5,
    Level6,
    Level7,

    MAX,
}
public enum SubWeaponPower
{
    DEBUG = -1,

    None,
    Level1,
    Level2,
    Level3,
    Level4,

    MAX,
}
public enum MainWeaponType
{
    ShotGun,
    Laser,

    MAX,
}
public enum SubWeaponType
{
    NULL,

    Homing,
    Missile,

    MAX,
}
