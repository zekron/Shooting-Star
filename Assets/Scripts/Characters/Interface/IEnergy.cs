using UnityEngine;

public interface IEnergy
{
    void GainEnergy(int value);
    void DrainEnergy(int value);
    bool IsEnough(int value);
}