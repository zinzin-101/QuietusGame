using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyMasterDetect : MonoBehaviour
{
    private string selectedItemName;
    public string SelectedItemName => selectedItemName;

    private bool canSelect;
    public bool CanSelect => canSelect;

    private void Start()
    {
        canSelect = false;
    }

    public IEnumerator SelectItem()
    {
        if (canSelect)
        {
            InventoryManager.Instance.SetKeyMasterItemName("");
            yield return new WaitUntil(() => InventoryManager.Instance.SelectedKeyMasterItemName != "");
            selectedItemName = InventoryManager.Instance.SelectedKeyMasterItemName;
        }
    }

    public void SetCanSelect(bool value)
    {
        canSelect = value;
    }
}
