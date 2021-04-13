using UnityEngine;

public class StockZone : MonoBehaviour
{
    [HideInInspector]
    public Inventory inv;

    private void Awake()
    {
        inv = GetComponent<Inventory>();
    }
}