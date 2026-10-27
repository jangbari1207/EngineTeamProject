using UnityEngine;
[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public StatToChange statToChange;
    public ItemCategory category;
    public int amountToChangeStat;
    public Sprite sprite;
    [TextArea] public string itemDescription;

    public bool UseItem()
    {
        PlayerHealth player = PlayerHealth.instance;

        if (statToChange == StatToChange.health)
        {
            if (player.currentHealth == player.maxHealth)
                return false;

            player.RestoreHealth(amountToChangeStat);
            return true;
        }
        return false;
    }
    public enum StatToChange
    {
        none,
        health,
        mana,
        stamina
    }
    public enum ItemCategory    
    {
        tool,
        recovery,
        clue
    }
}
