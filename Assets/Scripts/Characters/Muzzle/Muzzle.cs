using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muzzle : MonoBehaviour
{
    public Transform muzzle;

    private void OnValidate()
    {
        muzzle = GetComponent<Transform>();
    }

    public virtual Vector3[] GetMuzzle(MuzzlePower muzzlePower = MuzzlePower.Single)
    {
        return new Vector3[] { muzzle.position };
    }

    public static Vector3[] ToArray(params Muzzle[] muzzles)
    {
        var result = new Vector3[muzzles.Length];

        for (int i = 0; i < muzzles.Length; i++)
            result[i] = muzzles[i].muzzle.position;

        return result;
    }
}
