using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : ScriptableObject
{

   public static void save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/profile.sav";
        FileStream stream = new FileStream(path, FileMode.Create);

        playerProfile profile = new playerProfile();
        formatter.Serialize(stream, profile);
        stream.Close();

    }

    public static void load()
    {
        string path = Application.persistentDataPath + "/profile.sav";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.OpenOrCreate);

            playerProfile profile = formatter.Deserialize(stream) as playerProfile;

            UserUnlockable.addMoney(profile.money);
            UserUnlockable.changeActiveWeapon(profile.activeWeapon);
            UserUnlockable.setAllWeapons(profile.weapons);
            UserUnlockable.setAllItems(profile.items);

            stream.Close();
        }else
        {
            Debug.LogError("ERROR!");
        }
    }
}

[System.Serializable]
public class playerProfile
{
    public int money = UserUnlockable.getMoney();

    public bool[] weapons = UserUnlockable.getAllWeapons();
    public bool[] items = UserUnlockable.getAllItems();

    public int activeWeapon = UserUnlockable.getActiveWeapon();
}
