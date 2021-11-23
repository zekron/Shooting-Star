using UnityEngine;
using UnityEngine.UI;

public class MissileDisplay : MonoBehaviour
{
    [SerializeField] private Text amountText;
    [SerializeField] private Image cooldownImage;

    public void UpdateAmountText(int amount) => amountText.text = amount.ToString();

    public void UpdateCooldownImage(float fillAmount) => cooldownImage.fillAmount = fillAmount;
}