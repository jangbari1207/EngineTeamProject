using UnityEngine;

[CreateAssetMenu(fileName = "OrgansSO_JCY", menuName = "JCY/OrgansSO_JCY")]
public class OrgansSo_JCY : ScriptableObject
{
    public string organsName;
    public IOrgansEffects_JCY organsEffect;
    [TextArea(4 , 10)] public string description;

    public void SetItemName()
    {
         A a = new A();
         a.itemName = "ddd";
        
    }
    
        
    
    public struct  A
    {
        public string itemName;
    }
}
