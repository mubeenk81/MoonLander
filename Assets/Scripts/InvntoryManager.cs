using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Button closeButton;

    void Start()
    {
        closeButton.onClick.AddListener(() => inventoryPanel.SetActive(false));
    }

    public void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }
}
