using UnityEngine;
[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public StartToChange startToChange;
    public int amountToChangeStart;
    public Sprite sprite;
    [TextArea] public string itemDescription;

    public bool UseItem()
    {
        PlayerHealth player = PlayerHealth.instance;

        if (startToChange == StartToChange.health)
        {
            if (player.currentHealth == player.maxHealth)
                return false;

            player.RestoreHealth(amountToChangeStart);
            return true;
        }
        return false;
    }
    public enum StartToChange
    {
        none,
        health,
        mana,
        stamina
    }
}
