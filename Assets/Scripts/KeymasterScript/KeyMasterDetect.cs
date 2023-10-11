using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyMasterDetect : MonoBehaviour
{
    private string selectedItemName;
    public string SelectedItemName => selectedItemName;

    public IEnumerator SelectItem()
    {
        InventoryManager.Instance.SetKeyMasterItemName("");
        yield return new WaitUntil(() => InventoryManager.Instance.SelectedKeyMasterItemName != "");
        selectedItemName = InventoryManager.Instance.SelectedKeyMasterItemName;
    }
}
