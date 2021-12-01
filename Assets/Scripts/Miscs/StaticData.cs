using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticData
{
    public static MuzzlePower GetShotGunPower(WeaponPower weaponPower)
    {
        switch (weaponPower)
        {
            case WeaponPower.Level1:
            case WeaponPower.Level3:
                return MuzzlePower.Double;
            case WeaponPower.Level2:
            case WeaponPower.Level4:
            case WeaponPower.Level6:
                return MuzzlePower.Triple;
            case WeaponPower.DEBUG:
            case WeaponPower.Level5:
            case WeaponPower.Level7:
                return MuzzlePower.Quadruple;
            default:
                return MuzzlePower.Single;
        }
    }
}
public enum WeaponPower
{
    DEBUG = -1,

    Level1,
    Level2,
    Level3,
    Level4,
    Level5,
    Level6,
    Level7,
}
