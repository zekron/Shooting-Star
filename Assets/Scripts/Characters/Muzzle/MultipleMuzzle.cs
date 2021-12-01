using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleMuzzle : Muzzle
{
    public Muzzle[] OddMuzzles;
    public Muzzle[] EvenMuzzles;

    public override Vector3[] GetMuzzle(MuzzlePower muzzlePower)
    {
        switch (muzzlePower)
        {
            case MuzzlePower.Single:
                return ToArray(OddMuzzles[0]);
            case MuzzlePower.Double:
                return ToArray(EvenMuzzles[0], EvenMuzzles[1]);
            case MuzzlePower.Triple:
                return ToArray(OddMuzzles);
            case MuzzlePower.Quadruple:
                return ToArray(EvenMuzzles);
            default:
                return base.GetMuzzle();
        }
    }
}
