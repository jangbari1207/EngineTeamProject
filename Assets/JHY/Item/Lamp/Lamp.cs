using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Lamp : MonoBehaviour
{
    [SerializeField] private ItemSO candle;        
    [SerializeField] private Light2D candleLight;   

    private void Start()
    {
        candleLight.enabled = false;
    }

    private void Update()
    {
        if (InventoryManager.instance.HasItem(candle))
        {
            candleLight.enabled = true;
            enabled = false;    
        }
    }
}
