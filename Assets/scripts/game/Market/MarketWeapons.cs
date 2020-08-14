using UnityEngine;
using TMPro;

public class MarketWeapons : MonoBehaviour
{
    [SerializeField] private Weapon thisWeapon;
    [SerializeField] private int value;
    [SerializeField] private TextMeshProUGUI valueUI;

    private bool bought = false;
    private bool equipped = false;

    private void Awake()
    {
        UserUnlockable.saveWeapon(0, true);
    }

    private void OnEnable()
    {
        bought = UserUnlockable.isBought(thisWeapon.getWeaponIndex());

        if (bought)
        {
            valueUI.text = "OWNED";
        }
        else
        {
            valueUI.text = value.ToString();
        }
    }

    public void onClick()
    {
        if (!bought)
        {
            buy();
        }
        else
        {
            equip();
        }
    }

    private void buy()
    {
        if (UserUnlockable.getMoney() >= value)
        {
            UserUnlockable.addMoney(-value);
            bought = true;
            valueUI.text = "OWNED";
            UserUnlockable.saveWeapon(thisWeapon.getWeaponIndex(), true);
        }
    }

    private void equip()
    {
        if (!equipped)
        {
            UserUnlockable.changeActiveWeapon(thisWeapon.getWeaponIndex());
            Market.Instance.unequip();
            equipped = true;
            valueUI.text = "EQUIPED";
        }
    }

    public bool isEquiped()
    {
        return equipped;
    }

    public void unequip()
    {
        equipped = false;
        valueUI.text = "OWNED";
    }

}
