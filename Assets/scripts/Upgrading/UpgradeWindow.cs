using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UpgradeWindow : MonoBehaviour
{
    [SerializeField] private IUpgradeData[] upgrades;
    [SerializeField] private Button upgradeButtonA, upgradeButtonB;
    [SerializeField] private GameState gameState;
    [SerializeField] private SellItem seller;
    
    private IUpgradeData optionA, optionB;

    private void Start()
    {
        foreach (IUpgradeData upgrade in upgrades)
        {
            upgrade.currentLevel = 0;
        }
    }

    private void OnEnable()
    {
        ShowRandomUpgrade();
    }

    void ShowRandomUpgrade()
    {
        do
        {
            optionA = upgrades[Random.Range(0, upgrades.Length)];
            optionB = upgrades[Random.Range(0, upgrades.Length)];
        } while (!optionA.CanActivate() || !optionB.CanActivate() || optionA.Equals(optionB));
        
        upgradeButtonA.onClick.RemoveAllListeners();
        upgradeButtonA.onClick.AddListener(() => TryBuy(optionA));
        
        upgradeButtonB.onClick.RemoveAllListeners();
        upgradeButtonB.onClick.AddListener(() => TryBuy(optionB));
    }

    void TryBuy(IUpgradeData upgrade)
    {
        if (gameState.currentMoney < upgrade.price) return;
        
        gameState.currentMoney -= upgrade.price;
        seller.UpdateUI();
        
        upgrade.Activate();
        var msgWindow = FindFirstObjectByType<MessageWindow>(FindObjectsInactive.Include);
        msgWindow.gameObject.SetActive(true);
        msgWindow.SetHeader(upgrade.upgradeName + new string('I', upgrade.currentLevel));
        msgWindow.SetDescription(upgrade.GetLastActivationDescription());
        msgWindow.confirmButton.onClick.AddListener(ToEventScreen);
    }
    
    public void ToEventScreen()
    {
        FindFirstObjectByType<EventManager>(FindObjectsInactive.Include).gameObject.SetActive(true);
        FindFirstObjectByType<MessageWindow>(FindObjectsInactive.Include).confirmButton.onClick.RemoveListener(ToEventScreen);
        gameObject.SetActive(false);
    }
    
    public void OnPointerEnterButtonA()
    {
        MouseHoverWindow.Instance.Show();
        MouseHoverWindow.Instance.SetName(optionA.upgradeName);
        MouseHoverWindow.Instance.SetDescription(optionA.upgradeDescription + "\nPrice: $" + optionA.price);
    }
    
    public void OnPointerEnterButtonB()
    {
        MouseHoverWindow.Instance.Show();
        MouseHoverWindow.Instance.SetName(optionB.upgradeName);
        MouseHoverWindow.Instance.SetDescription(optionB.upgradeDescription + "\nPrice: $" + optionB.price);
    }

    public void OnPointerExitButton() => MouseHoverWindow.Instance.Hide();
}
