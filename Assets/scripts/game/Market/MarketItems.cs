using UnityEngine;
using TMPro;

public class MarketItems : MonoBehaviour
{
    [SerializeField] private int itemIndex;
    [SerializeField] private int value;
    [SerializeField] private TextMeshProUGUI valueUI;

    private bool bought = false;

    private void OnEnable()
    {
        bought = UserUnlockable.isItemActive(itemIndex);

        if (bought)
        {
            valueUI.text = "OWNED";
        }
        else
        {
            valueUI.text = value.ToString();
        }
    }

    private void buy()
    {
        if (UserUnlockable.getMoney() >= value)
        {
            UserUnlockable.addMoney(-value);
            UserUnlockable.changeActiveItem(itemIndex, true);
            bought = true;
            valueUI.text = "OWNED";
        }
    }

    public void onClick()
    {
        buy();
    }
}
