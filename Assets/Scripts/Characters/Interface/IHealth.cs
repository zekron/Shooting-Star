using UnityEngine;

public interface IHealth
{
    void GetDamage(float damage);
    void GetHealing(float healing);
    void GetDie();
}