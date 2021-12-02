using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Inventory Package")]
public class InventoryPackage : ScriptableObject
{
    [SerializeField] private float packageDropRate;
    [SerializeField] private List<Inventory> inventoryList;
    [SerializeField] private List<float> inventoryDropRate;

    private void OnValidate()
    {
        Inventory newKey = inventoryList[inventoryList.Count - 1];
        if (inventoryList.FindAll((key) => key == newKey).Count > 1)
        {
            inventoryList.Remove(newKey);
            return;
        }

        float totalRate = 0;
        for (int i = 0; i < inventoryDropRate.Count; i++)
        {

            if (totalRate + inventoryDropRate[i] > 1)
            {
                inventoryDropRate[i] = 0;
                break;
            }
            else totalRate += inventoryDropRate[i];
        }
        EditorUtility.SetDirty(this);
    }

    public Inventory CanDrop()
    {
        if (Random.value > packageDropRate) return null;

        float tempRate = 0;
        float rnd = Random.value;
        for (int i = 0; i < inventoryDropRate.Count; i++)
        {
            tempRate += inventoryDropRate[i];
            if (rnd < tempRate)
            {
                return inventoryList[i];
            }
        }

        return null;
    }
}