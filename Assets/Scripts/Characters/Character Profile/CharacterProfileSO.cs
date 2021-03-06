using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProfileSO : ScriptableObject
{
    public int MaxHealth;

    public float MoveSpeed;
    public float MoveRotationAngle;

    public virtual bool InitializeByString(string dataString) { return true; }
}
