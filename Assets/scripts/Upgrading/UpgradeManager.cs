using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UpgradeManager : MonoBehaviour
{
    [FormerlySerializedAs("upgrades")] [SerializeField] private IUpgradeData[] possibleUpgrades;
    [SerializeField] private Button upgradeButtonA, upgradeButtonB;
    [SerializeField] private GameState gameState;
    [SerializeField] private SellItem seller;
    
    private IUpgradeData optionA, optionB;

    [SerializeField] private List<IUpgradeData> unlockedUpgrades;

    private static UpgradeManager instance;

    private static UpgradeManager Instance => instance ??= FindFirstObjectByType<UpgradeManager>(FindObjectsInactive.Include);

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        foreach (IUpgradeData upgrade in possibleUpgrades)
        {
            upgrade.ResetUpgrade();
        }
    }

    public void ResetUpgrades()
    {
        foreach (IUpgradeData upgrade in possibleUpgrades)
        {
            upgrade.ResetUpgrade();
        }
        unlockedUpgrades.Clear();
    }

    private void OnEnable()
    {
        ShowRandomUpgrade();
    }

    void ShowRandomUpgrade()
    {
        do
        {
            optionA = possibleUpgrades[Random.Range(0, possibleUpgrades.Length)];
            optionB = possibleUpgrades[Random.Range(0, possibleUpgrades.Length)];
        } while (!optionA.CanActivate() || !optionB.CanActivate() || optionA.Equals(optionB));

        upgradeButtonA.GetComponent<Image>().sprite = optionA.upgradeCard;
        upgradeButtonA.onClick.RemoveAllListeners();
        upgradeButtonA.onClick.AddListener(() => TryBuy(optionA));
        upgradeButtonA.gameObject.SetActive(true);
        
        upgradeButtonB.GetComponent<Image>().sprite = optionB.upgradeCard;
        upgradeButtonB.onClick.RemoveAllListeners();
        upgradeButtonB.onClick.AddListener(() => TryBuy(optionB));
        upgradeButtonB.gameObject.SetActive(true);
    }

    void TryBuy(IUpgradeData upgrade)
    {
        if (gameState.currentMoney < upgrade.price) return;
        
        gameState.currentMoney -= upgrade.price;
        seller.UpdateUI();
        
        upgrade.Activate();
        if (!unlockedUpgrades.Contains(upgrade))
            unlockedUpgrades.Add(upgrade);

        MouseHoverWindow.Instance.Hide();
        upgradeButtonA.gameObject.SetActive(false);
        upgradeButtonB.gameObject.SetActive(false);
        var msgWindow = FindFirstObjectByType<MessageWindow>(FindObjectsInactive.Include);
        msgWindow.gameObject.SetActive(true);
        msgWindow.SetHeader(upgrade.upgradeName + " " + new string('I', upgrade.currentLevel));
        msgWindow.SetDescription(upgrade.GetLastActivationDescription());
        msgWindow.confirmButton.onClick.AddListener(ToEventScreen);
        
        SoundEffectsManager.SFX.PlaySellSound();
    }

    public static float GetValueMultiplier()
    {
        float multiplier = 1f;
        foreach (var upgrade in Instance.unlockedUpgrades)
        {
            if (upgrade is not MultiplierUpgrade multiplierUpgrade) continue;

            multiplier *= multiplierUpgrade.Multiplier;
        }
        return multiplier;
    }
    
    public void ToEventScreen()
    {
        SoundEffectsManager.SFX.PlayFlipSound();
        FindFirstObjectByType<EventManager>(FindObjectsInactive.Include).gameObject.SetActive(true);
        FindFirstObjectByType<MessageWindow>(FindObjectsInactive.Include).confirmButton.onClick.RemoveListener(ToEventScreen);
        gameObject.SetActive(false);
    }
    
    public void OnPointerEnterButtonA()
    {
        MouseHoverWindow.Instance.Show(true);
        MouseHoverWindow.Instance.SetName(optionA.upgradeName + " " + new string('I', optionA.currentLevel+1));
        MouseHoverWindow.Instance.SetDescription(optionA.upgradeDescription + "\nPrice: $" + optionA.price);
    }
    
    public void OnPointerEnterButtonB()
    {
        MouseHoverWindow.Instance.Show(true);
        MouseHoverWindow.Instance.SetName(optionB.upgradeName + " " + new string('I', optionB.currentLevel+1));
        MouseHoverWindow.Instance.SetDescription(optionB.upgradeDescription + "\nPrice: $" + optionB.price);
    }

    public void OnPointerExitButton() => MouseHoverWindow.Instance.Hide();
}
